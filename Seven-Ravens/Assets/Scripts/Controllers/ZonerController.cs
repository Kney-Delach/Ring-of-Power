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

        [SerializeField]
        private string _zoneName;

        [SerializeField]
        private string _zoneSceneName;

        // static reference to player tag to be checked on trigger in every instance 
        private static string _playerTag = "Player";

        // if triggered by player, trigger scene switch
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == _playerTag)
            {
                StartCoroutine(OnZonerTriggered());
            }
        }


        private IEnumerator OnZonerTriggered()
        {
            TransitionFader.PlayTransition(_transitionPrefab, _zoneName);
            yield return new WaitForSeconds(_playDelay);
            LevelLoader.LoadLevel(_zoneSceneName);
        }
    }
}
