using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [CreateAssetMenu(fileName = "New Item", menuName = "Abilities/Spell")]
    public class Ability : ScriptableObject
    {
        public string _name = "Ability Name"; 

        public Sprite _icon;
        //public Sprite _sprite; 

        public string _description = "Description of ability goes here"; 

        public int _damage =  0; 

        public float _speed = 0; 

        public float _reloadTime = 1; 

        public float _castTime = 0; 

        public float _cost; 

        public Color _castBarColor; 

        public GameObject _prefab;
    }

}