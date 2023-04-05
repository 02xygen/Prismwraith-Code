using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core_Change : MonoBehaviour
{
    private Material material;
    public Light coreLight;
    public GameObject player;
    private float startValue = -2f;
    private float endValue = 0.3f;
    private float lerpSpeed = 1f;
    private float lerpDuration = 1f;

    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        material.SetColor("_OldColor", Color.white);
        material.SetColor("_NewColor", Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCore(Color oldColor, Color newColor)
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        material.SetColor("_NewColor", newColor);
        StartCoroutine(AnimateShader(oldColor, newColor));
    }

    IEnumerator AnimateShader(Color oldColor, Color newColor)
    {
        float timeElapsed = 0;
        material.SetFloat("_startValue", -2f);
        lerpSpeed = 1;
        startValue = -2f;

        while (timeElapsed < lerpDuration)
        {
            if(player.GetComponent<Gun_Controller>().ejecting == true)
            {
                player.GetComponent<Gun_Controller>().ejecting = false;
                yield return null;
            }
            material.SetFloat("_CurrentTime", Mathf.Lerp(startValue, endValue, (timeElapsed / lerpDuration) * lerpSpeed));
            coreLight.color = Color.Lerp(oldColor, newColor, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_CurrentTime", endValue);
        material.SetColor("_OldColor", gameObject.GetComponent<Renderer>().material.GetColor("_NewColor"));
        material.SetFloat("_CurrentTime", startValue);
    }
}
