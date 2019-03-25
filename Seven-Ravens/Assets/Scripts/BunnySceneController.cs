using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class BunnySceneController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _badZoner; 

        [SerializeField]
        private GameObject _neutralZoner;

        [SerializeField]
        private GameObject _goodZoner; 

        private static BunnySceneController _instance; 
        public static BunnySceneController Instance { get { return _instance ; } }
        private void Awake()
        {
            _instance = this;
        }

        // destroy instance on destroy
        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public void ActivateZoner(ChoicesMadeType type)
        {
            _badZoner.SetActive(false);
            _neutralZoner.SetActive(false);
            _goodZoner.SetActive(false);

            switch(type)
            {
                case ChoicesMadeType.Good:
                    _goodZoner.SetActive(true);
                    break; 
                case ChoicesMadeType.Neutral:
                    _neutralZoner.SetActive(true);
                    break; 
                case ChoicesMadeType.Bad:
                    _badZoner.SetActive(true);
                    break; 
                default:
                    break;
            }
        }
    }
}