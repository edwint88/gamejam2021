using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{

    public GrabbableSettings AdditionalSettings { get; private set; }

    private OCController _holdBy;
    public OCController HoldBy {
        get => _holdBy;
        set
        {
            if (_holdBy != value)
            {
                foreach (var coll in _colliders)
                {
                    coll.isTrigger = value != null;
                }
                _holdBy = value;
            }
        }
    }

    private readonly List<Collider> _colliders = new List<Collider>();
    private void Awake()
    {
        AdditionalSettings = GetComponent<GrabbableSettings>();
        foreach (var coll in GetComponents<Collider>())
        {
            if (!coll.isTrigger) _colliders.Add(coll);
        }
    }

}
