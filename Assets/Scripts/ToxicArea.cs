using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter method caca");
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.Log("Enter method");
            return;
        }

        

        PhotonView photonView = other.GetComponent<PhotonView>();
        if(photonView != null)
        {
            PlayerManagement.Instance.ModifyHealth(photonView.owner, -10);
            Debug.Log("HIT");
        }
    }

}
