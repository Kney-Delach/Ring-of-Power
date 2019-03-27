using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelManagement;

namespace Rokemon {


    public class EndGame : MonoBehaviour
    {

        [SerializeField]
        private TransitionFader _transitionPrefab;
        private static EndGame _instance;
        public static EndGame Instance { get { return _instance ; } }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public void ComepleteGame()
        {
            StartCoroutine(GameOver());
        }

        private IEnumerator GameOver()
        {
            TransitionFader.PlayTransition(_transitionPrefab, "THE END");
            yield return new WaitForSeconds(1.5f);

            // reset item object counters
            ItemObject.ItemIdCounter = 0;
            ItemObject.ItemIdDatabase = null;

            DontDestroyOnLoad[] objectsToDestroy = FindObjectsOfType<DontDestroyOnLoad>();
            foreach(DontDestroyOnLoad obj in objectsToDestroy)
            {
                if(obj.tag != "Persist")
                    Destroy(obj.gameObject);
            }



            LevelLoader.LoadMainMenuLevel();
        }
    }

}