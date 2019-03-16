using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class CutsceneDialogueManager : MonoBehaviour
    {
        [Header("List of dialogues")]
        [SerializeField]
        private Dialogue[] _dialogues; 

        [SerializeField]
        private float[] _waitTimes;
        private bool _nextDialogue;

        private int _curDialogueIndex = 0;

        //[SerializeField]
        private int _dialogueCounts; 

        [Header("List of active npc's for this scene")]

        [SerializeField]
        private NpcController[] _cutsceneNpcs;
        
        private PlayerController _playerConroller; 

        private void Start()
        {
            _dialogueCounts = _dialogues.Length;
            _playerConroller = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {   
            if(_curDialogueIndex < _dialogueCounts)
            {
                if(Input.GetKeyDown(KeyCode.K))
                {
                    BeginCutscene();
                    _playerConroller.FreezePlayer();
                }

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
            }
            else 
            {
                _playerConroller.UnfreezePlayer();
            }
        }
            
         

        private void BeginCutscene()
        {
            _nextDialogue = true;
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
    }   
}
