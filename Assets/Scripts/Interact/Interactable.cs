using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public int RequiredPlayerId;
    public string RequiredItem;
    public event Action OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
    }

}
