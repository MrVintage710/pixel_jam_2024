using Unity.Entities;

namespace Enemy {
    public struct EnemySpawnComponent : IComponentData {
        public Entity enemyPrefab;
    }
}