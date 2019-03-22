using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {

    [RequireComponent(typeof(Collider2D))]
    public class Tangler : MonoBehaviour
    {
        private static string PLAYER_TAG = "Player";

        [SerializeField]
        private GameObject _rooterTrigger; 

        [SerializeField]
        private GameObject _rootedObject; 

        [SerializeField]
        private Collider2D _collider; 

        // trigger automation reference
        [SerializeField]
        private bool _initialTrigger = false;

        private bool _triggered = false;

        private void Start()
        {
            if(_initialTrigger)
                TriggerRoot();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == PLAYER_TAG && !_triggered)
            {
                _triggered = true; 
                PlayerController.Instance.FreezePlayer();
                PlayerController.Instance.SetPosition(GetComponent<Transform>());
                TriggerRoot();
            }
        }

        public void TriggerRoot()
        {
            _rooterTrigger.SetActive(false);
            _rootedObject.SetActive(true);
            _collider.enabled = false;
        }

        public void DestroyRoot()
        {
            if(_triggered)
                PlayerController.Instance.UnfreezePlayer();
                
            gameObject.SetActive(false);
        }
    }
}