using Enemy;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Projectile {
    public class Projectile : MonoBehaviour {
        public float speed = 0.1f;
        public Vector2 direction = default;
        public bool is_friendly = false;

        public class EnemyBaker : Baker<Projectile> {
            public override void Bake(Projectile authoring) {
                var direction = authoring.direction.normalized;
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectileComponent() {
                    speed = authoring.speed,
                    direction = new float2(direction.x, direction.y),
                    
                });
            }
        }
    }
}