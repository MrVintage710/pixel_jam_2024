using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy {
    public class EnemySpawner : MonoBehaviour {
        public GameObject enemyPrefab;

        class Baker : Baker<EnemySpawner> {
            public override void Bake(EnemySpawner authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemySpawnComponent() {
                    enemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.None)
                });
            }
        }
    }
}