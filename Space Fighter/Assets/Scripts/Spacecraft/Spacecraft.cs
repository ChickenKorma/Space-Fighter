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

    protected GameObject target;
    protected GameObject proxyTarget;

    private List<Transform> muzzles = new List<Transform>();

    public enum Weapon
    {
        laserCannon,
        missileLauncher
    }

    protected List<Weapon> availableWeapons = new List<Weapon>();
    protected int currentWeapon;

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

        CheckWeapons();
    }

    // Builds list of available weapons from attached scripts
    // Messy code but this helps speed up creation of new spacecraft
    private void CheckWeapons()
    {
        if(laserCannon != null)
        {
            availableWeapons.Add(Weapon.laserCannon);
        }

        if(missileLauncher != null)
        {
            availableWeapons.Add(Weapon.missileLauncher);
        }
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

    // Set current weapon index to input if possible
    protected void SelectWeapon(int weaponNumber)
    {
        if(weaponNumber >= 0 && weaponNumber < availableWeapons.Count)
        {
            currentWeapon = weaponNumber;
        }       
    }

    // Cycles the current weapon index up or down depending on the input
    protected void CycleWeapon(int direction)
    {
        currentWeapon = (currentWeapon + (direction > 0 ? 1 : -1)) % availableWeapons.Count;
    }

    // Checks the current selected weapon and calls shoot
    protected void Shoot()
    {
        switch (availableWeapons[currentWeapon])
        {
            case Weapon.laserCannon:
                laserCannon.Shoot();
                break;

            case Weapon.missileLauncher:
                missileLauncher.Shoot();
                break;
        }
    }

    // Returns the object from the objects array that has the minimum angle to the forward transform, must be within max target range and target angle
    protected GameObject ObjectNearestForward(List<GameObject> objects)
    {
        GameObject result = null;
        float closestAngle = data.MaxTargetingAngle;

        foreach(GameObject obj in objects)
        {
            (bool isValid, float angle) = isTargetValid(obj);

            if (isValid && (angle <= closestAngle))
            {
                result = obj;

                closestAngle = angle;
            }
        }

        return result;
    }

    // Checks if target is within valid range and angle, returns result and the calculated angle
    protected (bool, float) isTargetValid(GameObject obj)
    {
        Vector3 directionToObject = obj.transform.position - transform.position;

        bool distanceValid = Vector3.SqrMagnitude(directionToObject) <= Mathf.Pow(data.MaxTargetRange, 2);

        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.forward, directionToObject.normalized));
        bool angleValid = angle <= data.MaxTargetingAngle;

        return ((distanceValid && angleValid), angle);
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

    protected void UpdateTargets()
    {
        if (Time.time > lastTargetUpdate + 0.1f)
        {
            proxyTarget = ObjectNearestForward(GameManager.Instance.Enemies);

            if(target != null)
            {
                (bool isValid, float _) = isTargetValid(target);

                if (!isValid)
                {
                    target = null;
                }
            }

            lastTargetUpdate = Time.time;
        }
    }

    protected void LockTarget()
    {
        if(proxyTarget != null)
        {
            target = proxyTarget;
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
