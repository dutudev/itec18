using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float sanity = 100, amplitude = 1;
    [SerializeField] private TMP_Text sanityText;
    [SerializeField] private PostProcessProfile ppProfile;
    public static GameManager instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {
        sanity -= Time.deltaTime * 0.6f;
        UpdateSanityText();
        SanityEffect();
    }

    public void SanityEffect()
    {
        sanityText.ForceMeshUpdate();
        Mesh mesh = sanityText.mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < sanityText.text.Length*4-4; i += 4)
        {
            vertices[i] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 1] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 2] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)), Mathf.Cos(Time.time + i) * amplitude);
            vertices[i + 3] += new Vector3(Mathf.Sin(Time.time + Random.Range(0.5f, 1.5f)), Mathf.Cos(Time.time + i) * amplitude);
        }

        mesh.vertices = vertices;
        sanityText.canvasRenderer.SetMesh(mesh);
        //add pp
    }

    public void UpdateSanityText()
    {
        sanityText.text = "Sanity : " + Mathf.FloorToInt(sanity) + "%";
    }
}
