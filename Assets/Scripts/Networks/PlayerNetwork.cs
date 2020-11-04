using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string playerName { get; private set; }
    private PhotonView photonView;
    private int playersIngame = 0;

    private PlayerMovement currentPlayer;
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
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(2);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        PlayerManagement.Instance.AddPlayerStats(photonPlayer);

        playersIngame++;
        if(playersIngame == PhotonNetwork.playerList.Length)
        {
            Debug.Log("All players are in the game scene");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    public void NewHealth(PhotonPlayer PhotonPlayer, int health)
    {
        photonView.RPC("RPC_NewHealth", PhotonPlayer, health);
    }

    [PunRPC]
    private void RPC_NewHealth(int health)
    {
        if(currentPlayer == null)
        {
            return;
        }
        if(health <= 0)
        {
            PhotonNetwork.Destroy(currentPlayer.gameObject);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        float randomValue = Random.Range(0f, 5f);
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up * randomValue, Quaternion.identity, 0);
        currentPlayer = obj.GetComponent<PlayerMovement>();
    }
}
