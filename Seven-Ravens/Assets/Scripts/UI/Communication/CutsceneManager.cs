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

        // if cutscene triggered automatically, set to true
        [Header("Cutscene automation trigger")]
        [SerializeField]
        private bool _playNextDialogue;

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
                if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].enableSpeakerVisibility)
                    npcCont.gameObject.SetActive(true);
                
                if(npcCont.gameObject.tag == _dialogues[_curDialogueIndex].speakerTag && _dialogues[_curDialogueIndex].moveSpeaker)
                {
                    npcCont.Cutscene = true;
                }
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
                    // if(_dialogues[_curDialogueIndex].displayDialogue)
                    // {   
                    //     //DialogueManager.Instance.StartDialogue(_dialogues[_curDialogueIndex]);
                    //     ComManager.Instance.BeginCommunication(ComType.Cutscene, _dialogues[_curDialogueIndex]);
                    //     StartCoroutine(CutsceneWaitRoutine(true));
                    // }
                    // else 
                    // {
                    //     StartCoroutine(CutsceneWaitRoutine(false));

                    // }
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
            TransitionFader.PlayTransition(_transitionPrefab, "Home");
            yield return new WaitForSeconds(_playDelay);
            
            ComManager.Instance.EndCommunication(); // end communication
            
            LevelLoader.LoadNextLevel();
            _active = false;
        }
         

        public void BeginCutscene()
        {
            _active = true;
            _playNextDialogue = true;
        }


        IEnumerator CutsceneWaitRoutine()
        {
            _playNextDialogue = false; 
            yield return new WaitForSeconds(_waitTimes[_curDialogueIndex]);

            //if(communicationActive)
            ComManager.Instance.EndCommunication();

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
