using PocketTanks.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingHandler
{
    public void OnStartButton()
    {
        NetworkService.Instance.StartMatchMaking();
    }
}
