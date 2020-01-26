using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketTanks.Bullets
{
    public class BulletView : MonoBehaviour
    {
        Rigidbody2D rgbd;
        public float damage = 10;
        public GameObject OnHitParticleEffect;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Debug.LogError("++Player " + PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer));
                damagable.OnDamage(damage);
                if (OnHitParticleEffect != null)
                {
                    Instantiate(OnHitParticleEffect, transform.position,Quaternion.identity);

                }
                Destroy(this.gameObject);
            }
        }
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

