using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour, IPointerClickHandler
{
    [Header("Deck Params")]
    public Image cardImage;
    public TextMeshProUGUI titleText;
    public Image disabledUI;

    SpecialAbilityData abilityData;
    Coroutine disabledCoroutine;
    bool isAbilityOnCooldown = false;

    public void Initialize(Sprite cardSprite, string title, SpecialAbilityData abilityData)
    {
        cardImage.sprite = cardSprite;
        titleText.text = title;
        disabledUI.fillAmount = 0;
        this.abilityData = abilityData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAbilityOnCooldown || disabledCoroutine!=null)
        {
            Debug.Log($"Ability in Cooldown!!");
            return;
        }

        // TODO: Launch Ability
        GameManager.Instance.Player.GetComponent<Move>().InstantiateAbility(abilityData.abilityPrefab);

        disabledCoroutine = StartCoroutine(DisableAbility());
    }

    IEnumerator DisableAbility()
    {
        isAbilityOnCooldown = true;
        float disabledDuration = abilityData.AbilityCooldown.Evaluate(abilityData.abilityPrefab.Level);
        float t = 0;
        float progress = 0;
        Debug.Log($"Ability: Attacking and Disabling ability {titleText}");
        Debug.Log($"Ability: Waiting for {disabledDuration}");

        while (progress < 1)
        {
            t += Time.deltaTime;
            progress = Mathf.Clamp01(t / disabledDuration);
            disabledUI.fillAmount = progress;
            yield return null;
        }

        isAbilityOnCooldown = false;
        disabledCoroutine = null;
        disabledUI.fillAmount = 0;
        Debug.Log($"Ability: Enabling ability {titleText}");
    }
}
