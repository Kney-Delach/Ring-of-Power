using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class PositionPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if(PlayerInformationController.Instance.PreviousZoneName != "Fire Valley")
            {
                PlayerController player = Object.FindObjectOfType<PlayerController>(); 
                player.SetPosition(GetComponent<Transform>());
                PlayerInformationController.Instance.UpdateZones("Forest Roads");
            }

        }

    }
    
}