using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExploder 
{
    void GiveObjectsToExploder(List<Transform> objectsToExplode, ExplosionViewHandler viewHandler = null);

    void Explode(float explosionForce);

}
