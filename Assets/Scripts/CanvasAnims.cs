using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CanvasAnims : MonoBehaviour
{
    [SerializeField] private TMP_Text interactText;
    
    [SerializeField] private RectTransform panel, notifPanel;
    [SerializeField] private GameObject notifPrefab;
    public static CanvasAnims instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        ResetToOriginBottom();
    }

    public void SetInteractText(string input)
    {
        interactText.text = input;
    }

    public void StartNotif(string input)
    {
        var notifpanel = Instantiate(notifPrefab, new Vector3(0, 150, 0), quaternion.identity, notifPanel);
        var notifText = notifpanel.GetComponentInChildren<TMP_Text>();
        notifText.text = input;
        Destroy(notifpanel, 5);
        LeanTween.move(notifpanel.GetComponent<RectTransform>(), new Vector3(0, -100, 0), 1).setEaseOutExpo();
        LeanTween.move(notifpanel.GetComponent<RectTransform>(), new Vector3(0, 150, 0), 1).setEaseOutExpo().setDelay(4f);
    }

    public void ResetToOriginBottom()
    {
        if (panel == null || interactText.gameObject == null)
        {
            return;
        }
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        LeanTween.scale(panel, new Vector2(1, 1), panel.localScale.y - 1).setEaseOutExpo().setOnComplete(ResetScale);
        LeanTween.alphaCanvas(interactText.GetComponent<CanvasGroup>(), 0, (panel.localScale.x - 1) / 2).setEaseOutExpo();
    }

    public void AnimateInteract()
    {
        if (panel == null || interactText.gameObject == null)
        {
            return;
        }
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        SetInteractText("Interact : E");
        LeanTween.scale(panel, new Vector2(1f, 1.8f), 1.8f - panel.localScale.y).setEaseOutExpo();
        LeanTween.alphaCanvas(interactText.GetComponent<CanvasGroup>(), 1, (panel.localScale.y - 1) / 2).setEaseOutExpo();
    }

    public void AnimateEndInteract()
    {
        if (panel == null || interactText.gameObject == null)
        {
            return;
        }
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        LeanTween.scale(panel, new Vector2(1f, 1f), panel.localScale.y - 1).setEaseOutExpo();
        LeanTween.alphaCanvas(interactText.GetComponent<CanvasGroup>(), 0, (panel.localScale.y - 1) / 2).setEaseOutExpo().setOnComplete(
            () =>
            {
                SetInteractText("Interact : E");
            });
    }

    public void ResetScale()
    {
        if (panel == null || interactText.gameObject == null)
        {
            return;
        }
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        SetInteractText("Interact : E");
        panel.localScale = new Vector3(1, 1, 1);
        interactText.GetComponent<CanvasGroup>().alpha = 0;
    }
}
