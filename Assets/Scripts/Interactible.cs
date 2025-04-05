using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField] private bool canInteract = true;
    [SerializeField] private string interactionRestriction;
    [SerializeField] private ItemType itemType;

    public enum ItemType
    {
        WaterCan,
        Button,
        Potion,
        Flower,
    };
    
    
    public void Interact()
    {
        switch (itemType)
        {
            case ItemType.WaterCan:
                //distruge item si arata ca ai watercan
                break;
            case ItemType.Button:
                //gaseste usa cu numele butonului si deschide
                Destroy(GameObject.Find(gameObject.name + "door")); /// transform.rotation = Quaternion.Euler(0, 0, GameObject.Find(gameObject.name + "door").transform.rotation.eulerAngles.z - 90); ;
                
                canInteract = false;
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
