using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalShard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (GameManager.Instance.currentUpgrade < GameManager.Instance.UpgradeFunctions.Count)
            {
                GameUI.Instance.StartMessageAnim(GameManager.Instance.UpgradeFunctions[GameManager.Instance.currentUpgrade].Message, GameManager.Instance.UpgradeFunctions[GameManager.Instance.currentUpgrade].FunctionToCall);
                
               
                GameManager.Instance.currentUpgrade++;
            }
            GameObject.Find("ExitDoor").GetComponent<ExitDoor>().OpenDoor();
            Destroy(gameObject);
        }
    }
}
