using GraphFoundation;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class GraphManager : MonoBehaviour
{
    public Dictionary<int, GameObject> nodes = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> edges = new Dictionary<int, GameObject>();
    public Dictionary<int, SpringJoint> joints = new Dictionary<int, SpringJoint>();
    private GameObject EdgePrefab;
    private Vector3 startPosition;
    private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {   
        startPosition = Camera.main.transform.position;
        startRotation = Camera.main.transform.rotation;
        EdgePrefab = Resources.Load("Prefabs/EdgePrefab", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Make edges adjust based on how the joints move.
        AdjustEdges();
    }

    private void AdjustEdges()
    {
        foreach (var edge in edges)
        {
            var hookPoint = edge.Value.GetComponent<EdgeBehaviour>().startNode;
            edge.Value.transform.position = new Vector3(hookPoint.transform.position.x, hookPoint.transform.position.y, hookPoint.transform.position.z);
            SpringJoint sj = joints[edge.Key];
            GameObject target = sj.connectedBody.gameObject;
            //Debug.Log(target.transform.GetComponent<NodeBehaviour>().nodeID);
            edge.Value.transform.LookAt(target.transform);
            Vector3 ls = edge.Value.transform.localScale;
            ls.z = Vector3.Distance(hookPoint.transform.position, target.transform.position);
            edge.Value.transform.localScale = ls;
            edge.Value.transform.position = new Vector3((hookPoint.transform.position.x + target.transform.position.x) / 2,
                              (hookPoint.transform.position.y + target.transform.position.y) / 2,
                              (hookPoint.transform.position.z + target.transform.position.z) / 2);
        }
    }

    public void BuildGraph((List<INode>, List<IRelationship>) results)
    {
        ClearGraph();
        // revert camera to starting position;
        ResetCamera();
        // first go over nodes
        var _nodes = results.Item1;
        var _edges = results.Item2;
        for (int index = 0; index < _nodes.Count; index++)
        {
            bool present = false; // to check if node is not already present in list
            int id = (int)_nodes[index].Id; // get Node ID
            foreach (var sphere in nodes)
            {
                if (sphere.Value.transform.GetComponent<NodeBehaviour>().nodeID == id) // prevent duplicate nodes
                {
                    present = true; 
                }
            }
            if (!present)
            {
                var labels = _nodes[index].Labels; // get Node labels
                var _properties = _nodes[index].Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
                /* Get the spawn position */ /* Get the spawn position */
                Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 180f));

                string prefabText = labels[1].ToLower().Split(new string[] { "__" }, StringSplitOptions.None)[1];
                var prefab = Resources.Load($"Prefabs/{prefabText}", typeof(GameObject)) as GameObject;
                var node = Instantiate(prefab, spawnPos, Quaternion.identity);
                node.name = $"Node_{id}";
                node.transform.parent = GameObject.FindGameObjectWithTag("Graph").transform;
                var nodeAttributes = node.GetComponent<NodeBehaviour>();
                nodeAttributes.nodeID = id;
                nodeAttributes.labels = (List<string>)labels;
                nodeAttributes.properties = (Dictionary<string, object>)_properties;
                nodeAttributes.defaultColor = node.GetComponent<Renderer>().material.color;
                nodes.Add(id,node);
            }
            
        }
        // then go over edges
        for (int index = 0; index < _edges.Count; index++)
        {
            int edgeID = (int)_edges[index].Id; // get Edge ID 
            int startNodeID = (int)_edges[index].StartNodeId;
            int endNodeID = (int)_edges[index].EndNodeId;
            string typeEdge = _edges[index].Type;
            var properties = _edges[index].Properties;
            SpringJoint springjoint = nodes[startNodeID].AddComponent<SpringJoint>();
            springjoint.autoConfigureConnectedAnchor = false;
            springjoint.anchor = new Vector3(0.5f, 0.5f, 0);
            springjoint.connectedAnchor = new Vector3(0.5f, 0, 0);
            springjoint.enableCollision = true;
            springjoint.connectedBody = nodes[endNodeID].transform.GetComponent<Rigidbody>();
            springjoint.spring = 8;
            GameObject edge = Instantiate(EdgePrefab, new Vector3(nodes[startNodeID].gameObject.transform.position.x, nodes[startNodeID].gameObject.transform.position.y, nodes[startNodeID].gameObject.transform.position.z), Quaternion.identity);
            edge.transform.parent = GameObject.FindGameObjectWithTag("Graph").transform;
            edge.GetComponent<EdgeBehaviour>().edgeID = edgeID;
            edge.GetComponent<EdgeBehaviour>().startNodeID = startNodeID;
            edge.GetComponent<EdgeBehaviour>().startNode = nodes[startNodeID];
            edge.GetComponent<EdgeBehaviour>().endNodeID = endNodeID;
            edge.GetComponent<EdgeBehaviour>().endNode = nodes[endNodeID];
            edge.GetComponent<EdgeBehaviour>().typeEdge = typeEdge;
            edge.GetComponent<EdgeBehaviour>().properties = (Dictionary<string, object>)properties;
            edge.GetComponent<EdgeBehaviour>().defaultColor = edge.GetComponent<Renderer>().material.color;
            joints.Add(edgeID, springjoint);
            edges.Add(edgeID, edge);
        }
    }

    private void ResetCamera()
    {
        Camera.main.transform.position = startPosition;
        Camera.main.transform.rotation = startRotation;
    }

    public void ClearGraph()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        nodes = new Dictionary<int, GameObject>();
        edges = new Dictionary<int, GameObject>();
        joints = new Dictionary<int, SpringJoint>();
    }
}
