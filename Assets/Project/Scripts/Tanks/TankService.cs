using PocketTanks.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketTanks.Tanks
{
    public class TankService : MonoSingletongeneric<TankService>
    {
        public TankView tankView;

        public TankView GetTank(Vector3 SpawnToPos)
        {
            TankView view = Instantiate<TankView>(tankView);
            view.SetTankPos(SpawnToPos);
            return view;
        }
    }
}


