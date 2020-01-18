using PocketTanks.Generics;
using PocketTanks.Tanks;
using System;


public class EventService : MonoSingletongeneric<EventService>
{
    public event Action<TankView, TankView> OnBulletCollide;

    public void OnBulletCollideEvent(TankView T1,TankView T2)
    {
        OnBulletCollide?.Invoke(T1,T2);
    }
}
