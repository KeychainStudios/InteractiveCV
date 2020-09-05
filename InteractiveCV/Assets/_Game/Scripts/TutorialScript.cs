using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    Transform m_baseTextContainer;

    SpriteRenderer[] m_renderersTutorial;
    TextMeshPro m_text;

    float m_speedAlpha = 1f;
    float m_alpha;
    float m_direction;
    bool m_isAlphaChanging;

    bool m_isAlreadyShown;

    public bool IsAlreadyShown { get => m_isAlreadyShown; }

    void Awake()
    {
        m_baseTextContainer = transform.Find(NamesManager.NAME_TEXT_CONTAINER);

        SpriteRenderer[] renderersChild = m_baseTextContainer.GetComponentsInChildren<SpriteRenderer>();
        int countChildren = renderersChild.Length;

        m_renderersTutorial = new SpriteRenderer[countChildren + 1];
        for (int i = 0; i < countChildren; i++)
        {
            renderersChild[i].color = new Color(1, 1, 1, 0);
            m_renderersTutorial[i] = renderersChild[i];
        }

        m_renderersTutorial[countChildren] = m_baseTextContainer.GetComponent<SpriteRenderer>();

        m_text = m_baseTextContainer.GetComponentInChildren<TextMeshPro>();
        m_text.alpha = 0f;

        m_isAlphaChanging = false;
        m_isAlreadyShown = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_isAlphaChanging)
        {
            m_alpha += m_direction * m_speedAlpha * Time.deltaTime;
            if (m_direction < 0)
            {
                if (m_alpha <= 0)
                {
                    m_alpha = 0;
                    m_isAlphaChanging = false;
                    gameObject.SetActive(false);
                }
            }
            if (m_direction > 0)
            {
                if (m_alpha >= 1)
                {
                    m_alpha = 1;
                    m_isAlphaChanging = false;
                    m_isAlreadyShown = true;
                }
            }

            // Changing values.
            int count = m_renderersTutorial.Length;
            for (int i = 0; i < count; i++)
            {
                m_renderersTutorial[i].color = new Color(1, 1, 1, m_alpha);
            }
            m_text.alpha = m_alpha;
        }
    }

    public void OnShowTutorial(bool show)
    {
        m_alpha = show ? 0 : 1;
        m_direction = show ? 1 : -1;
        m_isAlphaChanging = true;
    }
}
