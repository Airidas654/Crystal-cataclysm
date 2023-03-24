using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;
    [SerializeField] RectTransform messageUI;
    [SerializeField] RectTransform playerTextRect;
    TMPro.TextMeshProUGUI playerTextText;
    [SerializeField] TMPro.TextMeshProUGUI messageText;
    [SerializeField] float messageUITime;
    [SerializeField] float playerTextTime;
    [SerializeField] Vector2 playerTextOffset;

    float messageAnimTime = 0;
    float playerTextAnimTime = 0;
    animationType currentAnimation;

    [SerializeField] bool showPlayerText = false;

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
        messageUI.gameObject.SetActive(true);
    }

    public void ShowPlayerText(string text)
    {
        playerTextText.text = text;
        showPlayerText = true;
    }

    public void HidePlayerText()
    {
        showPlayerText = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerTextText = playerTextRect.GetComponent<TMPro.TextMeshProUGUI>();
    }

    bool waitingForInput = false;

    void Update()
    {
        if (showPlayerText && playerTextAnimTime < 1)
        {
            playerTextAnimTime += Time.deltaTime / playerTextTime;
            playerTextAnimTime = Mathf.Max(0, Mathf.Min(1, playerTextAnimTime));
            float val = messageAnimCurve.Evaluate(playerTextAnimTime);
            Color col = playerTextText.color;
            playerTextText.color = new Color(1, col.g, col.b, val);
        }
        if (!showPlayerText && playerTextAnimTime > 0)
        {
            playerTextAnimTime -= Time.deltaTime / playerTextTime;
            playerTextAnimTime = Mathf.Max(0, Mathf.Min(1, playerTextAnimTime));
            float val = messageAnimCurve.Evaluate(playerTextAnimTime);
            Color col = playerTextText.color;
            playerTextText.color = new Color(col.r, col.g, col.b, val);
        }
        if (playerTextAnimTime > 0 && PlayerMovement.playerObject != null)
        {
            playerTextRect.position = Camera.main.WorldToScreenPoint(PlayerMovement.playerObject.transform.position + new Vector3(playerTextOffset.x, playerTextOffset.y));
        }
        //if (Input.GetKey(KeyCode.F)) StartMessageAnim("lololol");

        if (messageAnimTime > 0 && currentAnimation == animationType.messageOpen)
        {
            messageAnimTime -= Time.deltaTime / messageUITime;
            messageAnimTime = Mathf.Max(0, Mathf.Min(1, messageAnimTime));
            float val = messageAnimCurve.Evaluate(1 - messageAnimTime);
            messageUI.localPosition = new Vector2(0,Mathf.Lerp(-messageUI.sizeDelta.y / 2 - 360, messageUI.sizeDelta.y / 2 - 360, val));
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
            messageUI.localPosition = new Vector2(0, Mathf.Lerp(messageUI.sizeDelta.y / 2 - 360, -messageUI.sizeDelta.y / 2 - 360, val));
            if (messageAnimTime <= 0)
            {
                messageUI.gameObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.E) && waitingForInput)
        {
            waitingForInput = false;
            currentAnimation = animationType.messageClose;
            messageAnimTime = 1;
        }
    }
}
