using Enemy;
using Unity.Entities;
using UnityEngine;

namespace Projectile {
    public class ProjectileSpawner : MonoBehaviour {

        public GameObject enemyProjectile;
        public GameObject friendlyProjectile;

        public class ProjectileSpawnerBaker : Baker<ProjectileSpawner> {
            public override void Bake(ProjectileSpawner authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ProjectileSpawnerComponent() {
                    enemyProjectile = GetEntity(authoring.enemyProjectile, TransformUsageFlags.Dynamic),
                    friendlyProjectile = GetEntity(authoring.friendlyProjectile, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}