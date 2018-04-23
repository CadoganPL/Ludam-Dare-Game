using PlayFab;
using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;

public class LoginOrganizer : MonoBehaviour
{
    public GameObject Client;

    [HideInInspector] public string playfabID;
    [HideInInspector] public string SessionTicket;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Login()
    {
        LoginWithCustomIDRequest req = new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = Random.Range(1, 9999).ToString()
        };

        PlayFabClientAPI.LoginWithCustomID(req,
            (result) =>
            {
                playfabID = result.PlayFabId;
                SessionTicket = result.SessionTicket;
                //connect the server
                Instantiate(Client);
            },
            (error) =>
            {
                print("error : " + error.GenerateErrorReport());
            });
    }

    public bool isLoggedIn()
    {
        return !string.IsNullOrEmpty(SessionTicket);
    }
}
