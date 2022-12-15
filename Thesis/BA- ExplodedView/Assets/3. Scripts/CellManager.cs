using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


/// <summary>
/// Main manager class. Contains important properties, adds new cells and drives animation. 
/// </summary>
public class CellManager : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] bool animated;
    public int maxTimeStep;
    [SerializeField] uint currentTimeStep;
    [SerializeField] int previousTimeStepsShown;
    [SerializeField] float previousTimeStepsOpacityFactor = 1f;
    [Range(0.05f, 2f)]
    public float animationSpeed;
    public bool lerpReferencePoint = false;

    [Header("Cells")]
    [SerializeField] GameObject cellPrefab;
    public Transform cellContainer;
    [SerializeField] Vector3 gridOffset;
    [SerializeField] float voxelSize = 0.1f;
    public List<Cell> cells = new List<Cell>();

    [SerializeField] float minContainerSize = 0.1f;
    [SerializeField] float maxContainerSize = 1.5f;

    [SerializeField] List<Population> populations = new List<Population>();

    [SerializeField] GameObject[] controlObjects;
    [SerializeField] TMP_Text pauseText;

    public delegate void OnSizeChange();
    public OnSizeChange onSizeChanged;
    
    [HideInInspector] public Cell currentlySelectedCell;
    [SerializeField] private TMP_Text cellIDText;
    [SerializeField] private TMP_Text cellPopulationText;

    private static CellManager _instance;

    float currentSize = 1f;

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
        var keyb = Keyboard.current;
        if (keyb.upArrowKey.wasPressedThisFrame)
        {
            currentSize += 0.05f;
            OnSizeSliderChange(Mathf.Clamp01(currentSize));
        }
        else if (keyb.downArrowKey.wasPressedThisFrame)
        {

            currentSize -= 0.05f;
            OnSizeSliderChange(Mathf.Clamp01(currentSize));
        }

    }

    /// <summary>
    /// Advances time on all cells
    /// </summary>
    /// <returns></returns>
    IEnumerator AdvanceTime()
    {
        while (true)
        {

            if (animated)
            {
                foreach (var cell in cells)
                {
                    //cell.AdvanceTime();
                    cell.ShowTimeStep(currentTimeStep, previousTimeStepsShown, previousTimeStepsOpacityFactor);
                }
                currentTimeStep++;

                if (currentTimeStep >= maxTimeStep)
                    currentTimeStep = 0;
            }
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    /// <summary>
    /// Adds a new cell object to container. Called from XML loader
    /// </summary>
    /// <param name="id"></param>
    /// <param name="voxelPos"></param>
    /// <param name="center"></param>
    /// <param name="timeStep"></param>
    /// <param name="population"></param>
    public void AddCell(int id, Vector3Int[] voxelPos, Vector3 center, int timeStep, string population)
    {

        var cell = cells.FirstOrDefault(it => id == it.id && it.population == population);

        var scaledCenter = center * voxelSize;

        if (cell == null)
        {
            var newCellObj = Instantiate(cellPrefab, scaledCenter, Quaternion.identity, cellContainer);

            cell = newCellObj.GetComponent<Cell>();
            cell.id = id;
            cell.GridOffset = gridOffset;
            cell.cellColor = GetOrAddPopulation(population).color;
            cell.population = population;
            cells.Add(cell);
        }

        cell.centers.Add(scaledCenter);
        cell.AddTimeStep(voxelPos, center, timeStep, voxelSize);


    }

    /// <summary>
    /// Adds new population and gives it a random color or returns it if it already exists. 
    /// </summary>
    /// <param name="name">name of population</param>
    /// <returns></returns>
    Population GetOrAddPopulation(string name)
    {
        var pop = populations.FirstOrDefault(p => p.name == name);

        if(pop == null)
        {
            Debug.Log("Adding new Population: " + name);
            pop = new Population(name, Color.HSVToRGB(Random.Range(0f, 360f) / 360f, 0.9f, 0.7f));
            populations.Add(pop);
        }
        return pop;
    }


    public void OnSizeSliderChange(float value)
    {
        float dist = maxContainerSize - minContainerSize;
        var val = minContainerSize + (dist * value);

        cellContainer.transform.localScale = Vector3.one * val;

        foreach (var item in controlObjects)
        {
            item.transform.localScale = Vector3.one * val;
        }
    }

    public void OnButtonTimePause()
    {
        animated = !animated;

        if (animated)
            pauseText.text = "II";
        else
            pauseText.text = ">";
    }

    public void OnButtonTimeForward()
    {
        currentTimeStep++;

        if (currentTimeStep >= maxTimeStep)
            currentTimeStep = 0;

        foreach (var cell in cells)
        {
            cell.ShowTimeStep(currentTimeStep, previousTimeStepsShown, previousTimeStepsOpacityFactor);
        }
    }

    public void OnButtonTimeBackward()
    {
        currentTimeStep--;

        if (currentTimeStep == 0)
            currentTimeStep = (uint)maxTimeStep - 1;

        foreach (var cell in cells)
        {
            cell.ShowTimeStep(currentTimeStep, previousTimeStepsShown, previousTimeStepsOpacityFactor);
        }
    }

    public void OnAnimationSpeedSliderChange(float value) => animationSpeed = value;
    public void OnPrevTimeStepsShownChange(float value) => previousTimeStepsShown = (int)value;

    public void OnCellSelected(Cell cell)
    {
        currentlySelectedCell = cell;
        cellIDText.text = cell.id.ToString();
        cellPopulationText.text = cell.population;
    }
}


/// <summary>
/// Population class. Stores name and color.
/// </summary>
[System.Serializable]
public class Population
{
    public string name;
    public Color color;

    public Population(string popName, Color popColor)
    {
        name = popName;
        color = popColor;
    }
}
