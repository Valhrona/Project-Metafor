using Neo4j.Driver;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public List<GameObject> nodes = new List<GameObject>();
    public List<GameObject> edges = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildGraph((List<INode>, List<IRelationship>) results)
    {
        ClearGraph();
        // first go over nodes
        // then go over edges
        //for (int index = 0; index < results.Count; index++)
        //{
        //    int id = (int)results[index].Item1.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
        //    var labels = results[index].Item1.Labels; // get Node labels
        //    var _properties = results[index].Item1.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
        //    var radians = 2 * Math.PI / results.Count * index;
        //    var vertical = MathF.Sin((float)radians);
        //    var horizontal = MathF.Cos((float)radians);
        //
        //    var spawnDir = new Vector3(horizontal, vertical, 0);
        //
        //    /* Get the spawn po sition */ /* Get the spawn position */
        //    var spawnPos = new Vector3(0,0,120f) + spawnDir * 20; // Radius is just the distance away from the point
        //    startingNode = Instantiate(APM_CDE_NODE, spawnPos, Quaternion.identity);
        //    startingNode.transform.parent = GameObject.FindGameObjectWithTag("Graph").transform;
        //    var nodeAttributes = startingNode.GetComponent<NodeBehaviour>();
        //    startingNode.name = $"Node_{id}";
        //    nodeAttributes.nodeID = id;
        //    nodeAttributes.labels = (List<string>)labels;
        //    nodeAttributes.properties = (Dictionary<string, object>)_properties;
        //    nodeAttributes.defaultColor = startingNode.GetComponent<Renderer>().material.color;
        //}
    }

    public void ClearGraph()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
