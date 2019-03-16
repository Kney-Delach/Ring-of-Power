using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement; 

namespace Rokemon
{
    public class ZonerController : MonoBehaviour
    {
        // delay before scene switch 
        [SerializeField]
        private float _playDelay = 0.5f;
        
        // reference to transition prefab
        [SerializeField]
        private TransitionFader _transitionPrefab;

        // refernece to zone transporting to name
        [SerializeField]
        private string _zoneName;

        // reference to zone transporting to scene name
        [SerializeField]
        private string _zoneSceneName;

        // static reference to player tag to be checked on trigger in every instance 
        private static string _playerTag = "Player";

        // if triggered by player, trigger scene switch
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == _playerTag)
                StartCoroutine(OnZonerTriggered());
        }
        
        // called when zone is triggered
        private IEnumerator OnZonerTriggered()
        {
            PlayerController.Instance.FreezePlayer();
            TransitionFader.PlayTransition(_transitionPrefab, _zoneName);
            yield return new WaitForSeconds(_playDelay);
            PlayerInformationController.Instance.UpdateZones(_zoneName);
            LevelLoader.LoadLevel(_zoneSceneName);
            PlayerController.Instance.UnfreezePlayer();
        }
    }
}
