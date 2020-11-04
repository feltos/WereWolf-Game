using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDDOL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.LoadLevel(1);
    }


}
