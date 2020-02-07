using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject camPivot;

    private const float YMin = -2.0f;
    private const float YMax = 30.0f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    public Vector3 distance;
    public float sensivityX;
    public float sensivityY;

    private float currentX;
    private float currentY;

    float camYPos;

    void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        
        camYPos = gameObject.transform.position.y;
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensivityX;
        currentY += Input.GetAxis("Mouse Y") * sensivityY;
        currentY = Mathf.Clamp(currentY, YMin, YMax);

        //Vector3 desPos = new Vector3(gameObject.transform.position.x, camYPos, gameObject.transform.position.z);
        Vector3 desPos = new Vector3(0f, camYPos, 0f);

        Quaternion desRot = Quaternion.Euler(0f, gameObject.transform.eulerAngles.y, 0f);
        camPivot.transform.position = desPos;
        camPivot.transform.rotation = desRot;
    }

    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * distance;
        camTransform.LookAt(lookAt.position);
    }

}
