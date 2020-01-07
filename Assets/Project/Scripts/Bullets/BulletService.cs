using PocketTanks.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PocketTanks.Bullets
{
    public class BulletService : MonoSingletongeneric<BulletService>
    {
        public BulletView bulletView;

        public BulletView GetBullet(Transform instantPos,float Power,float AngleVal)
        {
            BulletView view = Instantiate<BulletView>(bulletView);
            view.SetBulletPos(instantPos.position);
            view.SetBulletRotation(AngleVal);
            view.FireBullet(Power);           
            return view;
        }

    }
}
