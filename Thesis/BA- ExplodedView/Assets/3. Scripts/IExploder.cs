using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExploder 
{
    /// <summary>
    /// Methods takes transforms that are exploded. Called by ExplosionViewHandler, when this exploder is selected.
    /// </summary>
    /// <param name="objectsToExplode">List of transfroms that are displaced during the explosion</param>
    /// <param name="viewHandler">ExplosionviewHandler that calls this function</param>
    void GiveObjectsToExploder(List<Transform> objectsToExplode, ExplosionViewHandler viewHandler = null);

    /// <summary>
    /// Called every frame, with explosion force set in UI.
    /// </summary>
    void Explode(float explosionForce);

}
