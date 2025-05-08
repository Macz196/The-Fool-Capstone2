using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target; // Player
    [SerializeField] private Transform bossRoomView; // Fixed camera position in the boss room
    
    [Header("Settings")]
    [SerializeField] private float followSmoothTime = 0.25f;
    [SerializeField] private float zoomSmoothTime = 0.5f;
    [SerializeField] private float defaultCameraSize = 5f;
    [SerializeField] private float bossRoomCameraSize = 16f;
    
    private Vector3 offset = new Vector3(0f, 1.5f, -5f);
    private Vector3 velocity = Vector3.zero;
    private float zoomVelocity;
    private bool isInBossRoom = false;
    private Camera cam;
    private Vector3 previousTargetPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultCameraSize;
        previousTargetPosition = target.position;
    }

    void LateUpdate()
    {
        if (isInBossRoom)
        {
            // Smoothly move to boss room position (only X and Y change)
            Vector3 targetPosition = new Vector3(bossRoomView.position.x, bossRoomView.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothTime);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, bossRoomCameraSize, ref zoomVelocity, zoomSmoothTime);
        }
        else
        {
            // Calculate velocity of player movement
            Vector3 playerVelocity = (target.position - previousTargetPosition) / Time.deltaTime;
            previousTargetPosition = target.position;
            
            // Determine a slight lag effect based on player movement
            Vector3 targetPosition = target.position + offset - playerVelocity * 0.1f;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSmoothTime);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, defaultCameraSize, ref zoomVelocity, zoomSmoothTime);
        }
    }

    public void EnterBossRoom()
    {
        isInBossRoom = true;
    }

    public void ExitBossRoom()
    {
        isInBossRoom = false;
    }
}