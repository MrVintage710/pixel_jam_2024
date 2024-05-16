using System;
using System.Collections.Generic;
using Projectile;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Enemy {
    // public partial struct EnemyMovementSystem : ISystem {
    //     public void OnUpdate(ref SystemState state) {
    //         foreach ( var (enemy, transform) in SystemAPI.Query<RefRW<EnemyComponent>, RefRW<LocalTransform>>()) {
    //             var enemyValue = enemy.ValueRO;
    //             var virtualPlayerPos = new float2(GameManager.playerPosition.x, GameManager.playerPosition.y);
    //             var virtualTarget = enemyValue.target + virtualPlayerPos;
    //             var movement = (math.normalizesafe(virtualTarget - enemyValue.virtualPos) * enemyValue.speed * SystemAPI.Time.DeltaTime);
    //
    //             enemy.ValueRW.virtualPos += movement;
    //             transform.ValueRW.Position = new float3((enemyValue.virtualPos - virtualPlayerPos), 0.0f);
    //         }
    //     }
    // }
    
    public partial class EnemySpawnSystem : SystemBase {
    
        private List<SpawnEnemyEvent> queue;

        private void GameManagerOnSpawnEventHandler(object sender, SpawnEnemyEvent e) {
            this.queue.Add(e);
        }

        protected override void OnUpdate() {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entitySpawner = SystemAPI.GetSingleton<EnemySpawnComponent>();
            foreach (var e in this.queue) {
                var entity = entityManager.Instantiate(entitySpawner.enemyPrefab);
                entityManager.SetComponentData(entity, e.enemyComponent);
                entityManager.AddComponentData(entity, e.sizedComponent);
                entityManager.SetComponentData(entity, new LocalTransform {
                    Position = new float3(e.position.x, e.position.y, 0.0f),
                    Scale = e.sizedComponent.size,
                    Rotation = Quaternion.identity
                });
            }
            this.queue.Clear();
        }

        protected override void OnCreate() {
            this.queue = new List<SpawnEnemyEvent>();
            ECSManager.SpawnEventHandler += GameManagerOnSpawnEventHandler;
        }
    }
    
    public partial class EnemyDamageSystem : SystemBase {
        protected override void OnUpdate() {
            foreach (var (enemy, enemyTransform, enemySize) in SystemAPI.Query<RefRW<EnemyComponent>, RefRO<LocalTransform>, RefRO<SizedComponent>>()) {
                foreach (var (projectile, projectileTransform, projectileSize) in SystemAPI.Query<RefRO<ProjectileComponent>, RefRO<LocalTransform>, RefRO<SizedComponent>>()) {
                    var distance = math.distance(enemyTransform.ValueRO.Position, projectileTransform.ValueRO.Position);
                    var r = projectile;
                    if (distance > enemySize.ValueRO.size && distance > projectileSize.ValueRO.size) { continue; } else {
                        enemy.ValueRW.health -= projectile.ValueRO.strength;
                        Debug.Log("Damage Dealt!");
                    }
                }
            }
            
            
            foreach (var enemy in SystemAPI.Query<RefRO<EnemyComponent>>()) {
                var virtualPlayerPos = new float2(GameManager.playerPosition.x, GameManager.playerPosition.y);
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