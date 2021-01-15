using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    bool allPlayerHere = false;

    int werewolf = 2;

    void Awake()
    {
        StartCoroutine(RandomArray());
    }
    IEnumerator RandomArray()
    {
        yield return new WaitForSeconds(3);
        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length < 5)
        {
            werewolf = 1;
        }

        if(players.Length > 5)
        {
            werewolf = 2;
        }

        for (int positionOfArray = 0; positionOfArray < players.Length; positionOfArray++)
        {
            GameObject obj = players[positionOfArray];
            int randomArray = Random.Range(0, players.Length);
            players[positionOfArray] = players[randomArray];
            players[randomArray] = obj;           
        }

        for(int i = 0; i < players.Length; i++)
        {
            if(i <= werewolf)
            {
                players[i].GetComponent<PlayerManagement>().SetRoleId(2);
                
            }
            else
            {
                players[i].GetComponent<PlayerManagement>().SetRoleId(1);
            }
            Debug.Log(players[i].GetComponent<PlayerManagement>().GetRoleId());
        }


        foreach(GameObject player in players)
        {
            player.GetComponentInChildren<Camera>().fieldOfView = 60;           
        }

        StopAllCoroutines();
    }

    void Update()
    {

    }
}
