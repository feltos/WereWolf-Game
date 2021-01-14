using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameTag : Photon.MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameText; 
    void Start()
    {
        if (photonView.isMine)
        {
            return;
        }

        SetName();
    }

    private void SetName()
    {
        nameText.text = photonView.owner.NickName;
    }
}
