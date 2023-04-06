using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

        private GameObject CDE_LD_NODE;
        private GameObject EdgePrefab;
        private Neo4jDatabase database;

        void Start()
        {
            CDE_LD_NODE = Resources.Load("Prefabs/CDE_LNODE", typeof(GameObject)) as GameObject;
            EdgePrefab = Resources.Load("Prefabs/EdgePrefab", typeof(GameObject)) as GameObject;
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
            int i = 0;
            foreach (GameObject edge in expandedEdges)
            {
                edge.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                SpringJoint sj = joints[i];
                GameObject target = sj.connectedBody.gameObject;
                edge.transform.LookAt(target.transform);
                Vector3 ls = edge.transform.localScale;
                ls.z = Vector3.Distance(transform.position, target.transform.position);
                edge.transform.localScale = ls;
                edge.transform.position = new Vector3((transform.position.x + target.transform.position.x) / 2,
                                  (transform.position.y + target.transform.position.y) / 2,
                                  (transform.position.z + target.transform.position.z) / 2);
                i++;
            }
            // have text always face camera
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        public async void DoUnfolding()
        {   
            var result = await database.CustomFetch($"MATCH (n:ns0__APM_CDE)-[r]-(z) WHERE ID(n) = {nodeID} RETURN z, r LIMIT 3", "z", "r");
            for (int index = 0; index < result.Count; index++)
            {
                int id = (int)result[index].Item1.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
                var labels = result[index].Item1.Labels; // get Node labels
                var _properties = result[index].Item1.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs
                
                var radians = 2 * Math.PI / result.Count * index;
                var vertical = MathF.Sin((float)radians);
                var horizontal = MathF.Cos((float)radians);
                var spawnDir = new Vector3(horizontal, vertical, 0);
                var spawnPos = transform.position + spawnDir * 30; // Radius is just the distance away from the point

                var node = Instantiate(CDE_LD_NODE, spawnPos, Quaternion.identity);
                node.name = $"Node_{id}";
                
                var nodeAttributes = node.GetComponent<NodeBehaviour>();
                nodeAttributes.nodeID = id;
                nodeAttributes.labels = (List<string>)labels;
                nodeAttributes.properties = (Dictionary<string, object>)_properties;
                nodeAttributes.defaultColor = node.GetComponent<Renderer>().material.color;

                expandedNodes.Add(node);
                AddEdge(node);
            }
            expanded = true;
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
            SpringJoint sj = gameObject.AddComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.anchor = new Vector3(0, 0.5f, 0);
            sj.connectedAnchor = new Vector3(0, 0, 0);
            sj.enableCollision = true;
            sj.connectedBody = n.transform.GetComponent<Rigidbody>();
            GameObject edge = Instantiate(EdgePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            
            joints.Add(sj);
            expandedEdges.Add(edge);
        }

        public void OnCollisionExit(Collision collision)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}

