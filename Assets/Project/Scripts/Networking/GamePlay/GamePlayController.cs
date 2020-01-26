using PocketTanks.Networking;
using PocketTanks.Screens;
using PocketTanks.Tanks;
using SocketIO;
using UnityEngine;

public class GamePlayController
{
    private SocketIOComponent socketIOComponent;


    public GamePlayController(SocketIOComponent socket)
    {
        socketIOComponent = socket;
    }

    public void StartNewGame(SocketIOEvent ResponseData)
    {
        Debug.Log("Start Game");
        if (ScreenService.Instance.GetActiveScreen.screenType != ScreenType.GamePlay)
        {
            ScreenService.Instance.ChangeToScreen(ScreenType.GamePlay);
            NetworkService.Instance.tankPlayer1 = TankService.Instance.GetTank(new Vector3(-6, -3, 0));// change it later
            NetworkService.Instance.tankPlayer2 = TankService.Instance.GetTank(new Vector3(6, 3, 0));

        }
        BaseScreen currentScreen = ScreenService.Instance.GetActiveScreen;
        GamePlayScreen gamePlayScreen = currentScreen.GetComponent<GamePlayScreen>();
        EnablePlayer enableData = JsonUtility.FromJson<EnablePlayer>(ResponseData.data.ToString());
        if (enableData != null)
        {
            if (enableData.Enable == true && gamePlayScreen != null)
            {
                if (PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer) == "")
                {
                    Debug.Log("PlayerPriorityServer" + PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer));
                    PlayerPrefs.SetString(KeyStrings.PlayerPriorityServer, "1");     //1 refers to be a player 1 on server
                    gamePlayScreen.angleSlider.onValueChanged.AddListener(NetworkService.Instance.tankPlayer1.OnAngleChange);
                    gamePlayScreen.fireButton.onClick.AddListener(() => { NetworkService.Instance.SendGamePlayData(gamePlayScreen); });
                    gamePlayScreen.fireButton.onClick.AddListener(() => { NetworkService.Instance.GetAndFireBullet(NetworkService.Instance.tankPlayer1.BulletSpawnPos, gamePlayScreen.powerSlider.value, gamePlayScreen.angleSlider.value); });

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
                    gamePlayScreen.angleSlider.onValueChanged.AddListener(NetworkService.Instance.tankPlayer2.OnAngleChange);
                    gamePlayScreen.fireButton.onClick.AddListener(() => { NetworkService.Instance.SendGamePlayData(gamePlayScreen); });
                    gamePlayScreen.fireButton.onClick.AddListener(() => { NetworkService.Instance.GetAndFireBullet(NetworkService.Instance.tankPlayer2.BulletSpawnPos, gamePlayScreen.powerSlider.value, gamePlayScreen.angleSlider.value); });
                }
                gamePlayScreen.DisableAllInput();
                Debug.Log("Player2");
            }

        }

    }

}
