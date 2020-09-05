using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    const float TIME_TO_NEXT_SCENE = 0.5f;
    const float CAMERA_OFFSET_NEXT_SCENE = 48f;

    [Header("Reference to Next Door")]
    public Transform m_warpDoor;

    bool m_playerToWarp;
    float m_timeInside;
    int m_indexScene;
    Animator m_animatorTextNextScene;

    float m_speedRotate = 90f; // 90 degrees per second.

    void Awake()
    {
        m_playerToWarp = false;
        m_timeInside = 0;

        Transform warpDoorParent = m_warpDoor.parent;
        string parentName = warpDoorParent.name;
        m_indexScene = int.Parse(parentName[parentName.Length - 1].ToString());
        //Debug.Log("m_indexScene " + m_indexScene);

        Transform textTransform = warpDoorParent.Find(NamesManager.NAME_TEXT);
        if (textTransform != null)
        {
            m_animatorTextNextScene = textTransform.GetComponent<Animator>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerToWarp)
        {
            m_timeInside -= Time.deltaTime;
            if (m_timeInside <= 0)
            {
                m_timeInside = 0;
                m_playerToWarp = false;
                // Code to next scene.
                EventManager.instance.CallMovePlayer(m_warpDoor.position, (m_indexScene - 1) * CAMERA_OFFSET_NEXT_SCENE);
                if (m_animatorTextNextScene != null)
                {
                    m_animatorTextNextScene.gameObject.SetActive(true);
                    m_animatorTextNextScene.SetTrigger(NamesManager.ANIMATOR_TRIGGER_SHOWTEXT);
                }
            }
        }

        // Rotating Portal.
        Vector3 rotation = transform.eulerAngles;
        float newZRot = rotation.z + m_speedRotate * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, newZRot);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(NamesManager.TAG_PLAYER))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (!player.JustWarped)
            {
                m_playerToWarp = true;
                m_timeInside = TIME_TO_NEXT_SCENE;
            }
            else
            {
                player.JustWarped = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(NamesManager.TAG_PLAYER))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            if (!player.JustWarped)
            {
                m_playerToWarp = false;
                m_timeInside = 0;
            }
        }
    }
}
