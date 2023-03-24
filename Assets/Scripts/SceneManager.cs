using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private int currentLevel;

    [SerializeField]

    public static SceneManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartNextScene(int direction)
    {

    }
}
