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
                GameManager.Instance.Invoke(GameManager.Instance.UpgradeFunctions[GameManager.Instance.currentUpgrade], 0);
                GameManager.Instance.currentUpgrade++;
            }
            GameObject.Find("ExitDoor").GetComponent<ExitDoor>().OpenDoor();
            Destroy(gameObject);
        }
    }
}
