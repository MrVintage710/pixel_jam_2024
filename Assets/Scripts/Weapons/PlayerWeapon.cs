using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class PlayerWeapon : MonoBehaviour
{
    [SerializeField, Range(0f, 999f)] //Note that current logic also caps the fire rate at the frame rate.
    float rateOfFire = 1f; //attacks/second
    
    public int Level {  get; private set; }

    protected float lastFireTime = 0;

    public void SetLevel(int level)
    {
        Level = level;
    }

    public abstract string WeaponName { get; }

    public virtual void PullTrigger()
    {
        float firingInterval = 1 / (rateOfFire + Mathf.Epsilon);
        if (Time.time - lastFireTime >= firingInterval)
        {
            FireWeapon();
            lastFireTime = Time.time;
        }
    }

    protected abstract void FireWeapon();
}
