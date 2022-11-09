using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExploder 
{
    void GiveObjectsToExplode(List<Transform> objectsToExplode);

    void Explode(float explosionForce);
}
