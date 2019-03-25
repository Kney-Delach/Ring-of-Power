using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class WizardFightController : MonoBehaviour
    {

        [SerializeField]
        private ComController _wizardKilledComms;

        [SerializeField]
        private ComController _wizardSparedComms;

        [SerializeField]
        private ComController _wizardVictorComms;

        [SerializeField]
        private GameObject[] _enemies; 

        [SerializeField]
        private EnemyController _phoenixController;

    
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private Transform _targetTransform;

        private static WizardFightController _instance ;

        public static WizardFightController Instance { get { return _instance ; } }

        public void Awake()
        {
            _instance = this; 
        }

        public void Update()
        {
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
        public void ProcessWizardKilledComplete()
        {
            _wizardKilledComms.TriggerCommunicationEvents();
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            itemDictionary = GetNeutralChoicesReferences(itemDictionary, false);
            CheckBrotherStatus(itemDictionary);
        }

        public void ProcessWizardSparedComplete()
        {
            _wizardSparedComms.TriggerCommunicationEvents();
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            itemDictionary = GetNeutralChoicesReferences(itemDictionary, true);
            CheckBrotherStatus(itemDictionary);
        }

        public void ProcessWizardWonComplete()
        {
            _wizardVictorComms.TriggerCommunicationEvents();
            Dictionary<BrotherRefColor,bool> itemDictionary = ItemInventory.Instance.RemoveTransformItems();
            itemDictionary = GetNeutralChoicesReferences(itemDictionary, false);
            
            CheckBrotherStatus(itemDictionary);
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