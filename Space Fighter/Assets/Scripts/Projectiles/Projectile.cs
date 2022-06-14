using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] private float lifetime;

    private float endTime;

    protected virtual void Start()
    {
        endTime = Time.time + lifetime;
    }

    protected virtual void Update()
    {
        if(Time.time >= endTime)
        {
            DestroyProjectile();
        }

        Move();
    }

    // Moves the projectile forward by the speed stat
    protected void Move()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
    }

    // Checks if the input object has a destructible component and applies projectile damage
    protected void TryDamage(Transform target)
    {
        Destructible destructible = target.GetComponent<Destructible>();

        if (destructible != null)
        {
            destructible.ApplyDamage(damage);
        }
    }

    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
