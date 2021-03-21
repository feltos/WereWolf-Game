using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerManagement : MonoBehaviour
{
    public static PlayerManagement Instance;
    private PhotonView PhotonView;
    public int roleId; // 1 = loup-garou, 2 = villageois
    private int nmbOfVotes = 0;
    [SerializeField] TextMeshProUGUI nmbOfVotesText;
    string playerName;
    bool dead = false;
    [SerializeField]GameObject meshRoot;
    [SerializeField] Collider collider;

    void Start()
    {

    }

    public void SetDead()
    {
        if (PhotonView.isMine)
        {
            dead = true;
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    public void SetPlayerName(string newPlayerName)
    {
        playerName = newPlayerName;
        PhotonView.name = playerName;
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
        if (dead)
        {            
            PhotonView.RPC("Phantom", PhotonTargets.All);          
        }
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
        if (!dead)
        {
            nmbOfVotes++;
            nmbOfVotesText.text = nmbOfVotes.ToString();
        }    
    }

    [PunRPC]
    public void MinusNmbOfVotes()
    {
        if (!dead)
        {
            nmbOfVotes--;
            nmbOfVotesText.text = nmbOfVotes.ToString();
        }
    }

    [PunRPC]
    public void ResetNmbOfVotes()
    {
        nmbOfVotes = 0;
        nmbOfVotesText.text = nmbOfVotes.ToString();
    }

    [PunRPC]
    void Phantom()
    {
        meshRoot.SetActive(false);
    }



}