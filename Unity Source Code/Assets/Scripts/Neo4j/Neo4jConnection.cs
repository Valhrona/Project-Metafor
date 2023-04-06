using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Database;
using System.Collections.Generic;
using Neo4j.Driver;
using GraphFoundation;
using System.Diagnostics;
using UnityEngine.Networking.Types;
using System;

public class Neo4jConnection : MonoBehaviour
{

    public Neo4jDatabase currentDatabase;
    public GameObject PopUpUI;
    public GameObject AttributesCell;
    public Process process = new Process();
    public ProcessStartInfo startInfo = new ProcessStartInfo();
    private List<(INode, IRelationship)> results;
    private GameObject startingNode;
    private bool startingLine = false;


    private async void Start()
    {
        // connect to Neo4j DBMS through command line
        //EstablishClientConnection();

        GameObject APM_CDE_NODE = Resources.Load("Prefabs/APM_CDE_NODE", typeof(GameObject)) as GameObject;

        //Provide credentials.json path to connect to local Neo4j instance
        string credentialsFile = "Assets/Scripts/Neo4j/credentials.json";

        //Read credentials files
        string json = File.ReadAllText(credentialsFile);

        //Initialize credentials
        currentDatabase = JsonConvert.DeserializeObject<Neo4jDatabase>(json);

        //Establish connection with local running Neo4j instance
        currentDatabase.Connect();

        // fetch the starting point nodes
        results = await currentDatabase.CustomFetch("MATCH (n:ns0__APM_CDE) RETURN n", "n");
        for (int index = 0; index < results.Count; index++)
        {
            int id = (int)results[index].Item1.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
            var labels = results[index].Item1.Labels; // get Node labels
            var _properties = results[index].Item1.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
            var radians = 2 * Math.PI / results.Count * index;
            var vertical = MathF.Sin((float)radians);
            var horizontal = MathF.Cos((float)radians);

            var spawnDir = new Vector3(horizontal, vertical, 0);

            /* Get the spawn po sition */ /* Get the spawn position */
            var spawnPos = new Vector3(0,0,120f) + spawnDir * 20; // Radius is just the distance away from the point
            startingNode = Instantiate(APM_CDE_NODE, spawnPos, Quaternion.identity);
            var nodeAttributes = startingNode.GetComponent<NodeBehaviour>();
            startingNode.name = $"Node_{id}";
            nodeAttributes.nodeID = id;
            nodeAttributes.labels = (List<string>)labels;
            nodeAttributes.properties = (Dictionary<string, object>)_properties;
            nodeAttributes.defaultColor = startingNode.GetComponent<Renderer>().material.color;
        }

    }


    private void EstablishClientConnection()
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
