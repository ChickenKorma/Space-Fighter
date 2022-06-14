using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : ProjectileWeapon
{
    public Transform[] muzzles;

    [Header("Weapon Variables")]
    [SerializeField] private float shootRate;
    [SerializeField] private float heating;
    [SerializeField] private float cooling;
    [SerializeField] private float maxOperatingTemperature;

    private float lastShot;
    private float temperature;

    public float Temperature
    {
        get { return temperature; }
    }

    public float MaxOperatingTemperature
    {
        get { return maxOperatingTemperature; }
    }

    void Update()
    {
        UpdateTemperature();
    }

    // Checks if temperature is within operating range and spawns laser projectiles
    public void Shoot()
    {
        if(temperature <= maxOperatingTemperature && Time.time > lastShot + shootRate)
        {
            foreach(Transform muzzle in muzzles)
            {
                SpawnProjectile(muzzle);
            } 

            lastShot = Time.time;
            temperature += heating;
        }        
    }

    // Reduces temperature by the cooling stat and clamps above zero
    private void UpdateTemperature()
    {
        temperature = Mathf.Clamp(temperature - (cooling * Time.deltaTime), 0, maxOperatingTemperature * 2);
    }
}
