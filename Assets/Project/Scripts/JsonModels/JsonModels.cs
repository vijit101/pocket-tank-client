using System;

/// <summary>
/// Used For Authentication Player ID 
/// </summary>
[Serializable]
public class PlayerInfo
{
    public string PlayerID;
}
[Serializable]
public class EnablePlayer
{
    public bool Enable;
}
[Serializable]
public class FireDataServer
{
    public float powerSlider;
    public float angleSlider;
}
[Serializable]
public class HealthData
{
    public float playerHealth1;
    public float playerHealth2;

}
[Serializable]
public class PlayerDeathData
{
    public int playerDead;
}
