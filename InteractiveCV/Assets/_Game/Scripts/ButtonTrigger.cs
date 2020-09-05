using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    ButtonGroundScript m_buttonRef;

    void Awake()
    {
        Transform parentTransform = transform.parent;
        m_buttonRef = parentTransform.GetComponent<ButtonGroundScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(NamesManager.TAG_PLAYER))
        {
            m_buttonRef.OnButtonPress();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(NamesManager.TAG_PLAYER))
        {
            m_buttonRef.OnButtonRelease();
        }
    }
}
