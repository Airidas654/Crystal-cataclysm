using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalShard : Interactable
{
    public override void Interact()
    {
        if (GameManager.Instance.currentUpgrade < GameManager.Instance.UpgradeFunctions.Count)
        {
            GameManager.Instance.Invoke(GameManager.Instance.UpgradeFunctions[GameManager.Instance.currentUpgrade], 0);
            GameManager.Instance.currentUpgrade++;
        }
    }
}
