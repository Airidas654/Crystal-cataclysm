using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public override void Interact()
    {
        SceneManager.Instance.StartNextScene(new Vector2(0, 1),"");
    }
}
