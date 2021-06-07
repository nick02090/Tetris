using UnityEngine;

namespace Tetris.Core
{
    public class ObjectSpawner : MonoBehaviour
    {
        public static GameObject Spawn(GameObject objectToSpawn, Vector3 spawnPosition, Transform parent)
        {
            return Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, parent);
        }
    }
}
