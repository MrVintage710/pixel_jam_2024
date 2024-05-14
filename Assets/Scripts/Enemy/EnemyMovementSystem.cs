using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

public partial struct EnemyMovementSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach ( var (enemy, transform) in SystemAPI.Query<RefRW<EnemyComponent>, RefRW<LocalTransform>>()) {
            var enemyValue = enemy.ValueRO;
            var virtualPlayerPos = new float2(GameManager.playerPosition.x, GameManager.playerPosition.y);
            var virtualTarget = enemyValue.target + virtualPlayerPos;
            var movement = (math.normalizesafe(virtualTarget - enemyValue.virtualPos) * enemyValue.speed * SystemAPI.Time.DeltaTime);

            enemy.ValueRW.virtualPos += movement;
            transform.ValueRW.Position = new float3((enemyValue.virtualPos - virtualPlayerPos), 0.0f);
        }
    }
}
