using System.Collections.Generic;
using Enemy;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Projectile {
    public partial struct ProjectileMovementSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach (var (projectile, transform) in SystemAPI.Query<RefRO<ProjectileComponent>, RefRW<LocalTransform>>()) {
                transform.ValueRW.Position +=
                    new float3((projectile.ValueRO.direction * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime), 0.0f);
            }
        }
    }

    public partial class ProjectileSpawningSystem : SystemBase {

        private List<SpawnProjectileEvent> _spawnQueue;
        protected override void OnUpdate() {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entitySpawner = SystemAPI.GetSingleton<ProjectileSpawnerComponent>();
            foreach (var e in this._spawnQueue) {
                var entity = entityManager.Instantiate(entitySpawner.enemyProjectile);
                entityManager.SetComponentData(entity, e.data);
                entityManager.SetComponentData(entity, new LocalTransform {
                    Position = new float3(e.position.x, e.position.y, 0.0f),
                    Scale = 1.0f,
                    Rotation = Quaternion.identity
                });
            }
            this._spawnQueue.Clear();
        }
        
        protected override void OnCreate() {
            this._spawnQueue = new List<SpawnProjectileEvent>();
            ECSManager.SpawnProjectileEventHandler += ECSManagerOnSpawnProjectileEventHandler ;
        }

        private void ECSManagerOnSpawnProjectileEventHandler(object sender, SpawnProjectileEvent e) {
            _spawnQueue.Add(e);
        }
    }
}