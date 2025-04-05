using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] private string interactionRestriction;
    [SerializeField] private ItemType itemType;
    [SerializeField] private BoxCollider2D triggerCollider;

    public enum ItemType
    {
        WaterCan,
        Button,
        Potion,
        Flower,
    };
    
    
    public void Interact()
    {
        Destroy(triggerCollider);
        triggerCollider = null;
        canInteract = false;
        switch (itemType)
        {
            case ItemType.WaterCan:
                //distruge item si arata ca ai watercan
                Destroy(gameObject);
                CanvasAnims.instance.StartNotif("Picked up water can");
                GameObject.Find("floare").GetComponent<Interactible>().canInteract = true;
                break;
            case ItemType.Button:
                //gaseste usa cu numele butonului si deschide
                CanvasAnims.instance.StartNotif("Opened " + gameObject.name + " door");
                Destroy(GameObject.Find(gameObject.name + "door"));
                //Destroy(GameObject.Find(gameObject.name + "door"));
                break;
            case ItemType.Potion:
                //creste sanitatea
                break;
            case ItemType.Flower:
                //End menu smec cu pisisca si yippee
                break;
        }
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
