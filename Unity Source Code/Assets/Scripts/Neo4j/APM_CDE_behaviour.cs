using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

namespace GraphFoundation
{
    public class APM_CDE_behaviour : MonoBehaviour
    {
        public int nodeID;
        public List<string> labels = new List<string>();
        public Dictionary<string,object> properties = new Dictionary<string,object>();
        LineRenderer drawLine;

        void Start()
        {
            foreach (var kvp in properties)
            {
                if (kvp.Key == "rdfs__label")
                {
                    transform.GetComponentInChildren<TextMeshProUGUI>().text = ((string)kvp.Value).Split(".")[1];
                }
            };
            // Visualise the relationship, using lines
            //drawLine = gameObject.AddComponent<LineRenderer>();
            //drawLine.startWidth = 0.3f;
            //drawLine.endWidth = 0.3f;
            //drawLine.positionCount = 2;
            //drawLine.SetPosition(0,transform.position);
            //drawLine.SetPosition(1, transform.position + new Vector3(30, 30, 30));
        }

        private void Update()
        {
            // Cast a ray from the main camera
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // Define a maximum distance for the raycast
            float maxDistance = 80.0f;

            // Create a RaycastHit object to store the result of the raycast
            RaycastHit hit;

            // Check if the ray intersects with an object within the maximum distance
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                // If the ray intersects with an object, change its color to red
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Change the color of the object to red
                    renderer.material.color = Color.yellow;
                }
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }


            // have text always face camera
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }
    }
}

