using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public enum ChoicesMadeType
    {
        Good,
        Bad,
        Neutral
    }

    public class PlayerInformationController : MonoBehaviour
    {

        private static string STARTING_ZONE_ID = "Zone-1";

        // refernece to player previous player zone 
        private string _previousZoneName;
        public string PreviousZoneName { get {return _previousZoneName ; } }

        // reference to current player zone 
        private string _currentZoneName; 
        public string CurrentZoneName { get { return _currentZoneName ; } }

        // refernece to recent choices 
        private List<ChoicesMadeType> _choicesMadeHistory;
        public List<ChoicesMadeType> ChoicesMadeHistory { get { return _choicesMadeHistory ; } } 

        private int _currentChoiceIndex = 0;



        // reference to instance
        private static PlayerInformationController _instance = null;
        public static PlayerInformationController Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                _instance = this;
                _previousZoneName = string.Copy(STARTING_ZONE_ID);
                _currentZoneName = string.Copy(STARTING_ZONE_ID);
                _choicesMadeHistory = new List<ChoicesMadeType>();
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

        // function to update the zone string references of the character
        public void UpdateZones(string newZone)
        {
            _previousZoneName = string.Copy(_currentZoneName);
            _currentZoneName = string.Copy(newZone);
        }

        #region Choices Made 

        public void AddChoice(ChoicesMadeType choice)
        {
            _choicesMadeHistory.Add(choice);
            
            Debug.Log(_choicesMadeHistory[_currentChoiceIndex]);

            _currentChoiceIndex ++;

        }

        public void ReplaceChoice(ChoicesMadeType choice)
        {
            _choicesMadeHistory[_currentChoiceIndex-1] = choice;

            Debug.Log(_choicesMadeHistory[_currentChoiceIndex-1]);
        }

        public ChoicesMadeType GetRecentChoiceMade()
        {
            return _choicesMadeHistory[_currentChoiceIndex-1];

        }


        #endregion
    }
}
