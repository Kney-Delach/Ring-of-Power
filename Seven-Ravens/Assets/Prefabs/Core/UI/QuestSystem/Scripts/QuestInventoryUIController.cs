using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rokemon {

    public class QuestInventoryUIController : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _questLogCanvasGroup; 

        [SerializeField]
        private Text _activeQuestText; 

        QuestInventory _questInventory; 

        private Button _selectedQuest; 

        [Header("Quest Context Panel References")]
        [SerializeField]
        private Text _questContextTitleText; 

        [SerializeField]
        private Text _questContextDescriptionText; 

        [SerializeField]
        private GameObject[] _questContextProgressSlots;

        [SerializeField]
        private ItemInventorySlot[] _questContextRewardSlots;

        //InventoryUIController _inventoryUiController;

        private bool _active; 
        
        
        #region #singleton

        // reference to instance 
        private static QuestInventoryUIController _instance = null; 
        public static QuestInventoryUIController Instance { get { return _instance ; } }

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
            _questInventory = QuestInventory.Instance;
            _questInventory.onQuestChangedCallback += UpdateUI;
            // _inventoryUiController = InventoryUIController.Instance;
            // _inventoryUiController.onInventoryActiveCallback += UpdateUI;
            UpdateUI();
        }

        
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q) && !_active)
            {
                DisplayQuestLog();
            } else if(Input.GetKeyDown(KeyCode.Q) && _active)
            {
                HideQuestLog();
            }
        }

        public void UpdateUI()
        {
            QuestInventorySlot[] slots = GetComponentsInChildren<QuestInventorySlot>();

            if(_selectedQuest == null)
                _selectedQuest = slots[0].QuestDataButton;
                
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < _questInventory.Quests.Count)
                {
                    slots[i].AddItem(_questInventory.Quests[i]);
                } else
                {
                    slots[i].ClearSlot();
                }
            }
            UpdateActiveQuestsDisplay();

            if(QuestInventory.Instance.Quests.Count != 0)
            {
                UpdateQuestDescriptionText(slots[0].Quest);
                UpdateQuestProgressText(slots[0].Quest);
            }
            else
            {
                UpdateQuestDescriptionText(null);
                UpdateQuestProgressText(null);
            }


        }

        public void HideQuestLog()
        {
            _questLogCanvasGroup.interactable = false; 
            _questLogCanvasGroup.blocksRaycasts = false; 
            _questLogCanvasGroup.alpha = 0;
            _active = false;
        }

        public void DisplayQuestLog()
        {
            _questLogCanvasGroup.interactable = true; 
            _questLogCanvasGroup.blocksRaycasts = true; 
            _questLogCanvasGroup.alpha = 1;
            _selectedQuest.Select();
            _selectedQuest.OnSelect(null);
            UpdateUI();
            _active = true; 
        }

        private void UpdateActiveQuestsDisplay()
        {
            _activeQuestText.text = "<Color=Green>QUESTS:</Color> " + QuestInventory.Instance.Quests.Count + "/" + QuestInventory.Instance.QuestSlots;
        }

        public void UpdateQuestDescriptionText(Quest quest)
        {
            if(quest == null)
                _questContextDescriptionText.text = "";
            else
            {
                _questContextDescriptionText.text = quest.questDescription;
            }    
        }

        public void UpdateQuestProgressText(Quest quest)
        {   
            if(quest == null)
            {
                for(int i = 0; i < 3; i++)
                    _questContextProgressSlots[i].SetActive(false);
            }
            else 
            {
                QuestGoal tempGoal = quest.questGoal;

                if(tempGoal.battleGoal)
                {
                    _questContextProgressSlots[0].SetActive(true);
                    if(tempGoal.battleGoalComplete)
                        _questContextProgressSlots[0].GetComponent<Text>().text = tempGoal.battleTag + " Defeated: " + "1/1";
                    else
                        _questContextProgressSlots[0].GetComponent<Text>().text = tempGoal.battleTag + " Defeated: " + "0/1";
                }
                else
                {
                    _questContextProgressSlots[0].SetActive(false);
                }   

                if(tempGoal.collectionGoal)
                {
                    _questContextProgressSlots[1].SetActive(true);
                    _questContextProgressSlots[1].GetComponent<Text>().text = tempGoal.collectionItem + " Collected: " + tempGoal.collectedAmount + "/" + tempGoal.collectionAmount;
            }
                else
                {
                    _questContextProgressSlots[1].SetActive(false);
                } 

                if(tempGoal.findNpcGoal)
                {
                    _questContextProgressSlots[2].SetActive(true);
                    if(tempGoal.findNpcGoalComplete)
                        _questContextProgressSlots[2].GetComponent<Text>().text = tempGoal.npcTag + " Found: " + "1/1";
                    else
                        _questContextProgressSlots[2].GetComponent<Text>().text = tempGoal.npcTag + " Found: " + "0/1";
                }
                else
                {
                    _questContextProgressSlots[2].SetActive(false);
                }      
            }
            
        }        
    }
}