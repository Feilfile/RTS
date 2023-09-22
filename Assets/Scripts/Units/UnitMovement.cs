using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private float maxPosition = 1f;
    [SerializeField] private Targeter targeter = null;
    [SerializeField] float chaseRange = 8f;

    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();
        if (targeter.GetTarget() != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange )
            {
                agent.SetDestination(target.transform.position);
            }
            else if (agent.hasPath)
            {
                agent.ResetPath();
            }
        }

        if (!agent.hasPath) {
            return;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            return;
        }

        agent.ResetPath();
    }

    [Command] 
    public void CmdMove(Vector3 position)
    {
        ServerMove(position);
    }

    [Server]
    public void ServerMove(Vector3 position)
    {
        targeter.ClearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, maxPosition, NavMesh.AllAreas))
        {
            return;
        }

        agent.SetDestination(hit.position);
    }

    [Server]
    private void ServerHandleGameOver()
    {
        agent.ResetPath();
    }

    #endregion

    #region Client

    /* override void OnStartAuthority()
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
    }*/

    #endregion
}
