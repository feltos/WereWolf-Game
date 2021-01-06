using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    private PhotonView PhotonView;

    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
    }

}