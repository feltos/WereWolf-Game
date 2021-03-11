using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    bool allPlayerHere = false;
    int werewolfNb = 1;

    //for gameplay loop
    bool gameStarted = false;
    [SerializeField] Text timerText;
    bool firstRound = true;
    [SerializeField]double loopTimer;
    bool nightPhase = false;
    bool nightPhaseStart = false;
    bool nightPhaseEnd = false;
    bool dayPhase = true;
    int mostVoted = 0;
    int nmbOfVotes = 0;
    int ratioPlayers;
    bool voteLoup;
    string playerKill = "Nobody";
    int loupsGarous = 0;
    int villageois = 0;

    
    //Règles 

    bool needNewRule = true;

    bool rule1 = false; //Tout le monde voit lors de la nuit
    bool rule2 = false; //Un joueur aléatoire est éliminé
    bool rule3 = false; //Le temps de vote passe à 30 secondes 
    bool rule4 = false; //Les loups-garous ne tuent qu'une fois
    bool rule5 = false; //Si un innocent est visé il est épargné
    bool rule6 = false; //Lors d'un vote des villageois le kill à 50& de chance de rater


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
        yield return new WaitForSeconds(2);

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
            player.GetComponent<PlayerManagement>().GetComponent<PhotonView>().RPC("SetRoleId", PhotonTargets.AllViaServer, 2);
        }

        players[0].GetComponent<PlayerManagement>().GetComponent<PhotonView>().RPC("SetRoleId", PhotonTargets.AllViaServer, 1);
        

        yield return new WaitForSeconds(2);

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
                
                loopTimer -= Time.deltaTime;
                timerText.text = "La partie commence dans " + ((int)loopTimer).ToString() + " secondes";


                foreach (GameObject player in players)
                {
                    player.GetComponentInChildren<Camera>().fieldOfView = 60;
                }

                if (loopTimer <= 0)
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
                    loopTimer = 5;
                    mostVoted = 0;
                    state = State.KILL_CALCUL;                   
                }

                break;

                //tout le monde s'endort
            case State.DAY_TO_NIGHT:

                loopTimer -= Time.deltaTime;
                timerText.text = "Le village s'endort... " + ((int)loopTimer).ToString();

                foreach (GameObject player in players)
                {
                    if (!rule1)
                    {
                        player.GetComponentInChildren<Camera>().fieldOfView = 0;
                    }
                }

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
                mostVoted = 0;

                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerManagement>().GetRoleId() == 1)
                    {
                        if (!rule1)
                        {
                            player.GetComponentInChildren<Camera>().fieldOfView = 0;
                        }
                        player.GetComponent<PlayerMovement>().SetCanVote(false);

                    }
                }

                if(loopTimer <= 0)
                {
                    mostVoted = 0;
                    voteLoup = true;
                    loopTimer = 5;
                    state = State.KILL_CALCUL;
                }

                break;

                //le kill loup garou ou villageois est annoncé
            case State.KILL_CALCUL:

                loopTimer -= Time.deltaTime;
                timerText.text = "En attente de la suite... " + ((int)loopTimer).ToString();
                foreach (GameObject player in players)
                {           
                    player.GetComponentInChildren<Camera>().fieldOfView = 60;
           
                }

              
                for (int i = 0; i < players.Count; i++)
                {              
                    if (players[i].GetComponent<PlayerManagement>().GetnmbOfVotes() > nmbOfVotes)
                    {
                        nmbOfVotes = players[i].GetComponent<PlayerManagement>().GetnmbOfVotes();
                        mostVoted = i;
                        playerKill = players[mostVoted].GetPhotonView().owner.NickName;
                    }
                }

              
                
                
                if(loopTimer <= 0)
                {                   
                    loopTimer = 10;                  
                    state = State.KILL_REVEAL;
                }

                
                break;

            case State.KILL_REVEAL:

                loopTimer -= Time.deltaTime;
                timerText.text = playerKill + " a été tué... " + ((int)loopTimer).ToString();

 
                players[mostVoted].GetComponent<PhotonView>().GetComponent<PlayerManagement>().SetDead();
                
                

                if (loopTimer < 6 && loopTimer > 3)
                {                   
                    foreach (GameObject player in players)
                    {
                        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
                        PhotonView playerPhotonView = playerManagement.GetComponent<PhotonView>();
                        playerPhotonView.RPC("ResetNmbOfVotes", PhotonTargets.AllViaServer);
                    }
                    
                }

                if (loopTimer <= 0)
                {
                    players.RemoveAt(mostVoted);

                    loopTimer = 5;
                    state = State.CHECK_WIN;
                }

                break;

                //Check si le nombre de villageois est supérieur aux loups garous
            case State.CHECK_WIN:

                loopTimer -= Time.deltaTime;
                timerText.text =  "En attente de la suite... " + ((int)loopTimer).ToString();


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

                if (loopTimer <= 0)
                {
                    if (loupsGarous == 0 || villageois <= 1)
                    {
                        timerText.text = "Les villageois ont gagnés !";
                    }
                    if (loupsGarous >= villageois)
                    {
                        timerText.text = "Les lous-garous ont gagnés !";
                    }
                    if (villageois > loupsGarous && voteLoup)
                    {
                        mostVoted = 0;
                        loupsGarous = 0;
                        villageois = 0;
                        nmbOfVotes = 0;
                        if (needNewRule)
                        {
                            loopTimer = 5;
                            state = State.NEW_RULE;
                        }
                        if (rule3)
                        {
                            loopTimer = 30;
                            state = State.DAY;
                        }
                        if(!needNewRule && !rule3)
                        {
                            loopTimer = 90;
                            state = State.DAY;
                        }
                        
                    }
                    if (loupsGarous < villageois && !voteLoup && loupsGarous != 0)
                    {
                        mostVoted = 0;
                        loopTimer = 5;
                        loupsGarous = 0;
                        villageois = 0;
                        nmbOfVotes = 0;
                        state = State.DAY_TO_NIGHT;
                    }
                }
                break;

            case State.NEW_RULE:
                loopTimer -= Time.deltaTime;

                //Rule1
                //timerText.text = "Tout le monde peut voir lors de la prochaine nuit... " + ((int)loopTimer).ToString();
                //rule1 = true;

                //if (loopTimer <= 0 && rule1)
                //{
                //    loopTimer = 90;
                //    needNewRule = false;
                //    state = State.DAY;
                //}

                //Rule2
                //timerText.text = "Un joueur aléatoire va être éliminé... " + ((int)loopTimer).ToString();
                //rule2 = true;

                //if (loopTimer <= 0 && rule2)
                //{                   
                //    needNewRule = false;
                //    loopTimer = 10;
                //    state = State.KILL_CALCUL;
                //}

                //Rule3
                //timerText.text = "Le temps de vote des villageois passe à 30 secondes... " + ((int)loopTimer).ToString();
                //rule3 = true;

                //if(loopTimer <= 0 && rule3)
                //{
                //    loopTimer = 30;
                //    needNewRule = false;
                //    state = State.DAY;
                //}

                //Rule4
                timerText.text = "Les loups-garous ne tuens plus..." + ((int)loopTimer).ToString();
                rule4 = true;
                if (loopTimer <= 0 && rule4)
                {
                    loopTimer = 30;
                    needNewRule = false;
                    state = State.DAY;
                }






                break;
        }
    }
}
