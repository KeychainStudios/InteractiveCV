using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactScreen : MonoBehaviour
{
    const int ID_LEFT_CHAIN = 11;
    const int ID_RIGHT_CHAIN = 12;

    [Header("Parameters")]
    public float m_speedMoveChain;
    public float m_distanceToFall;

    Rigidbody2D m_platformChainBody1;
    Rigidbody2D m_platformChainBody2;

    float m_initY;
    float m_finishY;

    float m_directionChain1;
    float m_directionChain2;

    bool m_moveChain1;
    bool m_moveChain2;

    void Awake()
    {
        Transform chain1 = transform.Find(NamesManager.NAME_CHAIN1);
        Transform platformChain1 = chain1.Find(NamesManager.NAME_PLATFORM);
        m_platformChainBody1 = platformChain1.GetComponent<Rigidbody2D>();

        Transform chain2 = transform.Find(NamesManager.NAME_CHAIN2);
        Transform platformChain2 = chain2.Find(NamesManager.NAME_PLATFORM);
        m_platformChainBody2 = platformChain2.GetComponent<Rigidbody2D>();

        m_initY = m_platformChainBody1.position.y;
        m_finishY = m_initY - m_distanceToFall;

        m_directionChain1 = 0;
        m_directionChain2 = 0;

        m_moveChain1 = false;
        m_moveChain2 = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnActivateContactChain += OnActivateChain;
    }

    void OnDestroy()
    {
        EventManager.instance.OnActivateContactChain -= OnActivateChain;
    }

    void FixedUpdate()
    {
        if (m_moveChain1)
        {
            m_platformChainBody1.position += (m_directionChain1 * m_speedMoveChain * Time.fixedDeltaTime) * Vector2.up;
            if (m_directionChain1 > 0)
            {
                if ( m_platformChainBody1.position.y >= m_initY)
                {
                    m_platformChainBody1.position = new Vector2(m_platformChainBody1.position.x, m_initY);
                    m_moveChain1 = false;
                }
            }
            else
            {
                if (m_platformChainBody1.position.y <= m_finishY)
                {
                    m_platformChainBody1.position = new Vector2(m_platformChainBody1.position.x, m_finishY);
                    m_moveChain1 = false;
                }
            }
        }
        if (m_moveChain2)
        {
            m_platformChainBody2.position += (m_directionChain2 * m_speedMoveChain * Time.fixedDeltaTime) * Vector2.up;
            if (m_directionChain2 > 0)
            {
                if (m_platformChainBody2.position.y >= m_initY)
                {
                    m_platformChainBody2.position = new Vector2(m_platformChainBody2.position.x, m_initY);
                    m_moveChain2 = false;
                }
            }
            else
            {
                if (m_platformChainBody2.position.y <= m_finishY)
                {
                    m_platformChainBody2.position = new Vector2(m_platformChainBody2.position.x, m_finishY);
                    m_moveChain2 = false;
                }
            }
        }
    }

    void OnActivateChain(bool activate, int id)
    {
        switch (id)
        {
            case ID_LEFT_CHAIN:
                m_moveChain1 = true;
                m_directionChain1 = (activate) ? (-1) : (1);
                break;
            case ID_RIGHT_CHAIN:
                m_moveChain2 = true;
                m_directionChain2 = (activate) ? (-1) : (1);
                break;
        }
    }
}
