using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable] public struct Downgrade
{
    public string Message;
    public string FunctionToCall;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [NonSerialized] public int currentUpgrade;

    public List<Downgrade> UpgradeFunctions;

    [SerializeField] float DeathTime;
    [SerializeField] Image BlackPanel;
    [SerializeField] AnimationCurve DeathCurve;
    [NonSerialized] public bool playerDead,waitingToReload;
    private bool transitionStage;
    private float time;
    [System.NonSerialized] public bool CanReset = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerDead = false;
            currentUpgrade = 0;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void PlayerDeath()
    {
        if (!playerDead)
        {
            playerDead = true;
            time = 0;
            transitionStage = false;
            BlackPanel.gameObject.SetActive(true);
            waitingToReload = false;
        }
    }

    public void ApplyAllOldUpgrades()
    {
        for(int i = 0;i < currentUpgrade; i++)
        {
            AllDowngrades.Instance.Invoke(UpgradeFunctions[i].FunctionToCall, 0);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerDead)
        {
            if(GameObject.Find("Shard") != null)
            {
                currentUpgrade--;
            }
            ApplyAllOldUpgrades();
            Time.timeScale = 1;
            waitingToReload = false;
            transitionStage = true;
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && CanReset && !playerDead && !GameUI.Instance.InDialog) {
            PlayerDeath();
        }
        if (playerDead && !waitingToReload)
        {
            if (!transitionStage)
            {
                time = Mathf.Min(DeathTime, time + Time.unscaledDeltaTime);
                BlackPanel.color = new Color(BlackPanel.color.r, BlackPanel.color.g, BlackPanel.color.b, Mathf.Lerp(0, 1, DeathCurve.Evaluate(time / DeathTime)));
                if (time == DeathTime)
                {
                    
                    waitingToReload = true;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                    
                }
            }
            else
            {
                time = Mathf.Max(0, time - Time.unscaledDeltaTime);
                BlackPanel.color = new Color(BlackPanel.color.r, BlackPanel.color.g, BlackPanel.color.b, Mathf.Lerp(0, 1, DeathCurve.Evaluate(time / DeathTime)));
                if (time == 0)
                {
                    playerDead = false;
                    BlackPanel.gameObject.SetActive(false);
                }
            }
        }
    }


}
