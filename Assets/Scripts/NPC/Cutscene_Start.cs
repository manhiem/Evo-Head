using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SignalReceiver))]

public class Cutscene_Start : Interactable
{
    [SerializeField] private GameObject _cutsceneToPlay;
    public override void Activate()
    {
        base.Activate();
        _cutsceneToPlay.SetActive(true);
        PlayerController.Instance.CutsceneCamera.SetActive(true);
        PlayerController.Instance.FirstPersonCamera.SetActive(false);
        PlayerController.Instance.Dialogue.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _cutsceneToPlay.SetActive(false);
        PlayerController.Instance.CutsceneCamera.SetActive(false);
        PlayerController.Instance.FirstPersonCamera.SetActive(true);
        PlayerController.Instance.Dialogue.SetActive(false);
        PlayerController.Instance.Teleporter.SetActive(true);
    }
}
