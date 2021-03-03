using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    private PhotonView PhotonView;
    public int roleId = 0; // 1 = loup-garou, 2 = villageois
    private int nmbOfVotes = 0;
    [SerializeField] TextMeshProUGUI nmbOfVotesText;
    string playerName;
    private Text uiText;

    public void SetPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    [PunRPC]
    public void GetPlayerName(float timer)
    {
        uiText.text = PhotonNetwork.playerName + " a été tué..." + ((int)timer).ToString();
    }
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        uiText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
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