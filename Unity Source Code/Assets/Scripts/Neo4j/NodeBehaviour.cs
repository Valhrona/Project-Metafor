using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GraphFoundation
{
    public class NodeBehaviour : MonoBehaviour
    {
        public int nodeID;
        public List<string> labels = new List<string>();
        public Dictionary<string,object> properties = new Dictionary<string,object>();

        public Camera CameraToMove;
        public float MovementSpeed = 0.5F;
        public bool expanded;
        public List<string> expandedNodes = new List<string>();
        private Neo4jDatabase database;
        public Color defaultColor;
        private GameObject CDE_LD_NODE;

        void Start()
        {
            CDE_LD_NODE = Resources.Load("Prefabs/CDE_LDN", typeof(GameObject)) as GameObject;
            database = GameObject.FindGameObjectWithTag("NodeSpawner").transform.GetComponent<Neo4jConnection>().currentDatabase;
            foreach (var kvp in properties)
            {
                if (kvp.Key == "rdfs__label")
                {
                    transform.GetComponentInChildren<TextMeshProUGUI>().text = ((string)kvp.Value).Split(".")[1];
                }
            };
        }

        private void Update()
        {
            // have text always face camera
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        public async void DoUnfolding()
        {   
            var result = await database.CustomFetch($"MATCH (z:ns0__APM_CDE)-[r]-(n) WHERE ID(z) = {nodeID} RETURN n LIMIT 4", "n");
            Debug.Log(result.Count());
            for (int index = 0; index < result.Count; index++)
            {
                int id = (int)result[index].Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
                var labels = result[index].Labels; // get Node labels
                var _properties = result[index].Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
                var radians = 2 * Math.PI / result.Count * index;
                var vertical = MathF.Sin((float)radians);
                var horizontal = MathF.Cos((float)radians);

                var spawnDir = new Vector3(horizontal, vertical, 0);

                /* Get the spawn po sition */ /* Get the spawn position */
                var spawnPos = transform.position + spawnDir * 30; // Radius is just the distance away from the point

                var member = Instantiate(CDE_LD_NODE, spawnPos, Quaternion.identity);
                member.name = $"Node_{id}";
                expandedNodes.Add(member.name);
                var nodeAttributes = member.GetComponent<NodeBehaviour>();
                nodeAttributes.nodeID = id;
                nodeAttributes.labels = (List<string>)labels;
                nodeAttributes.properties = (Dictionary<string, object>)_properties;
                nodeAttributes.defaultColor = member.GetComponent<Renderer>().material.color;
            }
            expanded = true;
        }

        public void UndoUnfolding()
        {
            foreach(string node in expandedNodes)
            {
                GameObject nodeToBeDestroyed = GameObject.Find(node);
                Destroy(nodeToBeDestroyed);
            }
            expanded = false;
           
        }
    }
}

