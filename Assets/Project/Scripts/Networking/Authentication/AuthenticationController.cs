using PocketTanks.Networking;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationController
{
    protected JSONObject PlayerData = new JSONObject();
    private SocketIOComponent socketIOComponent;

    public AuthenticationController(SocketIOComponent socket)
    {
        socketIOComponent = socket;
    }

    public void AuthenticationRequest()
    {
        if (PlayerPrefs.GetString(KeyStrings.PlayerID) != "")
        {

            PlayerData[KeyStrings.PlayerID] = new JSONObject(PlayerPrefs.GetString(KeyStrings.PlayerID));
            Debug.Log("AuthenticationRequest + playerID" + PlayerPrefs.GetString(KeyStrings.PlayerID));
            PlayerAuthentication();
        }
        else
        {
            PlayerData[KeyStrings.PlayerID] = new JSONObject("0"); // set 0 for null check on server and assign a pid
            PlayerAuthentication();
        }
    }

    public void PlayerAuthentication()
    {
        socketIOComponent.Emit(KeyStrings.AuthenticationRequest, PlayerData);
        socketIOComponent.On(KeyStrings.AuthenticationResponse, OnAuthenticationResponse);

    }

    public void OnAuthenticationResponse(SocketIOEvent ResponseData)
    {
        Debug.Log("NetworkService + OnAuthenticationResponse" + ResponseData);
        PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(ResponseData.data.ToString());
        Debug.Log("Player id from json" + playerInfo.PlayerID);
        if (playerInfo.PlayerID != "")
        {
            PlayerPrefs.SetString(KeyStrings.PlayerID, playerInfo.PlayerID);
            Debug.Log("ID in if" + PlayerPrefs.GetString(KeyStrings.PlayerID));
        }
        Debug.Log("ID" + PlayerPrefs.GetString(KeyStrings.PlayerID));
    }
    
}
