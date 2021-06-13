using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityTask : MonoBehaviour
{

    [SerializeField] private AnnoyTaskListener _listener;
    [SerializeField] private AnnoyTask _task;

    private Interactable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    private void OnEnable()
    {
        _interactable.OnInteract += OnInteract;
    }

    private void OnDisable()
    {
        _interactable.OnInteract -= OnInteract;
    }

    private void OnInteract()
    {
        _listener.SignalTaskDone(_task);
        Destroy(this);
    }

}
