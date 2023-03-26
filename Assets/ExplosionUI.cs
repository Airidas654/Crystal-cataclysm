using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionUI : MonoBehaviour
{
    public void CutSceneEnded()
    {
        GameUI.Instance.StartGame();
    }
    [SerializeField] Image title;
    [SerializeField] Image text;

    bool animationStarted = false;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float dissappearTime = 0.2f;
    float animTime = 1;
    void StartAnimation()
    {
        if (!animationStarted)
        {
            animationStarted = true;
            animTime = 0;
            GetComponent<Animator>().Play("Game start");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && !animationStarted)
        {
            StartAnimation();
        }

        if (animTime < 1)
        {
            animTime += Time.deltaTime / dissappearTime;
            animTime = Mathf.Max(0, Mathf.Min(1, animTime));

            float val = animCurve.Evaluate(1-animTime);

            title.color = new Color(1, 1, 1, val);
            text.color = new Color(1, 1, 1, val);
        }
    }

    public void Crack()
    {
        SoundManager.Instance.Play("Crack");
    }
    public void Explosion()
    {
        SoundManager.Instance.Play("Explosion");
    }
}
