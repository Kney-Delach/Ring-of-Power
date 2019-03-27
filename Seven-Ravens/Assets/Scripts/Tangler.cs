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

        [SerializeField]
        private bool _isBunny = false;
        public bool IsBunny { get { return _isBunny ; } }

        [SerializeField]
        private ComController _bunnyController;

        [SerializeField]
        private GameObject _npcBunny;

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
                PlayerController.Instance.IsRooted = true;
                PlayerController.Instance.FreezePlayer();
                PlayerController.Instance.SetPosition(GetComponent<Transform>());

                ActionBarUIController.Instance.ActivateRootsFlash();
                ActionBarUIController.Instance.ActivateFireboltFlash();

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
            if(_isBunny)
            {
                _npcBunny.layer = 8;
                _bunnyController.TriggerCommunicationEvents();
            }
            else 
            {
                PlayerController.Instance.IsRooted = false;
            }

            if(_triggered)
            {
                PlayerController.Instance.UnfreezePlayer();
                PlayerController.Instance.RemoveRootTarget();

                ActionBarUIController.Instance.HideRootsFlash();
                ActionBarUIController.Instance.HideFireboltFlash();
            }
                
            gameObject.SetActive(false);
        }
    }
}