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
    [SerializeField] Text timerText;
    bool firstRound = true;
    [SerializeField]float loopTimer;
    bool nightPhase = false;
    bool nightPhaseStart = false;
    bool nightPhaseEnd = false;
    bool dayPhase = true;

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


        if (gameStarted)
        {
            //la partie commence dans 30 secondes (a ne faire qu'une fois)
            if (!nightPhase && firstRound)
            {
                timerText.text = "La partie commence dans " + ((int)loopTimer).ToString() + " secondes";
                loopTimer -= Time.deltaTime;                 
            }













            //MAIN LOOP / RESTART HERE


            //la vue des villageois se ferme
            if (loopTimer <= 0 && !nightPhase && !nightPhaseStart && !nightPhaseEnd)
            {
                foreach (GameObject player in players)
                {
                    player.GetComponentInChildren<Camera>().fieldOfView = 0;
                }
                nightPhaseStart = true;
                loopTimer = 5;
                firstRound = false;
            }

            if (nightPhaseStart)
            {
                timerText.text = "Le village s'endort... " + ((int)loopTimer).ToString();
                loopTimer -= Time.deltaTime;

                if(loopTimer <= 0)
                {
                    nightPhase = true;
                    loopTimer = 30;
                }
            }
            //le timer de 30 secondes commence pour les villageois; les loups se réveillent et votent
            if (nightPhase)
            {
                nightPhaseStart = false;

                foreach (GameObject player in players)
                {
                    if(player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 60;
                        player.GetComponent<PlayerMovement>().SetCanVote(true);
                    }
                }
                loopTimer -= Time.deltaTime;
                timerText.text = "Les loups-garous choissisent leur victime... " + ((int)loopTimer).ToString();
                if(loopTimer <= 0)
                {
                    nightPhaseEnd = true;
                    loopTimer = 5;
                }
            }
            //la nuit se termine
            if (nightPhaseEnd)
            {
                nightPhase = false;
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 0;
                        player.GetComponent<PlayerMovement>().SetCanVote(false);
                    }
                }
                loopTimer -= Time.deltaTime;
                timerText.text = "Le village se réveille... " + ((int)loopTimer).ToString();
                if(loopTimer <= 0)
                {
                    nightPhaseEnd = false;
                }
            }

            // la vue des villageois revient et le mort du round est annoncé


            //le timer de 60 secondes démarre et les villageois peuvent voter

            // le joueur avec le plus de vote est tué 

            //check du nombre de villageois vs de loups pour voir si la partie se finit

            //ajout d'une régle aléatoire ou décidée par les joueurs

            //on recommence
        }

    }
}
