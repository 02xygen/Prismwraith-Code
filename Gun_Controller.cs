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

    // If shoot input read
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Cancel shoot action if still recoiling
            if (Time.time - lastActionTime < recoilTime)
                return;

            gunAnim.SetTrigger("Shoot"); // Trigger shoot animation
            Shoot();
            lastActionTime = Time.time; // Save current time 
        }
            
    }

    // If siphon input read
    public void OnSiphon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Cancel siphon if still recoiling
            if (Time.time - lastActionTime < recoilTime)
                return;
            gunAnim.SetTrigger("Siphon"); // Trigger siphon animation
            Siphon();
            lastActionTime = Time.time; // Save current time 
        }
            
    }

    // If eject input read
    public void OnEject(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (storedColor != Color.white) // Cancel eject if the stored color is white
            {
                if (Time.time - lastEjectTime < recoilTime)
                    return;
                ejecting = true;
                gunAnim.SetTrigger("Eject"); // Trigger eject animation
                Eject();
                lastEjectTime = Time.time; // Save current time 
            }
            return;
        }
            
    }

    void Shoot() // Shoot the color gun
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitinfo, 100, canvasLayer); // Check if ray hits a canvasLayer object
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit buildHitInfo, 100, buildLayer); // Check if ray hits a buildLayer object
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit collapseHitInfo, 100, collapseLayer); // Check if ray hits a collapseLayer object
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitpoint, 100); // Check if ray hits anything

        if (hitinfo.collider != null) // If ray hits a canvas object
        {
            laser.transform.LookAt(hitpoint.point); // Orient gun
            laser.GetComponent<ParticleSystem>().Play(); // Play particle effects
            Material targetMat = hitinfo.collider.gameObject.GetComponent<Renderer>().material; // Save targeted material
            // Use the color manager to index and compare the stored and target color
            int mixedColor = colorManager.GetComponent<ColorMaterialManager>().ColorComparer(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat), colorManager.GetComponent<ColorMaterialManager>().Colorindexer(targetMat));
            if (mixedColor != 0) // Check if color mix is legal
            {
                hitinfo.collider.GetComponent<Paint>().ChangeColor(mixedColor); // Use the target object's paint function to change the object's color

                target.transform.position = hitinfo.point;

                targetAudio.clip = colorChangeSound; // Set hitsound
                targetAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(mixedColor); // Set playback pitch using the pitch manager
                targetAudio.Play(); // Play hitsound
            }
        }

       if (buildHitInfo.collider != null) // If ray hits a built object
       {
            Debug.Log("hit built");
            // Use the color manager to index the stored color and pass it into the object's BuildOrCollapse function to collapse the object
            buildHitInfo.collider.GetComponent<Buildable>().BuildOrCollapse(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
            target.transform.position = buildHitInfo.point;
            targetAudio.clip = colorChangeSound;
       }


        if (collapseHitInfo.collider != null) // If ray hits a collapsed object
        {
            Debug.Log("hit collapsed");
            // Use the color manger to index the stored color and pass it into the object's BuildOrCollapse function to build the object
            collapseHitInfo.collider.GetComponent<Buildable>().BuildOrCollapse(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
            target.transform.position = collapseHitInfo.point;
            targetAudio.clip = colorChangeSound;
        }

        if (hitpoint.collider != null) // If the ray hits anything
            laser.transform.LookAt(hitpoint.point); // point the gun foreward
        else
            laser.transform.rotation = gunTrans.rotation; // point the gun towards the target 
        
        laser.GetComponent<ParticleSystem>().Play(); // Play particle effects

        gunAudio.clip = shootSound; // set shoot sound
        // set the playback pitch using the color manager and pitch manager
        gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
        gunAudio.Play(); // Play shoot sound

    }

    void Siphon() // Siphon color from the targeted object
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitinfo, 100, canvasLayer); // Check if ray hits a canvasLayer object

        if (hitinfo.collider != null) // If ray hit a canvas object
        {
            target.transform.position = hitinfo.point;
            // If target's color isn't white
            if (hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor") != colorManager.GetComponent<ColorMaterialManager>().colors[1].GetColor("_OldColor"))
            {
                // Change the core color using the ChangeCore function
                core.GetComponent<Core_Change>().ChangeCore(storedColor, hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor"));
                storedColor = hitinfo.collider.gameObject.GetComponent<Renderer>().material.GetColor("_OldColor");
                storedMat = hitinfo.collider.gameObject.GetComponent<Renderer>().material;
                storedColor.a = 255; // Set stored color alpha to opaque

                suction.SetVector4("Color", storedColor); // Set particle color to the target color
                suction.Play(); // Play suction particles
                StartCoroutine(StopSuck());

                gunAudio.clip = siphonSound; // Set siphon sound
                // Set playback pitch using the color mangager and pitch manager
                gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
                gunAudio.Play(); // Play siphon sound
            }
            
        }

        else // if ray didn't hit a canvasLayer object
        {
            gunAudio.clip = siphonFail;
            gunAudio.pitch = 1;
            gunAudio.Play(); // Play siphonFail sound
        }

        // Set all the gun's particle systems color to the siphoned color
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

    void Eject() // Eject the stored color
    {
        gunAudio.clip = ejectSound;
        gunAudio.pitch = pitchManager.GetComponent<PitchManager>().ColorPitch(colorManager.GetComponent<ColorMaterialManager>().Colorindexer(storedMat));
        gunAudio.Play(); // Play eject sound

        ejectedParticles.Play(); // Play eject particles

        // Set stored color, light color, core color, and particle system color to white
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

    IEnumerator StopSuck() // Stop suction particles after set time elasped
    {
        yield return new WaitForSecondsRealtime(0.5f);
        suction.Stop();
    }
}
