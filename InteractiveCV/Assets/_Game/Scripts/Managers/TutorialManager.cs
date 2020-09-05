using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    public TutorialScript[] m_tutorials;
    public TutorialScript[] m_tutorialsPhone;
    public GameObject m_firstDoor;

    float m_timeToShow = 7f;
    float m_timer;
    int m_currentIdTutorial;
    TutorialScript[] m_tutorialsSelected;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnActivateTutorial += OnActivateTutorial;

#if UNITY_ANDROID
        m_tutorialsSelected = m_tutorialsPhone;
#else
        m_tutorialsSelected = m_tutorials;
#endif

        m_timer = m_timeToShow;
        m_currentIdTutorial = 0;
        m_tutorialsSelected[m_currentIdTutorial].OnShowTutorial(true);
    }

    void OnDestroy()
    {
        EventManager.instance.OnActivateTutorial -= OnActivateTutorial;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentIdTutorial < 2)
        {
            if (m_timer > 0)
            {
                m_timer -= Time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = m_timeToShow;
                    m_tutorialsSelected[m_currentIdTutorial].OnShowTutorial(false);
                    m_currentIdTutorial++;
                    m_tutorialsSelected[m_currentIdTutorial].OnShowTutorial(true);
                    if (m_currentIdTutorial == 2)
                    {
                        m_firstDoor.SetActive(true);
                    }
                }
            }
        }
    }

    public void OnActivateTutorial(bool activate, int id)
    {
        if (activate && !m_tutorialsSelected[id].IsAlreadyShown)
        {
            m_tutorialsSelected[id].OnShowTutorial(activate);
        }
    }
}
