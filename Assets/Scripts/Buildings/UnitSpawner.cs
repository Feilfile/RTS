using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    #region Server
    [Command]
    private void CmdSpawnUnit()
    {
        GameObject untInstance = Instantiate(unitPrefab, 
            unitSpawnPoint.position, 
            unitSpawnPoint.rotation);

        NetworkServer.Spawn(untInstance, connectionToClient);
    }

    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!isOwned)
        {
            return;
        }

        CmdSpawnUnit();
    }

    #endregion
}
