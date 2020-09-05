using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAirScript : MonoBehaviour
{
    [Header("Parameters")]
    public float m_speedPressScale;
    public float m_speedReleaseScale;
    public float m_minHeightButton;
    public TYPE_BUTTON m_type;

    public enum TYPE_BUTTON
    {
        PROJECTS,
        EXPERIENCE,
        CONTACT
    }

    enum STATES
    {
        IDLE,
        PRESSING_DOWN,
        PRESSED,
        RETURNING_TO_IDLE
    }

    int m_id;
    STATES m_state;

    SpriteRenderer m_buttonSprite;
    BoxCollider2D m_buttonCollider;

    float m_initHeight;
    float m_currentHeight;
    float m_currentScaleSpeed;

    PlayerScript m_playerReference;

    void Awake()
    {
        string idString = transform.parent.name.Remove(0, 15); // Remove name 'ButtonContainer'
        m_id = int.Parse(idString) - 1;
        // Debug.Log("ButtonScript m_id " + m_id);

        m_state = STATES.IDLE;

        m_buttonSprite = GetComponent<SpriteRenderer>();
        m_buttonCollider = GetComponent<BoxCollider2D>();

        m_initHeight = m_buttonSprite.size.y;
        m_currentHeight = m_initHeight;

        GameObject playerObject = GameObject.FindGameObjectWithTag(NamesManager.TAG_PLAYER);
        m_playerReference = playerObject.GetComponent<PlayerScript>();

        m_currentScaleSpeed = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnActivateButton += OnActivateButton;
    }

    void OnDestroy()
    {
        EventManager.instance.OnActivateButton -= OnActivateButton;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case STATES.IDLE:
                break;
            case STATES.PRESSING_DOWN:
                if (m_currentHeight > m_minHeightButton)
                {
                    m_currentHeight -= m_speedPressScale * Time.deltaTime;
                    if (m_currentHeight <= m_minHeightButton)
                    {
                        m_currentHeight = m_minHeightButton;
                        m_state = STATES.RETURNING_TO_IDLE;
                        m_currentScaleSpeed = m_speedReleaseScale;
                        if (m_type == TYPE_BUTTON.CONTACT)
                        {
                            EventManager.instance.CallActivateContactChain(true, m_id);
                        }
                        else
                        {
                            EventManager.instance.CallActivateBanner(true, m_id);
                            EventManager.instance.CallActivateButton(false, m_id);
                        }
                    }
                    m_buttonSprite.size = new Vector2(m_buttonSprite.size.x, m_currentHeight);
                    m_buttonCollider.offset = new Vector2(m_buttonCollider.offset.x, m_currentHeight * 0.5f);
                    m_buttonCollider.size = new Vector2(m_buttonCollider.size.x, m_currentHeight);
                }
                break;
            case STATES.PRESSED:
                break;
            case STATES.RETURNING_TO_IDLE:
                if (m_currentHeight < m_initHeight)
                {
                    m_currentHeight += m_currentScaleSpeed * Time.deltaTime;
                    if (m_currentHeight >= m_initHeight)
                    {
                        m_currentHeight = m_initHeight;
                        m_state = STATES.IDLE;
                        if (m_type == TYPE_BUTTON.CONTACT)
                        {
                            EventManager.instance.CallActivateContactChain(false, m_id);
                        }
                        else
                        {
                            EventManager.instance.CallActivateBanner(false, m_id);
                        }
                    }
                    m_buttonSprite.size = new Vector2(m_buttonSprite.size.x, m_currentHeight);
                    m_buttonCollider.offset = new Vector2(m_buttonCollider.offset.x, m_currentHeight * 0.5f);
                    m_buttonCollider.size = new Vector2(m_buttonCollider.size.x, m_currentHeight);
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_type == TYPE_BUTTON.PROJECTS)
        {
            CheckTag(collision, NamesManager.TAG_PLAYER);
        }
        if (m_type == TYPE_BUTTON.EXPERIENCE || m_type == TYPE_BUTTON.CONTACT)
        {
            CheckTag(collision, NamesManager.TAG_BALL);
        }
    }

    void CheckTag(Collision2D collision, string tag)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            SoundManager.instance.PlaySound(SoundManager.ID_SOUND_ACTIVE_BUTTON);
            if (m_type == TYPE_BUTTON.PROJECTS)
            {
                // Tutorial Button projects.
                EventManager.instance.CallActivateTutorial(true, 3);
            }
            m_state = STATES.PRESSING_DOWN;
            m_currentHeight = m_buttonSprite.size.y;
        }
    }

    void OnActivateButton(bool activate, int id)
    {
        if (!activate && m_id != id)
        {
            m_currentHeight = m_buttonSprite.size.y;
            m_state = STATES.RETURNING_TO_IDLE;
            m_currentScaleSpeed = m_speedPressScale;
        }
    }
}
