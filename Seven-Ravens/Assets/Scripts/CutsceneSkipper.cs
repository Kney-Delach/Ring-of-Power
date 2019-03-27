using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement;

namespace Rokemon {
    public class CutsceneSkipper : MonoBehaviour
    {   
        [SerializeField]
        private TransitionFader _transitionPrefab;

        [SerializeField]
        private CanvasGroup _skipableGroup;
        private bool _skipableActivated = false;

        public void Update()
        {
            if(CutsceneManager.Instance.Active && !_skipableActivated)
            {
                _skipableActivated = true;
                AbilityDescriptionUIController.Instance.HideCanvas();
                AbilityDescriptionUIController.Instance.Activatable = false;
                ActivateSkipable();
            }
            if(CutsceneManager.Instance.Active && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SkipCutscene());
            }
        }

        private void ActivateSkipable()
        {
            _skipableGroup.alpha = 1;
        }
        private IEnumerator SkipCutscene()
        {
            CutsceneManager.Instance.Active = false;
            CutsceneManager.Instance.StopAllCoroutines();

            TransitionFader.PlayTransition(_transitionPrefab, CutsceneManager.Instance.TransitionName);
            yield return new WaitForSeconds(0.5f);
            
            ComManager.Instance.EndCommunicationCutscene(); // end communication
            
            if(CutsceneManager.Instance.DeactivatePlayer && PlayerController.Instance != null)
            {
                PlayerController.Instance.FreezePlayer();
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";    
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -10;  
            }
            
            if(CutsceneManager.Instance.ActivatePlayer && PlayerController.Instance != null)
            {
                PlayerController.Instance.UnfreezePlayer();
                
                PlayerController.Instance.DisplayHUD();
                PlayerController.Instance.AttackRangeOn();

                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";    
                PlayerController.Instance.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;  
                AbilityDescriptionUIController.Instance.Activatable = true;
            }
            
            LevelLoader.LoadNextLevel();
        }
    }
}