using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon{
    public class ButtonHighlighter : MonoBehaviour
    {
        [SerializeField]
        private Button _thisButton; 

        [SerializeField]
        private ButtonHighlighter _otherButtonOne; 

        [SerializeField]
        private ButtonHighlighter _otherButtonTwo;

        public void HighlightButton()
        {
            _otherButtonOne.UnHighlightButton();
            _otherButtonTwo.UnHighlightButton();
            _thisButton.image.color = Color.yellow;
        }

        public void UnHighlightButton()
        {
            _thisButton.image.color = Color.white;
        }
    }
}