using UnityEngine;
using Neo4j.Driver;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class Neo4jConnection : MonoBehaviour
{
    private IAsyncSession session;

    // Set up the connection to the Neo4j database
    private void Start()
    {
        var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "q9385Moq!"));

        session = driver.AsyncSession(o => o.WithDatabase("neo4j"));
        FetchData();
    }

    private void Update()
    { 
    
    }

    // Fetch data from the Neo4j database
    private async void FetchData()
    {
        var result = await session.RunAsync("MATCH (n) RETURN n");

        await result.ForEachAsync(record =>
        {
            var node = record["n"].As<INode>();
            //Debug.Log(node.ElementId.Split(':').Last());
            Debug.Log(node.Properties);
        });
    }

    // Empty the Neo4j database
    private async void DeleteData()
    {
        await session.RunAsync("MATCH (n) DETACH DELETE n");

    }

    // Clean up the Neo4j session when the script is destroyed
    private async void OnDestroy()
    {
        await session?.CloseAsync();
    }
}
