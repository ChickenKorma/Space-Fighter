using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    [SerializeField] private float damageRange;

    private void OnEnable()
    {
        GameManager.Instance.AddMissile(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRange);

        foreach(Collider hit in hits)
        {
            TryDamage(hit.transform);
        }

        DestroyProjectile();
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveMissile(gameObject);
    }
}

public class GuidedMissile : Missile
{
    [SerializeField] private float maxAngularRate;
    [SerializeField] private float maxLockingAngle;

    protected Transform target;

    protected override void Start()
    {
        base.Start();

        GameObject targetObject = PlayerSpacecraft.Instance.Target;

        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }

    protected void PointTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;

        float angleToTarget = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.forward, directionToTarget.normalized));

        if (angleToTarget > maxLockingAngle)
        {
            target = null;
            
            return;
        }

        float turnAngle = Mathf.Clamp(angleToTarget, -maxAngularRate, maxAngularRate);

        Vector3 normal = Vector3.Cross(transform.forward, directionToTarget.normalized);

        transform.RotateAround(transform.position, normal, turnAngle * Time.deltaTime);
    }
}
