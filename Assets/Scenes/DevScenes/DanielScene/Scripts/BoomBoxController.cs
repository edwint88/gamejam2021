using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class BoomBoxController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private AnnoyTaskListener _listener;
    [SerializeField] private AudioSource _musicPlayer;

    [Header("Music")]
    [SerializeField] private AudioClip _defaultMusic;
    [SerializeField] private AudioClip _boomBoxMusic;

    [Header("Enable/Disable Targets")]
    [SerializeField] [InspectorName("Game Objects")] private GameObject[] _targetGameObjects;
    [SerializeField] [InspectorName("Components")] private MonoBehaviour[] _targetCompontents;

    [Header("Materials")]
    [SerializeField] private Material _deactivated;
    [SerializeField] private Material _activated;

    [Header("Task")]
    [SerializeField] private AnnoyTask _task;

    private bool _isPlaying = false;
    private bool _isTaskExecuted = false;

    private MeshRenderer _renderer;
    private Interactable _interactable;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
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
        _isPlaying = !_isPlaying;
        SetPlaying(_isPlaying);
        if (!_isTaskExecuted)
        {
            _listener.SignalTaskDone(_task);
            _isTaskExecuted = true;
        }
    }

    public void SetPlaying(bool status)
    {
        foreach (var obj in _targetGameObjects) obj.SetActive(status);
        foreach (var comp in _targetCompontents) comp.enabled = status;
        _renderer.material = status ? _activated : _deactivated;
        _musicPlayer.clip = status ? _boomBoxMusic : _defaultMusic;
        _musicPlayer.Play();

        if (status && !_isTaskExecuted)
        {
            _isTaskExecuted = true;
            _listener.SignalTaskDone(_task);
        }
    }
}
