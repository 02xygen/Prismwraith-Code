using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orient : MonoBehaviour
{
    public Transform holder;
    public float lerpStrength;
    public float rotLerpStrength;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, holder.position, lerpStrength * Time.deltaTime * (holder.position - transform.position).magnitude);
        transform.rotation = Camera.main.transform.rotation;
    }
}
