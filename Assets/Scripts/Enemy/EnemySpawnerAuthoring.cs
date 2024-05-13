using Unity.Entities;
using UnityEngine;

namespace Enemy {
    public class EnemySpawnerAuthoring : MonoBehaviour {
        public GameObject enemyPrefab;

        class Baker : Baker<EnemySpawnerAuthoring> {
            public override void Bake(EnemySpawnerAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemySpawnComponent() {
                    enemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}