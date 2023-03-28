using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Database;

public class Neo4jConnection : MonoBehaviour
{

    private Neo4jDatabase currentDatabase;

    private void Start()
    {
        //Provide credentials.json path to connect to local Neo4j instance
        string credentialsFile = "Assets/Scripts/Neo4j/credentials.json";

        //Read credentials files
        string json = File.ReadAllText(credentialsFile);

        //Initialize credentials
        currentDatabase = JsonConvert.DeserializeObject<Neo4jDatabase>(json);

        //Establish connection with local running Neo4j instance
        currentDatabase.Init();


    }

    private void Update()
    {

    }

}
