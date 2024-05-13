using System.Collections.Generic;
using Enemy;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnSystem : SystemBase {
    
    private List<GameManager.SpawnEvent> queue;

    private void GameManagerOnSpawnEventHandler(object sender, GameManager.SpawnEvent e) {
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
        this.queue = new List<GameManager.SpawnEvent>();
        GameManager.SpawnEventHandler += GameManagerOnSpawnEventHandler;
    }
}

// public partial struct EnemySpawnJob : IJob {
//     public float2 spawnLocation;
//     
//     public void Execute() {
//         throw new System.NotImplementedException();
//     }
// }