using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cell : MonoBehaviour
{

    public int id;
    public Color cellColor;

    public string population;

    public List<Vector3> centers = new List<Vector3>();
    Dictionary<uint, int> timeSteps = new Dictionary<uint, int>(); //timeStep as key, childIndex as value

    Quaternion savedRotation;
    public GameObject originalTransformObject { get; set; }

    public Vector3 previousTargetPosition;
    public Vector3 targetPosition;

    [SerializeField] float timeSinceLastTimeStep = 0;

    public Vector3 GridOffset { get; set; }

    [SerializeField] Material cellMaterial;
    [SerializeField] Color selectedOriginColor = Color.red;
    Color originalOriginColor;

    bool selected = false;

    CellManager cellManager;

    private void Start()
    {
        cellManager = CellManager.Instance;
    }

    public void ShowTimeStep(uint timeStep, int previousTimeSteps, float opacityFactor = 1f)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        for (uint i = timeStep; i >= (timeStep - previousTimeSteps); i--)
        {

            if (i == timeStep && timeSteps.ContainsKey(i))
            {
                if (timeSteps[i] == 0)
                {
                    //Debug.Log("Reset reference point.");
                    previousTargetPosition = centers[0];
                    targetPosition = centers[0];
                }
                EnableChildWithOpacity(timeSteps[i], 1f);
                timeSinceLastTimeStep = Time.timeSinceLevelLoad;
                previousTargetPosition = targetPosition;
                targetPosition = centers[timeSteps[i]];

            }
            else if (timeSteps.ContainsKey(i))
                EnableChildWithOpacity(timeSteps[i], (previousTimeSteps / (float)(timeStep + previousTimeSteps)) * opacityFactor);
            if (i == 0)
                break;
        }
    }

    void EnableChildWithOpacity(int childIndex, float opacity)
    {
        transform.GetChild(childIndex).gameObject.SetActive(true);
        transform.GetChild(childIndex).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Opacity", opacity);

    }


    public void AddTimeStep(Vector3Int[] positions, Vector3 center, int timeStep, float voxelSize)
    {
        var meshObj = new GameObject(timeStep.ToString());
        meshObj.transform.SetParent(transform);

        var meshFilter = meshObj.AddComponent<MeshFilter>();
        var meshRenderer = meshObj.AddComponent<MeshRenderer>();
        
        meshRenderer.material = cellMaterial;
        meshRenderer.material.SetColor("_BaseColor", cellColor);
        meshFilter.mesh = GenerateMesh(positions, voxelSize);

        timeSteps.Add((uint)timeStep, meshObj.transform.GetSiblingIndex());
        meshObj.SetActive(false);


        targetPosition = centers[0];
        previousTargetPosition = centers[0];
    }


    /// <summary>
    /// returns the lerped reference point, if enabled in the CellManager.
    /// returned value is in world space.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentReferencePoint()
    {
        if (!cellManager.lerpReferencePoint)
            return centers[0];

        return Vector3.Lerp(previousTargetPosition, targetPosition, (Time.timeSinceLevelLoad - timeSinceLastTimeStep) / cellManager.animationSpeed);
    }

    public void ActivateCell()
    {
        cellManager.OnCellSelected(this); //This shows cell info

    }

    public void DeactivateCell() //This resets the cells rotation
    {
        transform.localRotation = Quaternion.identity;
    }
 
    public void CellSelected() //This handles force based selection of cells 
    {
        if (selected)
        {
            DeselectCell();
        }
        else
        {
            SelectCell();
        }
        selected = !selected;
    }

    void SelectCell()
    {
        if(!cellManager.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Contains(transform))
            cellManager.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Add(transform);
        originalOriginColor = originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color;
        originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color = selectedOriginColor;

    }

    void DeselectCell()
    {
        if (CellManager.Instance.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Contains(transform))
            CellManager.Instance.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Remove(transform);
        originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color = originalOriginColor;
    }

    Mesh GenerateMesh(Vector3Int[] positions, float voxelSize)
    {
        var m = new Mesh();

        var v = new List<Vector3>();
        var t = new List<int>();

        for (int i = 0; i < positions.Length; i++)
        {
            v.AddRange(GenerateCubeVertices(positions[i], GridOffset, voxelSize));
            t.AddRange(GenerateCubeTriangles(i));
        }

        m.vertices = v.ToArray();
        m.triangles= t.ToArray();

        m.RecalculateNormals();
        m.Optimize();
        m.RecalculateBounds();
        return m;
    }

    Vector3[] GenerateCubeVertices(Vector3Int position, Vector3 offset, float voxelSize)
    {
        Vector3[] vertices = {
            new Vector3 (-.5f + offset.x + position.x, -.5f + position.y + offset.y, -.5f + position.z + offset.z) * voxelSize,
            new Vector3 ( .5f + offset.x + position.x, -.5f + position.y + offset.y, -.5f + position.z + offset.z) * voxelSize,
            new Vector3 ( .5f + offset.x + position.x,  .5f + position.y + offset.y, -.5f + position.z + offset.z) * voxelSize,
            new Vector3 (-.5f + offset.x + position.x,  .5f + position.y + offset.y, -.5f + position.z + offset.z) * voxelSize,
            new Vector3 (-.5f + offset.x + position.x,  .5f + position.y + offset.y,  .5f + position.z + offset.z) * voxelSize,
            new Vector3 ( .5f + offset.x + position.x,  .5f + position.y + offset.y,  .5f + position.z + offset.z) * voxelSize,
            new Vector3 ( .5f + offset.x + position.x, -.5f + position.y + offset.y,  .5f + position.z + offset.z) * voxelSize,
            new Vector3 (-.5f + offset.x + position.x, -.5f + position.y + offset.y,  .5f + position.z + offset.z) * voxelSize,
        };
        return vertices;
    }

    int[] GenerateCubeTriangles(int cubeID)
    {
        var offset = cubeID * 8;

        int[] triangles = {
            0 + offset, 2 + offset, 1 + offset, //face front
			0 + offset, 3 + offset, 2 + offset,
            2 + offset, 3 + offset, 4 + offset, //face top
			2 + offset, 4 + offset, 5 + offset,
            1 + offset, 2 + offset, 5 + offset, //face right
			1 + offset, 5 + offset, 6 + offset,
            0 + offset, 7 + offset, 4 + offset, //face left
			0 + offset, 4 + offset, 3 + offset,
            5 + offset, 4 + offset, 7 + offset, //face back
			5 + offset, 7 + offset, 6 + offset,
            0 + offset, 6 + offset, 7 + offset, //face bottom
			0 + offset, 1 + offset, 6 + offset
        };
        return triangles;
    }

}
