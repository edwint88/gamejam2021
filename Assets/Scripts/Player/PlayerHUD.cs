using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private int _maxRage;
    // This variable will be set in the inspector.
    [SerializeField] private AnnoyTaskListener _listener;
    [SerializeField] private int _playerId;

    public RageBar rageBarSlider;
    [SerializeField] private int _rage;

    private void Start()
    {
        rageBarSlider.SetRage(_rage);
        rageBarSlider.SetMaxRage(_maxRage);
        Debug.Log("PlayerID: " + _playerId);
    }

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
        if (_playerId == task.enemyPlayerId)
        {
            Debug.Log("Task: " + task.name + " points: " + task.rageLevel + " (before: " + rageBarSlider.GetRage() + ")");
            // do something with the completed task!
            rageBarSlider.AddRage(task.rageLevel);
        }
    }
}
