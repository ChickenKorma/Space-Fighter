using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    // Public variables not serialized
    public static FlightController instance;

    [System.NonSerialized] public Vector3 mouseRelative;

    [System.NonSerialized] public float velocity;

    // Private variables set in inspector
    [SerializeField] private float pitchSensitivity, yawSensitivity, pitchYawScale, rollSensitivity, maxRotation, rotationDamping, deadZone;
    [SerializeField] private float thrust, velocityDamping, minVelocity, maxVelocity;

    // Private variables
    private Vector3 screenCenter;

    private float targetVelocity = 0.0f;
    private float rotX, rotY, rotZ;

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
        // Lock cursor to game window and hide
        Cursor.lockState = CursorLockMode.Locked;

        // Find screen size and set center
        screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
    }

    void Update()
    {
        mouseRelative = Input.mousePosition - screenCenter;

        float newRotX = 0.0f, newRotY = 0.0f;

        // Apply a dead zone to the mouse position and clamp the rotation intensity
        if(mouseRelative.y >= deadZone)
        {
            float rotationAmount = Mathf.Clamp(mouseRelative.y - deadZone, 0.0f, maxRotation);
            newRotX = -rotationAmount * pitchSensitivity * Time.deltaTime * pitchYawScale;
        }
        else if(mouseRelative.y <= -deadZone)
        {
            float rotationAmount = Mathf.Clamp(mouseRelative.y + deadZone, -maxRotation, 0.0f);
            newRotX = -rotationAmount * pitchSensitivity * Time.deltaTime * pitchYawScale;
        }

        if (mouseRelative.x >= deadZone)
        {
            float rotationAmount = Mathf.Clamp(mouseRelative.x - deadZone, 0.0f, maxRotation);
            newRotY =  rotationAmount * yawSensitivity * Time.deltaTime * pitchYawScale;
        }
        else if (mouseRelative.x <= -deadZone)
        {
            float rotationAmount = Mathf.Clamp(mouseRelative.x - deadZone, -maxRotation, 0.0f);
            newRotY = rotationAmount * yawSensitivity * Time.deltaTime * pitchYawScale;
        }

        // Get roll amount from keyboard input
        float newRotZ = -Input.GetAxis("Horizontal") * rollSensitivity * Time.deltaTime;

        // Smooth rotation values from previous frame and apply rotations
        rotX = Mathf.Lerp(rotX, newRotX, rotationDamping);
        rotY = Mathf.Lerp(rotY, newRotY, rotationDamping);
        rotZ = Mathf.Lerp(rotZ, newRotZ, rotationDamping);

        Vector3 rotation = new Vector3(rotX, rotY, rotZ);
        transform.Rotate(rotation);

        // Get thrust amount from keyboard input and apply to target velocity
        targetVelocity += Input.GetAxis("Vertical") * thrust * Time.deltaTime;
        targetVelocity = Mathf.Clamp(targetVelocity, minVelocity, maxVelocity);

        // Smooth velocity from previous frame and apply to position
        velocity = Mathf.Lerp(velocity, targetVelocity, velocityDamping);
        transform.position += velocity * transform.forward * Time.deltaTime;
    }
}
