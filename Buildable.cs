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


    // Start is called before the first frame update
    void Start()
    {
        isBuilt = false;
        isBuildable = true;
        materials = GetComponent<Renderer>().materials;
        collision = GetComponent<MeshCollider>();
        materials[0].SetColor("_Color", baseMat.GetColor("_OldColor"));
        materials[1].SetColor("_Color", baseMat.GetColor("_OldColor"));
        gameObject.layer = LayerMask.NameToLayer("Collapsed");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildOrCollapse(int colorIndex)
    {
        if (colorIndex == colorManager.GetComponent<ColorMaterialManager>().Colorindexer(baseMat) && !isBuilt && isBuildable) 
        {
            Debug.Log("building");
            materials[0].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
            materials[1].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorIndex].GetColor("_OldColor"));
            StartCoroutine(Build(colorIndex));
        }
        else if (colorIndex == colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(baseMat)) && isBuilt)
        {
            Debug.Log("Collapsing");
            materials[0].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorIndex)].GetColor("_OldColor"));
            materials[1].SetColor("_Color", colorManager.GetComponent<ColorMaterialManager>().colors[colorManager.GetComponent<ColorMaterialManager>().OppositeColor(colorIndex)].GetColor("_OldColor"));
            StartCoroutine(Collapse(colorIndex));
        }
    }

    IEnumerator Build(int colorIndex)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            materials[0].SetFloat("_CurrentTime", Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        materials[0].SetFloat("_CurrentTime", endValue);
        isBuilt = true;
        gameObject.layer = LayerMask.NameToLayer("Buildable");

    }

    IEnumerator Collapse(int colorIndex)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            materials[0].SetFloat("_CurrentTime", Mathf.Lerp(endValue, startValue, timeElapsed / lerpDuration));
            materials[1].SetColor("_Color", Color.Lerp(baseMat.GetColor("_OldColor"), colorManager.GetComponent<ColorMaterialManager>().colors[9].GetColor("_OldColor"), timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        materials[0].SetFloat("_CurrentTime", startValue);
        gameObject.layer = LayerMask.NameToLayer("Collapsed");
        isBuildable = false;
        isBuilt = false;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        float timeElapsed = 0;
        yield return new WaitForSeconds(cooldownTime);
        while (timeElapsed < lerpDuration)
        {
            materials[1].SetColor("_Color", Color.Lerp(colorManager.GetComponent<ColorMaterialManager>().colors[9].GetColor("_OldColor"), baseMat.GetColor("_OldColor"), timeElapsed / fastLerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isBuildable = true;
        
    }
}
