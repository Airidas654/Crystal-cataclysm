using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] float blackScreenTime = 0.5f;
    [SerializeField] AnimationCurve animCurve;
    
    float animTime = 1;
    Image blackScreen;

    public void endGame()
    {
        if (GameUI.Instance != null)
        {
            Destroy(GameUI.Instance.gameObject);
        }
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    void Start()
    {
        animTime = 0;
        blackScreen = GameUI.Instance.transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animTime < 1)
        {
            animTime += Time.deltaTime / blackScreenTime;
            animTime = Mathf.Max(0, Mathf.Min(1, animTime));
            float val = animCurve.Evaluate(animTime);

            blackScreen.color = new Color(0, 0, 0, 1-val);
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
