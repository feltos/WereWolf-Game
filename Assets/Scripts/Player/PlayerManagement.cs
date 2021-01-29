using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    private PhotonView PhotonView;
    public int roleId = 0; // 1 = loup-garou, 2 = villageois
    private int nmbOfVotes = 0;
    [SerializeField] TextMeshProUGUI nmbOfVotesText;
    string playerName;

    public void SetPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    [PunRPC]
    public string GetPlayerName()
    {
        return playerName;
    }
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
    }

    void Update()
    {

    }

    [PunRPC]
    public void SetRoleId(int newId)
    {
        roleId = newId;
    }

    [PunRPC]
    public int GetRoleId()
    {
        return roleId;
    }

    public int GetnmbOfVotes()
    {
        return nmbOfVotes;
    }

    [PunRPC]
    public void UpNmbOfVotes()
    {
        nmbOfVotes++;
        nmbOfVotesText.text = nmbOfVotes.ToString();      
    }

    [PunRPC]
    public void MinusNmbOfVotes()
    {
        nmbOfVotes--;
        nmbOfVotesText.text = nmbOfVotes.ToString();
    }

    [PunRPC]
    public void ResetNmbOfVotes()
    {
        nmbOfVotes = 0;
        nmbOfVotesText.text = nmbOfVotes.ToString();
    }

}