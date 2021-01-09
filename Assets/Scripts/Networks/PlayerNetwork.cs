using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string playerName { get; private set; }
    public int Id { get => id; set => id = value; }

    private PhotonView photonView;
    private int playersIngame = 0;
    private PlayerMovement currentPlayer;
    SoclesManager soclesManager;
    GameObject[] socles;
    private int id;
    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
        name = "Player#" + Random.Range(1000,9999);
       
        PhotonNetwork.playerName = name;

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
 
        if(scene.name == "Game")
        {
            soclesManager = GameObject.FindGameObjectWithTag("SoclesManager").GetComponent<SoclesManager>();
            socles = soclesManager.Socles;

            if (PhotonNetwork.isMasterClient)
            {         
                MasterLoadedGame();
            }
            else
            {
                NonMasterLoadedGame();
            }
        }
    }

    private void MasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient,PhotonNetwork.player);
        photonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient,PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        playersIngame++;
        if(playersIngame == PhotonNetwork.playerList.Length)
        {
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(2);
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        id = PhotonNetwork.player.ID -1;
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "unitychan"), socles[id].transform.position + new Vector3(0, 1, 0), Quaternion.identity, 0);        
    }
}
