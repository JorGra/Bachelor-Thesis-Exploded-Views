using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class LoaderXML : MonoBehaviour
{

    CellManager cellManager;


    // Start is called before the first frame update
    void Start()
    {
        cellManager = CellManager.Instance;

        var path = Path.Combine(Application.dataPath, "Resources/");
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles("*.xml");

        for (int i = 0; i < fileInfo.Length; i++)
        {
            LoadFile(Path.Combine(path, fileInfo[i].Name), i);
        }
    }

    void LoadFile(string path, int timeStep)
    {
        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Xml2CSharp.MorpheusModel));

        try
        {
            Debug.Log("Trying to read file at: " + path);
            using FileStream stream = new FileStream(path, FileMode.Open);
            var se = serializer.Deserialize(stream) as Xml2CSharp.MorpheusModel;

            foreach (var pop in se.CellPopulations.Population)
            {
                Debug.Log("New Population starting");
                Debug.Log("----------------------------------------");
                foreach (var cell in pop.Cell)
                {

                    if (string.IsNullOrEmpty(cell.Id) || string.IsNullOrEmpty(cell.Center) || string.IsNullOrEmpty(cell.Nodes))
                        break;

                    //Debug.Log(cell.Nodes);

                    //id
                    var id = int.Parse(cell.Id);

                    //Center
                    var centerVals = cell.Center.Split(",");
                    Vector3 center = Vector3.zero;
                    center[0] = float.Parse(centerVals[0]);
                    center[1] = float.Parse(centerVals[1]);
                    center[2] = float.Parse(centerVals[2]);

                    //Nodes
                    var nodeVals = cell.Nodes.Split(";");
                    var nodeList = new List<Vector3Int>();
                    foreach (var node in nodeVals)
                    {
                        var positions = node.Split(",");

                        Vector3Int pos = Vector3Int.zero;
                        pos[0] = int.Parse(positions[0]);
                        pos[1] = int.Parse(positions[1]);
                        pos[2] = int.Parse(positions[2]);

                        nodeList.Add(pos);
                    }

                    cellManager.AddCell(id, nodeList.ToArray(), center, timeStep);

                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception loading model file: " + e);
        }
    }
}
