using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class EnemyController : MonoBehaviour
    {
        
        [SerializeField]
        private Ability _ability; 

        [SerializeField]
        private bool _canCastFirebolt = false;
        public bool CanCastFirebolt { set { _canCastFirebolt = value ; } }

        private static string PLAYER_TAG = "Player";

        [SerializeField]
        private Transform _targetTransform; 

        [SerializeField]
        private Stats _enemyHealth; 

        [Header("Enemy Type")]
        [SerializeField]
        private bool _oneHealthTrigger = false; 

        [SerializeField]
        private bool _isCerberus = false;

        private bool _aggro = false; 

        private bool _stopActivity = false;
        public bool StopActivity{ get { return _stopActivity ; } }

        private bool _playerInvisibile = false;

        private bool _reloading = false; 
        private void Start()
        {
            if(PlayerController.Instance != null)
                _targetTransform = PlayerController.Instance.GetComponent<Transform>();
            
            if(_oneHealthTrigger)
                _enemyHealth.OneHealthTrigger = true;
        }

        private void Update()
        {   
            if(_aggro && _enemyHealth.OneHealthTrigger && !_playerInvisibile)
            {   
                if(_canCastFirebolt)
                {
                    StartCoroutine(FireboltRoutine());
                }
            }else if(_aggro && !_playerInvisibile && _isCerberus && _enemyHealth.CurrentValue != 0)
            {
                if(_canCastFirebolt)
                    StartCoroutine(FireboltRoutine());
            }
        }

        public void TogglePlayerInvisibility()
        {
            _playerInvisibile = !_playerInvisibile;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == PLAYER_TAG)
            {
                _aggro = true; 
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.tag == PLAYER_TAG)
            {
                _aggro = false; 
            }
        }

        public void StopFireboltActivity()
        {
            StopAllCoroutines();
            _stopActivity = true;
            _canCastFirebolt = false;
        }
        public void StopFireboltActivity(bool isCharmed)
        {
            StopAllCoroutines();
            _canCastFirebolt = false;
        }

        private IEnumerator FireboltRoutine()
        {
            _canCastFirebolt = false;
            GameObject ability = (GameObject)Instantiate(_ability._prefab, transform.position, Quaternion.identity);
            if(ability != null)
            {
                EnemyFireboltController fireboltController = ability.GetComponent<EnemyFireboltController>();
                fireboltController._target = _targetTransform;
                fireboltController.Damage = _ability._damage;
            }
            yield return new WaitForSeconds(4f);

            //ActionBarUIController.Instance.HideBubbleFlash();

            _canCastFirebolt = true;
        }    
    }
}