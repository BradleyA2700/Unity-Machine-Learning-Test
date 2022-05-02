using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStandardAI : MonoBehaviour
{

    public GameObject targetObject;

    CarMotor carMotor;

    private void Awake()
    {
        carMotor = GetComponent<CarMotor>();
    }

    public void ChangeTarget(GameObject to) 
    {
        targetObject = to;
    }

    void Update()
    {
        Vector3 targetLocation = targetObject.transform.position;
        Vector3 vectorToTarget = (targetLocation - transform.position).normalized;
        
        float forward, turn;
        float distanceToTarget = Vector3.Distance(transform.position, targetLocation);

        if (distanceToTarget > 5f)
        {
            bool targetForward = Vector3.Dot(transform.forward, vectorToTarget) > 0;

            if (targetForward)
            {
                forward = 1;
            }
            else { forward = -1; }

            bool targetRight = Vector3.SignedAngle(transform.forward, vectorToTarget, Vector3.up) > 0;
            if (targetRight)
            {
                turn = 1;
            }
            else { turn = -1; }

            
        }
        else 
        {
            if (carMotor.GetSpeed() > 20f) {
                forward = -1f;
            }else{
                forward = 0;
            }
            turn = 0;

            if (targetObject.GetComponent<CheckPointSystem>().nextCheckpoint != null)
            {
                targetObject = targetObject.GetComponent<CheckPointSystem>().nextCheckpoint;
            }
            else { //race completed
                 }
        }
        carMotor.InputControls(forward, turn);
    }
        
}
