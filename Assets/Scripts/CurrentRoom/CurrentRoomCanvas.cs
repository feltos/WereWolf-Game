using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    public void OnClickStart()
    {
        if (PhotonNetwork.isMasterClient /*&& PhotonNetwork.room.PlayerCount >= 4*/)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
            
            PhotonNetwork.LoadLevel(2);
        }
    }
}
