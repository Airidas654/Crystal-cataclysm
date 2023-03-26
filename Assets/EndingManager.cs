using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] float blackScreenTime = 0.5f;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] Image localBlackScreen;
    
    float animTime = 1;

    public void endGame()
    {
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        if (GameUI.Instance != null)
        {
            Destroy(GameUI.Instance.gameObject);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    void Start()
    {
        GameUI.Instance.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        localBlackScreen.color = new Color(0, 0, 0, 1);
        if (GameUI.Instance != null)
        {
            Destroy(GameUI.Instance.transform.GetChild(0).gameObject);
        }

        animTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animTime < 1)
        {
            animTime += Time.deltaTime / blackScreenTime;
            animTime = Mathf.Max(0, Mathf.Min(1, animTime));
            float val = animCurve.Evaluate(animTime);

            localBlackScreen.color = new Color(0, 0, 0, 1-val);
        }
    }

    public void Crack()
    {
        SoundManager.Instance.PlayOneShot("Crack");
    }

    public void Place()
    {
        SoundManager.Instance.PlayOneShot("Place");
    }
}
