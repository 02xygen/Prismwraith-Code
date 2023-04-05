using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetMovement : MonoBehaviour
{
    public Transform playerHead;
    public Transform lookTarget;
    public float bodySlerp;
    private Vector3 lookDirection;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = lookTarget.position - playerHead.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, bodySlerp * Time.deltaTime);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.localRotation = targetRotation;
        transform.position = playerHead.transform.position;
    }
}
