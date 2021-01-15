using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    private PhotonView PhotonView;
    public int roleId = 0; // 1 = loup-garou, 2 = villageois
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
    }

    public void SetRoleId(int newId)
    {
        roleId = newId;
    }

    public int GetRoleId()
    {
        return roleId;
    }

}