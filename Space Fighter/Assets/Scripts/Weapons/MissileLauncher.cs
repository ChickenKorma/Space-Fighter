using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : ProjectileWeapon
{
    public Transform[] muzzles;

    [Header("Weapon Variables")]
    [SerializeField] private float shootRate;

    private float lastShot;

    private int muzzleIndex;

    public float ShootRate
    {
        get { return shootRate; }
    }

    public float LastShot
    {
        get { return lastShot; }
    }

    // Checks if it is reloaded and spawns missile projectile
    public void Shoot()
    {
        if (Time.time > lastShot + shootRate)
        {
            SpawnProjectile(muzzles[muzzleIndex]);

            muzzleIndex = (muzzleIndex + 1) % muzzles.Length;

            lastShot = Time.time;
        }
    }
}
