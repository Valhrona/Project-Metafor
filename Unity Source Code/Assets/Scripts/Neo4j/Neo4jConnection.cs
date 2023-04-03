using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Database;
using System.Collections.Generic;
using Neo4j.Driver;
using GraphFoundation;
using System.Diagnostics;
using System;

public class Neo4jConnection : MonoBehaviour
{

    private Neo4jDatabase currentDatabase;
    private List<INode> results;
    private GameObject member;
    private bool startingLine = false;
    public GameObject PopUpUI;
    public GameObject AttributesCell;
    public string quitText = "Player Quits";
    public Process process = new Process();
    public ProcessStartInfo startInfo = new ProcessStartInfo();


    private async void Start()
    {
        // connect to Neo4j DBMS through command line
        EstablishCLConnection();

        // set PopUpUI false on awake AFTER we have the reference to it stored.
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

    private void EstablishCLConnection()
    {
        startInfo.FileName = "cmd.exe"; //the application we want to execute, in this case its command line which is cmd.exe
        startInfo.Arguments = "/C cd C:/Users/selsabrouty/.Neo4jDesktop/relate-data/dbmss/dbms-3fec07b2-1923-4bbf-9e7c-4c40b889bd27/bin && neo4j console"; // the command we want to execute
        process.StartInfo = startInfo; // attach this info to the process
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        //process.StartInfo.CreateNoWindow = true; // No GUI

        process.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
        {
            if (e.Data.Contains("Started"))
            {
                startingLine = true;
            }
            UnityEngine.Debug.Log(e.Data);
        });


        process.Start();
        process.BeginOutputReadLine();
        while (!startingLine) { }
    }

    private void Update()
    {
    }


    // Terminate process when the application is stopped.
    private void OnApplicationQuit()
    {
        process.Kill();

    }
}
