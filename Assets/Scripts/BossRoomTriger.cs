using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossRoomDetector : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BossRoom"))
        {
            cameraFollow.EnterBossRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BossRoom"))
        {
            cameraFollow.ExitBossRoom();
        }
    }
}