using SocketIO;
using UnityEngine;
namespace PocketTanks.Networking
{
    [RequireComponent(typeof(SocketIOComponent))]
    public class NetworkService : MonoBehaviour
    {
        public SocketIOComponent socketIOComponent;
        ConnectionStatus connectionStatus;

        // Start is called before the first frame update
        void Start()
        {
            connectionStatus = ConnectionStatus.UnConnected;
            PlayerPrefs.SetInt(KeyStrings.PlayerID, 0);
            socketIOComponent.Connect();
            socketIOComponent.On("connection", Connections);
            socketIOComponent.On("error", OnError);
        }

        private void Update()
        {
        }
        private void AuthentictionRequest()
        {
            if (connectionStatus == ConnectionStatus.Connected)
            {
                JSONObject PlayerData = new JSONObject();
                PlayerData[KeyStrings.PlayerID] = new JSONObject(PlayerPrefs.GetInt(KeyStrings.PlayerID));
                socketIOComponent.On(KeyStrings.AuthenticationRequest, AuthentictionResponse);
                socketIOComponent.Emit(KeyStrings.AuthenticationRequest, PlayerData);
            }
        }
        private void AuthentictionResponse(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + Server Connected" + socketIOEvent);
        }
        private void OnError(SocketIOEvent obj)
        {
            Debug.Log("NetworkService + Error" + obj);
        }

        void Connections(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + Server Connected" + socketIOEvent);
            connectionStatus = ConnectionStatus.Connected;
            AuthentictionRequest();
        }
    }


}
