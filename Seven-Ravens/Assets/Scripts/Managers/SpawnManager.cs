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

        private bool _new = false;

        // reference to instance
        private static SpawnManager _instance = null; 
        public static SpawnManager Instance { get { return _instance ; } }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this; 
            
             if (PlayerController.Instance == null)
            {
                _new = true;
                GameObject dynamicParent = GameObject.FindGameObjectWithTag("DynamicParent");
                GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

                Instantiate(_playerPrefab, spawnPoint.transform.position, Quaternion.identity, dynamicParent.transform);
            }
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
            if (_new)
            {
                // do nothing
            }
            else if(PlayerInformationController.Instance != null && PlayerInformationController.Instance.CurrentZoneName == "MainMenu")
            {
                PlayerInformationController.Instance.UpdateZones("Zone-1"); // TOD: Fix bug where player spawns facing previously faced direction 
                GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
            }
        }        
    }

}
