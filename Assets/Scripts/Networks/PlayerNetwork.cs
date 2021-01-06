using Boo.Lang;
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
    [SerializeField]List<GameObject> socles = new List<GameObject>();
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
            foreach(GameObject socle in GameObject.FindGameObjectsWithTag("Socle"))
            {
                socles.Add(socle);
            }

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

        playersIngame++;
        if(playersIngame == PhotonNetwork.playerList.Length)
        {
            Debug.Log("All players are in the game scene");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Debug.Log(socles.Count);
        int randomPos = Random.Range(0, socles.Count);
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "unitychan"),socles[randomPos].transform.position + new Vector3(0,1,0), Quaternion.identity, 0);
        socles.RemoveAt(randomPos);
        Debug.Log(socles.Count);

        currentPlayer = obj.GetComponent<PlayerMovement>();
        currentPlayer.id = Random.Range(1, 2);
        //Debug.Log(currentPlayer.id);
    }
}
