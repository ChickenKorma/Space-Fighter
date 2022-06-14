using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float maxHealth;

    private float health;

    void Start()
    {
        health = maxHealth;
    }

    // Reduces health by damage and destroys game object if needed
    public void ApplyDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
