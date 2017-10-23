using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //reference to the camera
    public Camera cam = null;

    //camera parameters
    public float distance = 0.0f;
    public float FOV = 0.0f;

    [Range(0, 90)]
    public float cameraAngle = 0.0f;

    public float cameraSpeed = 5.0f;

    [Range(0.0f, 5.0f)]
    public float smoothingSpeed = 0.0f;
    
    [Range(0.0f, 2.0f)]
    public float transitionLinearTimr = 0.0f;

    [Range(0.0f, 2.0f)]
    public float transitionRotationTime = 0.0f;

    //focal movement limits
    public Rect limits;

    //automated reference to the input
    private CustomInput input = null;

    //smoothed vector
    private Vector2 smoothInput = Vector2.zero;

	// Use this for initialization
	void Start ()
    {
        input = GameObject.FindObjectOfType<CustomInput>();
	}

    void ClampPosition()
    {
        if (transform.position.x < limits.x)
        {
            transform.position = new Vector3(limits.x, transform.position.y, transform.position.z);
            smoothInput.x = 0.0f;
        }

        if (transform.position.x > limits.x + limits.width)
        {
            transform.position = new Vector3(limits.x + limits.width, transform.position.y, transform.position.z);
             smoothInput.x = 0.0f;
        }

        if (transform.position.z < limits.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limits.y);
            smoothInput.y = 0.0f;
        }

        if (transform.position.z > limits.y + limits.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limits.y + limits.height);
            smoothInput.y = 0.0f;
        }

    }

    // Update is called once per frame
    void Update ()
    {
        //set the distance through the local z of the camera because it is parented to this object
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, distance, cam.transform.localPosition.z);

        cam.fieldOfView = FOV;
        transform.eulerAngles = new Vector3(cameraAngle - 90.0f, transform.eulerAngles.y, transform.eulerAngles.z);

        //smooth the input
        Vector2 relative = input.keyInput - smoothInput;

        if (relative.magnitude < smoothingSpeed * Time.deltaTime)
        {
            smoothInput = input.keyInput;
        }
        else
        {
            smoothInput += relative.normalized * smoothingSpeed * Time.deltaTime;
        }

        //moves the camera if input is given and snaps the y to 0
        transform.position = new Vector3(transform.position.x + smoothInput.x * cameraSpeed * Time.deltaTime, 0.0f, 
            transform.position.z + smoothInput.y * cameraSpeed * Time.deltaTime);

        ClampPosition();

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
