using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPictureNearNPCWithKeyPress : MonoBehaviour
{
    public string npcTag = "NPC"; // Tag to identify the NPC
    public Image picture; // UI Image to show above the NPC
    public float displayDistance = 3.0f; // Distance to show the picture

    private GameObject[] npcs;
    private Transform playerTransform;
    private GameObject currentNPC;
    private bool isPictureVisible = false;

    private void Start()
    {
        npcs = GameObject.FindGameObjectsWithTag(npcTag);
        playerTransform = transform;
        picture.enabled = false; // Initially hide the picture
    }

    private void Update()
    {
        if (isPictureVisible && Input.GetKeyDown(KeyCode.E))
        {
            picture.enabled = false;
            isPictureVisible = false;
            return;
        }

        foreach (GameObject npc in npcs)
        {
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
            if (distance <= displayDistance)
            {
                currentNPC = npc;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(npc.transform.position + Vector3.up * 2.0f);
                picture.transform.position = screenPosition;
                picture.enabled = true;
                isPictureVisible = true;
                return;
            }
        }
        picture.enabled = false;
        isPictureVisible = false;
    }
}
