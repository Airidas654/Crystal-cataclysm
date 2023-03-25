using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance = null;
    public bool InDialog { get; private set; }
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
    bool blackBackgroundOn;

    [SerializeField] bool showPlayerText = false;

    [SerializeField] AnimationCurve messageAnimCurve;

    [SerializeField] GameObject mainUi;

    string functionToCall;

    bool startPressed,loading, inStart = true;
    [SerializeField] Image BlackScreen;
    [SerializeField] float maxCameraSpeed;
    [SerializeField] AnimationCurve blackCurve;
    [SerializeField] AnimationCurve cameraMoveCurve;
    [SerializeField] float TransitionSpeed;
    float time;


    public void StartGame()
    {
        if (!startPressed)
        {
            mainUi.SetActive(false);
            startPressed = true;
            time = 0;
            BlackScreen.gameObject.SetActive(true);
        }
    }


    enum animationType
    {
        messageOpen,
        messageClose
    }

    public void StartMessageAnim(string text, string InteractEvent = "", bool blackBackgroundOn = false)
    {
        if (GameManager.Instance.playerDead)
        {
            return;
        }
        messageAnimTime = 1;
        currentAnimation = animationType.messageOpen;
        messageText.text = text;
        messageUI.gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerMovement.playerObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        InDialog = true;
        functionToCall = InteractEvent;
        if (blackBackgroundOn)
        {
            this.blackBackgroundOn = blackBackgroundOn;
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, 0);
            BlackScreen.gameObject.SetActive(true);
        }
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
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        playerTextText = playerTextRect.GetComponent<TMPro.TextMeshProUGUI>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (inStart && startPressed)
        {
            loading = true;
            inStart = false;
            startPressed = false;
            time = TransitionSpeed;
        }
    }

    bool waitingForInput = false;

    void Update()
    {

        if (startPressed)
        {
            if (time < TransitionSpeed)
            {
                time += Time.unscaledDeltaTime;
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, Mathf.Lerp(0, 1, blackCurve.Evaluate(time / TransitionSpeed)));
                float newPos = Mathf.Lerp(0, maxCameraSpeed, cameraMoveCurve.Evaluate(time / TransitionSpeed));
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - newPos * Time.unscaledDeltaTime, Camera.main.transform.position.z);
                if (time >= TransitionSpeed)
                {

                    UnityEngine.SceneManagement.SceneManager.LoadScene("1");

                }
            }
        }
        if (loading)
        {
            time -= Time.unscaledDeltaTime;
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, Mathf.Lerp(0, 1, blackCurve.Evaluate(time / TransitionSpeed)));
            if(time <= 0)
            {
                loading = false;
                BlackScreen.gameObject.SetActive(false);
                GameManager.Instance.CanReset = true;
            }
        }

        if (showPlayerText && playerTextAnimTime < 1)
        {
            playerTextAnimTime += Time.unscaledDeltaTime / playerTextTime;
            playerTextAnimTime = Mathf.Max(0, Mathf.Min(1, playerTextAnimTime));
            float val = messageAnimCurve.Evaluate(playerTextAnimTime);
            Color col = playerTextText.color;
            playerTextText.color = new Color(1, col.g, col.b, val);
        }
        if (!showPlayerText && playerTextAnimTime > 0)
        {
            playerTextAnimTime -= Time.unscaledDeltaTime / playerTextTime;
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
            messageAnimTime -= Time.unscaledDeltaTime / messageUITime;
            messageAnimTime = Mathf.Max(0, Mathf.Min(1, messageAnimTime));
            float val = messageAnimCurve.Evaluate(1 - messageAnimTime);
            messageUI.localPosition = new Vector2(0, Mathf.Lerp(-messageUI.sizeDelta.y / 2 - 360, messageUI.sizeDelta.y / 2 - 360, val));
            if (blackBackgroundOn)
            {
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, Mathf.Lerp(0, 1, val));
            }
            if (messageAnimTime <= 0)
            {
                waitingForInput = true;
            }
        }
        if (messageAnimTime > 0 && currentAnimation == animationType.messageClose)
        {
            messageAnimTime -= Time.unscaledDeltaTime / messageUITime;
            messageAnimTime = Mathf.Max(0, Mathf.Min(1, messageAnimTime));
            float val = messageAnimCurve.Evaluate(1 - messageAnimTime);
            messageUI.localPosition = new Vector2(0, Mathf.Lerp(messageUI.sizeDelta.y / 2 - 360, -messageUI.sizeDelta.y / 2 - 360, val));
            if (blackBackgroundOn)
            {
                BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, Mathf.Lerp(1, 0, val));
            }
            if (messageAnimTime <= 0)
            {
                messageUI.gameObject.SetActive(false);
                InDialog = false;
                Time.timeScale = 1;
                if (blackBackgroundOn)
                {
                    BlackScreen.gameObject.SetActive(false);

                }
            }
        }
        if (Input.GetKey(KeyCode.E) && waitingForInput)
        {
            waitingForInput = false;
            currentAnimation = animationType.messageClose;
            messageAnimTime = 1;
            if (functionToCall != "")
            {
                AllDowngrades.Instance.Invoke(functionToCall, 0);
            }
        }

    }
}
