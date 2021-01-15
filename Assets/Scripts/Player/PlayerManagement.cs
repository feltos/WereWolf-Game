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

    public int GetnmbOfVotes()
    {
        return nmbOfVotes;
    }

    public void UpNmbOfVotes()
    {
        nmbOfVotes++;
        nmbOfVotesText.text = nmbOfVotes.ToString();
    }

    public void MinusNmbOfVotes()
    {
        nmbOfVotes--;
        nmbOfVotesText.text = nmbOfVotes.ToString();
    }
}