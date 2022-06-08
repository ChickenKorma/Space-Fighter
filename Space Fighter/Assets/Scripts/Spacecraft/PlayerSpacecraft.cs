using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpacecraft : Spacecraft
{
    [HideInInspector] public static PlayerSpacecraft instance;

    private Vector3 mouseRelative;
    public Vector3 MouseRelative 
    {
        get { return mouseRelative; } 
    }

    private Vector3 screenCenter;

    public float Velocity 
    {
        get { return velocity; }
    }

    [Header("Handling Variables")]
    [SerializeField] private float pitchSensitivity;
    [SerializeField] private float yawSensitivity;
    [SerializeField] private float pitchYawScale;
    [SerializeField] private float deadZone;
    [SerializeField] private float controlZone;

    [SerializeField] private bool pitchInverted;

    private void Awake()
    {
        // Create Singleton instance
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        // Find screen size and set center
        screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
    }

    void Update()
    {
        UpdateInputs();     

        Rotate();
        Move();
    }

    // Gets inputs from player and updates the velocity and rotation step on spacecraft class
    private void UpdateInputs()
    {
        mouseRelative = ApplyZones(Input.mousePosition - screenCenter);

        Vector3 rotationStep = new Vector3(mouseRelative.y * (pitchInverted ? 1 : -1), mouseRelative.x, -Input.GetAxis("Horizontal"));
        UpdateRotation(rotationStep);

        UpdateVelocity(Input.GetAxis("Vertical"));
    }

    // Clears position to zero if it is within dead zone, otherwise returns direction with corrected scale
    private Vector3 ApplyZones(Vector3 pixelPosition)
    {
        float distanceToCenter = pixelPosition.magnitude;

        if (distanceToCenter > deadZone)
        {
            float scale = Mathf.Clamp((distanceToCenter - deadZone) / controlZone, 0.0f, 1.0f);

            return pixelPosition.normalized * scale;
        }

        return Vector3.zero;
    }
}
