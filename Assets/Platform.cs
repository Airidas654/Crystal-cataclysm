using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D playerCollider;
    BoxCollider2D platformCollider;

    void Start()
    {
        playerCollider = PlayerMovement.playerObject.GetComponent<BoxCollider2D>();
        platformCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    bool collide = true;
    // Update is called once per frame
    void Update()
    {
        float playerBottomPos = playerCollider.transform.position.y - playerCollider.size.y / 2 + playerCollider.offset.y;
        float platformTop = transform.position.y + platformCollider.size.y / 2 + platformCollider.offset.y;

        if (!collide && platformTop < playerBottomPos + 0.1f)
        {
            collide = true;
            platformCollider.enabled = true;
        }
        if (collide && (platformTop > playerBottomPos || playerCollider.GetComponent<PlayerMovement>().onLadder))
        {
            collide = false;
            platformCollider.enabled = false;
        }
    }
}
