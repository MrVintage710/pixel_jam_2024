using UnityEngine;

public class Weapon_Pistol : PlayerWeapon
{
    public override string WeaponName
    {
        get { return "Pistol"; }
    }

    protected override void FireWeapon()
    {
        Debug.Log("Pistol Weapon Fired");
    }
}
