﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
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
    int mostVoted = 0;
    int nmbOfVotes;
    int ratioPlayers;
    bool voteLoup;
    string playerKill = "Nobody";


    enum State
    {
        NONE,
        START,
        DAY,
        DAY_TO_NIGHT,
        NIGHT,
        NIGHT_TO_DAY,
        KILL_CALCUL,
        KILL_REVEAL,
        CHECK_WIN,
        NEW_RULE
    }

    State state = State.NONE;
    void Awake()
    {
        StartCoroutine(RandomArray());
    }
    IEnumerator RandomArray()
    {
        yield return new WaitForSeconds(3);

        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }

        if(players.Count <= 5)
        {
            werewolfNb = 1;
        }

        else        
        {
            werewolfNb = 2;
        }

        for (int positionOfArray = 0; positionOfArray < players.Count; positionOfArray++)
        {
            GameObject obj = players[positionOfArray];
            int randomArray = Random.Range(0, players.Count);
            players[positionOfArray] = players[randomArray];
            players[randomArray] = obj;           
        }

        for(int i = 0; i < players.Count; i++)
        {
            if(i < werewolfNb)
            {
                players[i].GetComponent<PlayerManagement>().GetComponent<PhotonView>().RPC("SetRoleId", PhotonTargets.All,1);
            }
            else
            {
                players[i].GetComponent<PlayerManagement>().GetComponent<PhotonView>().RPC("SetRoleId", PhotonTargets.All, 2);
            }
        }

        foreach(GameObject player in players)
        {
            player.GetComponentInChildren<Camera>().fieldOfView = 60;           
        }

        loopTimer = 20;
        state = State.START;      
    }

    void Update()
    {

     
        switch (state)
        {
            //la partie commence dans 30 secondes (a ne faire qu'une fois)
            case State.START:

                StopAllCoroutines();

                timerText.text = "La partie commence dans " + ((int)loopTimer).ToString() + " secondes";
                loopTimer -= Time.deltaTime;
                if(loopTimer <= 0)
                {
                    loopTimer = 5;
                    state = State.DAY_TO_NIGHT;
                }
                break;

                //tout le monde vote pendant 60 secondes
            case State.DAY:

                voteLoup = false;

                loopTimer -= Time.deltaTime;
                timerText.text = "Les villageois choissisent leur victime... " + ((int)loopTimer).ToString();

                foreach (GameObject player in players)
                {   
                    player.GetComponent<PlayerMovement>().SetCanVote(true);                   
                }

                if(loopTimer <= 0)
                {
                    state = State.KILL_CALCUL;
                }

                break;

                //tout le monde s'endort
            case State.DAY_TO_NIGHT:

                foreach (GameObject player in players)
                {
                    player.GetComponentInChildren<Camera>().fieldOfView = 0;
                }

                timerText.text = "Le village s'endort... " + ((int)loopTimer).ToString();
                loopTimer -= Time.deltaTime;

                if(loopTimer <= 0)
                {
                    loopTimer = 20;
                    state = State.NIGHT;
                }

                break;

                //les loups garous votent
            case State.NIGHT:

                loopTimer -= Time.deltaTime;
                timerText.text = "Les loups-garous choissisent leur victime... " + ((int)loopTimer).ToString();

                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 60;
                        player.GetComponent<PlayerMovement>().SetCanVote(true);
                    }
                }
                
                if(loopTimer <= 0)
                {
                    loopTimer = 5;
                    state = State.NIGHT_TO_DAY;
                }

                break;

                //tout le monde se réveille
            case State.NIGHT_TO_DAY:

                loopTimer -= Time.deltaTime;
                timerText.text = "Le village se réveille... " + ((int)loopTimer).ToString();
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 0;
                    }
                }

                if(loopTimer <= 0)
                {
                    voteLoup = true;
                    loopTimer = 5;
                    state = State.KILL_CALCUL;
                }

                break;

                //le kill loup garou ou villageois est annoncé
            case State.KILL_CALCUL:

                foreach (GameObject player in players)
                {           
                    player.GetComponentInChildren<Camera>().fieldOfView = 60;                  
                }

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].GetComponent<PlayerManagement>().GetnmbOfVotes() > mostVoted)
                    {
                        if (players[i].GetComponent<PlayerManagement>().GetnmbOfVotes() > nmbOfVotes)
                        {
                            nmbOfVotes = players[i].GetComponent<PlayerManagement>().GetnmbOfVotes();
                            mostVoted = i;                          
                        }
                    }
                }
          
                loopTimer = 10;
                playerKill = players[mostVoted].GetPhotonView().owner.NickName;
                players[mostVoted].GetComponent<PlayerManagement>().SetDead();
                players.RemoveAt(mostVoted);
                state = State.KILL_REVEAL;
                
                break;

            case State.KILL_REVEAL:

                loopTimer -= Time.deltaTime;

                if(loopTimer <= 8)
                {                   
                    timerText.text = playerKill + " a été tué... " + ((int)loopTimer).ToString();
                    
                    foreach (GameObject player in players)
                    {
                        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
                        PhotonView playerPhotonView = playerManagement.GetComponent<PhotonView>();
                        playerPhotonView.RPC("ResetNmbOfVotes", PhotonTargets.All);
                    }
                }
               
                if (loopTimer <= 0)
                {
                    state = State.CHECK_WIN;
                }

                break;

                //Check si le nombre de villageois est supérieur aux loups garous
            case State.CHECK_WIN:
                             
                mostVoted = 0;

                int loupsGarous = 0;
                int villageois = 0;
    
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        loupsGarous++;
                    }
                    else
                    {
                        villageois++;
                    }
                }

                if (loupsGarous == 0)
                {
                    timerText.text = "Les villageois ont gagnés !";
                }
                if (loupsGarous >= villageois)
                {
                    timerText.text = "Les lous-garous ont gagnés !";
                }
                if (villageois > loupsGarous && voteLoup)
                {
                    loopTimer = 60;
                    loupsGarous = 0;
                    villageois = 0;
                    state = State.DAY;
                }
                if (villageois > loupsGarous && !voteLoup && loupsGarous != 0)
                {
                    loopTimer = 5;
                    loupsGarous = 0;
                    villageois = 0;
                    state = State.DAY_TO_NIGHT;
                }                       

                break;

            case State.NEW_RULE:
                break;
        }
    }
}
