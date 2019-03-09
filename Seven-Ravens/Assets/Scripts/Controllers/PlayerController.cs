using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class PlayerController : MonoBehaviour
    {
        // reference to player controller rigidbody
        [SerializeField]
        private Rigidbody2D _rigidbody;

        // reference to player controller speed
        [SerializeField]
        private float _speed;

        // reference to player controller animator 
        [SerializeField]
        private Animator _animator;

        // this controller's instance 
        private static PlayerController _instance;
        public static PlayerController Instance { get { return _instance; } }

        // reference to movement capability status 
        private bool _canMove = true;

        // reference to left bound limits
        private Vector3 _leftBoundary;

        // reference to right bound limits
        private Vector3 _rightBoundary;

        // initialize instance
        void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        // move player within bound limts
        void Update()
        {
            if (_canMove)
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * _speed;
            else
                _rigidbody.velocity = Vector2.zero;

            _animator.SetFloat("MoveX", _rigidbody.velocity.x);
            _animator.SetFloat("MoveY", _rigidbody.velocity.y);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                if (_canMove)
                {
                    _animator.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
                    _animator.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
                }
            }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _leftBoundary.x, _rightBoundary.x), Mathf.Clamp(transform.position.y, _leftBoundary.y, _rightBoundary.y), transform.position.z);
        }

        // set background map bounds of current level, stops player character from walking out of bounds
        public void SetBounds(Vector3 botLeft, Vector3 topRight)
        {
            _leftBoundary = botLeft + new Vector3(.5f, 1f, 0f);
            _rightBoundary = topRight + new Vector3(-.5f, -1f, 0f);
        }
    }
}