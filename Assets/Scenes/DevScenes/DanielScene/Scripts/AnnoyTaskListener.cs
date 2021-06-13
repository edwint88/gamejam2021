using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoyTaskListener : MonoBehaviour
{

    public event Action<AnnoyTask> OnTaskDone;

    public void SignalTaskDone(AnnoyTask task)
    {
        OnTaskDone?.Invoke(task);
        _source.clip = task.clip;
        _source.Play();
        Debug.Log("The Task " + task.Name + " was completed!");
    }

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    /*

        -- Example Usage: Completing Tasks --

        // This variable will be set in the inspector
        [SerializeField] private AnnoyTaskListener _listener;
        [SerializeField] private AnnoyTask _task;

        // Let's say our task completes by running the method below.
        public void TaskCompleted()
        {
            // Signal our task as completed.
            _listener.SignalTaskDone(_task);

            // Prevents signaling this task as completed again.
            Destroy(this);
        }

        -- Example Usage: Using the Callback --

        // This variable will be set in the inspector.
        [SerializeField] private AnnoyTaskListener _listener;

        private void OnEnable()
        {
            // registering the event to a method, so the listener knows what to call.
            _listener.OnTaskDone += ProcessCompletedTask;
        }

        private void OnDisable()
        {
            // we don't want `ProcessCompletedTask(...)` to run if our component is disabled,
            // so we remove the callback from the listener.
            _listener.OnTaskDone -= ProcessCompletedTask;
        }

        private void ProcessCompletedTask(AnnoyTask task)
        {
            // do something with the completed task!
        }


     */

}
