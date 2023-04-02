using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    public Quaternion xQuat;
    public Quaternion yQuat;
    private float speed = 10.0f;
    private float frontalMovementInput;
    private float sidewayMovementInput;

    private float verticalMovement;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    public Vector2 rotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Obtain frontal movement input
        frontalMovementInput = Input.GetAxisRaw("Vertical");
        // Obtain sideways movement input
        sidewayMovementInput = Input.GetAxisRaw("Horizontal");
        // Obtain vertical movement input
        verticalMovement = Input.GetAxis("QandE");
        // Forward movement
        transform.position += transform.forward * Time.deltaTime * speed * frontalMovementInput;
        // Sideways movement
        transform.position += transform.right * Time.deltaTime * speed * sidewayMovementInput;
        // Vertical movement
        transform.Translate(Vector3.up * speed * Time.deltaTime * verticalMovement);
        // Only rotate if inside playmode screen
        if (!screenRect.Contains(Input.mousePosition))
            return;
        // Camera movement
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
       
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
        // Lock cursor in window and disable presence
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
