using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    public class Ability : MonoBehaviour
    {

        [SerializeField]
        private string _name = "Ability Name"; 

        [SerializeField]
        private Sprite _sprite; 

        [SerializeField]
        private string _description = "Description of ability goes here"; 

    }

}