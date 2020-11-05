using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CheckCamera : Photon.MonoBehaviour
{

    public Camera cam; // Drag camera into here

    void Start()
    {
        if (photonView.isMine) return;

        else
        {
            cam.enabled = false;
        }
    }

}
