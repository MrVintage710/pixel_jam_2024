using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static Vector2 virtualPosition = new Vector2();
    public static event EventHandler<SpawnEvent> SpawnEventHandler;

    private NativeQueue<SpawnEvent> eventQueue;

    public struct SpawnEvent {
        public float2 position;
        public EnemyComponent data;
    }

    private void OnEnable() {
        eventQueue = new NativeQueue<SpawnEvent>(Allocator.Persistent);
    }

    private void OnDisable() {
        eventQueue.Dispose();
    }

    private void Start() {
        SpawnEnemy(this, 0.1f, 5.0f, Vector2.zero, new Vector2(2.0f, 5.0f));
        SpawnEnemy(this, 0.1f, 5.0f, Vector2.zero, new Vector2(-2.0f, 5.0f));
    }

    public void Update() {
        // var entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
    }

    public void SpawnEnemy(object source, float speed, float health, Vector2 target, Vector2 position) {
        var enemyComponent = new EnemyComponent {
            speed = speed, 
            health = health, 
            target = new float2(target.x, target.y)
        };
        SpawnEventHandler.Invoke(source, new SpawnEvent {position = position, data = enemyComponent});
    }
}
