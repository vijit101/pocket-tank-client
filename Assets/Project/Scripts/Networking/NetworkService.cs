using SocketIO;
using UnityEngine;
namespace PocketTanks.Networking
{
    [RequireComponent(typeof(SocketIOComponent))]
    public class NetworkService : MonoSingletonGeneric<NetworkService>
    {
        public SocketIOComponent socketIOComponent;
        protected ConnectionStatus connectionStatus;
        protected JSONObject PlayerData = new JSONObject();

        // Start is called before the first frame update
        void Start()
        {
            connectionStatus = ConnectionStatus.UnConnected;
            socketIOComponent.Connect();
            socketIOComponent.On("connection", OnConnection); // "Event",Function
            socketIOComponent.On("error", OnError);
        }

        private void Update()
        {
        }
        
        
        private void OnError(SocketIOEvent obj)
        {
            Debug.Log("NetworkService + Error" + obj);
        }

        void OnConnection(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + Server Connected" + socketIOEvent);
            connectionStatus = ConnectionStatus.Connected;
            AuthenticationRequest();
        }

        private void AuthenticationRequest()
        {
            if (connectionStatus == ConnectionStatus.Connected)
            {
                //socketIOComponent.Emit(KeyStrings.AuthenticationRequest, PlayerData);
                if(PlayerPrefs.GetString(KeyStrings.PlayerID) != "")
                {

                    PlayerData[KeyStrings.PlayerID] = new JSONObject(PlayerPrefs.GetString(KeyStrings.PlayerID));
                    Debug.Log("AuthenticationRequest" + PlayerPrefs.GetString(KeyStrings.PlayerID));
                    PlayerAuthentication();
                }
                else
                {
                    PlayerData[KeyStrings.PlayerID] = new JSONObject("0");
                    PlayerAuthentication();
                }
            }
        }

        private void PlayerAuthentication()
        {
            socketIOComponent.Emit(KeyStrings.AuthenticationRequest, PlayerData);
            socketIOComponent.On(KeyStrings.AuthenticationResponse, OnAuthenticationResponse);
            
        }

        private void OnAuthenticationResponse(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + OnAuthenticationResponse" + socketIOEvent);
            connectionStatus = ConnectionStatus.Authenticated;
        }
    }


}
