using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Changes the used exploder and stores the Transforms that should be exploded. 
/// </summary>
public class ExplosionViewHandler : MonoBehaviour
{

    [SerializeField] GameObject exploderObject;
    public List<Transform> objectsToExplode;
    public List<Transform> originalTrans;
    public List<Transform> selectedCells;
    public bool useFixedUpdate = false;
    [SerializeField] GameObject originalPositionPrefab;
    [SerializeField] bool showOriginalPosition = true;
    [Range(0f, 1f)]
    [SerializeField] float currentExplosionForce;

    IExploder exploder;

    // Start is called before the first frame update
    void Start()
    {
        var cellManager = GetComponent<CellManager>();
        cellManager.cells.ForEach(c => objectsToExplode.Add(c.transform));

        foreach (var cell in cellManager.cells)
        {
            var t = Instantiate(originalPositionPrefab, cell.transform.position, Quaternion.identity).transform;
            cell.originalTransformObject = t.gameObject;
            t.parent = cellManager.cellContainer;
            originalTrans.Add(t);
        }
        
        ChangeExploder(exploderObject);
    }

    private void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            showOriginalPosition = !showOriginalPosition;
            ToggleOriginalPositionVisibility(showOriginalPosition);
        }
        if(!useFixedUpdate)
            exploder.Explode(currentExplosionForce);
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
            exploder.Explode(currentExplosionForce);
    }


    public void ToggleOriginalPositionVisibility(bool visible) => originalTrans.ForEach(o => o.GetChild(0).gameObject.SetActive(visible));

    public void OnExplosionForceSliderChange(float value) => currentExplosionForce = value;

    public void ChangeExploder(GameObject newExploderObject)
    {
        exploderObject = newExploderObject;
        for (int i = 0; i < objectsToExplode.Count; i++)
        {
            objectsToExplode[i].localPosition = originalTrans[i].localPosition;
        }
        exploder = exploderObject.GetComponent<IExploder>();
        exploder.GiveObjectsToExploder(objectsToExplode, this);
    }

    public void AddCellToSelection(Cell cell) => selectedCells.Add(cell.gameObject.transform);

}
