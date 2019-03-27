using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Rokemon {
    public class AbilityDescriptionUIController : MonoBehaviour
    {
        [Header("UI Properties")]
        // reference to canvas group
        [SerializeField]
        private CanvasGroup _descriptionCanvasGroup; 

        // reference to title text 
        [SerializeField]
        private Text _descriptionTitle; 

        // reference to description text 
        [SerializeField]
        private Text _descriptionText; 

        // reference to mana text 
        [SerializeField]
        private Text _manaText; 

        [SerializeField]
        private Text _damageText; 

        [SerializeField]
        private Text _reloadText; 

        // reference to image
        [SerializeField]
        private Image _descriptionImage;

        // reference to active item
        private Ability _ability; 

        #region Singleton

        // reference to instance
        private static AbilityDescriptionUIController _instance = null;
        public static AbilityDescriptionUIController Instance { get { return _instance ; } }

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
            _descriptionCanvasGroup.interactable = false; 
            _descriptionCanvasGroup.blocksRaycasts = false; 
            _descriptionCanvasGroup.alpha = 0;
        }

        // hides description canvas
        public void HideCanvas()
        {
            _descriptionCanvasGroup.interactable = false; 
            _descriptionCanvasGroup.blocksRaycasts = false; 
            _descriptionCanvasGroup.alpha = 0;
        }

        // displays description canvas
        public void DisplayCanvas(Ability ability)
        {  
            Debug.Log("Display Canvas");
            _ability = ability;
            _descriptionTitle.text = _ability._name;
            _descriptionText.text = _ability._description;
            if(_ability._name == "Invisibility" || _ability._name == "ProtectiveBubble" || _ability._name == "Charm" || _ability._name == "Haste")
                _damageText.text = "Last Time: " + _ability._damage +"s";
            else if(_ability._name == "RemoveRoots" || _ability._name == "WaterFreeze")
                _damageText.text = "";
            else if(_ability.name == "Heal")
                _damageText.text = "Amount: " + _ability._damage + "hp";
            else 
            _damageText.text = "Damage: " + _ability._damage;

            _manaText.text = "Mana Cost: " + _ability._cost; 
            _reloadText.text = "Reload Time: " + _ability._reloadTime + "s";
            _descriptionImage.sprite = ability._icon;
            _descriptionCanvasGroup.interactable = true; 
            _descriptionCanvasGroup.blocksRaycasts = true; 
            _descriptionCanvasGroup.alpha = 1;
        }
    }

    
}