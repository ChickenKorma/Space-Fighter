using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictingMissile : GuidedMissile
{
    [SerializeField] private float maxPredictionTime;

    protected override void Update()
    {
        if (target != null)
        {
            PointTowards(PredictedPosition());
        }

        base.Update();
    }

    private Vector3 PredictedPosition()
    {
        Vector3 predictedPosition = target.position;

        Spacecraft targetSpacecraft = target.GetComponent<Spacecraft>();

        if (targetSpacecraft != null)
        {
            Vector3 targetDirection = target.position - transform.position;

            float predictionTime = Mathf.Clamp(targetDirection.magnitude / speed, 0, maxPredictionTime);

            predictedPosition = target.position + (targetSpacecraft.Velocity * predictionTime * Time.deltaTime);
        }

        return predictedPosition;
    }
}
