using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class LobbyNetwork : PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.connected)
        {
            print("connecting to server...");
            PhotonNetwork.ConnectUsingSettings("0.0.0");
        }

    }
    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        PhotonNetwork.automaticallySyncScene = false;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();

        }
    }

}
