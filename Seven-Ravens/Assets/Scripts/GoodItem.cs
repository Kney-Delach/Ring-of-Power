using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class GoodItem : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _itemColl; 

        [SerializeField]
        private ComController _controller;

        [SerializeField]
        private SpriteRenderer _renderer; 

        [SerializeField]
        private Collider2D _rendererCollider; 

        [SerializeField]
        private ItemObject _object; 

        [SerializeField]
        private GameObject _child;

        [SerializeField]
        private NpcController _sheepParent; 

        public void ActivateObject()
        {
            _itemColl.enabled = true;
            
            _controller.enabled = true;

            _renderer.enabled = true;

            _rendererCollider.enabled = true;

            _object.enabled = true;

            _child.SetActive(true);

            MoveSheepGood();

        }

        public void MoveSheepGood()
        {
            GameObject charmCom = GameObject.FindGameObjectWithTag("CharmCom");
            charmCom.GetComponent<ComController>().TriggerCommunicationEvents();
            _sheepParent.IdleMoving = true;
        }

        public void MoveSheepBad()
        {
            GameObject charmCom = GameObject.FindGameObjectWithTag("CharmComBad");
            charmCom.GetComponent<ComController>().TriggerCommunicationEvents();
            _sheepParent.IdleMoving = true;
        }
    }
}