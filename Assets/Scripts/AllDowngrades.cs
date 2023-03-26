using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDowngrades : MonoBehaviour
{
    public static AllDowngrades Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void FasterSpeed()
    {
        PlayerMovement.playerObject.GetComponent<PlayerMovement>().moveSpeed = 12;
    }

    public void SmallerJump()
    {
        PlayerMovement.playerObject.GetComponent<PlayerMovement>().jumpStrength = 11;
    }

    public void FlipCamera()
    {
        Camera.main.transform.rotation = Quaternion.Euler(0,0,180);
        PlayerMovement.playerObject.GetComponent<PlayerMovement>().flipped = true;
    }

    public void SliperyGround()
    {
        PlayerMovement.playerObject.GetComponent<PlayerMovement>().onIce = true;
    }

    public void SpawnEnemies()
    {
        if (GameObject.Find("NeedToBeSpawned") != null)
        {
            foreach (Transform enemy in GameObject.Find("NeedToBeSpawned").transform)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }

}
