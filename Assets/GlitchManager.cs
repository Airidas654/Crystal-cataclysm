using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    public static GlitchManager Instance;

    [SerializeField] float animDuration;
    [SerializeField] AnimationCurve animationCurve;
    [HideInInspector]
    public bool finished = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    List<GameObject> objectsAppear = new List<GameObject>();
    List<GameObject> objectsDisappear = new List<GameObject>();

    public void AddAppearing(GameObject obj)
    {
        objectsAppear.Add(obj);
    }
    public void AddDisappear(GameObject obj)
    {
        objectsDisappear.Add(obj);
    }

    public void ClearAll()
    {
        objectsAppear.Clear();
        objectsDisappear.Clear();
    }

    float timeVal = 1;

    public void StartAnim()
    {
        timeVal = 0;
        finished = false;
        flipped = false;
        foreach(GameObject obj in objectsAppear)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsDisappear)
        {
            obj.SetActive(true);
        }
    }
    bool flipped;
    void Update()
    {
        if (timeVal < 1)
        {
            timeVal += Time.deltaTime / animDuration;
            timeVal = Mathf.Max(Mathf.Min(1, timeVal), 0);

            float val = animationCurve.Evaluate(timeVal);

            if (val > 0.5 && !flipped)
            {
                flipped = true;
                foreach (GameObject obj in objectsAppear)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in objectsDisappear)
                {
                    obj.SetActive(false);
                }
            }else if (val <= 0.5f && flipped)
            {
                flipped = false;
                foreach (GameObject obj in objectsAppear)
                {
                    obj.SetActive(false);
                }
                foreach (GameObject obj in objectsDisappear)
                {
                    obj.SetActive(true);
                }
            }

            if (timeVal >= 1)
            {
                finished = true;
            }
        }
    }
}
