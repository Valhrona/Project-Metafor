using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody playerRb;
    public bool isOnGround = true;
    private float forceMultiplier = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Obtain horizontal input
        horizontalInput = Input.GetAxis("Horizontal");
        // Obtain vertical input
        verticalInput = Input.GetAxis("Vertical");
        // Horizontal movement
        transform.Translate(Vector3.forward * speed * Time.deltaTime * verticalInput);
        // Vertical movement
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);


       // Only allow player to when on ground
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * forceMultiplier, ForceMode.Impulse);
            isOnGround = false;
        }

        BoundPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Determine whether player is on ground or not;
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
        }
    }

    private void BoundPlayer() {
    }
}
