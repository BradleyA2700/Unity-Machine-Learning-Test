using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMotor : MonoBehaviour
{
    float currentSpeed;
    public float maxSpeed = 20;
    float acceleration = 10;
    float breakSpeed = 1000;
    float revSpeed = 5;
    float maxRevSpeed = -40;

    float currentTurnSpeed;
    float maxTurnSpeed = 250;
    float turnAcceleration = 75;

    float noDriveSlow = 10;
    float noDriveTurnSlow = 500;

    float forward;
    float turn;

    public Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (forward > 0)
        {
            //accelerating
            currentSpeed += forward * acceleration * Time.deltaTime;
        }
        else if (forward < 0)
        {
            if (currentSpeed > 0)
            {
                //breaking
                currentSpeed += forward * breakSpeed * Time.deltaTime;
            }
            else
            {
                //reversing
                currentSpeed += forward * revSpeed * Time.deltaTime;
            }
        }
        else 
        {
            //Not Driving
            if (currentSpeed > 0)//Stop sliding forward
            {
                currentSpeed -= noDriveSlow * Time.deltaTime;
            }
            else if (currentSpeed < 0) //Stop sliding back
            {
                currentSpeed += noDriveSlow * Time.deltaTime;
            }
        }
        currentSpeed = Mathf.Clamp(currentSpeed, maxRevSpeed, maxSpeed);

        rb.velocity = transform.forward * currentSpeed;

        if (currentSpeed < 0) 
        {
            turn *= -1f;
        }

        if (turn != 0) 
        {
            if ((currentTurnSpeed > 0 && turn < 0) || (currentTurnSpeed < 0 && turn > 0)) 
            {
                currentTurnSpeed = turn * 20f;
            }
            currentTurnSpeed += turn * turnAcceleration * Time.deltaTime;
        } else {
            if (currentTurnSpeed > 0)
            {
                currentTurnSpeed -= noDriveTurnSlow * Time.deltaTime;
            }
            else if (currentTurnSpeed < 0) 
            {
                currentTurnSpeed += noDriveTurnSlow * Time.deltaTime;
            }
            if (currentTurnSpeed > -1f && currentTurnSpeed < 1f) 
            {
                currentTurnSpeed = 0f;
            }
        }

        float normalizedSpeed = currentSpeed / maxSpeed;
        float negativeNormalizedSpeed = Mathf.Clamp(1 - normalizedSpeed, 0.75f, 1.0f);
        currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, -maxTurnSpeed, maxTurnSpeed);

        rb.angularVelocity = new Vector3(0, currentTurnSpeed * (negativeNormalizedSpeed * 1f) * Mathf.Deg2Rad, 0);
        if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        StopCar();
    }

    public void InputControls(float f, float t) 
    {
        forward = f;
        turn = t;
    }

    public void ClearTurn() 
    {
        currentTurnSpeed = 0;
    }

    public float GetSpeed() 
    {
        return currentSpeed;
    }

    public void SetMaxSpeed(float ms) 
    {
        maxSpeed = ms;
    }

    public void SetMaxTurnSpeed(float mts) 
    {
        maxTurnSpeed = mts;
    }

    public void SetTurnAcceleration(float sta) 
    {
        turnAcceleration = sta;
    }
    public void StopCar() 
    {
        currentSpeed = 0;
        currentTurnSpeed = 0;
    }
}
