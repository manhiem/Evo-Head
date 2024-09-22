using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Trigger : MonoBehaviour
{
    [SerializeField] private GameObject _levelSelection;  // The UI Panel for level selection
    [SerializeField] private GameObject _teleporterActive; // Optional teleporter object

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))  // If the player enters the trigger
        {
            PlayerController.Instance.FirstPersonCamera.SetActive(false); // Disable the player's camera
            _levelSelection.SetActive(true);  // Show the level selection panel
            Cursor.lockState = CursorLockMode.None;  // Unlock the cursor for selection
            Cursor.visible = true;  // Make the cursor visible
        }
    }
}
