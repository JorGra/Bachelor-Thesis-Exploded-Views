using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Concrete implementation of IExploder. Translates given Transforms from explosion center away.
/// </summary>
public class PointExplosion : MonoBehaviour, IExploder
{
    [SerializeField] Transform explosionCenter;
    [SerializeField] List<Cell> cells;


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
            if (cells.Count != parts.Count || parts[i].parent == null)
                break;

            var container = parts[i].parent;
            
            var origPos = container.InverseTransformPoint(cells[i].GetCurrentReferencePoint());

            if (!CellManager.Instance.lerpReferencePoint)
                origPos = explosionOriginalPos[i];

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
    /// <summary>
    /// Methods takes transforms that are exploded. Called by ExplosionViewHandler, when this exploder is selected.
    /// </summary>
    /// <param name="objectsToExplode">List of transfroms that are displaced during the explosion</param>
    /// <param name="viewHandler">ExplosionviewHandler that calls this function</param>
    public void GiveObjectsToExploder(List<Transform> objectsToExplode, ExplosionViewHandler viewHandler = null)
    {
        explosionOriginalPos.Clear();
        explosionTargetPos.Clear();
        cells.Clear();

        parts = objectsToExplode;

        parts.ForEach(o => cells.Add(o.GetComponent<Cell>()));
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

    //Methods for UI Interaction. Called from buttons.
    public void OnMaxExplosionForceSliderChange(float val) => maxExplosionForce = val;
    public void OnLengthFactorSliderChange(float val) => lengthFactor = val;
    public void TogglerpReferencePos(bool val) => CellManager.Instance.lerpReferencePoint = val;
}
