using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public static FlightController instance;

    [SerializeField] private float thrust, velocityDamping, maxVelocity, minVelocity, pitchSensitivity, yawSensitivity, rollSensitivity, rotationScale, rotationDamping;

    public float velocity = 0.0f;

    private float targetVelocity = 0.0f;

    private Vector3 targetRotation = new Vector3(0, 0, 0);

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
    }

    void Update()
    {
        // Get rotation from player input and apply to transform
        float rotX = -Input.GetAxis("Mouse Y") * pitchSensitivity * Time.deltaTime * rotationScale;
        float rotY = Input.GetAxis("Mouse X") * yawSensitivity * Time.deltaTime * rotationScale;
        float rotZ = -Input.GetAxis("Horizontal") * rollSensitivity * Time.deltaTime * rotationScale;

        Vector3 rotation = new Vector3(rotX, rotY, rotZ);
        rotation = Vector3.Lerp(new Vector3(0, 0, 0), rotation, rotationDamping);

        transform.Rotate(rotation);


        // Get thrust input from player and apply velocity to transform
        targetVelocity += Input.GetAxis("Vertical") * thrust * Time.deltaTime;
        targetVelocity = Mathf.Clamp(targetVelocity, minVelocity, maxVelocity);

        velocity = Mathf.Lerp(velocity, targetVelocity, velocityDamping);

        transform.position += velocity * transform.forward * Time.deltaTime;
    }
}
