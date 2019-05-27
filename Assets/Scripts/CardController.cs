using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
    public GameManager.CardColor color;
    public int value;

    Vector3 m_origPosition;
    Vector3 m_dropPosition;

    Vector3 m_screenPoint;
    Vector3 m_offset;

    bool m_bSelected;
    bool m_bCanDrop;

    public bool m_bPlayed;

    GameObject m_target;

    void Awake()
    {
        m_bPlayed = false;
    }

    void OnMouseDown()
    {
        if (!m_bPlayed)
        {
            m_origPosition = transform.position;

            m_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
        }
    }

    void OnMouseDrag()
    {
        if (!m_bPlayed)
        {
            m_bSelected = true;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + m_offset + new Vector3(0.0f, 0.1f, 0.0f);
            transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        if (!m_bPlayed)
        {
            m_bSelected = false;

            if (m_bCanDrop)
            {
                transform.position = m_dropPosition;
                GameManager.instance.PlayCard(this);
                m_target.GetComponent<BoxCollider>().enabled = false;
                m_bPlayed = true;
            }
            else
            {
                transform.position = m_origPosition;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        m_bCanDrop = false;

        if (other.CompareTag("PlayerCard"))
        {
            if (m_bSelected)
            {
                m_target = null;
                if (GameManager.instance.IsValidPlay(gameObject.GetComponent<CardController>(), other.gameObject.GetComponent<CardController>()))
                {
                    m_bCanDrop = true;
                    m_target = other.gameObject;
                    m_dropPosition = other.gameObject.transform.position + new Vector3(0.0f, 0.001f, -0.5f);
                }
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        m_bCanDrop = false;
    }
}
