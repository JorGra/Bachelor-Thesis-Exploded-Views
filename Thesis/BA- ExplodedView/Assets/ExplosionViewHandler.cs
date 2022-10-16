using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionViewHandler : MonoBehaviour
{

    [SerializeField] Transform explosionCenter;
    [SerializeField] List<Transform> objectsToExplode;
    [SerializeField] List<Vector3> explosionOriginalPos = new List<Vector3>();
    [SerializeField] List<Vector3> explosionTargetPos = new List<Vector3>();
    [Range(0f, 10f)]
    [SerializeField] float maxExplosionForce;
    [Range(0f, 1f)]
    [SerializeField] float currentExplosionForce;


    // Start is called before the first frame update
    void Start()
    {
        var cellManager = GetComponent<CellManager>();
        cellManager.cells.ForEach(c => objectsToExplode.Add(c.transform));
        objectsToExplode.ForEach(o => explosionOriginalPos.Add(o.position));
        objectsToExplode.ForEach(o => explosionTargetPos.Add(o.position));
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < objectsToExplode.Count; i++)
        {
            var explosionDir = explosionOriginalPos[i] - explosionCenter.position; //might need to be normalized
            explosionTargetPos[i] = explosionDir * maxExplosionForce;

            objectsToExplode[i].position = Vector3.Lerp(explosionOriginalPos[i], explosionTargetPos[i], currentExplosionForce);
        }
    }
}
