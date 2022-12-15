using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Cell class. Stores data from the dataset and provides functionality to change time steps and generates the mesh.
/// </summary>
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

    /// <summary>
    /// Shows given time step by activating corresponding child object.
    /// can also activate previous timesteps with a lower opacity.
    /// </summary>
    /// <param name="timeStep">time step to show</param>
    /// <param name="previousTimeSteps">number of previous time steps to show</param>
    /// <param name="opacityFactor"></param>
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

    /// <summary>
    /// Adds a new child object, generates a mesh and stores the center
    /// </summary>
    /// <param name="positions">List of postitions of the cell at that time step</param>
    /// <param name="center">the center from the dataset at that time step</param>
    /// <param name="timeStep">the time step</param>
    /// <param name="voxelSize">overall scale factor for the mesh</param>
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

        //var coll = meshObj.AddComponent<MeshCollider>();
        var coll = meshObj.AddComponent<SphereCollider>();
        //coll.convex = true;
        coll.isTrigger = true;
        //coll.sharedMesh = meshFilter.mesh;

        var inter = GetComponent<XRGrabInteractable>();
        inter.colliders.Add(coll);
        inter.interactionManager.UnregisterInteractable(inter.GetComponent<IXRInteractable>());
        inter.interactionManager.RegisterInteractable(inter.GetComponent<IXRInteractable>());

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

    /// <summary>
    /// shows cell info on UI panel. Gets triggered when cell is grabbed
    /// </summary>
    public void ActivateCell()
    {
        cellManager.OnCellSelected(this);
    }

    /// <summary>
    /// Gets triggered when cell is not grabbed anymore
    /// </summary>
    public void DeactivateCell()
    {
        transform.localRotation = Quaternion.identity;
    }
 
    /// <summary>
    /// This gets triggered when selection button is pressed and adds this cell to the currently selected cell list for the force based explosion
    /// </summary>
    public void CellSelected() 
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

    /// <summary>
    /// Adds cell to selected list on CellManager and colors the inital transform marker
    /// </summary>
    void SelectCell()
    {
        if(!cellManager.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Contains(transform))
            cellManager.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Add(transform);
        originalOriginColor = originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color;
        originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color = selectedOriginColor;

    }

    /// <summary>
    /// Removes cell from selected list on CellManager and colors the inital transform marker
    /// </summary>
    void DeselectCell()
    {
        if (CellManager.Instance.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Contains(transform))
            CellManager.Instance.gameObject.GetComponent<ExplosionViewHandler>().selectedCells.Remove(transform);
        originalTransformObject.GetComponentInChildren<MeshRenderer>().material.color = originalOriginColor;
    }

    /// <summary>
    /// Generates mesh from given positions list
    /// </summary>
    /// <param name="positions">List of positions</param>
    /// <param name="voxelSize">Scaleing factor of the mesh</param>
    /// <returns></returns>
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

    /// <summary>
    /// Generates unit cube vertices at given postition
    /// </summary>
    /// <param name="position">position to spawn cube</param>
    /// <param name="offset">offset of cube</param>
    /// <param name="voxelSize">scaling factor</param>
    /// <returns></returns>
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

    /// <summary>
    /// Adds triangles for mesh rendering
    /// </summary>
    /// <param name="cubeID">cube id that is used for offsetting the indices</param>
    /// <returns></returns>
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
