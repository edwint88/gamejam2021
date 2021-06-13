using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeNestTask : MonoBehaviour
{

    [SerializeField] private AnnoyTaskListener _listener;
    [SerializeField] private AnnoyTask _task;
    [SerializeField] private GameObject _targetHouse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _targetHouse)
        {
            _listener.SignalTaskDone(_task);
            Destroy(this);
        }
    }

}
