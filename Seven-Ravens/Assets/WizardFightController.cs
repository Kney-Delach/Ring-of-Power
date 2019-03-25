using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class WizardFightController : MonoBehaviour
    {

        [SerializeField]
        private ComController _wizardKilledComms;

        [SerializeField]
        private ComController _wizardSparedComms;

        [SerializeField]
        private ComController _wizardVictorComms;

        [SerializeField]
        private GameObject[] _enemies; 

        [SerializeField]
        private EnemyController _phoenixController;

    
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
        public void ProcessWizardKilledComplete()
        {
            _wizardKilledComms.TriggerCommunicationEvents();
        }

        public void ProcessWizardSparedComplete()
        {
            _wizardSparedComms.TriggerCommunicationEvents();
        }

        public void ProcessGirlDeath()
        {
            RemoveEnemies();
            _phoenixController.StopFireboltActivity();
            _wizardVictorComms.TriggerCommunicationEvents();
            PlayerController.Instance.RegenMana = false;
            FireboltController[] firebolts = FindObjectsOfType<FireboltController>();
            EnemyFireboltController[] enemyFirebolts = FindObjectsOfType<EnemyFireboltController>();

            foreach(FireboltController cont in firebolts)
                Destroy(cont.gameObject);
            foreach(EnemyFireboltController econt in enemyFirebolts)
                Destroy(econt.gameObject);
        }
        
    }
}