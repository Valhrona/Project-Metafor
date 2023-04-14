using Database;
using System.Collections.Generic;
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
        public List<GameObject> expandedNodes = new List<GameObject>();
        public List<GameObject> expandedEdges = new List<GameObject>();
        public List<SpringJoint> joints = new List<SpringJoint>();
        public Color defaultColor;

        private Neo4jDatabase database;
        public bool focused = false;

        void Start()
        {
            database = GameObject.FindGameObjectWithTag("NodeSpawner").transform.GetComponent<Neo4jConnection>().currentDatabase;
            foreach (var kvp in properties)
            {
                if (kvp.Key == "rdfs__label")
                {
                    transform.GetComponentInChildren<TextMeshProUGUI>().text = ((string)kvp.Value);
                }
            };
        }

        private void Update()
        {

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            // have text rotate around node
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        public async void DoUnfolding()
        {   
            var result = await database.CustomFetch($"MATCH (n)-[r]-(z) WHERE ID(n) = {nodeID} RETURN z, r LIMIT 3", "z", "r");
            //for (int index = 0; index < result.Count; index++)
            //{
            //    
            //    int id = (int)result[index].Item1.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
            //    var labels = result[index].Item1.Labels; // get Node labels
            //    var _properties = result[index].Item1.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
            //    
            //    var radians = 2 * Math.PI / result.Count * index;
            //    var vertical = MathF.Sin((float)radians);
            //    var horizontal = MathF.Cos((float)radians);
            //    var spawnDir = new Vector3(horizontal, vertical, 0);
            //    var spawnPos = transform.position + spawnDir * 30; // Radius is just the distance away from the point
            //    string prefabText = labels[1].ToLower().Split(new string[] { "__" }, StringSplitOptions.None)[1];
            //    var prefab = Resources.Load($"Prefabs/{prefabText}", typeof(GameObject)) as GameObject;
            //    var node = Instantiate(prefab, spawnPos, Quaternion.identity);
            //    node.name = $"Node_{id}";
            //    node.transform.parent = GameObject.FindGameObjectWithTag("Graph").transform;
            //    var nodeAttributes = node.GetComponent<NodeBehaviour>();
            //    nodeAttributes.nodeID = id;
            //    nodeAttributes.labels = (List<string>)labels;
            //    nodeAttributes.properties = (Dictionary<string, object>)_properties;
            //    nodeAttributes.defaultColor = node.GetComponent<Renderer>().material.color;
            //
            //    expandedNodes.Add(node);
            //    AddEdge(node);
            //}
            //expanded = true;
        }

        public void UndoUnfolding()
        {
            foreach(GameObject node in expandedNodes)
            {
                Destroy(node);
            }
            foreach (GameObject edge in expandedEdges)
            {
                Destroy(edge);
            }
            foreach (SpringJoint joint in joints)
            {
                Destroy(joint);
            }
            expanded = false;
            expandedEdges.Clear();
            expandedEdges.Clear();
            joints.Clear(); 
           
        }

        public void AddEdge(GameObject n)
        {
            //SpringJoint sj = gameObject.AddComponent<SpringJoint>();
            //sj.autoConfigureConnectedAnchor = false;
            //sj.anchor = new Vector3(0, 0.5f, 0);
            //sj.connectedAnchor = new Vector3(0, 0, 0);
            //sj.enableCollision = true;
            //sj.connectedBody = n.transform.GetComponent<Rigidbody>();
            //sj.spring = 8;
            //GameObject edge = Instantiate(EdgePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            //edge.transform.parent = GameObject.FindGameObjectWithTag("Graph").transform;
            //joints.Add(sj);
            //expandedEdges.Add(edge);
        }

    }
}

