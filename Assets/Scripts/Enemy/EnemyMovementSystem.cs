using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct EnemyMovementSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        var job = new EnemyMovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
    }
}

public partial struct EnemyMovementJob : IJobEntity {
    public float deltaTime;
    
    public void Execute(ref EnemyComponent enemy, ref LocalTransform transform) {
        var movement = (math.normalize(enemy.target - transform.Position.xy) * enemy.speed * deltaTime);
        transform.Position += new float3(movement, transform.Position.z);
    }
}
