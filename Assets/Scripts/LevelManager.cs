using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<GameObject> Layouts = new List<GameObject>();
    [SerializeField] private int currentLayout = -1;
    [SerializeField] private int EDITORLAYOUT, numberoflayouts;
    public static LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            print("notok");
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            print("ok");    
        }
    }

    private void Start()
    {
        ChangeLayout();
    }

    public void ChangeLayout()
    {
        var nextLayout = Random.Range(0, Layouts.Count);
        while (nextLayout == currentLayout)
        {
            nextLayout = Random.Range(0, Layouts.Count);
        }

        currentLayout = nextLayout;
        foreach (var layout in Layouts)
        {
            layout.SetActive(false);
        }
        Layouts[currentLayout].SetActive(true);
        foreach (var itm in items)
        {
            if (itm.item != null)
            {
                itm.item.transform.position = itm.layoutTransforms[currentLayout]; 
            }
            
        }
    }

    public void SaveObj()
    {
        var obj = GameObject.FindGameObjectsWithTag("Interactible");
        foreach (var obiect in obj)
        {
            Item curit = new Item();
            curit.item = obiect;
            for (int i = 0; i < numberoflayouts; i++)
            {
                curit.layoutTransforms.Add(Vector3.zero);
            }
            items.Add(curit);
        }
        obj = GameObject.FindGameObjectsWithTag("Decor");
        foreach (var obiect in obj)
        {
            Item curit = new Item();
            curit.item = obiect;
            for (int i = 0; i < numberoflayouts; i++)
            {
                curit.layoutTransforms.Add(Vector3.zero);
            }
            items.Add(curit);
        }
    }
    
    public void SavePos()
    {
        var obj = GameObject.FindGameObjectsWithTag("Interactible");
        foreach (var obiect in obj)
        {
            int index = items.FindIndex(itm => obiect == itm.item);
            items[index].layoutTransforms[EDITORLAYOUT] = obiect.transform.position;
        }
        obj = GameObject.FindGameObjectsWithTag("Decor");
        foreach (var obiect in obj)
        {
            int index = items.FindIndex(itm => obiect == itm.item);
            items[index].layoutTransforms[EDITORLAYOUT] = obiect.transform.position;
        }
    }
}

[System.Serializable]
public class Item
{
    public GameObject item;
    public List<Vector3> layoutTransforms = new List<Vector3>();
}


