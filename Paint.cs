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



    // Set material to starting material
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void ChangeColor(int colorIndex) // Change the objects color to the color matching the given index
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        if (colorIndex != 0) // Check if input is legal
        {
            if (colorIndex == 9) // Check if input color is brown
            {
                // Set shader colors and alpha
                material.SetColor("_NewColor", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
                material.SetInt("_IsDissolving", 1);
                StartCoroutine(AnimateShader(colorIndex)); // Animate shader
            }
            else // If input is legal and not brown
            {
                // Set shader colors
                material.SetColor("_NewColor", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
                StartCoroutine(AnimateShader(colorIndex));
            }
          
        }
        
    }

    IEnumerator AnimateShader(int colorIndex) // Animated shader and disable collsion if nessesary
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

        if (colorIndex == 9) // If color is brown, dissolve the object and disable it
        {
            material.SetColor("_OldColor", gameObject.GetComponent<Renderer>().material.GetColor("_NewColor"));
            gameObject.SetActive(false);
        }
        else // If color isn't brown or illegal, set the current color to the new mixed color
        {
            material.SetColor("_OldColor", gameObject.GetComponent<Renderer>().material.GetColor("_NewColor"));
            material.SetFloat("_CurrentTime", -1);
        }

    }

}
