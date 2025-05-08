using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnHit2D : MonoBehaviour
{
    public Material newMaterial; // The material to change to when hit

    private bool isActivated = false; // To prevent multiple activations

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActivated && collision.gameObject.CompareTag("Ball"))
        {
            GetComponent<Renderer>().material = newMaterial;
            isActivated = true;

            // Notify the TargetManager
            FindObjectOfType<TargetManager>().ActivateTarget();
        }
    }
}