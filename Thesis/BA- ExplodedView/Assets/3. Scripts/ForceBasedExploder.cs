using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBasedExploder : MonoBehaviour, IExploder
{
    [SerializeField] List<Transform> parts;
    [SerializeField] List<Vector3> explosionOriginalPos = new List<Vector3>();
    List<Transform> selectedParts = new List<Transform>();
    ExplosionViewHandler explosionViewHandlder;

    public void Explode(float explosionForce)
    {
        selectedParts = explosionViewHandlder.selectedCells;
    }

    public void GiveObjectsToExploder(List<Transform> objectsToExplode, ExplosionViewHandler viewHandler = null)
    {
        parts = objectsToExplode;
        parts.ForEach(o => explosionOriginalPos.Add(o.localPosition));
        explosionViewHandlder = viewHandler;
    }
}
