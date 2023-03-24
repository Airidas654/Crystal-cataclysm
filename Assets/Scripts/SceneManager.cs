using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private int currentLevel;

    [SerializeField] RectTransform BlankObject;
    [SerializeField] AnimationCurve SpeedCurve;
    private bool loadingScene, transitioning, transitioning2;
    private float time;
    Vector2 newBlankPos;
    string newSceneName;

    public static SceneManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transitioning = false;
            transitioning2 = false;
            loadingScene = false;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadingScene)
        {
            transitioning2 = true;
            time = 0;
            Camera.current.transform.position = -newBlankPos;
        }
    }

    public void StartNextScene(Vector2 direction,string sceneName)
    {
        if (!loadingScene && !transitioning && !transitioning2)
        {
            newBlankPos = new Vector2(Screen.width * direction.x, Screen.height * direction.y);
            BlankObject.localPosition = newBlankPos;
            transitioning = true;
            loadingScene = true;
            time = 0;
            newSceneName = sceneName;
            BlankObject.gameObject.SetActive(true);
        }

    }

    

    private void Update()
    {
        if (transitioning)
        {
            Vector2 pos = Vector2.Lerp(newBlankPos, Vector2.zero, SpeedCurve.Evaluate(time));
            Camera.current.transform.position = Vector2.Lerp(Vector2.zero, newBlankPos, SpeedCurve.Evaluate(time));
            BlankObject.localPosition = pos;
            time += Time.unscaledDeltaTime;
            if(pos == Vector2.zero)
            {
                transitioning = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(newSceneName);
            }
        }else if (transitioning2)
        {
            Vector2 pos = Vector2.Lerp(Vector2.zero, -newBlankPos, SpeedCurve.Evaluate(time));
            Camera.current.transform.position = Vector2.Lerp(-newBlankPos, Vector2.zero, SpeedCurve.Evaluate(time));
            BlankObject.localPosition = pos;
            time += Time.unscaledDeltaTime;
            if (pos == -newBlankPos)
            {
                transitioning2 = false;
                loadingScene = false;
                BlankObject.gameObject.SetActive(false);
            }
        }
    }
}
