using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Public references")]
    public AnimationCurve m_curve;

    [Header("Parameters")]
    public float m_timeToMoveCamera;

    Vector3 m_initPosCamera;
    Vector3 m_targetPosCamera;

    bool m_isCameraMoving;
    float m_timeMoving;

    Camera m_camera;
    PlayerScript m_player;

    void Awake()
    {
        m_isCameraMoving = false;
        m_timeMoving = 0;

        m_camera = Camera.main;
        GameObject playerObject = GameObject.FindGameObjectWithTag(NamesManager.TAG_PLAYER);
        m_player = playerObject.GetComponent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.OnMovePlayer += OnMovePlayer;
    }

    void OnMovePlayer(Vector3 nextPosition, float posXCamera)
    {
        m_player.JustWarped = true;

        //Vector3 cameraPos = m_camera.transform.position;
        //m_camera.transform.position = new Vector3(posXCamera, cameraPos.y, cameraPos.z);
        m_isCameraMoving = true;
        m_timeMoving = 0;

        m_initPosCamera = m_camera.transform.position;
        m_targetPosCamera = new Vector3(posXCamera, m_initPosCamera.y, m_initPosCamera.z);

        Vector3 playerPos = m_player.transform.position;
        m_player.transform.position = new Vector3(nextPosition.x, playerPos.y, playerPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isCameraMoving)
        {
            m_timeMoving += Time.deltaTime;
            float percent = m_timeMoving / m_timeToMoveCamera;
            if (percent >= 1)
            {
                percent = 1;
                m_isCameraMoving = false;
            }
            m_camera.transform.position = Vector3.Lerp(m_initPosCamera, m_targetPosCamera, m_curve.Evaluate(percent));
        }
    }

    void OnDestroy()
    {
        EventManager.instance.OnMovePlayer -= OnMovePlayer;
    }
}
