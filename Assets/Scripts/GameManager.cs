using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static Vector2 playerPosition = new Vector2();

    private void Start() {
        ECSManager.SpawnEnemy(this, 0.0f, 1.0f, 16.0f, Vector2.zero, Vector2.up * 100.0f);
        ECSManager.SpawnProjectile(this, Vector2.zero, Vector2.up, 20.0f, 4.0f, 1, true);
    }
}
