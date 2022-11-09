using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionViewHandler : MonoBehaviour
{

    [SerializeField] Transform explosionCenter;
    [SerializeField] GameObject exploderObject;
    [SerializeField] List<Transform> objectsToExplode;
    [Range(0f, 10f)]
    [SerializeField] float maxExplosionForce;
    [Range(0f, 1f)]
    [SerializeField] float currentExplosionForce;

    IExploder exploder;

    // Start is called before the first frame update
    void Start()
    {
        var cellManager = GetComponent<CellManager>();
        cellManager.cells.ForEach(c => objectsToExplode.Add(c.transform));

        ChangeExploder(exploderObject);

    }

    private void Update()
    {
        exploder.Explode(currentExplosionForce);
    }


    public void OnExplosionForceSliderChange(float value)
    {
        currentExplosionForce = value;
    }

    public void ChangeExploder(GameObject exploderObject)
    {
        exploder = exploderObject.GetComponent<IExploder>();
        exploder.GiveObjectsToExplode(objectsToExplode);
    }
}
