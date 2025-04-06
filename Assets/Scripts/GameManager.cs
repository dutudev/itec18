using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float sanity = 100, amplitude = 1, sanitySpeed = 1;
    [SerializeField] private TMP_Text sanityText;
    //[SerializeField] private PostProcessVolume ppProfile;
    
    [SerializeField] private Volume volume;
    private ChromaticAberration chromatic;
    private Vignette vignette;
    private FilmGrain grain;
    private LensDistortion lens;
    [SerializeField] private bool drainSanity = true, hasBucket = false, animateVig = true;

    [SerializeField] private GameObject winMenu, winCat;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioSource winSoundSource;

    [SerializeField] private GameObject loseMenu;
    
    public static GameManager instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (volume.profile.TryGet<Vignette>(out var vign))
        {
            vignette = vign;
        }
        if (volume.profile.TryGet<LensDistortion>(out var lensDis))
        {
            lens = lensDis;
        }
        if (volume.profile.TryGet<FilmGrain>(out var fg))
        {
            grain = fg;
        }

        if (volume.profile.TryGet<ChromaticAberration>(out var ctx))
        {
            chromatic = ctx;
        }
    }

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void VigAnim(bool set)
    {
        animateVig = set;
    }

    // Update is called once per frame
    void Update()
    {
        if (drainSanity)
        {
            sanity -= Time.deltaTime * 0.6f * sanitySpeed;  
        }

        if (sanity <= 0)
        {
            drainSanity = false;
            sanity = 0;
            LoseGame();
        }
        
        UpdateSanityText();
        SanityEffect();
        if (Input.GetKeyDown(KeyCode.N))
        {
            hasBucket = true;
            WinGame();
        }
    }

    public void WinGame()
    {
        if (hasBucket)
        {
            chromatic.intensity.value =0;
            vignette.intensity.value = 0;
            grain.intensity.value = 0;
            lens.intensity.value = 0;
            
            drainSanity = false;
            winMenu.SetActive(true);
            winCat.SetActive(false);
            winMenu.GetComponent<CanvasGroup>().alpha = 0;
            winMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
            winSoundSource.PlayOneShot(winSound);
            LeanTween.alphaCanvas(winMenu.GetComponent<CanvasGroup>(), 1, 1.5f).setEaseOutExpo();
            winCat.GetComponent<RectTransform>().localScale = Vector3.zero;
            LeanTween.delayedCall(1.5f, () => { winCat.SetActive(true); LeanTween.scale(winCat.GetComponent<RectTransform>(), new Vector3(2.8f, 2.8f, 2.8f), 2).setEaseOutElastic(); });
            LeanTween.value(gameObject, 0, 1, 1.5f).setEaseOutExpo().setOnUpdate((value) => { winSoundSource.volume = value;});
            LeanTween.delayedCall(8f, () => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); });
        }
    }

    public Volume GetPPVolume()
    {
        return volume;
    }

    public void LoseGame()
    {
        chromatic.intensity.value =0;
        vignette.intensity.value = 0;
        grain.intensity.value = 0;
        lens.intensity.value = 0;
        loseMenu.SetActive(true);
        loseMenu.GetComponent<CanvasGroup>().alpha = 0;
        loseMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.alphaCanvas(loseMenu.GetComponent<CanvasGroup>(), 1, 1.5f).setEaseOutExpo();
        LeanTween.delayedCall(4f, () => { SceneManager.LoadScene("Main Menu"); });
    }

    public bool CheckBucket()
    {
        return hasBucket;
    }
    
    public void HaveBucket()
    {
        hasBucket = true;
    }

    public void SanityEffect()
    {
        sanityText.ForceMeshUpdate();
        Mesh mesh = sanityText.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < sanityText.text.Length*4-4; i += 4)
        {
            vertices[i] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)*amplitude), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 1] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)*amplitude), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 2] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)*amplitude), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 3] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)*amplitude), Mathf.Cos(Time.time + i) * amplitude);
        }

        mesh.vertices = vertices;
        sanityText.canvasRenderer.SetMesh(mesh);
        //add pp
        if (sanity <= 80 && drainSanity)
        {
            amplitude = Mathf.Lerp(1, 20, (80 - sanity) / 80f);
            /*
            ChromaticAberration chromatic;
            if (ppProfile.profile.TryGetSettings(out chromatic))
            {
                
            }*/
            chromatic.intensity.value = Mathf.Lerp(0, .5f, (80 - sanity) / 80f);
            if (animateVig)
            {
                /*
                Vignette vignette;
                if (ppProfile.profile.TryGetSettings(out vignette))
                { 
                }*/
                vignette.intensity.value = Mathf.Lerp(0, .5f, (80 - sanity) / 80f);
            }
            grain.intensity.value = Mathf.Lerp(0, 1, (80 - sanity) / 80f);
            /*
            Grain grain;
            if (ppProfile.profile.TryGetSettings(out grain))
            {
                
            }*/
            lens.intensity.value = Mathf.Lerp(0, -0.25f, (80 - sanity) / 80f);
            /*
            LensDistortion lens;
            if (ppProfile.profile.TryGetSettings(out lens))
            {
                
            }*/
        }
        
        
    }

    public void UpdateSanityText()
    {
        sanityText.text = "Sanity : " + Mathf.Abs(Mathf.FloorToInt(sanity)) + "%";
    }
    public float getSanity()
    {
        return sanity;
    }

    public void setSanity(float x)
    {
        sanity = x;
    }
}
