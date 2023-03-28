using Neo4j.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Database
{
    // Neo4j Database class that contains a connection to the desired Neo4j instance that is currently running on the machine (locally)
    public class Neo4jDatabase
    {

        private string databaseName { get; set; }
        private string uri { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private IAsyncSession session;

        // Initialize database such that it request and responses can be handled
        public void Init()
        {
            // Establish Neo4j connection with driver and start session linked to database
            session = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)).AsyncSession(o => o.WithDatabase(databaseName));
        }

        // Get data from custom query
        public async Task<List<INode>> CustomFetch(string cypherQuery)
        {

            var data = new List<INode>();
            var result = await session.RunAsync(cypherQuery);

            await result.ForEachAsync(record =>
            {
                var node = record["n"].As<INode>();
                //Debug.Log(node.ElementId.Split(':').Last());
                Debug.Log(node.Properties);

            });
            return data;
        }

        // Obtain the complete data of the database as a List of Nodes
        public async Task<List<INode>> GetAllData()
        {

            var data = new List<INode>();
            var result = await session.RunAsync("MATCH (n) RETURN n");

            await result.ForEachAsync(record =>
            {
                var node = record["n"].As<INode>();
                //Debug.Log(node.ElementId.Split(':').Last());
                Debug.Log(node.Properties);

            });
            return data;
        }


        // Empty the Neo4j database
        async void DeleteData()
        {
            await session.RunAsync("MATCH (n) DETACH DELETE n");

        }

        // Clean up the Neo4j session when the script is destroyed
        private async void OnDestroy()
        {
            await session?.CloseAsync();
        }
    }
}
