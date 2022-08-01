using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class Damagable : MonoBehaviour
{
    [SerializeField] LayerMask _damageLayer;

    Action action;

    private void OnTriggerEnter(Collider other)
    {
        if ((_damageLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            action.Invoke();
        }
    }

    public void SetAction(Action newAction) => action = newAction;
}
