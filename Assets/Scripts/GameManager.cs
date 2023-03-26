using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct Downgrade
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
    [NonSerialized] public bool playerDead, waitingToReload;
    private bool transitionStage;
    private float time;
    [System.NonSerialized] public bool CanReset = false;

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
            SoundManager.Instance.Play("Death");
            playerDead = true;
            time = 0;
            Time.timeScale = 0;
            transitionStage = false;
            BlackPanel.gameObject.SetActive(true);
            waitingToReload = false;
        }
    }

    public void ApplyAllOldUpgrades()
    {
        for (int i = 0; i < currentUpgrade; i++)
        {
            AllDowngrades.Instance.Invoke(UpgradeFunctions[i].FunctionToCall, 0);
        }
    }


    bool shardPaimtas;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerDead)
        {
            if (GameObject.Find("Shard") != null && shardPaimtas)
            {
                currentUpgrade--;
            }
            ApplyAllOldUpgrades();
            waitingToReload = false;
            transitionStage = true;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && CanReset && !playerDead && !GameUI.Instance.InDialog && !SceneManager.Instance.loadingScene)
        {
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

                    shardPaimtas = (GameObject.Find("Shard") == null);

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
                    Time.timeScale = 1;
                    BlackPanel.gameObject.SetActive(false);
                }
            }
        }
    }


}
