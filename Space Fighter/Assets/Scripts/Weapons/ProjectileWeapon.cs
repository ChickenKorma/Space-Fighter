using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Header("Weapon Components")]
    [SerializeField] private GameObject projectile;

    // Spawns projectile at the muzzle, pointing in the muzzle's direction
    protected void SpawnProjectile(Transform muzzle)
    {
        Instantiate(projectile, muzzle.position, muzzle.rotation);
    }
}
