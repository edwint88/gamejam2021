using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutTask : MonoBehaviour
{

    [SerializeField] private AnnoyTaskListener _listener;
    [SerializeField] private AnnoyTask _task;
    [SerializeField] private GameObject _beeNest;

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
        Rigidbody body = (Rigidbody)gameObject.AddComponent(typeof(Rigidbody));
        if (body != null)
        {
            body.mass = 50.0F;
        }

        // Set gravity for bee nest to fall from the tree
        _beeNest.GetComponent<Rigidbody>().useGravity = true;
        _listener.SignalTaskDone(_task);
        GetComponent<Rigidbody>().AddForce(new Vector3(1.0F, 1.0F, 1.0F));
        Destroy(this);
    }

}
