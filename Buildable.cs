using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    public Material baseMat;
    private Material[] materials;
    private Material outline;
    public GameObject colorManager;
    private MeshCollider collision;
    private float startValue = 0.5f;
    private float endValue = -1.2f;
    public float cooldownTime = 5f;
    private float lerpDuration = 0.5f;
    private float fastLerpDuration = 0.1f;
    private bool isBuilt;
    private bool isBuildable;

   
    void Start()
    {
        // Initialize buildable in collasped state
        isBuilt = false;
        isBuildable = true;
        materials = GetComponent<Renderer>().materials;
        collision = GetComponent<MeshCollider>();
        materials[0].SetColor("_Color", baseMat.GetColor("_OldColor"));
        materials[1].SetColor("_Color", baseMat.GetColor("_OldColor"));
        gameObject.layer = LayerMask.NameToLayer("Collapsed");

    }

    
    public void BuildOrCollapse(int colorIndex)
    {
        // Check if collapsed object can be built using the inputed color
        if (colorIndex == colorManager.GetComponent<ColorMaterialManager>().Colorindexer(baseMat) && !isBuilt && isBuildable) 
        {
            Debug.Log("building");
            materials[0].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
            materials[1].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
            StartCoroutine(Build(colorIndex));
        }
        // Check if built objectr can be collasped using the inputed color
        else if (colorIndex == colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(baseMat)) && isBuilt)
        {
            Debug.Log("Collapsing");
            materials[0].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorIndex)].GetColor("_OldColor"));
            materials[1].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorIndex)].GetColor("_OldColor"));
            StartCoroutine(Collapse(colorIndex));
        }
    }

    // Build object coroutine
    IEnumerator Build(int colorIndex)
    {
        float timeElapsed = 0;
        // Animate build shader
        while (timeElapsed < lerpDuration)
        {
            materials[0].SetFloat("_CurrentTime", Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Set the new color via shader value, built state, and make the object collider with the player by changing layers
        materials[0].SetFloat("_CurrentTime", endValue);
        isBuilt = true;
        gameObject.layer = LayerMask.NameToLayer("Buildable");

    }

    // Collapse object coroutine
    IEnumerator Collapse(int colorIndex)
    {
        float timeElapsed = 0;
        // Animate build shader backwards
        while (timeElapsed < lerpDuration)
        {
            materials[0].SetFloat("_CurrentTime", Mathf.Lerp(endValue, startValue, timeElapsed / lerpDuration));
            materials[1].SetColor("_Color", Color.Lerp(baseMat.GetColor("_OldColor"), colorManager.GetComponent<ColorMaterialManager>().colors[9].GetColor("_OldColor"), timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Set the new color to transparent via shader value, collapsed state, remove player-object collision by changing layers
        materials[0].SetFloat("_CurrentTime", startValue);
        gameObject.layer = LayerMask.NameToLayer("Collapsed");
        isBuildable = false;
        isBuilt = false;
        // Start build cooldown
        StartCoroutine(Cooldown());
    }

    // Build cooldown coroutine
    IEnumerator Cooldown()
    {
        float timeElapsed = 0;
        // Wait cooldown duration
        yield return new WaitForSeconds(cooldownTime);
        // Lerp outline color from brown to the default color
        while (timeElapsed < lerpDuration)
        {
            materials[1].SetColor("_Color", Color.Lerp(colorManager.GetComponent<ColorMaterialManager>().colors[9].GetColor("_OldColor"), baseMat.GetColor("_OldColor"), timeElapsed / fastLerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Set state to buildable
        isBuildable = true;
        
    }
}
