using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class EnemyFireboltController : MonoBehaviour
    {

        private Rigidbody2D _rigidBody;
        
        [SerializeField]
        private float _speed;

        public Transform _target { get; set; }

        private bool _targetSet = false; 

        private float _damage = 0;
        public float Damage{  set { _damage = value ; } } 

        // Use this for initialization
        void Start ()
        {
            //Creates a reference to the spell's rigidbody
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            if (_target != null)
            {
                //Calculates the spells direction
                Vector2 direction = _target.position - transform.position;

                //Moves the spell by using the rigid body
                _rigidBody.velocity = direction.normalized * _speed;

                //Calculates the rotation angle
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //Rotates the spell towards the target
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                //_target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();

            }


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((collision.tag == "Player") && collision.transform == _target)
            {
                //GetComponent<Animator>().SetTrigger("impact");
                _rigidBody.velocity = Vector2.zero;
                PlayerController.Instance.ReduceHealth(_damage);
                //_target.GetComponent<PlayerController>().ReduceValue(_damage);
                _target = null;
                Destroy(gameObject);
            }
        }
    }
}