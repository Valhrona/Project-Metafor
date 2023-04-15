using Database;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBehaviour : MonoBehaviour
{
    public int edgeID;
    public int startNodeID;
    public GameObject startNode;
    public int endNodeID;
    public GameObject endNode;
    public string typeEdge;
    public Dictionary<string, object> properties = new Dictionary<string, object>();

    public Color defaultColor;

    private Neo4jDatabase database;

    // Start is called before the first frame update
    void Start()
    {
        database = GameObject.FindGameObjectWithTag("NodeSpawner").transform.GetComponent<Neo4jConnection>().currentDatabase;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
