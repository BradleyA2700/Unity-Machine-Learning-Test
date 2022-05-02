using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MLDriver : Agent
{
    CarMotor cm;
    Vector3 startPos;
    Quaternion startRot;
    public GameObject startObject;
    GameObject nextCheckPoint;
    float cumalitiveNegativeRewards;
    float timeToComplete;
    public static float targetCompleteTime = 100000000;
    public float publicTargetCompleteTime;
    private void Update()
    {
        if (cumalitiveNegativeRewards < -2) { //EndEpisode();
                                            }
        publicTargetCompleteTime = targetCompleteTime;
    }
    private void FixedUpdate()
    {
        timeToComplete += Time.deltaTime;
    }
    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        cm = GetComponent<CarMotor>();
        nextCheckPoint = startObject;
    }
    public override void OnEpisodeBegin()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        cm.StopCar();
        nextCheckPoint = startObject;
        cumalitiveNegativeRewards = 0;
        timeToComplete = 0;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(cm.rb.velocity);
        sensor.AddObservation(startRot);
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        float y = 0f;
        float x = 0f;
        //Debug.Log(vectorAction);
        switch (vectorAction[0]) 
        {
            case 0:
                y = 0f;
                break;
            case 1:
                y = +1f;
                break;
            case 2:
                y = -1f;
                AddReward(-0.01f);
                break;
        }
        switch (vectorAction[1])
        {
            case 0:
                x = 0f;
                break;
            case 1:
                x = +1f;
                break;
            case 2:
                x = -1f;
                break;
        }
        
        cm.InputControls(y, x);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            AddReward(-2f);
            //EndEpisode();
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.05f);
            cumalitiveNegativeRewards -= 0.05f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint") 
        {
            CheckPointSystem cps = other.GetComponent<CheckPointSystem>();
            if (other.gameObject == nextCheckPoint)
            {
                AddReward(1f);
                if (cps.nextCheckpoint == null)
                {
                    if (timeToComplete <= targetCompleteTime)
                    {
                        
                        if (timeToComplete < targetCompleteTime)
                        {
                            AddReward(300);
                        }
                        else 
                        {
                            AddReward(240);
                        }
                        targetCompleteTime = timeToComplete;
                    }
                    else 
                    {
                        AddReward(200);
                    }
                    AddReward(-timeToComplete);
                    EndEpisode();
                }
                else { nextCheckPoint = cps.nextCheckpoint; }
            }
            else {
                AddReward(-1f);
                cumalitiveNegativeRewards += (-1f);
            }
            
        }
    }
}

