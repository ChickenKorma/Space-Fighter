using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : Projectile
{
    // Applies damage to the object hit by laser bolt
    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other.transform);

        DestroyProjectile();
    }
}
