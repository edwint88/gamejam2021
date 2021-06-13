using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandContainer : MonoBehaviour
{

    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;

    public GameObject LeftGrabbed { get; private set; }
    public GameObject RightGrabbed { get; private set; }

    void Update()
    {
        LeftGrabbed = GetGrabbedObject(_leftHand);
        RightGrabbed = GetGrabbedObject(_rightHand);
    }

    private GameObject GetGrabbedObject(GameObject hand)
    {
        var joint = hand.GetComponent<FixedJoint>();
        return joint != null ? joint.connectedBody.gameObject : null;
    }
}
