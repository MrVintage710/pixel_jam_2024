using Unity.Entities;
using Unity.Mathematics;

namespace Enemy {
    public struct EnemyComponent : IComponentData {
        public float speed;
        public float health;
        public float2 target;
        public float2 virtualPos;
    }
    
    public struct EnemySpawnComponent : IComponentData {
        public Entity enemyPrefab;
    }
}