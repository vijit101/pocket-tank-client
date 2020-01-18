using System;
using PocketTanks.Bullets;
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
                GetAndFireBullet(tankPlayer1.BulletSpawnPos, fireP1.powerSlider, fireP1.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 2 so fire player 1

            socketIOComponent.On(KeyStrings.HealthFromP1, (evtData) => {
                HealthData healthData = JsonUtility.FromJson<HealthData>(evtData.data.ToString());
                BaseScreen screen = ScreenService.Instance.GetActiveScreen;
                GamePlayScreen gamePlayScreen = screen.GetComponent<GamePlayScreen>();
                if (gamePlayScreen != null)
                {
                    gamePlayScreen.healthTextP1.text = "HP: "+healthData.playerHealth1.ToString();
                    gamePlayScreen.healthTextP2.text = "HP: "+healthData.playerHealth2.ToString();
                }
            });

            socketIOComponent.On(KeyStrings.HealthFromP2, (evtData) => {
                HealthData healthData = JsonUtility.FromJson<HealthData>(evtData.data.ToString());
                BaseScreen screen = ScreenService.Instance.GetActiveScreen;
                GamePlayScreen gamePlayScreen = screen.GetComponent<GamePlayScreen>();
                if (gamePlayScreen != null)
                {
                    gamePlayScreen.healthTextP1.text = "HP: " + healthData.playerHealth1.ToString();
                    gamePlayScreen.healthTextP2.text = "HP: " + healthData.playerHealth2.ToString();
                }
            });

            socketIOComponent.On(KeyStrings.FireFromPlayer2, (evtData) => {
                FireDataServer fireP2 = JsonUtility.FromJson<FireDataServer>(evtData.data.ToString());
                tankPlayer2.OnAngleChange(fireP2.angleSlider);
                GetAndFireBullet(tankPlayer2.BulletSpawnPos, fireP2.powerSlider, fireP2.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 1 so fire player 2
        }

        private void StartGame(SocketIOEvent ResponseData)
        {
            Debug.Log("Start Game");
            if (ScreenService.Instance.GetActiveScreen.screenType != ScreenType.GamePlay)
            {
                ScreenService.Instance.ChangeToScreen(ScreenType.GamePlay);
                tankPlayer1 = TankService.Instance.GetTank(new Vector3(-6,-3,0));// change it later
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
                        gamePlayScreen.fireButton.onClick.AddListener(() => { GetAndFireBullet(tankPlayer1.BulletSpawnPos,gamePlayScreen.powerSlider.value,gamePlayScreen.angleSlider.value); });

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
                        gamePlayScreen.fireButton.onClick.AddListener(() => { GetAndFireBullet(tankPlayer2.BulletSpawnPos, gamePlayScreen.powerSlider.value,gamePlayScreen.angleSlider.value); });
                    }
                    gamePlayScreen.DisableAllInput();
                    Debug.Log("Player2");
                }

            }
            
        }

        private void Instance_OnBulletCollide(TankView arg1, TankView arg2)
        {
            throw new NotImplementedException();
        }

        private void GetAndFireBullet(Transform spawnBulletPos,float BulletPower,float Angle)
        {
            Debug.Log("BulletPower" + BulletPower);
            BulletView bulletView = BulletService.Instance.GetBullet(spawnBulletPos,BulletPower,Angle);
        }

        private void SendGamePlayData(GamePlayScreen screen)
        {
            JSONObject SendGameplayJson = new JSONObject();
            SendGameplayJson[KeyStrings.powerSlider] = new JSONObject(screen.powerSlider.value);
            SendGameplayJson[KeyStrings.angleSlider] = new JSONObject(screen.angleSlider.value);
            socketIOComponent.Emit(KeyStrings.FireGamePlayData,SendGameplayJson);
        }

        public void EmitHealthEvent()
        {
            JSONObject SendHealthJson = new JSONObject();
            SendHealthJson[KeyStrings.playerHealth1] = new JSONObject(tankPlayer1.health); //remove to seperate fx create two events ondeath & onbullet trigger
            SendHealthJson[KeyStrings.playerHealth2] = new JSONObject(tankPlayer2.health); //remove to seperate fx
            socketIOComponent.Emit(KeyStrings.EmitHealthData,SendHealthJson);
        }
    }
}

