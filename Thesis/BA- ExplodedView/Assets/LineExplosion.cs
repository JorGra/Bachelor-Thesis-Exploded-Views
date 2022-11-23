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
    [SerializeField] protected Transform pointB;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float minOffset = 2f;
    [SerializeField] float distanceFactor = 1f;
    [SerializeField] bool drawDebugLines = true;
    [SerializeField] bool drawLines = true;

    protected bool drawLineBetweenPoints { get; set; } = true;

    LineDrawer lineDrawer;


    public virtual void Explode(float explosionForce)
    {
        DrawLine();

        pointB.LookAt(pointA);

        var AB = pointB.position - pointA.position;

        //Project parts onto line vector and transform them away
        for (int i = 0; i < parts.Count; i++)
        {
            if (parts[i].parent == null)
                break;

            var container = parts[i].parent;

            var ABloc = container.InverseTransformDirection(AB);

            var pA = container.transform.InverseTransformPoint(pointA.position);
            var AP = parts[i].localPosition - pA;
            var proj = pA + Vector3.Dot(AP, ABloc) / Vector3.Dot(ABloc, ABloc) * ABloc;

            if (drawDebugLines)
            {
                Debug.DrawLine(container.transform.TransformPoint(parts[i].localPosition), container.transform.TransformPoint(proj), Color.green);
            }
            else if(drawLines)
                lineDrawer.UpdatePositions(i, container.transform.TransformPoint(parts[i].localPosition), container.transform.TransformPoint(proj));

            var expDir = parts[i].localPosition - proj;

            var aProj = proj - pA;

            if (Vector3.Dot(AB, aProj) > 0.99f)
            {
                explosionTargetPos[i] = explosionOriginalPos[i] + expDir.normalized * minOffset
                    + aProj.magnitude * distanceFactor * expDir.normalized;
            }
            else
                explosionTargetPos[i] = explosionOriginalPos[i];

            parts[i].localPosition = Vector3.Lerp(explosionOriginalPos[i], explosionTargetPos[i], explosionForce);

        }
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode)
    {
        parts = objectsToExplode;

        parts.ForEach(o => explosionOriginalPos.Add(o.localPosition));
        parts.ForEach(o => explosionTargetPos.Add(o.localPosition));

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineDrawer = GetComponent<LineDrawer>();
        lineDrawer.ClearContainer();

        if (drawLines)
        {
            parts.ForEach(o => lineDrawer.GenerateLine(Color.green));
        }

    }


    void DrawLine()
    {
        if (drawLineBetweenPoints)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, pointA.position);
            lineRenderer.SetPosition(1, pointB.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
