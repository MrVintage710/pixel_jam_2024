using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour {

    public float speed = 0.1f;
    public float health = 10.0f;
    public Vector2 target = Vector2.zero;
    
    public class EnemyBaker : Baker<EnemyAuthoring> {
        public override void Bake(EnemyAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyComponent {
                speed = authoring.speed,
                health = authoring.health,
                target = new float2(authoring.target.x, authoring.target.y)
            });
        }
    }
}
