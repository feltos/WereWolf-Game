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

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    { 
        _playerName.text = photonPlayer.NickName;
    }
}
