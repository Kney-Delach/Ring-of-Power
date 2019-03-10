using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{

    public class SpawnManager : MonoBehaviour
    {
        // reference to player prefab
        [SerializeField]
        private GameObject _playerPrefab;

        // reference to instance
        private static SpawnManager _instance = null; 
        public static SpawnManager Instance { get { return _instance ; } }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this; 
        }

        // remove instance if destroyed
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

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
