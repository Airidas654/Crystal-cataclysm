using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable
{
    bool closed;
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openSprite;
    private void Awake()
    {
        if (GameObject.Find("Shard"))
        {
            closed = true;
            GetComponent<SpriteRenderer>().sprite = closedSprite;
        }
        else
        {
            closed = false;
        }
    }

    public void OpenDoor()
    {
        closed = false;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }

    public override void Interact()
    {
        if (!closed)
        {
            SceneManager.Instance.StartNextScene();
        }
    }
}
