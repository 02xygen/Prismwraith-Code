using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Gun_Controller : MonoBehaviour
{
    public Animator gunAnim;
    public Transform gunTrans;
    public GameObject core;
    public AudioSource gunAudio;
    public AudioSource targetAudio;
    public AudioClip shootSound;
    public AudioClip siphonSound;
    public AudioClip siphonFail;
    public AudioClip ejectSound;
    public bool ejecting = false;
    public AudioClip colorChangeSound;
    public Light coreLight;
    public VisualEffect suction;
    public GameObject target;
    public GameObject laser;
    public ParticleSystem laserTrail;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public ParticleSystem ejectedParticles;
    public Color storedColor = Color.white;
    public Material storedMat;
    public LayerMask canvasLayer;
    public LayerMask buildLayer;
    public LayerMask collapseLayer;
    public GameObject colorManager;
    public GameObject pitchManager;
    public float recoilTime = 1f;
    public float suctionTime = 1f;
    public float ejectTime = 1f;
    private float lastActionTime;
    private float lastEjectTime;


    private void Start()
    {
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time - lastActionTime < recoilTime)
                return;
            gunAnim.SetTrigger("Shoot");
            Shoot();
            lastActionTime = Time.time;
        }
            
    }
    public void OnSiphon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time - lastActionTime < recoilTime)
                return;
            gunAnim.SetTrigger("Siphon");
            Siphon();
            lastActionTime = Time.time;
        }
            
    }
    public void OnEject(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (storedColor != Color.white)
            {
                if (Time.time - lastEjectTime < recoilTime)
                    return;
                ejecting = true;
                gunAnim.SetTrigger("Eject");
                Eject();
                lastEjectTime = Time.time;
            }
            return;
        }
            
    }

    void Shoot()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitinfo, 100, canvasLayer);
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit buildHitInfo, 100, buildLayer);
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit collapseHitInfo, 100, collapseLayer);
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitpoint, 100);

        if (hitinfo.collider != null)
        {
            laser.transform.LookAt(hitpoint.point);
            laser.GetComponent<ParticleSystem>().Play();
            Material targetMat = hitinfo.collider.gameObject.GetComponent<Renderer>().material;
            int mixedColor = colorManager.GetComponent<ColorMaterialManager>().ColorComparer(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat), colorManager.GetComponent<ColorMaterialManager>().Colorindexer(targetMat));
            if (mixedColor != 0)
            {
                hitinfo.collider.GetComponent<Paint>().ChangeColor(mixedColor);

                target.transform.position = hitinfo.point;

                targetAudio.clip = colorChangeSound;
                targetAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(mixedColor);
                targetAudio.Play();
            }
        }

       if (buildHitInfo.collider != null)
       {
            Debug.Log("hit buildable");
            buildHitInfo.collider.GetComponent<Buildable>().BuildOrCollapse(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
            target.transform.position = buildHitInfo.point;
            targetAudio.clip = colorChangeSound;
       }


        if (collapseHitInfo.collider != null)
        {
            Debug.Log("hit collapsed");
            collapseHitInfo.collider.GetComponent<Buildable>().BuildOrCollapse(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
            target.transform.position = collapseHitInfo.point;
            targetAudio.clip = colorChangeSound;
        }

        if (hitpoint.collider != null)
            laser.transform.LookAt(hitpoint.point);
        else
            laser.transform.rotation = gunTrans.rotation;
        
        laser.GetComponent<ParticleSystem>().Play();

        gunAudio.clip = shootSound;
        gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
        gunAudio.Play();

    }

    void Siphon()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitinfo, 100, canvasLayer);

        if (hitinfo.collider != null)
        {
            target.transform.position = hitinfo.point;
            if (hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor") != colorManager.GetComponent<ColorMaterialManager>().colors[1].GetColor("_OldColor"))
            {
                core.GetComponent<Core_Change>().ChangeCore(storedColor, hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor"));
                storedColor = hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor");
                storedMat = hitinfo.collider.gameObject.GetComponent<Renderer>().material;
                storedColor.a = 255;

                suction.SetVector4("Color", storedColor);
                suction.Play();
                StartCoroutine(StopSuck());

                gunAudio.clip = siphonSound;
                gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
                gunAudio.Play();
            }
            
        }

        else
        {
            gunAudio.clip = siphonFail;
            gunAudio.pitch = 1;
            gunAudio.Play();
        }
        var main = laser.GetComponent<ParticleSystem>().main;
        main.startColor = storedColor;
        var laserMain = laserTrail.main;
        laserMain.startColor = storedColor;
        var ejectMain = ejectedParticles.main;
        ejectMain.startColor = storedColor;
        var muzzleMain = muzzleFlash.main;
        muzzleMain.startColor = storedColor;
        var muzzle2Main = muzzleFlash2.main;
        muzzle2Main.startColor = storedColor;
        
    }

    void Eject()
    {
        gunAudio.clip = ejectSound;
        gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
        gunAudio.Play();

        ejectedParticles.Play();
        core.GetComponent<Renderer>().material.SetColor("_OldColor", Color.white);
        core.GetComponent<Renderer>().material.SetColor("_NewColor", Color.white);
        storedColor = Color.white;
        coreLight.color = Color.white;
        storedMat = colorManager.GetComponent<ColorMaterialManager>().colors[1];
        var main = laser.GetComponent<ParticleSystem>().main;
        main.startColor = Color.white;
        var laserMain = laserTrail.main;
        laserMain.startColor = Color.white;
        var muzzleMain = muzzleFlash.main;
        muzzleMain.startColor = Color.white;
        var muzzle2Main = muzzleFlash2.main;
        muzzle2Main.startColor = Color.white;
    }

    IEnumerator StopSuck()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        suction.Stop();
    }
}
