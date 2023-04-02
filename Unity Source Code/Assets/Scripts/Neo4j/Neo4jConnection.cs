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
using GraphFoundation;

public class Neo4jConnection : MonoBehaviour
{

    private Neo4jDatabase currentDatabase;
    private List<INode> results;
    private GameObject member;
    public GameObject PopUpUI;
    public GameObject AttributesCell;


    private async void Start()
    {
        // set PopUpUI false on awake
        PopUpUI = GameObject.FindGameObjectWithTag("PopUp");
        AttributesCell = GameObject.FindGameObjectWithTag("Attributes");
        PopUpUI.SetActive(false);

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
            int id = (int)results[index].Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
            var labels = results[index].Labels; // get Node labels
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
            var nodeAttributes = member.GetComponent<APM_CDE_behaviour>();
            nodeAttributes.nodeID = id;
            nodeAttributes.labels = (List<string>)labels;
            nodeAttributes.properties = (Dictionary<string, object>)_properties;
        }


    }

    private void Update()
    {

    }

}
