using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject handLocation;
    public GameObject expeditionLocation;

    List<int> m_handCards;
    List<int>[] m_playedCards;

    public void Init()
    {
        GameObject obj = null;
        float xPos = 0.0f;
        m_handCards = new List<int> ();
        m_playedCards = new List<int>[5];

        int x = 0;

        List<GameObject> expeditionBase = GameManager.instance.expeditionBasePrefab;

        expeditionBase.ForEach(delegate (GameObject basePrefab)
        {
            obj = Instantiate(basePrefab, expeditionLocation.transform) as GameObject;
            xPos = basePrefab.transform.localScale.x + 2.0f;
            Vector3 offset = new Vector3(xPos * x, 0.0f, 0.0f);
            obj.transform.position = expeditionLocation.transform.position + offset;
        });       
    }

    public void AddCard(int index)
    {
        GameObject card = GameManager.instance.m_deckManager.GetCard(index);

        card.SetActive(true);

        float xPos = card.transform.localScale.x + 1.0f;
        Vector3 offset = new Vector3(xPos * m_handCards.Count, 0.0f, 0.0f);
        card.transform.position = handLocation.transform.position + offset;

        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 camRotation = cam.transform.rotation.eulerAngles;
  
        card.transform.rotation *= Quaternion.Euler(0.0f, camRotation.y + 180.0f, 0.0f);

        m_handCards.Add(index);
    }

    public void PlayCard(GameObject card)
    {
        m_handCards.Remove(GameManager.instance.m_deckManager.GetCardIndex(card));

        RearrangeCard();

        this.AddCard(GameManager.instance.m_deckManager.DrawCard());
        GameManager.instance.EndTurn();
    }

    void RearrangeCard()
    {
        int x = 0;
        GameObject card = null;
        Vector3 newPos;
        Vector3 offset;
        float xPos = 0.0f;

        for (x = 0; x < m_handCards.Count; x++)
        {
            card = GameManager.instance.m_deckManager.GetCard(m_handCards[x]);
            xPos = card.transform.localScale.x + 1.0f;
            offset = new Vector3(xPos * x, 0.0f, 0.0f);

            newPos = handLocation.transform.position + offset;
            card.GetComponent<Rigidbody>().MovePosition(newPos);
        }
    }
}
