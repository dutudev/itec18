using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private bool canInteract = true;

    [SerializeField] private string interactionRestriction;

    public enum ItemType
    {
        waterCan,
        button,
        potion,
        flower,
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public string GetRestriction()
    {
        return interactionRestriction;
    }
}
