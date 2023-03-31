using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Neo4j.Driver;
using System;

public class Neo4jConnection : MonoBehaviour
{

    private Neo4jDatabase currentDatabase;
    private List<INode> results;

    private async void Start()
    {
        //Provide credentials.json path to connect to local Neo4j instance
        string credentialsFile = "Assets/Scripts/Neo4j/credentials.json";

        //Read credentials files
        string json = File.ReadAllText(credentialsFile);

        //Initialize credentials
        currentDatabase = JsonConvert.DeserializeObject<Neo4jDatabase>(json);

        //Establish connection with local running Neo4j instance
        currentDatabase.Init();

        results = await currentDatabase.CustomFetch("MATCH (n:ns0__APM_CDE) RETURN n LIMIT 25");
        foreach (var node in results)
        {
            var id = node.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
            var _labels = node.Labels; // get Node labels
            var _properties = node.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
            var labels = string.Join(", ", _labels);
            Debug.Log($"Node {id} has labels: {labels}");
            
        }

    }

    private void Update()
    {

    }

}
