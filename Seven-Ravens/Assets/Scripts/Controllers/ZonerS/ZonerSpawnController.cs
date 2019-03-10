﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class ZonerSpawnController : MonoBehaviour
    {
        [SerializeField]
        private string _zoneName; 
        public string ZoneName { get; }

        // Start is called before the first frame update
        void Start()
        {
            if (PlayerInformationController.Instance.PreviousZoneName == _zoneName)
            {
                PlayerController.Instance.transform.position = gameObject.transform.position;
            }
        }
    }
}
