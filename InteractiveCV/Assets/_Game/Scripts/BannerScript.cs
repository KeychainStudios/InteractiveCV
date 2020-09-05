using System;
using UnityEngine;

public class BannerScript : MonoBehaviour
{
    [Header("Public references")]
    public AnimationCurve m_curve;

    [Header("Parameters")]
    public float m_timeToMove;

    enum STATES
    {
        MOVE_TO_FINISH,
        FINISH_POSITION,
        MOVE_TO_INIT,
        IDLE
    }

    int m_id;
    Transform m_movableBanner;
    Vector3 m_initPosition;
    Vector3 m_finishPosition;

    STATES m_state;
    Vector3 m_lastPosition;
    float m_percentMove;

    public int Id { get => m_id; }

    void Awake()
    {
        string idString = name.Remove(0, 15); // Remove name 'BannerContainer'
        m_id = int.Parse(idString) - 1;

        m_movableBanner = transform.Find(NamesManager.NAME_BANNER_MOVE);
        Transform initTransform = transform.Find(NamesManager.NAME_INIT_POSITION);
        m_initPosition = initTransform.localPosition;
        Transform finishTransform = transform.Find(NamesManager.NAME_FINISH_POSITION);
        m_finishPosition = finishTransform.localPosition;

        m_state = STATES.IDLE;
        m_lastPosition = m_movableBanner.localPosition;
        m_percentMove = 0;
    }

    void Start()
    {
        EventManager.instance.OnActivateBanner += OnActivateBanner;
    }

    void OnDestroy()
    {
        EventManager.instance.OnActivateBanner -= OnActivateBanner;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case STATES.IDLE:
                break;
            case STATES.MOVE_TO_FINISH:
                MoveTo(m_finishPosition);
                break;
            case STATES.MOVE_TO_INIT:
                MoveTo(m_initPosition);
                break;
        }
    }

    void OnActivateBanner(bool activate, int id)
    {
        if (m_id == id)
        {
            if (activate)
            {
                MoveToFinish();
            }
            else
            {
                MoveToInit();
            }
        }
    }

    void MoveTo(Vector3 targetPosition)
    {
        if (m_percentMove < 1f)
        {
            m_percentMove += Time.deltaTime;
            if (m_percentMove >= 1f)
            {
                m_percentMove = 1f;
                m_state = (m_state == STATES.MOVE_TO_FINISH) ? (STATES.FINISH_POSITION) : (STATES.IDLE);
            }
            m_movableBanner.localPosition = Vector3.Lerp(m_lastPosition, targetPosition, m_curve.Evaluate(m_percentMove));
        }
    }

    public void MoveToInit()
    {
        m_state = STATES.MOVE_TO_INIT;
        m_lastPosition = m_movableBanner.localPosition;
        m_percentMove = 0;
    }

    public void MoveToFinish()
    {
        m_state = STATES.MOVE_TO_FINISH;
        m_lastPosition = m_movableBanner.localPosition;
        m_percentMove = 0;
    }
}
