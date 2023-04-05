using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    private Material material;
    public GameObject colorManager;
    private float startValue = -1f;
    private float endValue = 0.3f;
    private float lerpDuration = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void ChangeColor(int colorIndex)
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        if (colorIndex != 0)
        {
            if (colorIndex == 9)
            {
                material.SetColor("_NewColor", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
                material.SetInt("_IsDissolving", 1);
                StartCoroutine(AnimateShader(colorIndex));
            }
            else
            {
                material.SetColor("_NewColor", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
                StartCoroutine(AnimateShader(colorIndex));
            }
          
        }
        
    }

    IEnumerator AnimateShader(int colorIndex)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            material.SetFloat("_CurrentTime", Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_CurrentTime", endValue);
        gameObject.GetComponent<Renderer>().material = colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex];
        if (colorIndex == 9)
        {
            material.SetColor("_OldColor", gameObject.GetComponent<Renderer>().material.GetColor("_NewColor"));
            gameObject.SetActive(false);
        }
        else
        {
            material.SetColor("_OldColor", gameObject.GetComponent<Renderer>().material.GetColor("_NewColor"));
            material.SetFloat("_CurrentTime", -1);
        }

    }

}
