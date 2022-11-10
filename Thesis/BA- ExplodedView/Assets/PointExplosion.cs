using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosion : MonoBehaviour, IExploder
{
    [SerializeField] Transform explosionCenter;
    [SerializeField] List<Transform> parts;
    [SerializeField] List<Vector3> explosionOriginalPos = new List<Vector3>();
    [SerializeField] List<Vector3> explosionTargetPos = new List<Vector3>();
    [Range(0f, 10f)]
    [SerializeField] float maxExplosionForce;

    public void Explode(float explosionForce)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            var explosionDir = explosionOriginalPos[i] - explosionCenter.position; //might need to be normalized
            explosionTargetPos[i] = explosionDir * maxExplosionForce;

            parts[i].position = Vector3.Lerp(explosionOriginalPos[i], explosionTargetPos[i], explosionForce);
        }
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode)
    {
        parts = objectsToExplode;

        parts.ForEach(o => explosionOriginalPos.Add(o.position));
        parts.ForEach(o => explosionTargetPos.Add(o.position));
    }
}
