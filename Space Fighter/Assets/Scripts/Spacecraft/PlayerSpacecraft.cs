using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpacecraft : Spacecraft
{
    [HideInInspector] public static PlayerSpacecraft Instance;

    [Header("Handling Variables")]
    [SerializeField] private float aimSensitivity;
    [SerializeField] private bool pitchInverted;

    public GameObject Target
    {
        get
        {
            return target;
        }
    }
    public GameObject ProxyTarget
    {
        get { return proxyTarget; }
    }

    public float Speed
    {
        get { return currentSpeed; }
    }
    public float Throttle
    {
        get { return throttle; }
    }

    public Weapon CurrentWeapon
    {
        get { return availableWeapons[currentWeapon]; }
    }
    public LaserCannon LaserCannon
    {
        get { return laserCannon; }
    }
    public MissileLauncher MissileLauncher
    {
        get { return missileLauncher; }
    }

    private void Awake()
    {
        // Create Singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        UpdateInputs();

        base.Update();
    }

    // Gets inputs from player, updates the velocity and rotation step on spacecraft class and fires weapons
    private void UpdateInputs()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.GetMouseButton(1))
        {
            LockTarget();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectWeapon(2);
        }

        Vector3 rotationStep = new Vector3(Input.GetAxis("Mouse Y") * aimSensitivity * (pitchInverted ? 1 : -1), Input.GetAxis("Mouse X") * aimSensitivity, -Input.GetAxis("Horizontal"));
        UpdateRotation(rotationStep);

        UpdateVelocity(Input.GetAxis("Vertical"));      
    }
}
