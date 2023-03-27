using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Database
{
    public class Neo4jDatabase
    {
        private const String Tag = "Lineage Information";

        private const String DATABASE_NAME = "neo4j";
        private const String URI = "bolt://localhost:7687";
        private const String USERNAME = "neo4j";
        private const String PASSWORD = "q9385Moq!";
        private IAsyncSession session;

        public Neo4jDatabase() 
        {
            var driver = GraphDatabase.Driver(URI, AuthTokens.Basic(USERNAME, PASSWORD));

            session = driver.AsyncSession(o => o.WithDatabase(DATABASE_NAME));
        }
        
        // Method that returns the complete data of the database as a List of Nodes
        private async Task<List<INode>> GetAllData()
        {
            var data = new List<INode>();
            var result = await session.RunAsync("MATCH (n) RETURN n");

            await result.ForEachAsync(record =>
            {
                var node = record["n"].As<INode>();
                
            });
            return data;
        }
    }
}
