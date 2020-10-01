using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string playerName { get; private set; }
    private void Awake()
    {
        Instance = this;

        name = "Player#" + Random.Range(1000,9999);
       
        PhotonNetwork.playerName = name;
    }

}
