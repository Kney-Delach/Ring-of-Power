using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class PlayerController : MonoBehaviour
    {   
        #region STATS VARIABLES

        [Header("Player Stats")]
        // refernece to player health stat
        [SerializeField]
        private Stats _health;
        // reference to player mana stat
        [SerializeField] 
        private Stats _mana; 
        
        #endregion

        #region ABILITIES VARIABLES

        private Transform _currentTarget; 

        // reference to the player's ability  database
        private static Dictionary<string, Ability> _abilitiesDatabase;
        
        [Header("Player Abilities")]
        [SerializeField]
        private Ability[] _abilities; 

        #endregion

        #region MOVEMENT VARIABLES
        [Header("Movement Variables")]
        // reference to player controller rigidbody
        [SerializeField]
        private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody { get { return _rigidbody ; } }

        // reference to player controller speed
        [SerializeField]
        private float _speed;

        // reference to player controller animator 
        [SerializeField]
        private Animator _animator;

        // reference to movement capability status 
        private bool _canMove = true;
        public bool CanMove { get { return _canMove ; } set { _canMove = value ; } } 

        // reference to left bound limits
        private Vector3 _leftBoundary;

        // reference to right bound limits
        private Vector3 _rightBoundary;

        #endregion

        #region SINGELTON
        // this controller's instance 
        private static PlayerController _instance;
        public static PlayerController Instance { get { return _instance; } }

        // initialize instance
        void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
            
        
            if(_abilitiesDatabase == null)
            {
                _abilitiesDatabase = new Dictionary<string, Ability>();
                foreach(Ability abil in _abilities)
                    _abilitiesDatabase.Add(abil.name, abil);
            }

            
        
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        #endregion

        // move player within bound limts
        void Update()
        {  
            ProcessTargetting();
            ProcessMovement();
            ProcessStats();
            ProcessAbilities();
        }

        #region TARGETTING 
        
        // process the clicking on targets
        private void ProcessTargetting()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log(LayerMask.GetMask("Clickable"));
                
                // raycast from the mouse position into the game world
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,256);

                if (hit.collider != null) // if hit something
                {
                    if (hit.collider.tag == "Enemy") // check if we hit an enemy
                    {
                        _currentTarget = hit.transform;
                    }
                
                }
                else
                {
                    //Detargets the target
                    _currentTarget = null;
                }
            }
        }

        #endregion

        #region MOVEMENT 
        // process characters movement 
        private void ProcessMovement()
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

        // freeze character movement
        public void FreezePlayer()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _canMove = false; 
        }

        // unfreeze character movement 
        public void UnfreezePlayer()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _canMove = true;
        }

        #endregion 

        #region ABILITIES
        
        private void ProcessAbilities()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {   
                if(_currentTarget != null)
                {
                     if(_currentTarget.tag == "Enemy")
                        CastSpell("Firebolt");
                    else 
                        Debug.Log("PlayerController ProcessAbilities: Current target is ["+ _currentTarget.tag +"] .. target an enemy to cast fireball");
                }
                else 
                {
                    Debug.Log("PlayerController ProcessAbilities: No target, cannot cast fireball");
                }
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Pressed Key: R");
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("Pressed Key: T");
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Pressed Key: F");
            }
            if(Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Pressed Key: G");
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Pressed Key: Z");
            }
            if(Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Pressed Key: X");
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Pressed Key: C");
            }
        }
        public void CastSpell(string spellName)
        {
            GameObject ability = null; 
            switch (spellName)
            {
                case "Firebolt":
                if(_abilitiesDatabase[spellName]._active)
                    ability = (GameObject)Instantiate(_abilitiesDatabase[spellName]._prefab, transform.position, Quaternion.identity);
                    if( ability != null)
                        ability.GetComponent<FireboltController>()._target = _currentTarget;

                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "Invisibility":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "Haste":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "ProtectiveBubble":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "RemoveRoots":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "WaterFreeze":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "Polymorph":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                case "Heal":
                if(_abilitiesDatabase[spellName]._active)
                    Debug.Log("Casting Spell: " + spellName);
                    break;
                default:
                    break;
            }
        }

        #endregion 

        #region STATS

        private void ProcessStats()
        {   
            if(_mana.CompareMaximum())
            {
                _mana.AddValue(0.15f);
            }
            if(Input.GetKeyDown(KeyCode.M))
            {
                _health.ReduceValue(10f);
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                _mana.ReduceValue(10f);
            }

            if(Input.GetKeyDown(KeyCode.L))
            {
                _mana.AddValue(1f);
            }
        }
        #endregion
    }
}