using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDoor : Interactable
{
    bool transitioning = false;
    [SerializeField] AnimationCurve transitionCurve;
    [SerializeField] float transitionTime;
    float time;
    Image newBlackScreen;
    public override void Interact()
    {
        transitioning = true;
        time = 0;
        newBlackScreen = GameUI.Instance.transform.GetChild(1).GetComponent<Image>();
        newBlackScreen.color = new Color(newBlackScreen.color.r, newBlackScreen.color.g, newBlackScreen.color.b, 0);
        newBlackScreen.gameObject.SetActive(true);
        SoundManager.Instance.Stop("Music");
    }


    private void Update()
    {
        if (transitioning)
        {
            time += Time.unscaledDeltaTime;
            newBlackScreen.color = new Color(newBlackScreen.color.r, newBlackScreen.color.g, newBlackScreen.color.b, transitionCurve.Evaluate(time / transitionTime));
            if (time >= transitionTime)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Last");
                GameUI.Instance.HidePlayerText();
                transitioning = false;
            }
        }
    }
}
