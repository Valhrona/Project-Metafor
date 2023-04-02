using System;
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
        public Camera CameraToMove;
        public float MovementSpeed = 0.5F;
        private LineRenderer drawLine;
        private Vector3 NormalVector;
        private Vector3 CameraStartPosition;
        private Vector3 CameraEndPosition;
        private Quaternion CameraStartRotation;
        private Quaternion CameraEndRotation;
        private bool DoMovement = false;
        private float LerpMovement = 0;
        float rotationThreshold = 0.01f; // example rotation threshold


        void Start()
        {
            CameraToMove = Camera.main;
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
            checkIfIntersectsWithCamera();
            MoveToPointUpdate();
            // have text always face camera
            transform.GetComponentInChildren<TextMeshProUGUI>().transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        private void checkIfIntersectsWithCamera()
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
                // Change the color of the object to yellow
                renderer.material.color = Color.yellow;
                if (Input.GetMouseButtonDown(0))
                {
                    CameraStartPosition = CameraToMove.transform.position;
                    CameraStartRotation = CameraToMove.transform.rotation;
                    CameraEndPosition = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z -50);
                    CameraEndRotation = Quaternion.identity;
                    LerpMovement = 0F;
                    DoMovement = true;
                }
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }
        private void MoveToPointUpdate()
        {
            if (DoMovement)
            {
                CameraToMove.transform.position = Vector3.Lerp(CameraStartPosition, CameraEndPosition, LerpMovement);
                CameraToMove.transform.rotation = Quaternion.Lerp(CameraStartRotation, CameraEndRotation, LerpMovement);
                LerpMovement += Time.deltaTime * MovementSpeed;
                if (LerpMovement >= 1F)
                {
                    LerpMovement = 1F;
                    DoMovement = false;
                    CameraToMove.transform.GetComponent<PlayerController>().rotation.x = 0;
                    CameraToMove.transform.GetComponent<PlayerController>().rotation.y = 0;
                }
            }
        }
    }
}

