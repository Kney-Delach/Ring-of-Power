using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class WizardFightController : MonoBehaviour
    {

        [SerializeField]
        private ComController _fightWonComms;

        [SerializeField]
        private ComController _fightLostComms;

        [SerializeField]
        private GameObject[] _enemies; 

        private bool _fightWon = false;

        private bool _fightLost = false;
    
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private Transform _targetTransform;

        private static WizardFightController _instance ;

        public static WizardFightController Instance { get { return _instance ; } }

        public void Awake()
        {
            _instance = this; 
        }

        public void Update()
        {
            if(_fightWon)
            {
                _fightWon = false;
                _fightWonComms.TriggerCommunicationEvents();
                // perform fight win routine 
            }
            else if(_fightLost)
            {
                _fightLost = false ;
                _fightLostComms.TriggerCommunicationEvents();
                // perform fight lost routine 
            }
        }

        public void InstantiateFirebolt()
        {
            GameObject ability = (GameObject)Instantiate(_ability._prefab, transform.position, Quaternion.identity);
            if(ability != null)
            {
                EnemyFireboltController fireboltController = ability.GetComponent<EnemyFireboltController>();
                fireboltController._target = _targetTransform;
                fireboltController.Damage = _ability._damage;
            }
        }
        public void RemoveEnemies()
        {
            foreach(GameObject enemy in _enemies)
                enemy.SetActive(false);
        }
        public void WinFight()
        {
            _fightWon = true;
        }

        public void LoseFight()
        {

        }
        
    }
}