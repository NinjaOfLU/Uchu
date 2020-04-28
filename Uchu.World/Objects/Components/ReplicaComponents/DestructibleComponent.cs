using System;
using System.Linq;
using System.Threading.Tasks;
using RakDotNet.IO;
using Uchu.Core;
using Uchu.Core.Client;

namespace Uchu.World
{
    [RequireComponent(typeof(Stats))]
    public class DestructibleComponent : ReplicaComponent
    {
        private Stats Stats { get; set; }

        public override ComponentId Id => ComponentId.DestructibleComponent;

        /// <summary>
        ///     Warning: Should not be used as a definitive client state. Could be unreliable.
        /// </summary>
        public bool Alive { get; private set; } = true;

        public float ResurrectTime { get; set; }

        /// <summary>
        ///     Killer, Loot Owner
        /// </summary>
        public AsyncEvent<GameObject, Player> OnSmashed { get; }

        public AsyncEvent OnResurrect { get; }

        protected DestructibleComponent()
        {
            OnSmashed = new AsyncEvent<GameObject, Player>();
            
            OnResurrect = new AsyncEvent();
            
            Listen(OnStart, async () =>
            {
                if (GameObject.Settings.TryGetValue("respawn", out var resTimer))
                {
                    ResurrectTime = resTimer switch
                    {
                        uint timer => timer,
                        float timer => timer,
                        int timer => timer,
                        _ => ResurrectTime
                    };
                }

                GameObject.Layer = StandardLayer.Smashable;

                await GameObject.AddComponentAsync<LootContainerComponent>();

                Stats = GameObject.GetComponent<Stats>();

                Listen(Stats.OnDeath, async () =>
                {
                    await SmashAsync(
                        Stats.LatestDamageSource,
                        Stats.LatestDamageSource is Player player ? player : default
                    );
                });
            });

            Listen(OnDestroyed, () =>
            {
                OnResurrect.Clear();

                OnSmashed.Clear();
                
                return Task.CompletedTask;
            });
        }

        public override void Construct(BitWriter writer)
        {
            writer.WriteBit(false);
            writer.WriteBit(false);

            GameObject.GetComponent<Stats>().Construct(writer);
        }

        public override void Serialize(BitWriter writer)
        {
            GameObject.GetComponent<Stats>().Serialize(writer);
        }

        public async Task SmashAsync(GameObject smasher, Player owner = default, string animation = default)
        {
            if (!Alive) return;

            Alive = false;

            owner ??= smasher as Player;

            if (owner != null)
            {
                var missionInventoryComponent = owner.GetComponent<MissionInventoryComponent>();

                if (missionInventoryComponent == default) return;

                await missionInventoryComponent.SmashAsync(GameObject.Lot);
            }

            Zone.BroadcastMessage(new DieMessage
            {
                Associate = GameObject,
                DeathType = animation ?? "",
                Killer = smasher,
                SpawnLoot = false,
                LootOwner = owner ?? GameObject
            });

            if (GameObject is Player)
            {
                await GeneratePlayerYieldsAsync(owner);
                
                return;
            }

            //
            // Normal Smashable
            //

            if (owner == null)
            {
                await OnSmashed.InvokeAsync(smasher, default);

                return;
            }

            GameObject.Layer -= StandardLayer.Smashable;
            GameObject.Layer += StandardLayer.Hidden;

            InitializeRespawn();

            await GenerateYieldsAsync(owner);

            await OnSmashed.InvokeAsync(smasher, owner);
        }
        
        private async Task GenerateYieldsAsync(Player owner)
        {
            var container = GameObject.GetComponent<LootContainerComponent>();

            await container.CollectDetailsAsync();

            foreach (var lot in container.GenerateLootYields())
            {
                var drop = await InstancingUtilities.InstantiateLootAsync(lot, owner, GameObject, Transform.Position);

                await StartAsync(drop);
            }

            var currency = container.GenerateCurrencyYields();

            if (currency > 0)
            {
                InstancingUtilities.InstantiateCurrency(currency, owner, GameObject, Transform.Position);
            }
        }

        private async Task GeneratePlayerYieldsAsync(Player owner)
        {
            var player = (Player) GameObject;

            var currency = await player.GetCurrencyAsync();
            
            var coinToDrop = Math.Min((long) Math.Round(currency * 0.1), 10000);

            await player.SetCurrencyAsync(currency - coinToDrop);

            InstancingUtilities.InstantiateCurrency((int) coinToDrop, owner, owner, Transform.Position);
        }
        
        private void InitializeRespawn()
        {
            Task.Run(async () =>
            {
                await Task.Delay((int) (ResurrectTime * 1000));

                await ResurrectAsync();
            });
        }

        public async Task ResurrectAsync()
        {
            Alive = true;

            if (GameObject is Player player)
            {
                Zone.BroadcastMessage(new ResurrectMessage
                {
                    Associate = player
                });

                await Stats.SetHealthAsync(Math.Min(Stats.MaxHealth, 4));
            }
            else
            {
                await Stats.SetHealthAsync(Stats.MaxHealth);

                GameObject.Layer += StandardLayer.Smashable;
                GameObject.Layer -= StandardLayer.Hidden;
            }

            await OnResurrect.InvokeAsync();
        }
    }
}