using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spacecraft : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float acceleration;
    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float velocityDamping;

    protected float velocity;
    private float targetVelocity;

    [Header("Rotation Variables")]
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float yawSpeed;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rotationDamping;

    private Vector3 rotationStep;

    // Updates the velocity based on an input thrust scale
    protected void UpdateVelocity(float thrustScale)
    {
        targetVelocity += thrustScale * acceleration * Time.deltaTime;
        targetVelocity = Mathf.Clamp(targetVelocity, minVelocity, maxVelocity);

        velocity = Mathf.Lerp(velocity, targetVelocity, velocityDamping);
    }

    // Moves game object along the forward vector by the velocity value
    protected void Move()
    {
        transform.position += velocity * transform.forward * Time.deltaTime;
    }

    // Updates the rotation step for this frame by clamping rotation speeds
    protected void UpdateRotation(Vector3 normalizedStep)
    {
        float clampedX = Mathf.Clamp(normalizedStep.x * pitchSpeed, -pitchSpeed, pitchSpeed);
        float clampedY = Mathf.Clamp(normalizedStep.y * yawSpeed, -yawSpeed, yawSpeed);
        float clampedZ = Mathf.Clamp(normalizedStep.z * rollSpeed, -rollSpeed, rollSpeed);

        Vector3 newRotationStep = new Vector3(clampedX, clampedY, clampedZ);
        rotationStep = Vector3.Lerp(rotationStep, newRotationStep, rotationDamping);
    }

    // Rotates game object by the current rotation step
    protected void Rotate()
    {
        transform.Rotate(rotationStep * Time.deltaTime);
    }
}
