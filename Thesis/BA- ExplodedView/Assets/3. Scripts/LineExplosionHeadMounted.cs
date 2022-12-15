using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension to LineExplosion to be view dependent. Sets point B to the same transform as the Headset during that frame.
/// </summary>
public class LineExplosionHeadMounted : LineExplosion
{
    [SerializeField] bool stickToHead = true;
    [SerializeField] Transform headsetTransform;

    public override void Explode(float explosionForce)
    {
        if (stickToHead)
        {
            drawLineBetweenPoints = false;
            pointB.gameObject.SetActive(false);
            pointB.position = headsetTransform.position;
        }
        else
        {
            drawLineBetweenPoints = true;
            pointB.gameObject.SetActive(true);
        }


        base.Explode(explosionForce);
    }

    public void OnToggleStickToHead(bool val) => stickToHead = val; 
}
