using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpacecraft : FriendlySpacecraft
{
    [HideInInspector] public static PlayerSpacecraft Instance;

    [Header("Handling Variables")]
    [SerializeField] private float aimSensitivity;
    [SerializeField] private bool pitchInverted;

    public float Speed
    {
        get { return currentSpeed; }
    }

    public float Throttle
    {
        get { return throttle; }
    }

    public GameObject Target
    {
        get
        {
            return target;
        }
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

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        UpdateTarget();
        UpdateMuzzlePointing();

        UpdateInputs();     
        Rotate();
        Move();
    }

    // Gets inputs from player, updates the velocity and rotation step on spacecraft class and fires weapons
    private void UpdateInputs()
    {
        Vector3 rotationStep = new Vector3(Input.GetAxis("Mouse Y") * aimSensitivity * (pitchInverted ? 1 : -1), Input.GetAxis("Mouse X") * aimSensitivity, -Input.GetAxis("Horizontal"));
        UpdateRotation(rotationStep);

        UpdateVelocity(Input.GetAxis("Vertical"));

        if (Input.GetMouseButton(0))
        {
            ShootLaserCannon();
        }

        if (Input.GetMouseButton(1))
        {
            ShootMissileLauncher();
        }
    }
}
