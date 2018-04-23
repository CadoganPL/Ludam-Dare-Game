using System.Text.RegularExpressions;
using System.Collections.Generic;
using PlayFab.ServerModels;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine;
using System.Net;
using System.IO;
using PlayFab;
using System;

public class Server : MonoBehaviour
{
    public int port = 15950;

    private List<ServerClient> clients;
	private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public string lobbyId;

    //Similar to start but called on demand
    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            StartListening();

            RegisterGameRequest req = new RegisterGameRequest() { Build = "1", GameMode = "Classic", Region = Region.Japan,ServerHost = GetIP(),ServerPort = port.ToString() };
            PlayFabServerAPI.RegisterGame(req, ServerRegisterCallBack, (result) => Debug.LogError(result.ErrorMessage));
        }
        catch (Exception e)
        {
            Debug.Log("Socket Eroor : " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
        {
            return;
        }

        RefreshGameServerInstanceHeartbeat();

        foreach (ServerClient c in clients)
        {
            // Is the client still connected
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if(data != null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            //Tell our Player someone has Disconnected

            NotifyMatchmakerPlayerLeftRequest req = new NotifyMatchmakerPlayerLeftRequest() { LobbyId = lobbyId, PlayFabId = clients[i].playfabID};
			print (clients [i].playfabID);
			PlayFabServerAPI.NotifyMatchmakerPlayerLeft(req, PlayerLeftCallBack, (result) => Debug.Log(result.ErrorMessage));
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }


    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient i in clients)
        {
            allUsers += i.clientName.ToString() + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        StartListening();
        
        BroadCast("SWHO|"+ clients.Count.ToString() + "|" + allUsers, sc);
    }


    private bool IsConnected(TcpClient c)
    {
        try
        {
            if(c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Connect Error : " + e.Message);
            return false;
        }
    }

    //Server Send
    private void BroadCast(string data,List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message);
            }
        }
    }
    private void BroadCast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };

        BroadCast(data, sc);
    }

    //Server Read
    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log("Server: " + data);
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.clientName = aData[1];
                c.playfabID = aData[2];
                BroadCast("SCNN|" + c.clientName, clients);
                break;
            case "CSELECTION":
                BroadCast("SSELECTION|" + aData[1], clients);
                break;
            case "CRESULT":
                if(aData[1] == "DRAW")
                {
                    BroadCast("SRESULT|DRAW", clients);
                    return;
                }
                BroadCast("SRESULT|" + aData[1] + "|WIN",clients);
                break;
        }
    }

    public string GetIP()
    {
        string externalIP = "";
        externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
        externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
        return externalIP;
    }

    private void Error(PlayFabError obj)
    {
        Debug.LogError(obj.ErrorMessage);
    }

    private void ServerRegisterCallBack(RegisterGameResponse obj)
    {
        lobbyId = obj.LobbyId;
        serverStarted = true;

        FindObjectOfType<Button>().interactable = false;
    }

    private void DeRegisterServerCallBack(DeregisterGameResponse obj)
    {
        Debug.Log(obj.ToString());
    }

    public void RefreshGameServerInstanceHeartbeat()
    {
        PlayFabServerAPI.RefreshGameServerInstanceHeartbeat(new RefreshGameServerInstanceHeartbeatRequest()
        {
            LobbyId = lobbyId
        }, RefreshServerCallBack, Error);
    }

    private void PlayerLeftCallBack(NotifyMatchmakerPlayerLeftResult obj)
    {
        Debug.Log(obj.PlayerState);
    }

    private void RefreshServerCallBack(RefreshGameServerInstanceHeartbeatResult obj)
    {
        Debug.Log(obj.ToString());
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
        if (!serverStarted)
        {
            return;
        }

        DeregisterGameRequest req = new DeregisterGameRequest() { LobbyId = lobbyId };
        PlayFabServerAPI.DeregisterGame(req, DeRegisterServerCallBack, Error);
    }
}

public class ServerClient
{
    public string clientName;
    public string playfabID;
    public TcpClient tcp;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
