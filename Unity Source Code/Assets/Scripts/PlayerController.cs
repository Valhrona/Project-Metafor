using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private float frontalMovementInput;
    private float sidewayMovementInput;

    private float horizontalCameraInput;
    private float verticalCameraInput;

    private float verticalMovement;

    private Rigidbody playerRb;
    private float forceMultiplier = 10.0f;
    public float sensitivity = 10f;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Obtain frontal movement input
        frontalMovementInput = Input.GetAxis("Horizontal");
        // Obtain sideways movement input
        sidewayMovementInput = Input.GetAxis("Vertical");
        // Obtain horizontal camera movement input
        horizontalCameraInput = Input.GetAxis("Mouse X");
        // Obtain vertical camera movement input
        verticalCameraInput = Input.GetAxis("Mouse Y");
        // Obtain vertical movement input
        verticalMovement = Input.GetAxis("QandE");
        // Forward movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime * frontalMovementInput);
        // Sideways movement
        transform.Translate(Vector3.right * speed * Time.deltaTime * sidewayMovementInput);
        // Vertical movement
        transform.Translate(Vector3.up * speed * Time.deltaTime * verticalMovement);
        // Only rotate if inside playmode screen
        if (!screenRect.Contains(Input.mousePosition))
            return;
        // Camera movement
        transform.Rotate(0, horizontalCameraInput * sensitivity, 0);
        transform.Rotate(-verticalCameraInput * sensitivity, 0, 0);
        // Lock cursor in window and disable presence
        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;

    }
}
