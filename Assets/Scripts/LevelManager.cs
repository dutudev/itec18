using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] private GameObject[] Layouts;
    public static LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLayout(int layoutIndex)
    {
        foreach (var itm in items)
        {
            itm.item.transform.position = itm.layoutTransforms[layoutIndex];
        }
    }
}

[System.Serializable]
public class Item
{
    public GameObject item;
    public Vector3[] layoutTransforms;
}
