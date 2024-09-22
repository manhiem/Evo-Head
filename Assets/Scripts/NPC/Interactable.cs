using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool _playerWithinRange;
    [SerializeField] private CanvasGroup _interactableUI;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _interactableUI.gameObject.SetActive(true);
            LeanTween.cancel(_interactableUI.gameObject);
            LeanTween.alphaCanvas(_interactableUI, to: 1, time: 1);
            _playerWithinRange = true;
        }   
    }

    private void Update()
    {
        if (_playerWithinRange && Input.GetKeyUp(KeyCode.E))
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        _interactableUI.gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _playerWithinRange = false;
            LeanTween.alphaCanvas(_interactableUI, to: 0, time: 1)
                .setOnComplete(() => UIHide());
        }
    }

    private void UIHide()
    {
        throw new NotImplementedException();
    }
}
