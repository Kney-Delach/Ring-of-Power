using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement; 

namespace Rokemon {
    public class CutsceneDialogueManager : MonoBehaviour
    {
        [Header("Transition FX")]  
        // reference to transition prefab
        [SerializeField]
        private TransitionFader _transitionPrefab;

        // if cutscene triggered automatically, set to true
        [Header("Cutscene automation trigger")]
        [SerializeField]
        private bool _nextDialogue;

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
        
        private PlayerController _playerConroller; 

        #region  singleton
        private static CutsceneDialogueManager _instance; 
        public static CutsceneDialogueManager Instance { get { return _instance ;} }

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
            _playerConroller = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {   
            ProcessCutscene();
        }


        private void ProcessCutscene()
        {
            if(_curDialogueIndex < _dialogueCounts && !_complete)
            {
                // if(Input.GetKeyDown(KeyCode.K))
                // {
                //     BeginCutscene();
                // }

                if(_nextDialogue)
                {   
                    foreach(NpcController npcCont in _cutsceneNpcs)
                    {
                        if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].enableSpeakerVisibility)
                            npcCont.gameObject.SetActive(true);
                        
                        if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].moveSpeaker)
                        {
                            npcCont.Cutscene = true;
                            Debug.Log("NPC: " + npcCont.gameObject.tag + npcCont.Cutscene);
                        }
                    }
                    
                    if(_dialogues[_curDialogueIndex].displayDialogue)
                    {   
                        DialogueManager.Instance.StartDialogue(_dialogues[_curDialogueIndex]);
                        StartCoroutine(CutsceneWaitRoutine(true));
                    }
                    else 
                    {
                        StartCoroutine(CutsceneWaitRoutine(false));

                    }
                }

                //SkipScene(_curDialogueIndex);
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
            DialogueManager.Instance.EnableContinue();

            //PlayerController.Instance.FreezePlayer();
            TransitionFader.PlayTransition(_transitionPrefab, "Home");
            yield return new WaitForSeconds(_playDelay);
            Destroy(_playerConroller.gameObject);
            //PlayerInformationController.Instance.UpdateZones("Home");
            LevelLoader.LoadNextLevel();
            //LevelLoader.LoadLevel(_zoneSceneName);
            PlayerController.Instance.UnfreezePlayer();
            _active = false;
        }
         

        public void BeginCutscene()
        {
            TargetUIController.Instance.TargetChange(null); // remove target UI's target
            _active = true;
            _nextDialogue = true;
            if(_playerConroller != null)
                _playerConroller.FreezePlayer();
            
            DialogueManager.Instance.DisableContinue(); // disable "press space to continue" dialogue text
        }


        IEnumerator CutsceneWaitRoutine(bool activateNextScene)
        {
            _nextDialogue = false; 
            yield return new WaitForSeconds(_waitTimes[_curDialogueIndex]);

            if(activateNextScene)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }

            _curDialogueIndex++;
            _nextDialogue = true;
           
        
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
