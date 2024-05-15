using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Enemy {
    public partial struct EnemyMovementSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach ( var (enemy, transform) in SystemAPI.Query<RefRW<EnemyComponent>, RefRW<LocalTransform>>()) {
                var enemyValue = enemy.ValueRO;
                var virtualPlayerPos = new float2(GameManager.virtualPosition.x, GameManager.virtualPosition.y);
                var virtualTarget = enemyValue.target + virtualPlayerPos;
                var movement = (math.normalizesafe(virtualTarget - enemyValue.virtualPos) * enemyValue.speed * SystemAPI.Time.DeltaTime);

                enemy.ValueRW.virtualPos += movement;
                transform.ValueRW.Position = new float3((enemyValue.virtualPos - virtualPlayerPos), 0.0f);
            }
        }
    }
    
    public partial class EnemySpawnSystem : SystemBase {
    
        private List<SpawnEvent> queue;

        private void GameManagerOnSpawnEventHandler(object sender, SpawnEvent e) {
            this.queue.Add(e);
        }

        protected override void OnUpdate() {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entitySpawner = SystemAPI.GetSingleton<EnemySpawnComponent>();
            foreach (var e in this.queue) {
                var entity = entityManager.Instantiate(entitySpawner.enemyPrefab);
                entityManager.SetComponentData(entity, e.data);
                entityManager.SetComponentData(entity, new LocalTransform {
                    Position = new float3(e.position.x, e.position.y, 0.0f),
                    Scale = 1.0f,
                    Rotation = Quaternion.identity
                });
            }
            this.queue.Clear();
        }

        protected override void OnCreate() {
            this.queue = new List<SpawnEvent>();
            ECSManager.SpawnEventHandler += GameManagerOnSpawnEventHandler;
        }
    }
    
    public partial struct EnemyDamageSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach (var enemy in SystemAPI.Query<RefRO<EnemyComponent>>()) {
                var virtualPlayerPos = new float2(GameManager.virtualPosition.x, GameManager.virtualPosition.y);
                var realPosition = enemy.ValueRO.virtualPos - virtualPlayerPos;
                var halfWidth = PlayerController.Hitbox.x / 2.0f;
                var halfHeight = PlayerController.Hitbox.y / 2.0f;
                if (realPosition.x >= -halfWidth && 
                    realPosition.x <= halfHeight && 
                    realPosition.y >= -halfHeight &&
                    realPosition.y <= halfHeight) {
                    
                    ECSManager.DamagePlayer(enemy, 1);
                }
            }
        }
    }
}