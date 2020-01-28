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
        public TankView tankPlayer1;
        public TankView tankPlayer2;


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

        /// <summary>
        /// written to see if any error whith socket
        /// </summary>
        /// <param name="obj"></param>        
        private void OnError(SocketIOEvent obj)
        {
            Debug.Log("NetworkService + Error" + obj);
        }

        /// <summary>
        /// When the server is connected go to authentication
        /// </summary>
        /// <param name="socketIOEvent"></param>
        void OnConnection(SocketIOEvent socketIOEvent)
        {
            Debug.Log("NetworkService + Server Connected" + socketIOEvent);
            connectionStatus = ConnectionStatus.Connected;
            AuthenticationController authentication = new AuthenticationController(socketIOComponent);
            authentication.AuthenticationRequest();
            //AuthenticationRequest();
        }

        /// <summary>
        /// bound to a button to start the matchmaking in the game and emit the socket
        /// </summary>
        public void StartMatchMaking()
        {
            ScreenService.Instance.ChangeToScreen(ScreenType.MatchMaking);
            socketIOComponent.Emit(KeyStrings.StartMatchMaking);
            socketIOComponent.On(KeyStrings.StartGamePlay, StartGame);

            socketIOComponent.On(KeyStrings.FireFromPlayer1, (evtData) =>
            {
                FireDataServer fireP1 = JsonUtility.FromJson<FireDataServer>(evtData.data.ToString());
                tankPlayer1.OnAngleChange(fireP1.angleSlider);
                GetAndFireBullet(tankPlayer1.BulletSpawnPos, fireP1.powerSlider, fireP1.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 2 so fire player 1

            socketIOComponent.On(KeyStrings.FireFromPlayer2, (evtData) =>
            {
                FireDataServer fireP2 = JsonUtility.FromJson<FireDataServer>(evtData.data.ToString());
                tankPlayer2.OnAngleChange(fireP2.angleSlider);
                GetAndFireBullet(tankPlayer2.BulletSpawnPos, fireP2.powerSlider, fireP2.angleSlider);
                Debug.Log(evtData);
            });// receieved from player 1 so fire player 2

            socketIOComponent.On(KeyStrings.HealthFromP1, (evtData) => {
                HealthData healthData = JsonUtility.FromJson<HealthData>(evtData.data.ToString());
                BaseScreen screen = ScreenService.Instance.GetActiveScreen;
                GamePlayScreen gamePlayScreen = screen.GetComponent<GamePlayScreen>();               
                if (gamePlayScreen != null)
                {
                    gamePlayScreen.healthTextP1.text = "HP: "+healthData.playerHealth1.ToString();
                    gamePlayScreen.healthTextP2.text = "HP: "+healthData.playerHealth2.ToString();
                }
                if (healthData.playerHealth1 == 0)
                {
                    ScreenService.Instance.ChangeToScreen(ScreenType.GameOver);
                    BaseScreen baseScreen = ScreenService.Instance.GetActiveScreen;
                    GameOverScreen gameOverScreen = baseScreen.GetComponent<GameOverScreen>();
                    gameOverScreen.PlayerGameOverText.text = "Player 2 Won";
                }
                if (healthData.playerHealth2 == 0)
                {
                    ScreenService.Instance.ChangeToScreen(ScreenType.GameOver);
                    BaseScreen baseScreen = ScreenService.Instance.GetActiveScreen;
                    GameOverScreen gameOverScreen = baseScreen.GetComponent<GameOverScreen>();
                    gameOverScreen.PlayerGameOverText.text = "Player 1 Won";
                }
            });

            socketIOComponent.On(KeyStrings.HealthFromP2, (evtData) =>
            {
                HealthData healthData = JsonUtility.FromJson<HealthData>(evtData.data.ToString());
                BaseScreen screen = ScreenService.Instance.GetActiveScreen;
                GamePlayScreen gamePlayScreen = screen.GetComponent<GamePlayScreen>();
                if (gamePlayScreen != null)
                {
                    gamePlayScreen.healthTextP1.text = "HP: " + healthData.playerHealth1.ToString();
                    gamePlayScreen.healthTextP2.text = "HP: " + healthData.playerHealth2.ToString();
                }
                if (healthData.playerHealth1 == 0)
                {
                    ScreenService.Instance.ChangeToScreen(ScreenType.GameOver);
                    BaseScreen baseScreen = ScreenService.Instance.GetActiveScreen;
                    GameOverScreen gameOverScreen = baseScreen.GetComponent<GameOverScreen>();
                    gameOverScreen.PlayerGameOverText.text = "Player 2 Won";
                }
                if (healthData.playerHealth2 == 0)
                {
                    ScreenService.Instance.ChangeToScreen(ScreenType.GameOver);
                    BaseScreen baseScreen = ScreenService.Instance.GetActiveScreen;
                    GameOverScreen gameOverScreen = baseScreen.GetComponent<GameOverScreen>();
                    gameOverScreen.PlayerGameOverText.text = "Player 1 Won";
                }
            });

        }

        private void StartGame(SocketIOEvent ResponseData)
        {
            GamePlayController gamePlay = new GamePlayController(socketIOComponent);
            gamePlay.StartNewGame(ResponseData);
        }
      
        public void GetAndFireBullet(Transform spawnBulletPos,float BulletPower,float Angle)
        {
            Debug.Log("BulletPower" + BulletPower);
            BulletView bulletView = BulletService.Instance.GetBullet(spawnBulletPos,BulletPower,Angle);
        }

        public void SendGamePlayData(GamePlayScreen screen)
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

