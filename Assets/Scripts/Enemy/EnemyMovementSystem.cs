using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct EnemyMovementSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach ( var (enemy, transform) in SystemAPI.Query<RefRO<EnemyComponent>, RefRW<LocalTransform>>()) {
            var enemyValue = enemy.ValueRO;
            var movement = (math.normalizesafe(enemy.ValueRO.target - transform.ValueRO.Position.xy) * enemy.ValueRO.speed * SystemAPI.Time.DeltaTime);
            
            transform.ValueRW.Position += new float3(movement.xy, 0.0f);
        }
    }
}

// public partial struct EnemyMovementJob : IJobEntity {
//     public float deltaTime;
//     
//     public void Execute(ref EnemyComponent enemy, ref LocalTransform transform) {
//         var movement = (math.normalize(enemy.target - transform.Position.xy) * enemy.speed * deltaTime);
//         transform.Position += new float3(movement, transform.Position.z);
//     }
// }
