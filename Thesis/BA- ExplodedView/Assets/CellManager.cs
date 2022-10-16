using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] bool animated;
    [Range(0.05f, 2f)]
    [SerializeField] float animationSpeed;
    [Header("Cells")]
    [SerializeField] GameObject cellPrefab;
    [SerializeField] Transform cellContainer;
    [SerializeField] Vector3 gridOffset;
    public List<Cell> cells = new List<Cell>();
    
    private static CellManager _instance;

    public static CellManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AdvanceTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AdvanceTime()
    {
        while (true)
        {

            if (animated)
            {
                foreach (var cell in cells)
                {
                    cell.AdvanceTime();
                }
            }
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    public void AddCell(int id, Vector3Int[] voxelPos, Vector3 center,int timeStep)
    {

        var cell = cells.FirstOrDefault(it => id == it.id);
        if (cell == null)
        {
            var newCellObj = Instantiate(cellPrefab, cellContainer);

            cell = newCellObj.GetComponent<Cell>();
            cell.id = id;
            cell.GridOffset = gridOffset;
            cell.cellColor = Random.ColorHSV();
            cells.Add(cell);
        }

        cell.centers.Add(center);
        cell.AddTimeStep(voxelPos, center, timeStep);


    }
}
