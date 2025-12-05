using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public PlayerState playerState = PlayerState.unchanged;
    void Update()
    {
        if (playerState == PlayerState.unchanged)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MovePlayer(PlayerMoves.up);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MovePlayer(PlayerMoves.down);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MovePlayer(PlayerMoves.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MovePlayer(PlayerMoves.right);
            }
        }
        else if (playerState == PlayerState.moved)
        {
            //should be reset to unchanged after a move and check for calculations
            StartCoroutine(ResetPlayerStateAfterDelay(2f));
            // Additional logic can be added here if needed
        }
    }

    private IEnumerator ResetPlayerStateAfterDelay(float delay)
    {
        playerState = PlayerState.processing;
        yield return new WaitForSeconds(delay);
        playerState = PlayerState.unchanged;
    }
    private void MovePlayer(PlayerMoves direction)
    {
        switch (direction)
        {
            case PlayerMoves.up:
                transform.Translate(Vector3.up);
                break;
            case PlayerMoves.down:
                transform.Translate(Vector3.down);
                break;
            case PlayerMoves.left:
                transform.Translate(Vector3.left);
                break;
            case PlayerMoves.right:
                transform.Translate(Vector3.right);
                break;
        }
        playerState = PlayerState.moved;
    }
}
