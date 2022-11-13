using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CellManager : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] bool animated;
    public int maxTimeStep;
    [SerializeField] uint currentTimeStep;
    [SerializeField] int previousTimeStepsShown;
    [SerializeField] float previousTimeStepsOpacityFactor = 1f;
    [Range(0.05f, 2f)]
    [SerializeField] float animationSpeed;

    [Header("Cells")]
    [SerializeField] GameObject cellPrefab;
    public Transform cellContainer;
    [SerializeField] Vector3 gridOffset;
    [SerializeField] float voxelSize = 0.1f;
    public List<Cell> cells = new List<Cell>();

    [SerializeField] float minContainerSize = 0.1f;
    [SerializeField] float maxContainerSize = 1.5f;

    [SerializeField] TMP_Text pauseText;

    public delegate void OnSizeChange();
    public OnSizeChange onSizeChanged;

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

    public void AddCell(int id, Vector3Int[] voxelPos, Vector3 center,int timeStep, string population)
    {

        var cell = cells.FirstOrDefault(it => id == it.id);

        var scaledCenter = center * voxelSize;

        if (cell == null)
        {
            var newCellObj = Instantiate(cellPrefab, scaledCenter, Quaternion.identity, cellContainer);

            cell = newCellObj.GetComponent<Cell>();
            cell.id = id;
            cell.GridOffset = gridOffset;
            cell.cellColor = Color.HSVToRGB(Random.Range(0f, 360f)/360f, 0.9f, 0.7f);// Random.ColorHSV();
            cell.population = population;
            cells.Add(cell);
        }

        cell.centers.Add(scaledCenter);
        cell.AddTimeStep(voxelPos, center, timeStep, voxelSize);


    }

    public void OnSizeSliderChange(float value)
    {
        float dist = maxContainerSize - minContainerSize;
        var val = minContainerSize + (dist * value);

        cellContainer.transform.localScale = Vector3.one * val;
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
}
