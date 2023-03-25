using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct NewSceneData
{
    public Vector2 goingDirection;
    public string sceneName;
}

public class SceneManager : MonoBehaviour
{
    private int currentLevel;
    [SerializeField] List<NewSceneData> allScenes;
    [SerializeField] RectTransform BlankObject;
    [SerializeField] AnimationCurve SpeedCurve, cameraCurve;
    private bool loadingScene, transitioning, transitioning2;
    private float time;
    Vector2 newBlankPos, newCameraPos;
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
            currentLevel = 0;
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadingScene)
        {
            transitioning2 = true;
            time = 0;
            Camera.main.transform.position = new Vector3(-newCameraPos.x, -newCameraPos.y, Camera.main.transform.position.z);
        }

    }

    public void StartNextScene()
    {
        if (!loadingScene && currentLevel < allScenes.Count)
        {
            newBlankPos = new Vector2(Screen.width * allScenes[currentLevel].goingDirection.x, Screen.height * allScenes[currentLevel].goingDirection.y);
            newCameraPos = Camera.main.ScreenToWorldPoint(new Vector2(newBlankPos.x + Screen.width / 2, newBlankPos.y + Screen.height / 2));
            Debug.Log(newCameraPos);
            BlankObject.localPosition = newBlankPos;
            transitioning = true;
            loadingScene = true;
            time = 0;
            newSceneName = allScenes[currentLevel].sceneName;
            BlankObject.GetComponent<Image>().color = new Color(BlankObject.GetComponent<Image>().color.r, BlankObject.GetComponent<Image>().color.g, BlankObject.GetComponent<Image>().color.b, 1);
            BlankObject.gameObject.SetActive(true);
            currentLevel++;
            PlayerMovement.playerObject.GetComponent<PlayerMovement>().canMove = false;
        }

    }



    private void Update()
    {
        if (transitioning)
        {
            Vector2 pos = Vector2.Lerp(newBlankPos, Vector2.zero, SpeedCurve.Evaluate(time));
            Vector2 newCameraTempPos = Vector2.Lerp(new Vector2(0, 2.5f),newCameraPos, cameraCurve.Evaluate(time));
            Camera.main.transform.position = new Vector3(newCameraTempPos.x, newCameraTempPos.y, Camera.main.transform.position.z);
            BlankObject.localPosition = pos;
            time += Time.unscaledDeltaTime;
            if (pos == Vector2.zero)
            {
                transitioning = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene(newSceneName);
                GameManager.Instance.ApplyAllOldUpgrades();
            }
        }
        else if (transitioning2)
        {
            Vector2 pos = Vector2.Lerp(Vector2.zero, -newBlankPos, 1 - SpeedCurve.Evaluate(1 - time));
            Vector2 newCameraTempPos = Vector2.Lerp(new Vector2(-newCameraPos.x,-newCameraPos.y+5), new Vector2(0, 2.5f), 1 - cameraCurve.Evaluate(1 - time));
            Camera.main.transform.position = new Vector3(newCameraTempPos.x, newCameraTempPos.y, Camera.main.transform.position.z);
            BlankObject.localPosition = pos;
            time += Time.unscaledDeltaTime;
            if (pos == -newBlankPos)
            {
                transitioning2 = false;
                loadingScene = false;
                BlankObject.gameObject.SetActive(false);
                BlankObject.localPosition = Vector2.zero;
                PlayerMovement.playerObject.GetComponent<PlayerMovement>().canMove = true;
            }
        }
    }
}
