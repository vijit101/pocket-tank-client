namespace PocketTanks.Networking
{
    public class MatchMakingHandler
    {
        public void OnStartButton()
        {
            NetworkService.Instance.StartMatchMaking();
            //NetworkService.Instance.socketIOComponent.On(KeyStrings.StartGamePlay,StartGame);
        }
    }
}
