using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spacecraft Data", menuName = "Spacecraft Data", order = 51)]
public class SpacecraftData : ScriptableObject
{
    [Header("Weapons")]
    [SerializeField] private float maxTargetingAngle;
    [SerializeField] private float maxTargetRange;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float mass;

    [Header("Rotation")]
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float yawSpeed;
    [SerializeField] private float rollSpeed;

    // Weapons
    public float MaxTargetingAngle
    {
        get
        {
            return maxTargetingAngle;
        }
    }
    public float MaxTargetRange
    {
        get
        {
            return maxTargetRange;
        }
    }

    // Movement
    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }
    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }
    public float Mass
    {
        get
        {
            return mass;
        }
    }

    // Rotation
    public float PitchSpeed
    {
        get
        {
            return pitchSpeed;
        }
    }
    public float YawSpeed
    {
        get
        {
            return yawSpeed;
        }
    }
    public float RollSpeed
    {
        get
        {
            return rollSpeed;
        }
    }
}
