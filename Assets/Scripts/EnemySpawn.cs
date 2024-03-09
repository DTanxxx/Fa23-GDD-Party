using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lurkers.Control
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] List<GameObject> spawnList = new List<GameObject>();

        private void Awake()
        {
            foreach (GameObject go in spawnList)
            {
                go.SetActive(false);
            }
        }
        public void SpawnEnemy()
        {
            if (spawnList.Count > 0)
            {
                foreach (GameObject go in spawnList)
                {
                    go.SetActive(true);
                }
            }
        }
    }
}
