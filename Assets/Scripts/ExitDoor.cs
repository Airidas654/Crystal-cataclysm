using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable
{
    public override void Interact()
    {
        SceneManager.Instance.StartNextScene();
    }
}
