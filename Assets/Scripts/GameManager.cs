using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CardColor { Blue, Red, White, Brown, Green };

    public static GameManager instance;

    public DeckManager m_deckManager;
    public PlayerController[] m_Player;

    public List<GameObject> expeditionBasePrefab;

    List<int>[] m_discardPile;
    int m_iCurrentPlayer;

    bool m_bStartGame;

    void Awake()
    {
        //If there currently isn't a GameManager, make this the game manager. Otherwise,
        //destroy this object. We only want one GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        int x = 0;

        for (x = 0; x < m_Player.Length; x++)
        {
            m_Player[x].Init();
        }

        m_bStartGame = false;
    }

    // Use this for initialization
    void Start ()
    {
        m_iCurrentPlayer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_bStartGame && Input.GetButtonDown("Submit"))
        {
            StartGame();
        }
	}

    void StartGame()
    {
        int x = 0;
        int y = 0;

        m_deckManager.Shuffle();

        for (x = 0; x < m_Player.Length; x++)
        {
            for (y = 0; y < 8; y++)
            {
                m_Player[x].AddCard(m_deckManager.DrawCard());
            }
        }

        LoadPlayerScreen(0);
    }

    void LoadPlayerScreen(int playerIndex)
    {

    }

    public void EndTurn()
    {
        if (m_deckManager.IsDeckEmpty())
        {
            Debug.Log("Someone won!");
            m_deckManager.DisableCards();
        }
        else
        {
            m_iCurrentPlayer = (m_iCurrentPlayer + 1) % m_Player.Length;
            LoadPlayerScreen(m_iCurrentPlayer);
        }
    }

    public void PlayCard(CardController card)
    {
        m_Player[m_iCurrentPlayer].PlayCard(card.gameObject);
    }

    public bool IsValidPlay(CardController heldCard, CardController expedition)
    {   
        if (heldCard.color == expedition.color)
        {
            if (heldCard.value == 1 && heldCard.value == expedition.value)
            {
                return true;
            }
            if (heldCard.value > expedition.value)
            {
                return true;
            }
        }

        return false;
    }
}
