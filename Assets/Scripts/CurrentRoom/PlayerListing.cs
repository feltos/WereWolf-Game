using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }
    [SerializeField]
    private Text _playerName;
    int playerSize;
    private Text PlayerName
    {
        get { return _playerName; }
    }
    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        //GameObject inputPlayerFieldEmptyObject = GameObject.Find("Pseudo");
        //InputField inputPlayerField = inputPlayerFieldEmptyObject.GetComponent<InputField>();
        //PhotonPlayer = photonPlayer;
        //photonPlayer.NickName = inputPlayerField.text;
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
        Debug.Log(PlayerName.text);       
    }
}
