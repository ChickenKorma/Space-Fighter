using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spacecraft : MonoBehaviour
{
    [SerializeField] private SpacecraftData data;

    [SerializeField] protected LaserCannon laserCannon;
    [SerializeField] protected MissileLauncher missileLauncher;

    protected float throttle;
    protected Vector3 velocity;
    protected float currentSpeed;

    private Vector3 rotationStep;

    [SerializeField] protected GameObject target;

    private List<Transform> muzzles = new List<Transform>();

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    protected virtual void Start()
    {
        if (laserCannon != null)
        {
            foreach (Transform laserMuzzle in laserCannon.muzzles)
            {
                muzzles.Add(laserMuzzle);
            }
        }

        target = null;
    }

    // Updates the velocity based on an input thrust scale
    protected void UpdateVelocity(float throttleChange)
    {
        throttle = Mathf.Clamp(throttle + (throttleChange * Time.deltaTime), -1, 1);

        float targetSpeed = throttle * data.MaxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, data.Acceleration);

        Vector3 velocityDirection = Vector3.Lerp(velocity, transform.forward * (currentSpeed >= 0 ? 1 : -1), 1 - data.Mass).normalized;

        velocity = Mathf.Abs(currentSpeed) * velocityDirection;
    }

    // Moves game object along the forward vector by the velocity value
    protected void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    // Updates the rotation step for this frame by clamping rotation speeds
    protected void UpdateRotation(Vector3 inputRotationStep)
    {
        float clampedX = Mathf.Clamp(inputRotationStep.x, -data.PitchSpeed, data.PitchSpeed);
        float clampedY = Mathf.Clamp(inputRotationStep.y, -data.YawSpeed, data.YawSpeed);
        float clampedZ = Mathf.Clamp(inputRotationStep.z, -data.RollSpeed, data.RollSpeed);

        Vector3 newRotationStep = new Vector3(clampedX, clampedY, clampedZ);
        //rotationStep = Vector3.Lerp(rotationStep, newRotationStep, data.RotationDamping);
        rotationStep = newRotationStep;
    }

    // Rotates game object by the current rotation step
    protected void Rotate()
    {

        transform.Rotate(rotationStep);
    }

    // Rotates muzzles to set the weapon range to the distance of the current target
    protected void UpdateMuzzlePointing()
    {
        if(target != null)
        {
            float targetDistance = (target.transform.position - transform.position).magnitude + 3;

            Vector3 aimPoint = transform.position + (transform.forward * targetDistance);

            foreach (Transform muzzle in muzzles)
            {
                muzzle.LookAt(aimPoint);
            }
        }
        else
        {
            foreach (Transform muzzle in muzzles)
            {
                muzzle.localRotation = Quaternion.identity;
            }
        }
    }

    // Checks if spacecraft has laser cannon object and calls shoot
    protected void ShootLaserCannon()
    {
        if(laserCannon != null)
        {
            laserCannon.Shoot();
        }   
    }

    // Checks if spacecraft has missile launcher object and calls shoot
    protected void ShootMissileLauncher()
    {
        if (missileLauncher != null)
        {
            missileLauncher.Shoot();
        }
    }

    // Returns the object from the objects array that has the minimum angle to the forward transform, must be within max target range and target angle
    protected GameObject ObjectNearestForward(List<GameObject> objects)
    {
        GameObject result = null;
        float closestAngle = data.MaxTargetingAngle;

        foreach(GameObject obj in objects)
        {
            Vector3 directionToObject = obj.transform.position - transform.position;

            if(directionToObject.magnitude <= data.MaxTargetRange)
            {
                float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.forward, directionToObject.normalized));

                if (angle <= closestAngle && angle <= data.MaxTargetingAngle)
                {
                    result = obj;

                    closestAngle = angle;
                }
            }
        }

        return result;
    }
}

public class FriendlySpacecraft : Spacecraft
{
    private float lastTargetUpdate;

    protected override void Start()
    {
        base.Start();

        lastTargetUpdate = Time.time;

        GameManager.Instance.AddFriendly(gameObject);
    }

    // Finds enemy nearest the forward direction
    protected void UpdateTarget()
    {
        if(Time.time > lastTargetUpdate + 0.1f)
        {
            target = ObjectNearestForward(GameManager.Instance.Enemies);

            lastTargetUpdate = Time.time;
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveFriendly(gameObject);
    }
}

public class EnemySpacecraft : Spacecraft
{
    private float lastTargetUpdate;

    protected override void Start()
    {
        base.Start();

        lastTargetUpdate = Time.time;

        GameManager.Instance.AddEnemy(gameObject);
    }

    // Finds friendly nearest the forward direction
    protected void UpdateTarget()
    {
        if (Time.time > lastTargetUpdate + 0.1f)
        {
            target = ObjectNearestForward(GameManager.Instance.Friendlies);
            
            lastTargetUpdate = Time.time;
        }       
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveEnemy(gameObject);
    }
}
