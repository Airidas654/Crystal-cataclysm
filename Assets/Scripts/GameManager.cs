using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [NonSerialized] public int currentUpgrade;

    public List<string> UpgradeFunctions;

    [SerializeField] float DeathTime;
    [SerializeField] Image BlackPanel;
    [SerializeField] AnimationCurve DeathCurve;
    [NonSerialized] public bool playerDead;
    private bool transitionStage;
    private float time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerDead = false;
            currentUpgrade = 0;
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
        }
    }

    private void Update()
    {
        if (playerDead)
        {
            if (!transitionStage)
            {
                time = Mathf.Min(DeathTime, time + Time.unscaledDeltaTime);
                BlackPanel.color = new Color(BlackPanel.color.r, BlackPanel.color.g, BlackPanel.color.b, Mathf.Lerp(0, 1, DeathCurve.Evaluate(time / DeathTime)));
                if (time == DeathTime)
                {
                    transitionStage = true;
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
