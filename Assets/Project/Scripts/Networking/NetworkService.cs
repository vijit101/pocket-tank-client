using PocketTanks.Generics;
using PocketTanks.Screens;
using SocketIO;
using UnityEngine;
namespace PocketTanks.Networking
{
    [RequireComponent(typeof(SocketIOComponent))]
    public class NetworkService : MonoSingletongeneric<NetworkService>
    {
        public SocketIOComponent socketIOComponent;
        public static ConnectionStatus connectionStatus;
        public bool IsDeletePlayerID = false;
        protected JSONObject PlayerData = new JSONObject();

        // Start is called before the first frame update
        void Start()
        {
            connectionStatus = ConnectionStatus.UnConnected;
            socketIOComponent.Connect();
            socketIOComponent.On("connection", OnConnection); // "Event",Function
            socketIOComponent.On("error", OnError);
            if (IsDeletePlayerID)
            {
                PlayerPrefs.DeleteKey(KeyStrings.PlayerID);
            }
        }

        private void OnError(SocketIOEvent obj)
        {
            Debug.Log("NetworkService + Error" + obj);
        }

        void OnConnection(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + Server Connected" + socketIOEvent);
            connectionStatus = ConnectionStatus.Connected;
            AuthenticationController authentication = new AuthenticationController(socketIOComponent);
            authentication.AuthenticationRequest();
            //AuthenticationRequest();
        }
        
        public void StartMatchMaking()
        {
            ScreenService.Instance.ChangeToScreen(ScreenType.MatchMaking);
            socketIOComponent.Emit(KeyStrings.StartMatchMaking);
            socketIOComponent.On(KeyStrings.StartGamePlay, StartGame);
        }

        private void StartGame(SocketIOEvent obj)
        {
            Debug.Log("Start Game");
            ScreenService.Instance.ChangeToScreen(ScreenType.GamePlay);

        }

    }
}

