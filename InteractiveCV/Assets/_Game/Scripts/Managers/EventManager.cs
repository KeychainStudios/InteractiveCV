using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public delegate void MovePlayer(Vector3 nextPosition, float posXCamera);
    public event MovePlayer OnMovePlayer;

    public delegate void ActivateBanner(bool activate, int id);
    public event ActivateBanner OnActivateBanner;

    public delegate void ActivateButton(bool activate, int id);
    public event ActivateButton OnActivateButton;

    public delegate void ActivateContactChain(bool activate, int id);
    public event ActivateContactChain OnActivateContactChain;

    public delegate void ActivateTutorial(bool activate, int id);
    public event ActivateTutorial OnActivateTutorial;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CallMovePlayer(Vector3 nextPosition, float posXCamera)
    {
        if (OnMovePlayer != null)
        {
            OnMovePlayer(nextPosition, posXCamera);
        }
    }

    public void CallActivateBanner(bool activate, int id)
    {
        if (OnActivateBanner != null)
        {
            OnActivateBanner(activate, id);
        }
    }

    public void CallActivateButton(bool activate, int id)
    {
        if (OnActivateButton != null)
        {
            OnActivateButton(activate, id);
        }
    }

    public void CallActivateContactChain(bool activate, int id)
    {
        if (OnActivateContactChain != null)
        {
            OnActivateContactChain(activate, id);
        }
    }

    public void CallActivateTutorial(bool activate, int id)
    {
        if (OnActivateTutorial != null)
        {
            OnActivateTutorial(activate, id);
        }
    }
}
