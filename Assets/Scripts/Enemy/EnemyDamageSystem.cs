using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

namespace Enemy {
    public partial struct EnemyDamageSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            foreach (var enemy in SystemAPI.Query<RefRO<EnemyComponent>>()) {
                var virtualPlayerPos = new float2(GameManager.playerPosition.x, GameManager.playerPosition.y);
                var realPosition = enemy.ValueRO.virtualPos - virtualPlayerPos;
                var halfWidth = PlayerController.Hitbox.x / 2.0f;
                var halfHeight = PlayerController.Hitbox.y / 2.0f;
                if (realPosition.x >= -halfWidth && 
                    realPosition.x <= halfHeight && 
                    realPosition.y >= -halfHeight &&
                    realPosition.y <= halfHeight) {
                    
                    PlayerController.Damage(enemy, 1);
                }
            }
        }
    }
}