using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PlayFab.ClientModels;
using System.Collections;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using PlayFab;
using System;

public class Client : MonoBehaviour
{
    public static Client Inctance { set; get; }

    public int clientNumber;
    public string clientName;
    public string playfabID;
    public string matchMakeTicket;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private List<GameClient> players = new List<GameClient>();

    private PlayerControls Enemy;
    private PlayerControls Player;

    private void Start()
    {
        Inctance = this;
        playfabID = FindObjectOfType<LoginOrganizer>().playfabID; ;
        DontDestroyOnLoad(gameObject);        
    }

    public void ConnectToServer()
    {
        if (socketReady)
        {
            return;
        }

        try
        {
            MatchmakeRequest req = new MatchmakeRequest() { BuildVersion = "1", GameMode = "Classic", Region = Region.Japan };
            PlayFabClientAPI.Matchmake(req, MatchMakeCallBack, Error);            
        }
        catch (Exception e)
        {
            Debug.Log("Socket Error " + e.Message);
        }        
    }

    private void MatchMakeCallBack(MatchmakeResult obj)
    {
        socket = new TcpClient(obj.ServerHostname,(int)obj.ServerPort);
        stream = socket.GetStream();
        writer = new StreamWriter(stream);
        reader = new StreamReader(stream);
        matchMakeTicket = obj.Ticket;

        socketReady = true;
    }

    private void Error(PlayFabError obj)
    {
        Debug.Log(obj.ErrorMessage);
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();

                if(data != null)
                {
                    OnIncomingData(data);
                }
            }
        } 

        if(Player == null)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
            }
        }

        if(Enemy == null)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                Enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<PlayerControls>();
            }
        }
    }

    //Sending Messages to the Server
    public void Send(string data)
    {
        if (!socketReady)
        {
            return;
        }

        writer.WriteLine(data);
        writer.Flush();
    }
    
    //Read Messages from the Server
    private void OnIncomingData(string data)
    {
        Debug.Log("Client: " + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "SWHO":
                clientNumber = int.Parse(aData[1]);
                for (int i = 2; i < aData.Length - 1; i++)
                {
                    UserConnected(aData[i], false);
                }
                Send("CWHO|" + clientName + "|" + playfabID);
                break;
            case "SCNN":
                UserConnected(aData[1], false);
                break;
            case "SSELECTION":
                if(Player.selection == aData[1])
                {
                    return;
                }                
                Enemy.selection = aData[1];
                Enemy.isChoosen = true;
                break;
            case "SRESULT":
                Text resultText = GameObject.Find("ResultText").GetComponent<Text>();
                if (aData[1] == "DRAW")
                {
                    resultText.text = "Draw!";
                    resultText.GetComponentInParent<Animator>().SetTrigger("Win");
					EndGame();
                    return;
                }

                if(int.Parse(aData[1]) == clientNumber)
                {
                    resultText.text = "You Win!";
					resultText.GetComponentInParent<Animator>().SetTrigger("Win");
					EndGame();
                }
                else
                {
                    resultText.text = "You Lose!";
					resultText.GetComponentInParent<Animator>().SetTrigger("Win");
					EndGame();
                }
                break;
        }
    }

    private void UserConnected(string name,bool host)
    {
        GameClient c = new GameClient();
        c.name = name;
        c.playfabID = playfabID;

        players.Add(c);
        if(players.Count == 2)
        {
            GameManager.Inctance.StartGame();
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
    private void CloseSocket()
    {
        if (!socketReady)
        {
            return;
        }

        writer.Close();
        reader.Close();
        stream.Close();
        socket.Close();
        socketReady = false;
    }

	IEnumerator EndGame() 
	{
		yield return new WaitForSeconds (4f);

		SceneManager.LoadScene ("Lobby");

		DestroyImmediate (gameObject);
	}
}

public class GameClient
{
    public string name;
    public string playfabID;
}
