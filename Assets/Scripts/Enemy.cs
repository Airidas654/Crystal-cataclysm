using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Vector2 secondPosition;
    [SerializeField] float period;
    [SerializeField] AnimationCurve curve;
    [SerializeField] bool needToFlip; 
    private bool first;
    private float time;
    Vector2 defaultPos;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + secondPosition);
    }
    void Start()
    {
        first = true;
        time = 0;
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            time = Mathf.Min(period, time + Time.deltaTime);
            if (time == period)
            {
                if (needToFlip)
                {
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                }
                first = !first;
            }
        }
        else
        {
            time = Mathf.Max(0, time - Time.deltaTime);
            if (time == 0)
            {
                if (needToFlip)
                {
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                }
                first = !first;
            }
        }
        gameObject.transform.position = Vector2.Lerp(defaultPos, (Vector2)defaultPos + secondPosition, curve.Evaluate(time / period));
    }
}
