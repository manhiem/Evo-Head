using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player;
    public GameData gameData;
    public Transform cardSpawnParent;
    public DeckCard cardPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SpawnAbilityCards();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SpawnAbilityCards()
    {
        SpecialAbilityData[] abilities = gameData.cardData;

        for (int i = 0; i < abilities.Length; i++)
        {
            SpecialAbilityData tmp = abilities[i];
            int r = Random.Range(i, abilities.Length);
            abilities[i] = abilities[r];
            abilities[r] = tmp;
        }

        for(int i = 0; i < 4;i++)
        {
            DeckCard card = Instantiate(cardPrefab, cardSpawnParent.transform.GetChild(i).position, cardSpawnParent.GetChild(i).rotation, 
                cardSpawnParent.GetChild(i));
            card.Initialize(abilities[i].AbilityImage, abilities[i].AbilityName, abilities[i]);
        }
    }
}
