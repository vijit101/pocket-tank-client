using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketTanks.Bullets
{
    public class BulletView : MonoBehaviour
    {
        Rigidbody2D rgbd;
        private void Awake()
        {
            rgbd = gameObject.GetComponent<Rigidbody2D>();
            Destroy(this.gameObject, 4);
        }
        
        public void SetBulletPos(Vector3 positionToSpawn)
        {
            transform.position = positionToSpawn;
        }

        public void SetBulletRotation(float AngularValue)
        {
            transform.rotation = Quaternion.Euler(0, 0, AngularValue);
        }

        public void FireBullet(float PowerVal)
        {
            Debug.Log("FireBullet" + PowerVal);
            rgbd.AddRelativeForce(transform.up * PowerVal,ForceMode2D.Impulse);
        }
    }
}

