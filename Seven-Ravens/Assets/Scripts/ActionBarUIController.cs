using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class ActionBarUIController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private Animator _fireboltFlashAnimator;

        [SerializeField]
        private Animator _healFlashAnimator;

        [SerializeField]
        private Animator _invisFlashAnimator;

        [SerializeField]
        private Animator _charmFlashAnimator;

        [SerializeField]
        private Animator _unrootFlashAnimator;
        
        [SerializeField]
        private Image[] _reloadImages;

        [SerializeField]
        private Text[] _reloadTexts; 


    }
}