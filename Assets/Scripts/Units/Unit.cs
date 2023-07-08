using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    #region Client

    [Client]
    public void Select()
    {
        if (!isOwned)
        {
            return;
        }

        onSelected?.Invoke();
    }

    public void Deselect()
    {
        if (!isOwned) { 
            return; 
        }

        onDeselected.Invoke();
    }

    #endregion
}
