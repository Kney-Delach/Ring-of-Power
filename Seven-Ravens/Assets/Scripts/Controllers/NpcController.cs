using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class NpcController : MonoBehaviour
    {
        // reference to npc name 
        [SerializeField]
        private string _npcName;

        // reference to movement capability status 
        private bool _canMove = true;
        public bool CanMove { get { return _canMove; } set { _canMove = value; } }

        // reference to freeze movement due to player trigger 
        private bool _freezeMovement = false; 
        public bool FreezeMovement { get { return _freezeMovement ; } set { _freezeMovement = value ; } }

        // reference to npc controller speed
        [SerializeField]
        private float _speed = 1;

        // reference to object rigidbody
        [SerializeField]
        private Rigidbody2D _rigidbody;

        // reference to npc controller animator 
        [SerializeField]
        private Animator _animator;

        // reference to looping status of object movement 
        [SerializeField]
        private bool _looping = false;

        // reference to waypoint pause amount
        [SerializeField]
        private float _pauseAmount = 0;
        public float PauseAmount { get { return _pauseAmount; }  set { _pauseAmount = value; } }
        
        // reference to waypoint array
        [SerializeField]
        private GameObject[] _waypoints;
        public GameObject[] Waypoints {get { return _waypoints ; } }

        // reference to current objective waypoint
        private GameObject _currentWaypoint;
        public GameObject CurrentWaypoint {get { return _currentWaypoint ; } }

        // reference to current waypoint index
        private int _waypointIndex;

        // reversal status of npc movement between waypoints
        private bool _isReversing = false;

        // reference to event completion status (used to start npc movement)
        [SerializeField]
        private bool _cutscene = false; 
        public bool Cutscene { get { return _cutscene ; } set { _cutscene = value ; } }

        private void Start()
        {
            if (_waypoints.Length > 0)
                _currentWaypoint = _waypoints[0];

            _rigidbody.velocity = Vector2.zero;
            _animator.SetFloat("MoveX", _rigidbody.velocity.x);
            _animator.SetFloat("MoveY", _rigidbody.velocity.y);
                
        }

        private void UpdateNPC()
        {
            if (_currentWaypoint != null && _canMove)
            {   
                MoveNpc();
            }
            else
            {                    
                    _rigidbody.velocity = Vector2.zero;
                    _animator.SetFloat("MoveX", _rigidbody.velocity.x);
                    _animator.SetFloat("MoveY", _rigidbody.velocity.y);
                    if(_cutscene)
                        _cutscene = false;
            }
        }

        private void Update()
        {
            if(_cutscene)
            {
                UpdateNPC();
            }

        }

        // stops npc from moving
        private void PauseMovement()
        {
            _canMove = !_canMove;
        }

        // move npc between waypoints 
        private void MoveNpc()
        {
            // npc current position
            Vector3 currentPosition = transform.position;

            // target waypoint position
            Vector3 targetPosition = _currentWaypoint.transform.position;

            // if the npc isn't that close to the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) > .1f)
            {
                // Get the direction and normalize
                Vector3 directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();

                _animator.SetFloat("MoveX", directionOfTravel.x);
                _animator.SetFloat("MoveY", directionOfTravel.y);
                _animator.SetFloat("LastMoveX", directionOfTravel.x);
                _animator.SetFloat("LastMoveY", directionOfTravel.y);

                //scale the movement on each axis by the directionOfTravel vector components
                transform.Translate(
                    directionOfTravel.x * _speed * Time.deltaTime,
                    directionOfTravel.y * _speed * Time.deltaTime,
                    directionOfTravel.z * _speed * Time.deltaTime,
                    Space.World
                );
            }
            else
            {
                // if the waypoint has a pause amount then wait a bit
                if (_pauseAmount > 0)
                {
                    PauseMovement();
                    Invoke("PauseMovement", _pauseAmount);
                }
                NextWaypoint(); // call next waypoint
            }
        }

        // continue to next waypoint in waypoint array
        private void NextWaypoint()
        {
            if (_looping)
            {
                if (!_isReversing)
                {
                    if (_waypointIndex == _waypoints.Length - 1)
                    {
                        _isReversing = true;
                        _waypointIndex--; 
                    }
                    else
                    {
                        _waypointIndex++;
                    }
                    //_waypointIndex = (_waypointIndex + 1 >= _waypoints.Length) ? 0 : _waypointIndex + 1;
                }
                else
                {
                    if (_waypointIndex == 0)
                    {
                        _isReversing = false;
                        _waypointIndex++;
                    }
                    else
                    {
                        _waypointIndex--;
                    }
                    //_waypointIndex = (_waypointIndex == 0) ? _waypoints.Length - 1 : _waypointIndex - 1;
                }
            }
            else
            {
                // if at the start or the end then reverse
                if ((!_isReversing && _waypointIndex + 1 >= _waypoints.Length) || (_isReversing && _waypointIndex == 0))
                {
                    _isReversing = !_isReversing;
                }
                _waypointIndex = (!_isReversing) ? _waypointIndex + 1 : _waypointIndex - 1;
            }
            _currentWaypoint = _waypoints[_waypointIndex];
        }
    }
}