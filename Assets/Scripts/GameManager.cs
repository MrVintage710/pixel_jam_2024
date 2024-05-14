using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static Vector2 playerPosition = new Vector2();
    public static event EventHandler<SpawnEvent> SpawnEventHandler;

    public struct SpawnEvent {
        public float2 position;
        public EnemyComponent data;
    }

    /// <summary>
    /// This method will tell the ECS world to spawn an Enemy that will start to move toward the player.
    /// </summary>
    /// <param name="source">The object that is the source of the Event</param>
    /// <param name="speed">The speed that the enemy should have, in units/second</param>
    /// <param name="health">The Health of the Enemy</param>
    /// <param name="target">The movement target of the Enemy. 0.0 is the player</param>
    /// <param name="position">The starting position of the enemy Relative to the player.</param>
    public static void SpawnEnemy(object source, float speed, float health, Vector2 target, Vector2 position) {
        var enemyComponent = new EnemyComponent {
            speed = speed, 
            health = health, 
            target = new float2(target.x, target.y),
            virtualPos = (position)
        };
        SpawnEventHandler.Invoke(source, new SpawnEvent {position = position, data = enemyComponent});
    }
}
