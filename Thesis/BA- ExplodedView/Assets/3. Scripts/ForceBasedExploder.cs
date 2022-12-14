using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Force based solution. Given objects are diplaced using forces to the rigid-bodies.
/// </summary>
public class ForceBasedExploder : MonoBehaviour, IExploder
{
    [SerializeField] List<Transform> parts;
    [SerializeField] List<Rigidbody> partsRB;
    [SerializeField] Transform cameraTrans;
    [SerializeField] List<Vector3> explosionOriginalPos = new List<Vector3>();
    List<Transform> selectedParts = new List<Transform>();
    ExplosionViewHandler explosionViewHandlder;

    [SerializeField] float explosionForceMultiplier = 3f;
    [SerializeField] float returnForce = 3f;
    [SerializeField] float viewingForce = 3f;
    [SerializeField] float spacingForce = 1f;

    public void Explode(float explosionForce)
    {
        selectedParts = explosionViewHandlder.selectedCells;

        for (int i = 0; i < parts.Count; i++)
        {
            if (parts.Count != partsRB.Count)
                break;

            Rigidbody prb = partsRB[i];

            ApplyReturnForce(prb, i);
            
            if (selectedParts.Contains(parts[i]))
                continue;

            foreach(var sp in selectedParts)
            {
                ApplyExplosionForce(prb, sp, selectedParts.Count, explosionForce);
                ApplyViewingForce(prb, sp);
            }
            if(spacingForce != 0f)
                ApplySpacingForce(prb);

        }

    }

    void ApplyReturnForce(Rigidbody prb, int index)
    {
        var retDir = explosionOriginalPos[index] - prb.transform.localPosition;
        var ln = retDir.magnitude == 0f ? 0f : Mathf.Log(retDir.magnitude);

        var retForce = returnForce * ln * retDir.normalized;

        prb.AddRelativeForce(retForce);
    }

    void ApplyExplosionForce(Rigidbody prb, Transform selectedPart, int selectedPartCount, float explosionForce)
    {
        var explDir = prb.transform.localPosition - selectedPart.localPosition;
        var explForce = (explosionForce / Mathf.Exp(explDir.magnitude)) * explDir * (explosionForce / selectedPartCount) * explosionForceMultiplier * 15f;
        //Debug.Log(explForce);
        prb.AddRelativeForce(explForce);
    }

    void ApplyViewingForce(Rigidbody prb, Transform selectedPart)
    {
        if(selectedPart.parent == null)
            return;
        var AB = selectedPart.parent.InverseTransformPoint(cameraTrans.position) - selectedPart.localPosition;
        var AP = prb.transform.localPosition - selectedPart.localPosition; 
        var proj = selectedPart.localPosition + Vector3.Dot(AP, AB) / Vector3.Dot(AB, AB) * AB;
        var forceDir = prb.transform.localPosition - proj;
        var viewForce = viewingForce * forceDir.normalized * (1f/forceDir.magnitude);

        prb.AddRelativeForce(viewForce);

    }

    void ApplySpacingForce(Rigidbody prb)
    {
        foreach(var p in partsRB)
        {
            var r = prb.transform.localPosition - p.transform.localPosition;

            if(r.magnitude != 0f)
            {
                var sForce = (spacingForce / Mathf.Pow(r.magnitude, 2)) * r / partsRB.Count;
                prb.AddRelativeForce(sForce);
            }
        }

    }


    public void GiveObjectsToExploder(List<Transform> objectsToExplode, ExplosionViewHandler viewHandler = null)
    {
        explosionOriginalPos.Clear();
        //parts.Clear();
        partsRB.Clear();

        parts = objectsToExplode;
        parts.ForEach(o => explosionOriginalPos.Add(o.localPosition));
        explosionViewHandlder = viewHandler;
        SetUpParts();
    }

    void SetUpParts()
    {
        parts.ForEach(p => partsRB.Add(p.GetComponent<Rigidbody>()));
        partsRB.ForEach(r => r.isKinematic = false);
    }

    public void OnExplosionForceMultiSliderChange(float val) => explosionForceMultiplier = val;
    public void OnReturnForceSliderChange(float val) => returnForce = val;
    public void OnViewingForceSliderChange(float val) => viewingForce = val;
    public void OnSpacingForceSliderChange(float val) => spacingForce = val;
}
