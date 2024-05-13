using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class PlayerWeapon : MonoBehaviour
{
    public int Level {  get; private set; }
    public void SetLevel(int level)
    {
        Level = level;
    }

    public abstract string WeaponName { get; }
}
