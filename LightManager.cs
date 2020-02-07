using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightManager : MonoBehaviour
{

    GameObject[] lights;
    public GameObject player;

    float[] lightList;
    float distanceI;
    float distanceX;
    float playerDistance;

    const float _Timer = 0.8f;
    float timer;

    Vector3 playerPos;
    Vector3 oldPlayerPos;

    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("light");

        lightList = new float[lights.Length];

        timer = _Timer;

        FindLights();

        oldPlayerPos = player.transform.position;
    }

    
    void Update()
    {
        playerPos = player.transform.position;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            playerDistance = Vector3.Distance(oldPlayerPos, playerPos);

            if(playerDistance > 3f)
            {
                FindLights();
            }
            else
            {
                resetTimer();
            }
        }

    }

    void FindLights()
    {
        //Find a Light 
        for (int i = 0; i < lights.Length; i++)
        {
            //Find Second Light 
            for(int x = i + 1; x < lights.Length; x++)
            {
                //Get the distance between both lights and the player  
                distanceI = Vector3.Distance(lights[i].transform.position, player.transform.position);
                distanceX = Vector3.Distance(lights[x].transform.position, player.transform.position);

                //Whatever is the larger light shift down
                if (distanceI > distanceX)
                {
                    GameObject Templight = lights[i];
                    lights[i] = lights[x];
                    lights[x] = Templight;
                }

            }
        }

        for (int i = 0; i < lights.Length; i++)
        {
            //Enable the top 4 lights of the list (would be the closed 4 lights to the player).
            if (i < 4)
            {
                lights[i].SetActive(true);
            }
            else
            {
                lights[i].SetActive(false);
            }
        }


        resetTimer();
    }

    void resetTimer()
    {
        timer = _Timer;

        oldPlayerPos = player.transform.position;
    }
}
