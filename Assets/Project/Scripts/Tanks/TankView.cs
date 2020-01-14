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
        // Start is called before the first frame update
        public void OnAngleChange(float AngularValue)
        {
            tankTurrent.transform.rotation = Quaternion.Euler(0, 0, AngularValue);
        }

        public void OnDamage(float damage)
        {
            float healthLeft = health - damage;
            if (healthLeft > 0)
            {
                health = healthLeft;
                NetworkService.Instance.EmitHealthEvent(this,PlayerPrefs.GetString(KeyStrings.PlayerPriorityServer));
            }
            else
            {
                Debug.Log("Player Dead");
            }
        }

        public void SetTankPos(Vector3 positionToSpawn)
        {
            transform.position = positionToSpawn;
        }
    }
}
