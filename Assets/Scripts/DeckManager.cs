using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<GameObject> m_cardListPrefab;

    public Transform deckLocation;
   
    Stack<int> m_deck;
    List<GameObject> m_cardList;

    public void Start()
    {
        InitCardObjects();
        Shuffle();
        Debug.Log("Deck Manager Init done!");
    }
    
    void InitCardObjects()
    {
        GameObject obj = null;
        m_cardList = new List<GameObject>();

        //m_cardListPrefab.ForEach(InstantiatePrefab);

        m_cardListPrefab.ForEach(delegate (GameObject cardPrefab)
        {
            obj = Instantiate(cardPrefab, deckLocation.transform) as GameObject;
            obj.SetActive(false);
            m_cardList.Add(obj);
        });
    }

    // Shuffle the deck
    public void Shuffle()
    {
        int tempNum = -1;
        m_deck = new Stack<int> ();
        m_deck.Clear();

        ResetCards();

        while (m_deck.Count < m_cardList.Count)
        {
            do
            {
                tempNum = Random.Range(0, m_cardList.Count);
            } while (m_deck.Contains(tempNum));

            m_deck.Push(tempNum);
        }
    }

    // Reset card locations
    void ResetCards()
    {
        m_cardList.ForEach(delegate (GameObject card)
        {
            card.transform.position = deckLocation.transform.position;
        });
    }

    // Get an the card details
    public GameObject GetCard(int index)
    {
        return m_cardList[index];
    }

    public int GetCardIndex(GameObject card)
    {
        return m_cardList.IndexOf(card);
    }

    // Draw a card from the deck
    public int DrawCard()
    {
        if (m_deck.Count > 0)
        {
            return m_deck.Pop();
        }

        return -1;
        
    }

    public bool IsDeckEmpty()
    {
        if (m_deck.Count > 0)
        {
            return false;
        }
        return true;
    }

    public void DisableCards()
    {
        m_cardList.ForEach(delegate (GameObject card)
        {
            card.GetComponent<CardController>().m_bPlayed = false;
        });
    }

    public void EnableCards()
    {
        m_cardList.ForEach(delegate (GameObject card)
        {
            card.GetComponent<CardController>().m_bPlayed = true;
        });
    }
}
