using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class WizardFightController : MonoBehaviour
    {
        
        [Header("Completion Communications")]
        [SerializeField]
        private ComController _wizardKilledComms;

        [SerializeField]
        private ComController _wizardSparedComms;

        [SerializeField]
        private ComController _wizardVictorComms;

        [Header("NPC References")]
        [SerializeField]
        private GameObject[] _enemies; 

        [Header("NPC References")]
        [SerializeField]
        private GameObject[] _redBrother;

        [SerializeField]
        private GameObject[] _purpleBrother;

        [SerializeField]
        private GameObject[] _blueBrother;

        [SerializeField]
        private GameObject[] _greenBrother; 

        [SerializeField]
        private GameObject[] _aquaBrother;

        [SerializeField]
        private GameObject[] _pinkBrother;

        [SerializeField]
        private GameObject[] _blackBrother;

        [SerializeField]
        private GameObject[] _mother;

        [Header("Phoenix Attack References")]

        [SerializeField]
        private EnemyController _phoenixController;
    
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private Transform _targetTransform;

        // reference to current item dicitonary 
        private Dictionary<BrotherRefColor, bool> _items;

        private bool _saveMother = false;

        private string _colorText = "";

        private string _brothersSavedText = "";

        private string _brothersNotSavedText = "";

        private static WizardFightController _instance ;

        public static WizardFightController Instance { get { return _instance ; } }

        public void Awake()
        {
            _instance = this; 
        }

        public string GetColorsText()
        {
            return _colorText;
        }

        public string GetBroSavedText()
        {
            return _brothersSavedText;
        }

        public string GetBroNotSavedText()
        {
            return _brothersNotSavedText;
        }

        public void InstantiateFirebolt()
        {
            GameObject ability = (GameObject)Instantiate(_ability._prefab, transform.position, Quaternion.identity);
            if(ability != null)
            {
                EnemyFireboltController fireboltController = ability.GetComponent<EnemyFireboltController>();
                fireboltController._target = _targetTransform;
                fireboltController.Damage = _ability._damage;
            }
        }
        public void RemoveEnemies()
        {
            foreach(GameObject enemy in _enemies)
                enemy.SetActive(false);
        }
        
        public void CalculateBrothersText()
        {
            int count = 0;
            
            if(_items[BrotherRefColor.Red])
            {
                _colorText += "Red, ";
                count ++;
            }

            if(_items[BrotherRefColor.Yellow])
            {
                _colorText += "Purple, ";    
                count ++;  
            }

            if(_items[BrotherRefColor.Blue])
            {
                _colorText += "Blue, ";
                count ++;
            }

            
            if(_items[BrotherRefColor.Green])
            {
                _colorText += "Green, ";
                count ++;
            }

            // AQUA BROTHER = ORANGE?
            if(_items[BrotherRefColor.Orange])
            {
                _colorText += "Aqua, ";
                count ++;
            }

            if(_items[BrotherRefColor.Pink])
            {
                _colorText += "Pink, ";
                count ++;
            }

            if(_items[BrotherRefColor.Black])
            {
                _colorText += "Black, ";
                count ++;
            }
            string tempstring = "";

            if(string.Compare(_colorText,tempstring) == 0)
            {
                _colorText = "No ";
            }
            _brothersSavedText = count.ToString();
            int temp = 7 - count; 
            _brothersNotSavedText += temp.ToString();
        }
        public void TransformBrothers()
        {
            if(_saveMother)
            {
                _mother[0].SetActive(false);
                _mother[1].SetActive(true);
            }
            
            if(_items[BrotherRefColor.Red])
            {
                _redBrother[0].SetActive(false);
                _redBrother[1].SetActive(true);
            }

            if(_items[BrotherRefColor.Yellow])
            {
                _purpleBrother[0].SetActive(false);
                _purpleBrother[1].SetActive(true); 
            }

            if(_items[BrotherRefColor.Blue])
            {
                _blueBrother[0].SetActive(false);
                _blueBrother[1].SetActive(true);
            }

            
            if(_items[BrotherRefColor.Green])
            {
                _greenBrother[0].SetActive(false);
                _greenBrother[1].SetActive(true);
            }

            // AQUA BROTHER = ORANGE?
            if(_items[BrotherRefColor.Orange])
            {
                _aquaBrother[0].SetActive(false);
                _aquaBrother[1].SetActive(true);
            }

            if(_items[BrotherRefColor.Pink])
            {
                _pinkBrother[0].SetActive(false);
                _pinkBrother[1].SetActive(true);;
            }

            if(_items[BrotherRefColor.Black])
            {
                _blackBrother[0].SetActive(false);
                _blackBrother[1].SetActive(true);
            }
        }

        public void ProcessWizardKilledComplete()
        {
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            _items = GetNeutralChoicesReferences(itemDictionary, false);
            _wizardKilledComms.TriggerCommunicationEvents();
            _saveMother = false;

        }

        public void ProcessWizardSparedComplete()
        {
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            _items = GetNeutralChoicesReferences(itemDictionary, true);
            _wizardSparedComms.TriggerCommunicationEvents();
            _saveMother = true;
        }

        public void ProcessWizardWonComplete()
        {
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            _items = GetNeutralChoicesReferences(itemDictionary, false);
            _wizardVictorComms.TriggerCommunicationEvents();
            _saveMother = false;
        }

        private void CheckBrotherStatus(Dictionary<BrotherRefColor,bool> dictionary)
        {
            foreach (KeyValuePair<BrotherRefColor, bool> kvp in dictionary)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Debug.Log("Brother : " + kvp.Key + " - " + kvp.Value);
            }
        }
        public void ProcessGirlDeath()
        {
            RemoveEnemies();
            _phoenixController.StopFireboltActivity();
            PlayerController.Instance.RegenMana = false;
            FireboltController[] firebolts = FindObjectsOfType<FireboltController>();
            EnemyFireboltController[] enemyFirebolts = FindObjectsOfType<EnemyFireboltController>();

            foreach(FireboltController cont in firebolts)
                Destroy(cont.gameObject);
            foreach(EnemyFireboltController econt in enemyFirebolts)
                Destroy(econt.gameObject);
            
            ProcessWizardWonComplete();
        }
        private Dictionary<BrotherRefColor, bool> GetNeutralChoicesReferences(Dictionary<BrotherRefColor,bool> inputList, bool neutralItemValue)
        {  
            List<BrotherRefColor> keyList = new List<BrotherRefColor>(inputList.Keys);
            
            if(!keyList.Contains(BrotherRefColor.Black))
                inputList.Add(BrotherRefColor.Black, neutralItemValue);
            else
                Debug.Log("Collected Black Item - Usable Status:" + inputList[BrotherRefColor.Black]);
            
            if(!keyList.Contains(BrotherRefColor.Blue))
                inputList.Add(BrotherRefColor.Blue, neutralItemValue);
            else
                Debug.Log("Collected Blue Item - Usable Status:" + inputList[BrotherRefColor.Blue]);

            if(!keyList.Contains(BrotherRefColor.Green))
                inputList.Add(BrotherRefColor.Green, neutralItemValue);
            else
                Debug.Log("Collected Green Item - Usable Status:" + inputList[BrotherRefColor.Green]);
            
            if(!keyList.Contains(BrotherRefColor.Orange))
                inputList.Add(BrotherRefColor.Orange, neutralItemValue);            
            else
                Debug.Log("Collected Orange Item - Usable Status:" + inputList[BrotherRefColor.Orange]);

            if(!keyList.Contains(BrotherRefColor.Pink))
                inputList.Add(BrotherRefColor.Pink, neutralItemValue);            
            else
                Debug.Log("Collected Pink Item - Usable Status:" + inputList[BrotherRefColor.Pink]);

            if(!keyList.Contains(BrotherRefColor.Red))
                inputList.Add(BrotherRefColor.Red, neutralItemValue);            
            else
                Debug.Log("Collected Red Item - Usable Status:" + inputList[BrotherRefColor.Red]);
            
            if(!keyList.Contains(BrotherRefColor.Yellow))
                inputList.Add(BrotherRefColor.Yellow, neutralItemValue);            
            else
                Debug.Log("Collected Yellow Item - Usable Status:" + inputList[BrotherRefColor.Yellow]);


            return inputList;
        }
        
    }
}