using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingMissile : GuidedMissile
{
    protected override void Update()
    {
        if (target != null)
        {
            PointTowards(target.position);
        }

        base.Update();
    }
}
