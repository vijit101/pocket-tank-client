using PocketTanks.Networking;
using UnityEngine;

namespace PocketTanks.Tanks
{
    public class TankView : MonoBehaviour,IDamagable
    {
        public GameObject tankTurrent;
        public Transform BulletSpawnPos;
        [HideInInspector]
        public float health = 100;
        public GameObject OnDeathParticle;
        // Start is called before the first frame update
        public void OnAngleChange(float AngularValue)
        {
            tankTurrent.transform.rotation = Quaternion.Euler(0, 0, AngularValue);
        }

        public void OnDamage(float damage)
        {
            health -= damage;
            NetworkService.Instance.EmitHealthEvent();
            if (health <= 0)
            {
                if (OnDeathParticle != null)
                {
                    Instantiate(OnDeathParticle, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }

        }

        public void SetTankPos(Vector3 positionToSpawn)
        {
            transform.position = positionToSpawn;
        }
    }
}
