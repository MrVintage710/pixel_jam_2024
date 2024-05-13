using UnityEngine;

public class Weapon_Claw : PlayerWeapon
{
    public override string WeaponName
    {
        get { return "Claw"; }
    }

    protected override void FireWeapon()
    {
        Debug.Log("Claw Weapon Fired");
    }
}
