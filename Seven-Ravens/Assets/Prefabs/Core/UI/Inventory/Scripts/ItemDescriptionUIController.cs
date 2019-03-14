using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {
    public class ItemDescriptionUIController : MonoBehaviour
    {
        [Header("UI Properties")]
        // reference to canvas group
        [SerializeField]
        private CanvasGroup _descriptionCanvasGroup; 

        // reference to text 
        [SerializeField]
        private Text _descriptionText; 

        // reference to image
        [SerializeField]
        private Image _descriptionImage;
        
        // reference to positional transform
        [SerializeField]
        private RectTransform _childTransform; 

        // reference to active item
        private Item _item; 

        #region Singleton

        // reference to instance
        private static ItemDescriptionUIController _instance = null;
        public static ItemDescriptionUIController Instance { get { return _instance ; } }

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
        public void DisplayCanvas(Item item, bool isLog, bool isQSource)
        {   
            if(isLog)
                _childTransform.anchoredPosition = new Vector2(850, -360); //localPosition = new Vector3(975, -360, transform.position.z);
            else if(isQSource)
                _childTransform.anchoredPosition = new Vector2(850, -275); 
            else 
                _childTransform.anchoredPosition = new Vector3(800, 200);
           
            _item = item;
            _descriptionText.text = _item.description; 
            _descriptionImage.sprite = _item.icon;
            _descriptionCanvasGroup.interactable = true; 
            _descriptionCanvasGroup.blocksRaycasts = true; 
            _descriptionCanvasGroup.alpha = 1;
        }
    }

}