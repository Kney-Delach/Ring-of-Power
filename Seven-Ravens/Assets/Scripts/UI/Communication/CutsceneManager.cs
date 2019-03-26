using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement; 

namespace Rokemon {
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Transition FX")]  
        // reference to transition prefab
        [SerializeField]
        private TransitionFader _transitionPrefab;

        // reference to transition name 
        [SerializeField]
        private string _transitionName; 

        // if cutscene triggered automatically, set to true
        [Header("Cutscene automation trigger")]
        [SerializeField]
        private AudioController _transformAudioController;

        [SerializeField]
        private AudioController _wizardAudioController;

        [SerializeField]
        private AudioController _ravenAudioController;

        [SerializeField]
        private bool _playNextDialogue;

        [SerializeField]
        private bool _deactivatePlayer = false; 

        [SerializeField]
        private bool _activatePlayer = false;

        [SerializeField]
        private GameObject[] _transformNpcObjects; 

        [SerializeField]
        private GameObject[] _transformNewObjects; 

        // reference to active status of cutscene
        private bool _active = false;
        public bool Active { get { return _active ; } }

        [SerializeField]
        private float _playDelay = 0.5f;

        [Header("List of dialogues")]
        [SerializeField]
        private Dialogue[] _dialogues; 

        [SerializeField]
        private float[] _waitTimes;

        private int _curDialogueIndex = 0;

        //[SerializeField]
        private int _dialogueCounts; 

        private bool _complete = false;

        [Header("List of active npc's for this scene")]

        [SerializeField]
        private NpcController[] _cutsceneNpcs;
        
        #region  singleton
        private static CutsceneManager _instance; 
        public static CutsceneManager Instance { get { return _instance ;} }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                _instance = this;
            }
        }

        // remove instance if destroyed
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        #endregion

        private void Start()
        {
            _dialogueCounts = _dialogues.Length;
            
            if(_playNextDialogue)
                _active = true;

        }

        private void Update()
        {
            if(_active)
                ProcessCutscene();
        }

        // process npcs for cutscene event 
        private void ProcessNpcs()
        {
            foreach(NpcController npcCont in _cutsceneNpcs)
            {
                if(_dialogues[_curDialogueIndex].speakerTag != null){
                    if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].enableSpeakerVisibility)
                    {
                        if(_wizardAudioController != null)
                            _wizardAudioController.PlaySfx();
                        npcCont.gameObject.SetActive(true);
                    }
                    
                    if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].moveSpeaker)
                    {
                        if(npcCont.tag == "WIZARD")
                            _ravenAudioController.PlaySfx();
                        npcCont.Cutscene = true;
                    }
                }
                
            }

            if(_dialogues[_curDialogueIndex].transformNpcs && _transformNpcObjects.Length > 0)
            {
                if(_transformAudioController != null)
                    _transformAudioController.PlaySfx();

                foreach(GameObject npc in _transformNpcObjects)
                    npc.SetActive(false);
                foreach(GameObject newNpc in _transformNewObjects)
                    newNpc.SetActive(true);
            }
        }
        
        // process cutscene events
        private void ProcessCutscene()
        {
            if(_curDialogueIndex < _dialogueCounts && !_complete)
            {
                if(_playNextDialogue)
                {   
                    ProcessNpcs();
                    ComManager.Instance.BeginCommunication(ComType.Cutscene, _dialogues[_curDialogueIndex]);
                    StartCoroutine(CutsceneWaitRoutine());
                }
            }
            else if(!_complete)
            {
                _complete = true;
                StartCoroutine(CutsceneCompleteRoutine());
            }
        }    

        // called when zone is triggered
        private IEnumerator CutsceneCompleteRoutine()
        {   
            TransitionFader.PlayTransition(_transitionPrefab, _transitionName);
            yield return new WaitForSeconds(_playDelay);
            
            ComManager.Instance.EndCommunicationCutscene(); // end communication
            
            if(_deactivatePlayer && PlayerController.Instance != null)
            {
                PlayerController.Instance.FreezePlayer();
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";    
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -10;  
            }
            
            if(_activatePlayer && PlayerController.Instance != null)
            {
                PlayerController.Instance.UnfreezePlayer();
                
                PlayerController.Instance.DisplayHUD();
                PlayerController.Instance.AttackRangeOn();
                // TODO: REPLACE PLAYER POSITION
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";    
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;  
            }
            
            LevelLoader.LoadNextLevel();
            _active = false;
        }
         

        public void BeginCutscene()
        {
            if(PlayerController.Instance != null)
            {
                PlayerController.Instance.HideHUD();
                PlayerController.Instance.AttackRangeOff();
            }

            _active = true;
            _playNextDialogue = true;
        }


        IEnumerator CutsceneWaitRoutine()
        {
            _playNextDialogue = false; 
            yield return new WaitForSeconds(_waitTimes[_curDialogueIndex]);

            ComManager.Instance.EndCommunicationCutscene();

            _curDialogueIndex++;
            _playNextDialogue = true;
           
        
        }

        // private void SkipScene(int index)
        // {
        //     if(Input.GetKeyDown(KeyCode.Space))
        //     {
        //         StopAllCoroutines();
        //         foreach(NpcController npcCont in _cutsceneNpcs)
        //         {
        //             if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].moveSpeaker)
        //             {
        //                 npcCont.transform.position = new Vector3(npcCont.transform.position.x, npcCont.transform.position.y, npcCont.transform.position.z);
        //             }
        //         }
        //         _curDialogueIndex++;
        //         _nextDialogue = true;
        //     }
        // } 
           
    }   
}
