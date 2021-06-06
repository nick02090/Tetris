using UnityEngine;

namespace Tetris.Core
{
    public class ObjectSpawner : MonoBehaviour
    {
        public static void Spawn(GameObject objectToSpawn, Vector3 spawnPosition, Transform parent)
        {
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, parent);
        }
    }
}
