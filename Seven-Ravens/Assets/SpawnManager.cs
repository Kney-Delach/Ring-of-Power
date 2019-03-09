using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rokemon {
    public class SpawnManager : MonoBehaviour
    {
        // reference to player prefab
        [SerializeField]
        private GameObject _playerPrefab;

        private void Start()
        {
            if (PlayerController.Instance == null)
            {
                GameObject dynamicParent = GameObject.FindGameObjectWithTag("DynamicParent");
                GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

                Instantiate(_playerPrefab, spawnPoint.transform.position, Quaternion.identity, dynamicParent.transform);
            }
        }
    }
}
