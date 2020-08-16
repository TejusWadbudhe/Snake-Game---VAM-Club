﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using System.Threading;

//to get the playing ground (detected plane)
public class SceneController : MonoBehaviour
{
    public Camera firstPersonCamera;
    //public ScoreBoardController scoreboard;
    public SnakeController snakeController;
    public GameObject Empty;
    public GameObject endGame;
   // public GameObject snake;


    // Start is called before the first frame update
    void Start()
    {
        QuitOnConnectionErrors();

    }

    // Update is called once per frame
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
     
        ProcessTouches();
       // scoreboard.SetScore(snakeController.GetLength());
       if( Time.timeScale == 0 )
        {
            endGame.SetActive(true);
        }

    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        }
        else if (Session.Status.IsError())
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }

    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount != 1 ||
            (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        } 

        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;
        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            //Frame holds information about ARCore's state including tracking status, 
            //the pose of the camera relative to the world, estimated lighting parameters, 
            //and information on updates to objects (like Planes or Point Clouds) that ARCore 
            //is tracking.
            SetSelectedPlane(hit.Trackable as DetectedPlane);
            //no previous number of foods
            FoodConsumer.ap = 0;
            FoodConsumer.ba = 0;
            FoodConsumer.pi = 0;
           // Empty.SetActive(false);
        }
    }

    //initialising plane to food and snake
    void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        //Debug.Log("Selected plane centered at " + selectedPlane.CenterPose.position);
        snakeController.SetPlane(selectedPlane);
        GetComponent<FoodController>().SetSelectedPlane(selectedPlane);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Body"))
    //    {
    //        EndGame.SetActive(true);
    //        snake.SetActive(false);
    //    }
    //}

}
