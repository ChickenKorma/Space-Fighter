using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : EnemySpacecraft
{
    void Update()
    {
        UpdateVelocity(1 / Time.deltaTime);

        Move();
    }
}
