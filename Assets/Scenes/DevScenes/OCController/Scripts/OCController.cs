using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OCController : MonoBehaviour
{

    [Header("Tooltip")]
    [SerializeField] private Transform _targetCamera;
    [SerializeField] private Transform _tooltipContainer;
    [SerializeField] private GameObject _pickupTooltip;

    [Header("References")]
    [SerializeField] private Transform _playerHand;

    [Header("Controller Settings")]
    [SerializeField] private float _movmentSpeed = 10.0F;
    [SerializeField] private string _inputPrefix = "Player1_";
    [SerializeField] private int _playerId;

    private readonly HashSet<Grabbable> _lookingAt = new HashSet<Grabbable>();
    private Grabbable _holding;

    public Grabbable Holding {

        get => _holding;

        set
        {
            if (_holding != value)
            {
                if (_holding != null)
                {
                    _holding.transform.parent = null;
                    _holding.HoldBy = null;

                    var joint = _playerHand.GetComponent<FixedJoint>();
                    if (joint != null)
                    {
                        Destroy(joint);
                    }

                }

                if (value != null)
                {

                    value.transform.parent = _playerHand;

                    if (value.AdditionalSettings != null)
                    {
                        value.transform.rotation = Quaternion.Euler(value.AdditionalSettings.TargetRotation);
                    }

                    value.transform.position = _playerHand.position;
                    value.HoldBy = this;

                    var body = value.GetComponent<Rigidbody>();
                    if (body != null)
                    {
                        var joint = _playerHand.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = body;
                    }

                }

                _holding = value;
            }
        }

    }

    private readonly List<Interactable> _interactables = new List<Interactable>();

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(_inputPrefix + "Grab")) GrabOrDrop();
        if (Input.GetButtonDown(_inputPrefix + "Interact")) TryInteract();
        UpdateTooltip();
    }

    private void FixedUpdate()
    {
        UpdateMovment(Input.GetAxis(_inputPrefix + "Horizontal"), Input.GetAxis(_inputPrefix + "Vertical"));
    }

    private void UpdateMovment(float horizontal, float vertical)
    {
        var translate = new Vector3(
            horizontal * _movmentSpeed * Time.fixedDeltaTime,
            0.0F,
            vertical * _movmentSpeed * Time.fixedDeltaTime
        );

        transform.Translate(translate, Space.World);
        transform.LookAt(transform.position + translate);
        _animator.SetFloat("Speed", translate.sqrMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<Grabbable>();
        if (grabbable != null && (grabbable.AdditionalSettings == null || grabbable.AdditionalSettings.TargetPlayer == _playerId))
        {
            _lookingAt.Add(grabbable);
        }

        var interactable = other.GetComponent<Interactable>();
        if (interactable != null && interactable.RequiredPlayerId == _playerId)
        {
            _interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            _lookingAt.Remove(grabbable);
        }

        var interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            _interactables.Remove(interactable);
        }
    }

    private Grabbable GetClosestItem()
    {
        Grabbable result = null;
        float distance = float.MaxValue;
        foreach (var item in _lookingAt)
        {
            if (item.HoldBy == null)
            {
                var iDistance = (item.transform.position - transform.position).sqrMagnitude;
                if (iDistance < distance)
                {
                    result = item;
                    distance = iDistance;
                }
            }
        }
        return result;
    }

    private void GrabOrDrop()
    {
        var target = Holding == null ? GetClosestItem() : null;
        if (target == null)
        {
            Holding = null;
        }
        else
        {
            _animator.SetTrigger("Grab");
            StartCoroutine(GrabCoroutine(target));
        }
    }

    private Interactable GetClosestPossibleInteractable()
    {
        Interactable result = null;
        float distance = float.MaxValue;
        foreach (var interactable in _interactables)
        {
            if (string.IsNullOrEmpty(interactable.RequiredItem) ||
                Holding != null && interactable.RequiredItem == Holding.gameObject.name)
            {
                var iDistance = (interactable.transform.position - transform.position).sqrMagnitude;
                if (iDistance < distance)
                {
                    result = interactable;
                    distance = iDistance;
                }
            }
        }
        return result;
    }

    private void TryInteract()
    {
        var target = GetClosestPossibleInteractable();
        if (target != null)
        {
            _animator.SetTrigger("Grab");
            StartCoroutine(InteractCoroutine(target));
        }
    }

    private IEnumerator GrabCoroutine(Grabbable target)
    {
        yield return new WaitForSeconds(0.28F);
        Holding = target;
    }

    private IEnumerator InteractCoroutine(Interactable target)
    {
        yield return new WaitForSeconds(0.28F);
        target.Interact();
    }

    private void UpdateTooltip()
    {
        if (Holding == null)
        {
            var item = GetClosestItem();
            if (item != null)
            {
                _tooltipContainer.position = item.transform.position + Vector3.up * 1.5F;
                _tooltipContainer.LookAt(_targetCamera);
                _pickupTooltip.gameObject.SetActive(true);
            }
            else
            {
                _pickupTooltip.gameObject.SetActive(false);
            }
        }
        else
        {
            _pickupTooltip.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

}
