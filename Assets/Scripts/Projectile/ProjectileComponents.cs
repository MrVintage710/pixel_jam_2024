using Unity.Entities;
using Unity.Mathematics;

namespace Projectile {
    public struct ProjectileComponent : IComponentData {
        public float2 direction;
        public float speed;
    }
}