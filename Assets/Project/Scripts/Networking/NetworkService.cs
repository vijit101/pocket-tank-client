using System;
using PocketTanks.Generics;
using PocketTanks.Screens;
using PocketTanks.Tanks;
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
        TankView tankPlayer1;
        TankView tankPlayer2;


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
            PlayerPrefs.DeleteKey(KeyStrings.PlayerPriorityServer);
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
            socketIOComponent.On(KeyStrings.FireFromPlayer1, (evtData) => {
                FireDataServer fireP1 = JsonUtility.FromJson<FireDataServer>(evtData.data.ToString());
                tankPlayer1.OnAngleChange(fireP1.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 2 so fire player 1

            socketIOComponent.On(KeyStrings.FireFromPlayer2, (evtData) => {
                FireDataServer fireP2 = JsonUtility.FromJson<FireDataServer>(evtData.data.ToString());
                tankPlayer2.OnAngleChange(fireP2.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 1 so fire player 2
        }

        private void StartGame(SocketIOEvent ResponseData)
        {
            Debug.Log("Start Game");
            if (ScreenService.Instance.GetActiveScreen.screenType != ScreenType.GamePlay)
            {
                ScreenService.Instance.ChangeToScreen(ScreenType.GamePlay);
                tankPlayer1 = TankService.Instance.GetTank(new Vector3(-6,-3,0));
                tankPlayer2 = TankService.Instance.GetTank(new Vector3(6,3,0));

            }
            BaseScreen currentScreen = ScreenService.Instance.GetActiveScreen;
            GamePlayScreen gamePlayScreen = currentScreen.GetComponent<GamePlayScreen>();
            EnablePlayer enableData = JsonUtility.FromJson<EnablePlayer>(ResponseData.data.ToString());
            if (enableData!=null)
            {
                if (enableData.Enable == true && gamePlayScreen != null)
                {
                    if (PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer)=="")
                    {
                        Debug.Log("PlayerPriorityServer" + PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer));
                        PlayerPrefs.SetString(KeyStrings.PlayerPriorityServer, "1");     //1 refers to be a player 1 on server
                        gamePlayScreen.angleSlider.onValueChanged.AddListener(tankPlayer1.OnAngleChange);
                        gamePlayScreen.fireButton.onClick.AddListener(() => { SendGamePlayData(gamePlayScreen); });
                    }
                    gamePlayScreen.EnableAllInput();
                    Debug.Log("Player1");
                }
                else
                {
                    if (PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer) == "")
                    {
                        Debug.Log("PlayerPriorityServer" + PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer));
                        PlayerPrefs.SetString(KeyStrings.PlayerPriorityServer, "2");     //1 refers to be a player 1 on server
                        gamePlayScreen.angleSlider.onValueChanged.AddListener(tankPlayer2.OnAngleChange);
                        gamePlayScreen.fireButton.onClick.AddListener(() => { SendGamePlayData(gamePlayScreen); });

                    }
                    gamePlayScreen.DisableAllInput();
                    Debug.Log("Player2");
                }

            }
            
        }

        private void SendGamePlayData(GamePlayScreen screen)
        {
            JSONObject SendGameplayJson = new JSONObject();
            SendGameplayJson[KeyStrings.powerSlider] = new JSONObject(screen.powerSlider.value);
            SendGameplayJson[KeyStrings.angleSlider] = new JSONObject(screen.angleSlider.value);
            socketIOComponent.Emit(KeyStrings.FireGamePlayData,SendGameplayJson);
        }
    }
}

