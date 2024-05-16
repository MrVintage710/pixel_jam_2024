using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Projectile;
using Unity.Mathematics;
using UnityEngine;

public class ECSManager {
    
    public static event EventHandler<SpawnEnemyEvent> SpawnEventHandler;
    public static event EventHandler<DamagePlayerEvent> DamagePlayerEventHandler;
    public static event EventHandler<SpawnProjectileEvent> SpawnProjectileEventHandler;
    public static event EventHandler<AreaDamageEvent> AreaDamageEventHandler; 
    
    /// <summary>
    /// This method will tell the ECS world to spawn an Enemy that will start to move toward the player.
    /// </summary>
    /// <param name="source">The object that is the source of the Event</param>
    /// <param name="speed">The speed that the enemy should have, in units/second</param>
    /// <param name="health">The Health of the Enemy</param>
    /// <param name="target">The movement target of the Enemy. 0.0 is the player</param>
    /// <param name="position">The starting position of the enemy Relative to the player.</param>
    public static void SpawnEnemy(object source, float speed, float health, float size, Vector2 target, Vector2 position) {
        var enemyComponent = new EnemyComponent {
            speed = speed, 
            health = health, 
            target = new float2(target.x, target.y),
            virtualPos = (position)
        };
        var sizedComponent = new SizedComponent { size = size };
        SpawnEventHandler.Invoke(source, new SpawnEnemyEvent {position = position, enemyComponent = enemyComponent, sizedComponent = sizedComponent});
    }

    /// <summary>
    /// Calling this method with damage the player by the specified amount if they are not invulnerable.
    /// </summary>
    /// <param name="source">The object that is dealing damage.</param>
    /// <param name="damage">The amount of damage being dealt.</param>
    public static void DamagePlayer(object source, int damage) {
        DamagePlayerEventHandler.Invoke(source, new DamagePlayerEvent { damage = damage });
    }

    public static void SpawnProjectile(object source, Vector2 pos, Vector2 direction, float speed, float size, int strength, bool isFriendly) {
        var data = new ProjectileComponent {
            direction = new float2(direction.x, direction.y),
            speed = speed,
            is_friendly = isFriendly,
            strength = strength
        };
        var sizedComponent = new SizedComponent { size = size };
        SpawnProjectileEventHandler.Invoke(source, new SpawnProjectileEvent {
            position = new float2(pos.x, pos.y),
            projectileComponent = data,
            sizedComponent = sizedComponent
        });
    }

    public static void AreaDamage(object source, int damage, Vector2 pos, float radius) {
        var data = new AreaDamageEvent {
            damage = damage,
            pos = new float2(pos.x, pos.y),
            radius = radius
        };
        AreaDamageEventHandler.Invoke(source, data);
    }
}

public struct SpawnEnemyEvent {
    public float2 position;
    public SizedComponent sizedComponent;
    public EnemyComponent enemyComponent;
}

public struct SpawnProjectileEvent {
    public float2 position;
    public SizedComponent sizedComponent;
    public ProjectileComponent projectileComponent;
}

public struct DamagePlayerEvent {
    public int damage;
}

public struct AreaDamageEvent {
    public int damage;
    public float2 pos;
    public float radius;
}
