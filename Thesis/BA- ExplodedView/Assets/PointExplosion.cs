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
    [SerializeField] float lengthFactor = 1f;

    [SerializeField] bool drawLines;
    LineDrawer lineDrawer;

    public void Explode(float explosionForce)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (explosionOriginalPos.Count != parts.Count || parts[i].parent == null)
                break;

            var origPos = explosionOriginalPos[i];
            var container = parts[i].parent;

            var explosionDir = origPos - container.InverseTransformPoint(explosionCenter.position);


            explosionTargetPos[i] = origPos + explosionDir.normalized * maxExplosionForce + explosionDir * lengthFactor * explosionDir.magnitude;


            if (drawLines)
            {
                Debug.DrawLine(explosionCenter.position, container.TransformPoint(origPos));
                lineDrawer.UpdatePositions(i, explosionCenter.position, container.TransformPoint(origPos));

                Debug.DrawLine(container.TransformPoint(origPos), container.TransformPoint(explosionTargetPos[i]), Color.green);
                lineDrawer.UpdatePositions(parts.Count + i, container.TransformPoint(origPos), parts[i].position);
            }


            parts[i].localPosition = Vector3.Lerp(origPos, explosionTargetPos[i], explosionForce);
        }
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode)
    {
        parts = objectsToExplode;

        parts.ForEach(o => explosionOriginalPos.Add(o.localPosition));
        parts.ForEach(o => explosionTargetPos.Add(o.localPosition));

        lineDrawer = GetComponent<LineDrawer>();
        lineDrawer.ClearContainer();

        if (drawLines)
        {
            parts.ForEach(o => lineDrawer.GenerateLine(Color.white));
            parts.ForEach(o => lineDrawer.GenerateLine(Color.green));
        }
    }

}
