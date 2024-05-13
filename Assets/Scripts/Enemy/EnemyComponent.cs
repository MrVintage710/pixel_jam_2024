using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

public struct EnemyComponent : IComponentData {
    public float speed;
    public float health;
    public float2 target;
}
