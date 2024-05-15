using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Projectile {
    public partial struct ProjectileMovementSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach (var (projectile, transform) in SystemAPI.Query<RefRO<ProjectileComponent>, RefRW<LocalTransform>>()) {
                transform.ValueRW.Position +=
                    new float3((projectile.ValueRO.direction * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime), 0.0f);
            }
        }
    }
}