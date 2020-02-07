using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ISOCamera : MonoBehaviour
{
    private Camera myCam;
    Func<Vector3> GetcamPosition;
    Func<float> GetCamZoom;

    float xOffset = -10;
    float yOffset = 10;
    float zOffset = -10;

    public void Setup(Func<Vector3> GetcamPosition, Func<float> GetCamZoom)
    {
        this.GetcamPosition = GetcamPosition;
        this.GetCamZoom = GetCamZoom;
        
    }

    void Start()
    {
        myCam = transform.GetComponent<Camera>();   
    }

    
    void FixedUpdate()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        //Finding playerPosition & offsetting 
        Vector3 cameraPos = GetcamPosition();
        cameraPos.x = cameraPos.x + xOffset;
        cameraPos.y = cameraPos.y + yOffset;
        cameraPos.z = cameraPos.z + zOffset;

        //Math for Cam Smooth
        Vector3 cameraMoveDir = (cameraPos - transform.position).normalized;
        float distance = Vector3.Distance(cameraPos, transform.position);
        float camMoveSpeed = 2f;

        //This Helps with lowered frame rate
        if (distance > 0)
        {
            Vector3 newCamPos = transform.position + cameraMoveDir * distance * camMoveSpeed * Time.deltaTime;

            float distanceAfterMove = Vector3.Distance(newCamPos, cameraPos);

            //if Target is overshot by the cameras movement 
            if (distanceAfterMove > distance)
            {
                newCamPos = cameraPos;
            }
        }

        //Smoothes the cameras movment to look nicer
        transform.position = transform.position + cameraMoveDir * distance * camMoveSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        float camZoom = GetCamZoom();
        float camZoomDif = camZoom - myCam.orthographicSize;
        float camZoomSpeed = 1f;

        myCam.orthographicSize += camZoomDif * camZoomSpeed * Time.deltaTime;

        //Makes sure zoom doesn't go larger zoom value or go smaller then zoom value.
        if (camZoomDif > 0)
        {
            if (myCam.orthographicSize > camZoom)
            {
                myCam.orthographicSize = camZoom;
            }
        }else { 

            if (myCam.orthographicSize < camZoom)
            {
                myCam.orthographicSize = camZoom;
            }
        }
    }
}
