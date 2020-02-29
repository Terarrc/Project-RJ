using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Move
        if (Mathf.Abs(horizontal) < 0.8)
            player.Move(Vector2.zero);
        else
        {

            if (Mathf.Abs(horizontal) < 0.8)
                horizontal = 0;
            else if (horizontal < 0)
               horizontal = -1;
            else
               horizontal = 1;

            if (Mathf.Abs(vertical) < 0.8)
                 vertical = 0;
           else if (vertical < 0)
                vertical = -1;
            else
               vertical = 1;
        }

        player.Move(new Vector2(horizontal, vertical));

        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.StopJump();
        }

        
        if (Input.GetButtonDown("Action 1"))
        {
            player.LaunchRocket();
        }
    }
}
