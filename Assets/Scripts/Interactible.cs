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
                GameManager.instance.HaveBucket();
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
                var val = Random.Range(15, 26);
                if(GameManager.instance.getSanity() + val >= 100)
                    GameManager.instance.setSanity(100);
                else
                    GameManager.instance.setSanity(GameManager.instance.getSanity() +  val);
                CanvasAnims.instance.StartNotif("Picked up \"green tea\"");
                Destroy(gameObject);
                break;
            case ItemType.Flower:
                //End menu smec cu pisisca si yippee
                if (GameManager.instance.CheckBucket())
                {
                    GameManager.instance.WinGame();
                }
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
