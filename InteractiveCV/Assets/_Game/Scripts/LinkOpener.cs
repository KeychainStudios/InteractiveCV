using System.Runtime.InteropServices;
using UnityEngine;

public class LinkOpener : MonoBehaviour
{
    [Header("Parameters")]
    public string m_url;

    void OnMouseDown()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
		openWindow(m_url);
#else 
        Application.OpenURL(m_url);
#endif
    }

#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void openWindow(string url);
#endif
}
