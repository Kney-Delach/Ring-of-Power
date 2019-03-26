using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelManagement;

namespace Rokemon {
    public class CinematicManager : MonoBehaviour
    {
        private bool _active = true;

        [SerializeField]
        private TransitionFader _transitionPrefab;

        [SerializeField]
        private string _nextSceneName;

        [SerializeField]
        private ComController _comController; 

        [SerializeField]
        private Image _imageRenderer; 

        [SerializeField]
        private float[] waitTimes;

        [SerializeField]
        private Sprite[] _images;

        private int count;
        
        [SerializeField]
        private AudioController[] _cinematicVoiceFX;
        
        private bool _cinematicComplete = false;
        private bool _spacePressed = false;
        
        private static CinematicManager _instance;
        
        public static CinematicManager Instance { get { return _instance ; } }
        
        private void Awake()
        {
            _instance = this;
            count = 0;
        }

        private void Update()
        {
            if(_active && !_cinematicComplete)
            {
                TriggerDialogue();
            }

            if(Input.GetKeyDown(KeyCode.Space) && !_spacePressed && !_cinematicComplete)
            {
                _spacePressed = true;
                ComManager.Instance.EndCommunication();
                Object.FindObjectOfType<AudioManager>().PauseSfx(); 
                TriggerCinematicFinished();
            }
        }

        private void TriggerDialogue()
        {
            StartCoroutine(DialogueCoroutine());
        }

        private IEnumerator DialogueCoroutine()
        {            
            _active = false;


            _comController.Instance.CinematicTrigger();
            if(count < _images.Length && _images[count] != null)
            {
                _imageRenderer.sprite = _images[count]; 
            }
            if(count < _cinematicVoiceFX.Length && _cinematicVoiceFX[count] != null)
            {
                _cinematicVoiceFX[count].PlaySfx();
            }

            yield return new WaitForSeconds(waitTimes[count]);
            count ++;
            _active = true;
        }
        public void TriggerCinematicFinished()
        {
            _cinematicComplete = true;

            StopAllCoroutines();

            _active = false;
            StartCoroutine(CinematicFinishedRoutine());            
        }

        private IEnumerator CinematicFinishedRoutine()
        {
            TransitionFader.PlayTransition(_transitionPrefab,_nextSceneName);
            yield return new WaitForSeconds(0.5f);
            LevelLoader.LoadNextLevel();
        }
    }
}