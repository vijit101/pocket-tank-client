using UnityEngine;

namespace PocketTanks.Tanks
{
    public class TankView : MonoBehaviour
    {
        public GameObject tankTurrent;
        public Transform BulletSpawnPos;
        // Start is called before the first frame update
        public void OnAngleChange(float AngularValue)
        {
            tankTurrent.transform.rotation = Quaternion.Euler(0, 0, AngularValue);
        }

        public void SetTankPos(Vector3 positionToSpawn)
        {
            transform.position = positionToSpawn;
        }
    }
}
