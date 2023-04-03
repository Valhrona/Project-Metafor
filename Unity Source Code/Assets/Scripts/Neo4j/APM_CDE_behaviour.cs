using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GraphFoundation
{
    public class APM_CDE_behaviour : MonoBehaviour
    {
        public int nodeID;
        public List<string> labels = new List<string>();
        public Dictionary<string,object> properties = new Dictionary<string,object>();

        public Camera CameraToMove;
        public float MovementSpeed = 0.5F;
        private LineRenderer drawLine;

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
            // have text always face camera
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }

   
    }
}

