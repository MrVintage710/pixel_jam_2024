using Unity.Entities;
using Unity.Mathematics;

namespace Projectile {
    public struct ProjectileComponent : IComponentData {
        public float2 direction;
        public float speed;
        public bool is_friendly;
    }

    public struct ProjectileSpawnerComponent : IComponentData {
        public Entity enemyProjectile;
        public Entity friendlyProjectile;
    }
}