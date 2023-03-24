using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;
    [SerializeField] RectTransform messageUI;
    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] float messageUITime;
    float messageAnimTime = 0;
    animationType currentAnimation;

    [SerializeField]AnimationCurve messageAnimCurve;

    enum animationType
    {
        messageOpen,
        messageClose
    }

    public void StartMessageAnim(string text)
    {
        messageAnimTime = 1;
        currentAnimation = animationType.messageOpen;
        messageText.text = text;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool waitingForInput = false;

    void Update()
    {

        if (messageAnimTime > 0 && currentAnimation == animationType.messageOpen)
        {
            messageAnimTime -= Time.deltaTime / messageUITime;
            messageAnimTime = Mathf.Max(0, Mathf.Min(1, messageAnimTime));
            float val = messageAnimCurve.Evaluate(1 - messageAnimTime);
            messageUI.localPosition = new Vector2(Mathf.Lerp(-messageUI.sizeDelta.x / 2 - 640, 0, val), 0);
            if (messageAnimTime <= 0)
            {
                waitingForInput = true;
            }
        }
        if (messageAnimTime > 0 && currentAnimation == animationType.messageClose)
        {
            messageAnimTime -= Time.deltaTime / messageUITime;
            messageAnimTime = Mathf.Max(0, Mathf.Min(1, messageAnimTime));
            float val = messageAnimCurve.Evaluate(1 - messageAnimTime);
            messageUI.anchoredPosition = new Vector2(Mathf.Lerp(0, messageUI.sizeDelta.x / 2 + 640, val), 0);
        }
        if (Input.GetKey(KeyCode.E) && waitingForInput)
        {
            waitingForInput = false;
            currentAnimation = animationType.messageClose;
            messageAnimTime = 1;
        }
    }
}
