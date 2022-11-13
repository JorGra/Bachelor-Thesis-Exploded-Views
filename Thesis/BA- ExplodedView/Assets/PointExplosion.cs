using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosion : MonoBehaviour, IExploder
{
    [SerializeField] Transform explosionCenter;
    [SerializeField] List<Transform> parts;
    [SerializeField] List<Vector3> explosionOriginalTrans = new List<Vector3>();
    [SerializeField] List<Vector3> explosionTargetPos = new List<Vector3>();
    [Range(0f, 10f)]
    [SerializeField] float maxExplosionForce;
    [SerializeField] float lengthFactor = 1f;

    public void Explode(float explosionForce)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (explosionOriginalTrans.Count != parts.Count || parts[i].parent == null)
                break;

            var origPos = explosionOriginalTrans[i];
            var container = parts[i].parent;

            var explosionDir = container.TransformPoint(origPos) - explosionCenter.position;

            Debug.DrawLine(explosionCenter.position, container.TransformPoint(origPos));

            explosionTargetPos[i] = origPos + explosionDir.normalized * maxExplosionForce + explosionDir * lengthFactor * explosionDir.magnitude;

            parts[i].localPosition = Vector3.Lerp(origPos, explosionTargetPos[i], explosionForce);
        }
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode)
    {
        parts = objectsToExplode;

        parts.ForEach(o => explosionOriginalTrans.Add(o.localPosition));
        parts.ForEach(o => explosionTargetPos.Add(o.localPosition));
    }

}
