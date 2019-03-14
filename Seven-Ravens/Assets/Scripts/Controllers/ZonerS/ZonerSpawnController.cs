using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class ZonerSpawnController : MonoBehaviour
    {
        [SerializeField]
        private string _zoneName; 
        public string ZoneName { get; }

        // initializes player position to zoner position on scene load if player previous location = spawner referenced location
        void Start()
        {
            if (PlayerInformationController.Instance.PreviousZoneName == _zoneName)
            {
                PlayerController.Instance.transform.position = gameObject.transform.position;
            }
        }
    }
}
