using Neo4j.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Database
{
    // Neo4j Database class that contains a connection to the desired Neo4j instance that is currently running on the machine (locally)
    public class Neo4jDatabase
    {

        public string databaseName { get; set; }
        public string uri {private get; set; }
        public string username { private get; set; }
        public string password { private get; set; }
        private IAsyncSession session;

        // Initialize database such that it request and responses can be handled
        public void Connect()
        {
            // Establish Neo4j connection with driver and start session linked to database
            session = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password)).AsyncSession(o => o.WithDatabase(databaseName));
        }

        // Get data from custom quersy
        public async Task<List<(INode, IRelationship)>> CustomFetch(string cypherQuery, params string[] keys) // Actually, can use any type for List content.
        {

            var nodesAndRelationships = new List<(INode, IRelationship)>();
            var result = await session.RunAsync(cypherQuery);
            await result.ForEachAsync(record =>
            {

                var node = record[keys[0]].As<INode>();
                if (keys.Length > 1)
                {
                    var relationship = record[keys[1]].As<IRelationship>();
                    nodesAndRelationships.Add((node, relationship));
                }
                else
                {
                    nodesAndRelationships.Add((node, null));
                }
                //foreach (var kvp in node.Properties)
                //{
                //    Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
                //}
                //foreach (var item in node.Labels) 
                //{
                //    Debug.Log(item.ToString());
                //}zzzzzzzzz

            });
            return nodesAndRelationships;
        }

        // Obtain the complete data of the database as a List of Nodes
        public async Task<List<INode>> GetAllData()
        {

            var nodes = new List<INode>();
            var result = await session.RunAsync("MATCH (n) RETURN n");

            await result.ForEachAsync(record =>
            {
                var node = record["n"].As<INode>();
                if (node != null)
                {
                    nodes.Add(node);
                }

            });
            return nodes;
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
