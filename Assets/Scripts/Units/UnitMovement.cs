using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private float maxPosition = 1f;

    private Camera mainCamera;

    #region Server

    [Command] 
    private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, maxPosition, NavMesh.AllAreas))
        {
            return;
        }

        agent.SetDestination(hit.position);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    [ClientCallback]
    public void Update()
    {
        if (!isOwned)
        {
            return;
        }

        if (!Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;
        }

        CmdMove(hit.point);
    }

    #endregion
}
