using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public Transform[] m_leftSideObjects;
    public Transform[] m_rightSideObjects;

    void Awake()
    {
#if UNITY_ANDROID
        // Check current resolution.
        float heightCamera = Camera.main.orthographicSize * 2;
        float widthCamera = heightCamera * Camera.main.aspect;

        float baseWidth = 9.6f;
        float diffWidth = widthCamera * 0.5f - baseWidth;

        int count = m_leftSideObjects.Length;
        for (int i = 0; i < count; i++)
        {
            m_leftSideObjects[i].localPosition -= diffWidth * Vector3.right;
        }
        count = m_rightSideObjects.Length;
        for (int i = 0; i < count; i++)
        {
            m_rightSideObjects[i].localPosition += diffWidth * Vector3.right;
        }
    }
#endif
}
