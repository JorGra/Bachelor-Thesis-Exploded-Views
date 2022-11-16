using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    [SerializeField] Transform lineContainer;
    [SerializeField] GameObject linePrefab;

    List<LineRenderer> lineRenderers = new List<LineRenderer>();


    public void UpdatePositions(int index, Vector3 start, Vector3 end)
    {
        lineRenderers[index].SetPosition(0, start);
        lineRenderers[index].SetPosition(1, end);
    }

    public void GenerateLine(Color color)
    {
        var lr = Instantiate(linePrefab, lineContainer).GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;
        lineRenderers.Add(lr);
    }

    public void ClearContainer()
    {
        for (int i = 0; i < lineContainer.childCount; i++)
        {
            Destroy(lineContainer.GetChild(i));
        }
    }

}
