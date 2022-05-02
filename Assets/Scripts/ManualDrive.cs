using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualDrive : MonoBehaviour
{

    CarMotor cm;
    private void Awake()
    {
        cm = GetComponent<CarMotor>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        cm.InputControls(y, x);
    }
}
