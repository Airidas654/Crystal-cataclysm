using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] float timeUntilBreaks;
    [SerializeField] float regenTime;
    bool isBreaking = false;
    bool isRegening = false;
    float time;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player" && !isBreaking && !isRegening)
        {
            time = 0;
            isBreaking = true;
            GetComponent<ParticleSystem>().Play();
        }
    }

    private void Update()
    {
        if (isBreaking)
        {
            time += Time.deltaTime;
            if(time >= timeUntilBreaks)
            {
                time = 0;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<ParticleSystem>().Stop();
                isBreaking = false;
                isRegening = true;
            }
        }else if (isRegening)
        {
            time += Time.deltaTime;
            if (time >= regenTime)
            {
                GetComponent<Collider2D>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                isRegening = false;
            }
        }
    }
}
