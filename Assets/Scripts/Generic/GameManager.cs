using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    PlayerManagement[] playerManagements;
    bool allPlayerHere = false;
    int werewolfNb;

    //for gameplay loop
    bool gameStarted = false;
    [SerializeField] float startTimer;
    [SerializeField] Text timerText;
    float loopTimer;

    void Awake()
    {
        StartCoroutine(RandomArray());
    }
    IEnumerator RandomArray()
    {
        yield return new WaitForSeconds(3);

        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length <= 5)
        {
            werewolfNb = 1;
        }

        if(players.Length > 5)
        {
            werewolfNb = 2;
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
            if(i <= werewolfNb)
            {
                players[i].GetComponent<PlayerManagement>().SetRoleId(1);
                
            }
            else
            {
                players[i].GetComponent<PlayerManagement>().SetRoleId(2);
            }
        }


        foreach(GameObject player in players)
        {
            player.GetComponentInChildren<Camera>().fieldOfView = 60;           
        }

        gameStarted = true;

        StopAllCoroutines();
    }

    void Update()
    {
        Debug.Log(startTimer);

        if (gameStarted)
        {
            //la partie commence dans 30 secondes (a ne faire qu'une fois)
          
                timerText.text = "La partie commence dans " + ((int)startTimer).ToString() + " secondes";
                startTimer -= Time.deltaTime;
            
            //la vue des villageois se ferme
            if(startTimer <= 0)
            {               
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 0;
                    }
                    else
                    {
                        player.GetComponent<PlayerMovement>().SetCanVote(true);
                    }
                }
            }
            //le timer de 30 secondes commence pour les villageois et les loups


            //les loups votent

            // la vue des villageois revient et le mort du round est annoncé

            //le timer de 60 secondes démarre et les villageois peuvent voter

            // le joueur avec le plus de vote est tué 

            //check du nombre de villageois vs de loups pour voir si la partie se finit

            //ajout d'une régle aléatoire ou décidée par les joueurs

            //on recommence
        }

    }
}
