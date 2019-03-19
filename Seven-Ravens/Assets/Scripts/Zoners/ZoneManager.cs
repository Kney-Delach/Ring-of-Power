using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 

namespace Rokemon {
    public class ZoneManager : MonoBehaviour
    {

        public void Start()
        {
            PlayerController.Instance.SetBounds();
        }
    }
}