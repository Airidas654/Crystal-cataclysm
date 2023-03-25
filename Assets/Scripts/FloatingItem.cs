using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] float maxHeight;
    [SerializeField] float period;
    [SerializeField] AnimationCurve curve;
    private bool up;
    private float time;
    Vector2 defaultPos;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + maxHeight, transform.position.z));
    }
    void Start()
    {
        up = true;
        time = 0;
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (up)
        {
            time = Mathf.Min(period, time + Time.deltaTime);
            if(time == period)
            {
                up = !up;
            }
        }
        else
        {
            time = Mathf.Max(0, time - Time.deltaTime);
            if (time == 0)
            {
                up = !up;
            }
        }
        gameObject.transform.position = Vector2.Lerp(defaultPos, new Vector3(defaultPos.x, defaultPos.y + maxHeight), curve.Evaluate(time / period));
    }
}
