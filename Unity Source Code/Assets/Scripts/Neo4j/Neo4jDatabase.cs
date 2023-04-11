using Neo4j.Driver;
using System;
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
        public async Task<(List<INode>, List<IRelationship>)> CustomFetch(string cypherQuery, params string[] keys) // Actually, can use any type for List content.
        {

            var nodes = new List<INode>();
            var relationships = new List<IRelationship>();
            
            var result = await session.RunAsync(cypherQuery);

            try
            {
                await result.ForEachAsync(record =>
                {
                    foreach(string key in keys)
                    {
                        if (record[key].GetType().ToString() == "Neo4j.Driver.Internal.Types.Node")
                        {
                            nodes.Add(record[key].As<INode>());
                        }
                        else
                        {
                            relationships.Add(record[key].As<IRelationship>());
                        }
                    }
                    
                });

            }
            catch (Exception) 
            {
                throw;
            }
            return (nodes, relationships);
        }

        public string[] GetKeys(string query)
        {
            var keys = query.ToLower().Split(new string[] { "return" }, StringSplitOptions.None)[1].Split(new string[] { "," }, StringSplitOptions.None);
            for (int index = 0; index < keys.Length;)
            {
                keys[index] = keys[index].Trim();
                index++;
            }
            return keys;
        }

        // Clean up the Neo4j session when the script is destroyed
        private async void OnDestroy()
        {
            await session?.CloseAsync();
        }
    }
}
