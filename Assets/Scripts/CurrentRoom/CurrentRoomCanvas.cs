using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    public void OnClickStart()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
            PhotonNetwork.LoadLevel(2);
        }
    }
}
