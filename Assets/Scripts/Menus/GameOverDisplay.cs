using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNameText = null;
    [SerializeField] private GameObject gameOverDisplayParent = null;

    // Start is called before the first frame update
    void Start()
    {
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;

    }

    public void LeaveGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    private void ClientHandleGameOver(string winner)
    {
        winnerNameText.text = $"{winner} has won";

        gameOverDisplayParent.SetActive(true);
    }
}
