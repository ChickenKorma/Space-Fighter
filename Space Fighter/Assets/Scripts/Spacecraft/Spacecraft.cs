using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spacecraft : MonoBehaviour
{
    [SerializeField] private SpacecraftData data;

    [SerializeField] protected LaserCannon laserCannon;
    [SerializeField] protected MissileLauncher missileLauncher;

    protected GameObject target;
    protected GameObject proxyTarget;
    private float lastTargetUpdate;

    protected Vector3 velocity;
    protected float throttle;
    protected float currentSpeed;

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    private Vector3 rotationStep;

    private List<Transform> muzzles = new List<Transform>();

    public enum Weapon
    {
        laserCannon,
        missileLauncher
    }

    protected List<Weapon> availableWeapons = new List<Weapon>();
    protected int currentWeapon;

    [System.Serializable]
    private enum Teams
    {
        Enemy,
        Friendly
    }
    [SerializeField] private Teams spacecraftTeam;

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
        lastTargetUpdate = Time.time;

        switch (spacecraftTeam)
        {
            case Teams.Friendly:
                GameManager.Instance.AddFriendly(gameObject);
                break;

            case Teams.Enemy:
                GameManager.Instance.AddEnemy(gameObject);
                break;
        }

        CheckWeapons();
    }

    protected virtual void Update()
    {
        UpdateTargets();
        UpdateMuzzlePointing();

        Rotate();
        Move();
    }

    private void OnDisable()
    {
        switch (spacecraftTeam)
        {
            case Teams.Friendly:
                GameManager.Instance.RemoveFriendly(gameObject);
                break;

            case Teams.Enemy:
                GameManager.Instance.RemoveEnemy(gameObject);
                break;
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

    // Updates the rotation step for this frame by clamping rotation speeds
    protected void UpdateRotation(Vector3 inputRotationStep)
    {
        float clampedX = Mathf.Clamp(inputRotationStep.x, -data.PitchSpeed, data.PitchSpeed);
        float clampedY = Mathf.Clamp(inputRotationStep.y, -data.YawSpeed, data.YawSpeed);
        float clampedZ = Mathf.Clamp(inputRotationStep.z, -data.RollSpeed, data.RollSpeed);

        rotationStep = new Vector3(clampedX, clampedY, clampedZ);
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

    // Set current weapon index to input if possible
    protected void SelectWeapon(int weaponNumber)
    {
        if (weaponNumber >= 0 && weaponNumber < availableWeapons.Count)
        {
            currentWeapon = weaponNumber;
        }
    }

    // Cycles the current weapon index up or down depending on the input
    protected void CycleWeapon(int direction)
    {
        currentWeapon = (currentWeapon + (direction > 0 ? 1 : -1)) % availableWeapons.Count;
    }

    // Switches target to current proxy target if not null
    protected void LockTarget()
    {
        if (proxyTarget != null)
        {
            target = proxyTarget;
        }
    }

    // Builds list of available weapons from attached scripts
    private void CheckWeapons()
    {
        if (laserCannon != null)
        {
            availableWeapons.Add(Weapon.laserCannon);
        }

        if (missileLauncher != null)
        {
            availableWeapons.Add(Weapon.missileLauncher);
        }
    }

    // Moves game object along the forward vector by the velocity value
    private void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    // Rotates game object by the current rotation step
    private void Rotate()
    {

        transform.Rotate(rotationStep);
    }

    // Rotates muzzles to set the weapon range to the distance of the current target
    private void UpdateMuzzlePointing()
    {
        if (target != null)
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

    // Every 1/10 second, update the proxy target to the nearest forward enemy, also check if current target is still valid
    private void UpdateTargets()
    {
        if (Time.time > lastTargetUpdate + 0.1f)
        {
            switch (spacecraftTeam)
            {
                case Teams.Friendly:
                    proxyTarget = ObjectNearestForward(GameManager.Instance.Enemies);
                    break;

                case Teams.Enemy:
                    proxyTarget = ObjectNearestForward(GameManager.Instance.Friendlies);
                    break;
            }

            if (target != null)
            {
                (bool isValid, float _) = IsTargetValid(target);

                if (!isValid)
                {
                    target = null;
                }
            }

            lastTargetUpdate = Time.time;
        }
    }

    // Returns the object from the objects array that has the minimum angle to the forward transform, must be within max target range and target angle
    private GameObject ObjectNearestForward(List<GameObject> objects)
    {
        GameObject result = null;
        float closestAngle = data.MaxTargetingAngle;

        foreach (GameObject obj in objects)
        {
            (bool isValid, float angle) = IsTargetValid(obj);

            if (isValid && (angle <= closestAngle))
            {
                result = obj;

                closestAngle = angle;
            }
        }

        return result;
    }

    // Checks if target is within valid range and angle, returns result and the calculated angle
    private (bool, float) IsTargetValid(GameObject obj)
    {
        Vector3 directionToObject = obj.transform.position - transform.position;

        bool distanceValid = Vector3.SqrMagnitude(directionToObject) <= Mathf.Pow(data.MaxTargetRange, 2);

        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.forward, directionToObject.normalized));
        bool angleValid = angle <= data.MaxTargetingAngle;

        return ((distanceValid && angleValid), angle);
    }
}
