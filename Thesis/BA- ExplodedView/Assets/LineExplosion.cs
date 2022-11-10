using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineExplosion : MonoBehaviour, IExploder
{
    [SerializeField] List<Transform> parts;
    [SerializeField] List<Vector3> explosionOriginalPos = new List<Vector3>();
    [SerializeField] List<Vector3> explosionTargetPos = new List<Vector3>();
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float minOffset = 2f;
    [SerializeField] float distanceFactor = 1f;
    [SerializeField] bool drawDebugLines = true;

    public void Explode(float explosionForce)
    {
        DrawLine();

        pointB.LookAt(pointA);

        var AB = pointB.position - pointA.position;

        //Project parts onto line vector and transform them away
        for (int i = 0; i < parts.Count; i++)
        {

            var AP = parts[i].position - pointA.position;
            var proj = pointA.position + Vector3.Dot(AP, AB) / Vector3.Dot(AB, AB) * AB;

            if(drawDebugLines)
                Debug.DrawLine(parts[i].position, proj, Color.green);

            var expDir = parts[i].position - proj;

            var aProj = proj - pointA.position;

            if (Vector3.Dot(AB, aProj) > 0.99f)
            {
                explosionTargetPos[i] = explosionOriginalPos[i] + expDir.normalized * minOffset
                    + aProj.magnitude * distanceFactor * expDir.normalized;
            }
            else
                explosionTargetPos[i] = explosionOriginalPos[i];

            parts[i].position = Vector3.Lerp(explosionOriginalPos[i], explosionTargetPos[i], explosionForce);

        }
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode)
    {
        parts = objectsToExplode;

        parts.ForEach(o => explosionOriginalPos.Add(o.position));
        parts.ForEach(o => explosionTargetPos.Add(o.position));

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    void DrawLine()
    {
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointB.position);
    }
}
