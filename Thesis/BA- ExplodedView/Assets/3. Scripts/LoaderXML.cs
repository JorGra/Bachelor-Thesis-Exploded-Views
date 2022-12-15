using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// XML loader class. Searches the Resources folder and loads any Morpheus snapshotfiles it can find. 
/// </summary>
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
        var dir = Directory.GetFiles(path, "*.xml").OrderBy(f => f); ;

        for (int i = 0; i < dir.ToArray().Length; i++)
        {
            LoadFile(Path.Combine(path, new FileInfo(dir.ToArray()[i]).Name), i);
        }

    }
    /// <summary>
    /// Loads file and sends data to CellManager. 
    /// </summary>
    /// <param name="path">path to file</param>
    /// <param name="timeStep">number of this file. Should be equal to the time step of the snapshot</param>
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
                Debug.Log("New Population starting: " + pop.Type);
                foreach (var cell in pop.Cell)
                {

                    if (string.IsNullOrEmpty(cell.Id) || string.IsNullOrEmpty(cell.Center) || string.IsNullOrEmpty(cell.Nodes))
                        break;

                    //id
                    var id = int.Parse(cell.Id);

                    //Center
                    var centerVals = cell.Center.Split(",");
                    Vector3 center = Vector3.zero;
                    center[0] = (float)double.Parse(centerVals[0], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    center[1] = (float)double.Parse(centerVals[1], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                    center[2] = (float)double.Parse(centerVals[2], System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

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

                    var population = pop.Type;
                    
                    cellManager.AddCell(id, nodeList.ToArray(), center, timeStep, population);

                }
            }
            cellManager.maxTimeStep++;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception loading model file: " + e);
        }
    }
}
