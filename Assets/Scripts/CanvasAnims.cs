using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasAnims : MonoBehaviour
{
    [SerializeField] private TMP_Text interactText;
    
    [SerializeField] private RectTransform panel;
    
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

    public void ResetToOriginBottom()
    {
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        LeanTween.scale(panel, new Vector2(1, 1), panel.localScale.y - 1).setEaseOutExpo().setOnComplete(ResetScale);
        LeanTween.alphaCanvas(interactText.GetComponent<CanvasGroup>(), 0, (panel.localScale.x - 1) / 2).setEaseOutExpo();
    }

    public void AnimateInteract()
    {
        LeanTween.cancel(panel);
        LeanTween.cancel(interactText.gameObject);
        SetInteractText("Interact : E");
        LeanTween.scale(panel, new Vector2(1f, 1.8f), 1.8f - panel.localScale.y).setEaseOutExpo();
        LeanTween.alphaCanvas(interactText.GetComponent<CanvasGroup>(), 1, (panel.localScale.y - 1) / 2).setEaseOutExpo();
    }

    public void AnimateEndInteract()
    {
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
        LeanTween.cancel(gameObject);
        SetInteractText("Interact : E");
        panel.localScale = new Vector3(1, 1, 1);
        interactText.GetComponent<CanvasGroup>().alpha = 0;
    }
}
