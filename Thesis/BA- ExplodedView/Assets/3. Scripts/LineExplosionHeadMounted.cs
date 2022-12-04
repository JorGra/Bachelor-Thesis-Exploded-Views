using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
