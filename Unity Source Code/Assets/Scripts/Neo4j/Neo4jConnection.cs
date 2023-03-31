using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Neo4j.Driver;
using System;
using System.Collections.ObjectModel;
using UnityEditor.Experimental.GraphView;
using TMPro;

public class Neo4jConnection : MonoBehaviour
{

    private Neo4jDatabase currentDatabase;
    private List<INode> results;
    private GameObject member;


    private async void Start()
    {
        GameObject APM_CDE_NODE = Resources.Load("Prefabs/APM_CDE_NODE", typeof(GameObject)) as GameObject;

        //Provide credentials.json path to connect to local Neo4j instance
        string credentialsFile = "Assets/Scripts/Neo4j/credentials.json";

        //Read credentials files
        string json = File.ReadAllText(credentialsFile);

        //Initialize credentials
        currentDatabase = JsonConvert.DeserializeObject<Neo4jDatabase>(json);

        //Establish connection with local running Neo4j instance
        currentDatabase.Init();

        results = await currentDatabase.CustomFetch("MATCH (n:ns0__APM_CDE) RETURN n LIMIT 25");
        for (int index = 0; index < results.Count; index++)
        {   
            var id = results[index].Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
            var _labels = results[index].Labels; // get Node labels
            var _properties = results[index].Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
            if (index == 0) // SORRY THIS IS HARDCODING LAZY BUT ITS FOR TESTING PURPOSES
            {
                member = Instantiate(APM_CDE_NODE, new Vector3(-20, 15, 50), Quaternion.identity);
            }
            else if (index == 1)
            {
                member = Instantiate(APM_CDE_NODE, new Vector3(20, 15, 50), Quaternion.identity);
            }
            else 
            {
                member = Instantiate(APM_CDE_NODE, new Vector3(0, -15, 50), Quaternion.identity);
            }
            foreach (var kvp in _properties)
            {
                if (kvp.Key == "rdfs__label")
                {
                    Debug.Log(member.transform.GetComponentInChildren<TextMeshProUGUI>().text);
                    member.transform.GetComponentInChildren<TextMeshProUGUI>().text = ((string)kvp.Value).Split(".")[1];
                }
            }
            var labels = string.Join(", ", _labels);
            //Debug.Log($"Node {id} has labels: {labels}.");
            
        }

    }

    private void Update()
    {

    }

}
