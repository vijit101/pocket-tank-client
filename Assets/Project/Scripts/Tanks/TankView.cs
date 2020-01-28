using PocketTanks.Networking;
using System.Collections;
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
            if (health <= 0)
            {
                InstantDeathEffects();
                StartCoroutine(WaitTillDestroy(2.5f));               
            }
            else
            {
                NetworkService.Instance.EmitHealthEvent();
            }
        }

        public void SetTankPos(Vector3 positionToSpawn)
        {
            transform.position = positionToSpawn;
        }
        public void InstantDeathEffects()
        {
            Instantiate(OnDeathParticle, transform.position, Quaternion.identity);
        }
        public IEnumerator WaitTillDestroy(float time)
        {
            yield return new WaitForSeconds(time);
            NetworkService.Instance.EmitHealthEvent();
            Destroy(gameObject);
        }
    }
}
