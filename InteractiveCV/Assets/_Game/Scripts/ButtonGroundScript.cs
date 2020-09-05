using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroundScript : MonoBehaviour
{
    [Header("Parameters")]
    public float m_speedScale;
    public float m_minHeightButton;

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

    PlayerScript m_playerReference;

    void Awake()
    {
        int lengthName = transform.parent.name.Length;
        string id = transform.parent.name[lengthName - 1].ToString();
        m_id = int.Parse(id) - 1;
        //Debug.Log("ButtonScript m_id " + m_id);

        m_state = STATES.IDLE;

        m_buttonSprite = GetComponent<SpriteRenderer>();
        m_buttonCollider = GetComponent<BoxCollider2D>();

        m_initHeight = m_buttonSprite.size.y;
        m_currentHeight = m_initHeight;

        GameObject playerObject = GameObject.FindGameObjectWithTag(NamesManager.TAG_PLAYER);
        m_playerReference = playerObject.GetComponent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {

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
                    m_currentHeight -= m_speedScale * Time.deltaTime;
                    if (m_currentHeight <= m_minHeightButton)
                    {
                        SoundManager.instance.PlaySound(SoundManager.ID_SOUND_ACTIVE_BUTTON);
                        m_currentHeight = m_minHeightButton;
                        m_state = STATES.PRESSED;
                        EventManager.instance.CallActivateBanner(true, m_id);
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
                    m_currentHeight += m_speedScale * Time.deltaTime;
                    if (m_currentHeight >= m_initHeight)
                    {
                        m_currentHeight = m_initHeight;
                        m_state = STATES.IDLE;
                    }
                    m_buttonSprite.size = new Vector2(m_buttonSprite.size.x, m_currentHeight);
                    m_buttonCollider.offset = new Vector2(m_buttonCollider.offset.x, m_currentHeight * 0.5f);
                    m_buttonCollider.size = new Vector2(m_buttonCollider.size.x, m_currentHeight);
                }
                break;
        }
    }

    public void OnButtonPress()
    {
        m_state = STATES.PRESSING_DOWN;
        m_currentHeight = m_buttonSprite.size.y;
    }

    public void OnButtonRelease()
    {
        m_state = STATES.RETURNING_TO_IDLE;
        m_currentHeight = m_buttonSprite.size.y;
        EventManager.instance.CallActivateBanner(false, m_id);
    }
}
