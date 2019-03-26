﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Rokemon
{
    public class PlayerController : MonoBehaviour
    {   
        #region Player HUD UI Components

        [SerializeField]
        private CanvasGroup[] _hudGroups;

        #endregion 
        #region STATS VARIABLES

        [Header("Player Stats")]
        // refernece to player health stat
        [SerializeField]
        private Stats _health;
        public Stats Health { get { return _health ; } }
        // reference to player mana stat
        [SerializeField] 
        private Stats _mana; 
        public Stats Mana { get { return _mana ; } }
        
        #endregion

        #region ABILITIES VARIABLES

        [SerializeField]
        private float _attackDistance = 6; 

        private Transform _currentTarget; 

        // reference to the player's ability  database
        private static Dictionary<string, Ability> _abilitiesDatabase;


        private static Dictionary<string, bool> _activeCheckDatabase; 

  
        private static Dictionary <string, bool> _reloadingCheckDatabase; 
        
        [Header("Player Abilities")]
        [SerializeField]
        private Ability[] _abilities; 

        private bool _regenMana = true; 
        public bool RegenMana { set { _regenMana = value ; } }

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

        #region OBSERVERS 

        // declare new target delegate type 
        public delegate void OnTargetChange(GameObject target); 

        // instantiate target observer set
        public event OnTargetChange notifyTargetObservers; 

        #endregion

        #region SINGELTON
        // this controller's instance 
        private static PlayerController _instance;
        public static PlayerController Instance { get { return _instance; } }

        // initialize instance
        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
            
            // TODO: Optimize this into a single iteration

            if(_activeCheckDatabase == null)
            {
                _activeCheckDatabase = new Dictionary<string, bool>(); 
                foreach(Ability abil in _abilities)
                    _activeCheckDatabase.Add(abil.name, true);
            }

            if(_reloadingCheckDatabase == null)
            {
                _reloadingCheckDatabase = new Dictionary<string, bool>(); 
                foreach(Ability abil in _abilities)
                    _reloadingCheckDatabase.Add(abil.name, false);
            }

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
            //&& !CutsceneDialogueManager.Instance.Active
            if(!ComManager.Instance.Active)
            {
                ProcessTargetting();
                ProcessAbilities();
            }
            ProcessMovement();
            ProcessStats();
        }

        #region HUD Hiding 
        public void HideHUD()
        {
            foreach(CanvasGroup group in _hudGroups)
                group.alpha = 0;
        }

        public void DisplayHUD()
        {
            foreach(CanvasGroup group in _hudGroups)
                group.alpha = 1;
        }
        #endregion

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
                    if (hit.collider.tag == "Enemy" || hit.collider.tag == "HealableEnemy" || hit.collider.tag == "CharmableEnemy" || hit.collider.tag == "Roots" || hit.collider.tag == "WIZARD") // check if we hit an enemy
                    {
                        _currentTarget = hit.transform;
                        notifyTargetObservers(_currentTarget.gameObject);

                    }
                    else if (hit.collider.tag == "TransformableGround")
                    {
                        _currentTarget = hit.transform;
                        notifyTargetObservers(null);

                    }
                    

                }
                else
                {
                    //Detargets the target
                    _currentTarget = null;
                    notifyTargetObservers(null);
                }
            }
        }

        // function removing current player target reference
        public void RemoveTarget()
        {
            if(_currentTarget != null)
            {
                _currentTarget = null;
                notifyTargetObservers(null);
            }

        }
        // function removing the roots selectable 
        public void RemoveRootTarget()
        {
            if(_currentTarget.tag == "Roots")
            {
                _currentTarget = null;
                notifyTargetObservers(null);
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
        
        // sets player controller gameobject position
        public void SetPosition(Transform newPosition)
        {
            GetComponent<Transform>().position = new Vector3(newPosition.position.x, newPosition.position.y, newPosition.position.z);
        }

        // set background map bounds of current level, stops player character from walking out of bounds
        public void SetBounds()
        {
            float orthographicHeight = Camera.main.orthographicSize;

            float orthographicWidth = orthographicHeight * Camera.main.aspect;

            Tilemap backgroundTilemap = GameObject.FindGameObjectWithTag("BackGround").GetComponent<Tilemap>();
            backgroundTilemap.CompressBounds();

            _leftBoundary = backgroundTilemap.localBounds.min + new Vector3(.5f, 1f, 0f);
            _rightBoundary = backgroundTilemap.localBounds.max + new Vector3(-.5f, -1f, 0f);
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

        // function deactivating an ability
        public void DeactivateAbility(string abilityName)
        {
            // TODO: Implement actionbar scrubbing for deactivated ability
            _activeCheckDatabase[abilityName] = false;
            ActionBarUIController.Instance.BlockAbility(abilityName);
        }

        // returns whether or not ability is currently active
        public bool CheckAbilityStats(string abilityName)
        {
            return _activeCheckDatabase[abilityName];
        }
        private void ProcessAbilities()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {   
                if(_currentTarget != null)
                {
                     if((_currentTarget.tag == "Enemy" || _currentTarget.tag == "HealableEnemy" || _currentTarget.tag == "CharmableEnemy" || _currentTarget.tag == "Roots") && _currentTarget.GetComponent<Stats>().CurrentValue > 0)
                        CastSpell("Firebolt");  // cast firebolt
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
                CastSpell("Haste"); // cast haste
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                CastSpell("Invisibility");  // cast invisibility
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                CastSpell("ProtectiveBubble");  // cast protective bubble
            }
            if(Input.GetKeyDown(KeyCode.G))
            {
                CastSpell("RemoveRoots");
            }
            if(Input.GetKeyDown(KeyCode.Z))
            {
                if(_currentTarget != null && _currentTarget.tag == "TransformableGround")
                {
                    CastSpell("WaterFreeze");
                }
            }
            if(Input.GetKeyDown(KeyCode.X))
            {
                CastSpell("Charm");         // cast charm
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                CastSpell("Heal");  // cast heal
            }
        }

        private IEnumerator SpellWaitCoroutine(float waitTime, string spellName)
        {
            _reloadingCheckDatabase[spellName] = true;
            yield return new WaitForSeconds(waitTime);
            _reloadingCheckDatabase[spellName] = false;
        }
        private IEnumerator HasteCoroutine(float abilityTime, float waitTime, string spellName)
        {
            float temp = waitTime - abilityTime; 

            _reloadingCheckDatabase[spellName] = true;
            yield return new WaitForSeconds(abilityTime);
            _speed /=2;
            yield return new WaitForSeconds(temp);
            _reloadingCheckDatabase[spellName] = false;
            

        }
        private IEnumerator InvisibleCoroutine(float abilityTime,float waitTime, string spellName, List<EnemyController> targetControllers)
        {
            float temp = waitTime - abilityTime; 
            _reloadingCheckDatabase[spellName] = true;
            yield return new WaitForSeconds(abilityTime);

            GetComponent<SpriteRenderer>().color = Color.white; 
            if(targetControllers != null)
            {
                foreach(EnemyController controller in targetControllers)
                    controller.TogglePlayerInvisibility();
            }

            yield return new WaitForSeconds(temp);
            _reloadingCheckDatabase[spellName] = false;
        }

        private IEnumerator ShieldRoutine(float abilityTime, float waitTime, string spellName)
        {
            float temp = waitTime - abilityTime; 
            _reloadingCheckDatabase[spellName] = true;
            yield return new WaitForSeconds(abilityTime);
            _health.DeactivateShield();

            GetComponent<SpriteRenderer>().color = Color.white; 
            yield return new WaitForSeconds(temp);

            _reloadingCheckDatabase[spellName] = false;
        }
        
        private IEnumerator CharmRoutine(float abilityTime, float waitTime, string spellName, GameObject target)
        {
            GameObject findCharm = GameObject.FindGameObjectWithTag("CharmScene");
            if(findCharm == null)
            {
                target.GetComponent<Stats>().DeactivateShield(); 
                if(target.GetComponentInChildren<EnemyController>() != null)
                    target.GetComponentInChildren<EnemyController>().StopFireboltActivity();
            }
            float temp = waitTime - abilityTime; 

            _reloadingCheckDatabase[spellName] = true;
            yield return new WaitForSeconds(abilityTime);
           
            if(findCharm == null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white; 
                //target.tag = "CharmableEnemy";
                target.layer = 8;
                target.GetComponentInChildren<EnemyController>().CanCastFirebolt = true;
            }

            yield return new WaitForSeconds(temp);
        }

        // TODO : Add checks for reload 
        // function casting a spell
        public void CastSpell(string spellName)
        {
            if( _activeCheckDatabase[spellName] && _mana.CurrentValue >= _abilitiesDatabase[spellName]._cost && !_reloadingCheckDatabase[spellName])
            {
                GameObject ability = null; 

                switch (spellName)
                {
                    case "Firebolt":
                        float dist = Vector3.Distance(_currentTarget.position, transform.position);
                        if(dist < _attackDistance)
                        {
                            ability = (GameObject)Instantiate(_abilitiesDatabase[spellName]._prefab, transform.position, Quaternion.identity);
                            if( ability != null)
                            {
                                UseMana(_abilitiesDatabase[spellName]._cost);
                                FireboltController fireboltController = ability.GetComponent<FireboltController>();
                                fireboltController._target = _currentTarget;
                                fireboltController.Damage = _abilitiesDatabase[spellName]._damage;
                                
                                ActionBarUIController.Instance.ReloadAbility(0,_abilitiesDatabase[spellName]._reloadTime);

                                StartCoroutine(SpellWaitCoroutine(_abilitiesDatabase[spellName]._reloadTime, spellName));
                            }
                        }
                        break;
                    case "Invisibility":
                        GameObject[] targetField = GameObject.FindGameObjectsWithTag("EnemyTargetField");
                        List<EnemyController> targetControllersList = new List<EnemyController>(); 
                        
                        if(targetField.Length != 0)
                        {
                            foreach(GameObject target in targetField)
                            {
                                targetControllersList.Add(target.GetComponent<EnemyController>()); 
                            }
                            if(targetControllersList.Count != 0)
                            {
                                GetComponent<SpriteRenderer>().color = Color.blue; 
                                foreach(EnemyController controller in targetControllersList)
                                {
                                    controller.TogglePlayerInvisibility();
                                }
                                UseMana(_abilitiesDatabase[spellName]._cost);
                                ActionBarUIController.Instance.ReloadAbility(2,_abilitiesDatabase[spellName]._reloadTime);
                                StartCoroutine(InvisibleCoroutine(_abilitiesDatabase[spellName]._damage,_abilitiesDatabase[spellName]._reloadTime, spellName, targetControllersList));
                            }                            
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().color = Color.blue; 
                            UseMana(_abilitiesDatabase[spellName]._cost);
                            ActionBarUIController.Instance.ReloadAbility(2,_abilitiesDatabase[spellName]._reloadTime);
                            StartCoroutine(InvisibleCoroutine(_abilitiesDatabase[spellName]._damage,_abilitiesDatabase[spellName]._reloadTime, spellName, null));
                        }                         
                        break;
                    case "Haste":
                        UseMana(_abilitiesDatabase[spellName]._cost);
                        _speed *= 2; 

                        ActionBarUIController.Instance.ReloadAbility(1,_abilitiesDatabase[spellName]._reloadTime);

                        StartCoroutine(HasteCoroutine(_abilitiesDatabase[spellName]._damage,_abilitiesDatabase[spellName]._reloadTime, spellName));
                        break;
                    case "ProtectiveBubble":
                        UseMana(_abilitiesDatabase[spellName]._cost);
                        GetComponent<SpriteRenderer>().color = Color.yellow; 
                        _health.ActivateShield();
                        ActionBarUIController.Instance.ReloadAbility(3,_abilitiesDatabase[spellName]._reloadTime);
                        StartCoroutine(ShieldRoutine(_abilitiesDatabase[spellName]._damage ,_abilitiesDatabase[spellName]._reloadTime,spellName));
                        break;
                    case "RemoveRoots":

                        if(_currentTarget != null && _currentTarget.tag == "Roots")
                        {   
                            float distance = Vector3.Distance(_currentTarget.position, transform.position);
                            if(distance <= _attackDistance)
                            {
                                UseMana(_abilitiesDatabase[spellName]._cost);
                                Tangler targetTangler = _currentTarget.gameObject.GetComponentInParent<Tangler>();
                                targetTangler.DestroyRoot();
                                RemoveTarget();
                                ActionBarUIController.Instance.ReloadAbility(4,_abilitiesDatabase[spellName]._reloadTime);
                                StartCoroutine(SpellWaitCoroutine(_abilitiesDatabase[spellName]._reloadTime, spellName));
                            }
                          
                        }                       
                        break;
                    case "WaterFreeze":
                        UseMana(_abilitiesDatabase[spellName]._cost);
                        TransformReference reference = _currentTarget.GetComponentInChildren<TransformReference>();
                        reference.TransformedObject.SetActive(true);
                        _currentTarget.gameObject.SetActive(false);
                        ActionBarUIController.Instance.ReloadAbility(5,_abilitiesDatabase[spellName]._reloadTime);
                        StartCoroutine(SpellWaitCoroutine(_abilitiesDatabase[spellName]._reloadTime, spellName));   
                        ActionBarUIController.Instance.HideFreezeFlash(); 
                        
                        GoodItem tempItem = FindObjectOfType<GoodItem>();
                        if(tempItem != null)
                        {
                            ActionBarUIController.Instance.ActivateFireboltFlash();
                            ActionBarUIController.Instance.ActivateCharmFlash();
                        }                        

                        break;
                    case "Charm":
                        if(_currentTarget != null && _currentTarget.tag == "CharmableEnemy" && _currentTarget.GetComponent<Stats>().CurrentValue > 0)
                        {   
                            float distance = Vector3.Distance(_currentTarget.position, transform.position);
                            if(distance < _attackDistance)
                            {
                                UseMana(_abilitiesDatabase[spellName]._cost);

                                _currentTarget.gameObject.GetComponent<SpriteRenderer>().color = Color.red; 
                                //_currentTarget.tag = "Untagged";
                                _currentTarget.gameObject.layer = 1;

                                ActionBarUIController.Instance.ReloadAbility(6,_abilitiesDatabase[spellName]._reloadTime);

                                StartCoroutine(CharmRoutine(_abilitiesDatabase[spellName]._damage,_abilitiesDatabase[spellName]._reloadTime, spellName, _currentTarget.gameObject));
                                RemoveTarget();
                            
                                // perform logic for charm scene
                                GoodItem item = FindObjectOfType<GoodItem>();
                                Debug.Log(item);
                                if(item != null)
                                {
                                    item.ActivateObject();
                                    ActionBarUIController.Instance.HideCharmFlash();
                                    ActionBarUIController.Instance.HideFireboltFlash();
                                }

                            }

                           

                        }                     
                        break;
                    case "Heal":
                        if(_currentTarget != null && _currentTarget.tag == "HealableEnemy" && (_currentTarget.gameObject.GetComponent<Stats>().CurrentValue != _currentTarget.gameObject.GetComponent<Stats>().MaxValue) && (_currentTarget.gameObject.GetComponent<Stats>().CurrentValue != 0) && (!_currentTarget.gameObject.GetComponent<Stats>().OneHealthTrigger))
                        {
                            float distanceHeal = Vector3.Distance(_currentTarget.position, transform.position);
                            if(distanceHeal <= _attackDistance)
                            {
                                UseMana(_abilitiesDatabase[spellName]._cost);
                                _currentTarget.gameObject.GetComponent<Stats>().AddValue(_abilitiesDatabase[spellName]._damage); 
                                ActionBarUIController.Instance.ReloadAbility(7,_abilitiesDatabase[spellName]._reloadTime);
                                StartCoroutine(SpellWaitCoroutine(_abilitiesDatabase[spellName]._reloadTime, spellName));
                            }
                        }
                        else if(_currentTarget == null && (_health.CurrentValue != _health.MaxValue) && _health.CurrentValue != 0)
                        {
                            UseMana(_abilitiesDatabase[spellName]._cost);
                            _health.AddValue(_abilitiesDatabase[spellName]._damage);   
                            ActionBarUIController.Instance.ReloadAbility(7,_abilitiesDatabase[spellName]._reloadTime); 
                            StartCoroutine(SpellWaitCoroutine(_abilitiesDatabase[spellName]._reloadTime, spellName));
                        }                           
                        
                        break;
                    default:
                        break;
                }
            }
            else 
            {
                if(!_activeCheckDatabase[spellName])
                    Debug.Log("Inactive Spell: " + spellName);
            }
           
        }

        // reduce mana value
        private void UseMana(float amount)
        {
            _mana.ReduceValue(amount);
        }

        // reduce player health
        public void ReduceHealth(float amount)
        {
            _health.ReduceValue(amount);
        }

        #endregion 

        #region STATS

        private void ProcessStats()
        {   
            if(_mana.CompareMaximum() && _regenMana)
            {
                _mana.AddValue(0.25f);
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
                _mana.AddValue(100f);
            }
        }
        #endregion
    }
}