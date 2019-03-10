using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class PlayerInformationController : MonoBehaviour
    {
        private static string STARTING_ZONE_ID = "Zone-1";

        // refernece to player previous player zone 
        private string _previousZoneName;
        public string PreviousZoneName { get {return _previousZoneName ; } }

        // reference to current player zone 
        private string _currentZoneName; 
        public string CurrentZoneName { get { return _currentZoneName ; } }

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

        public void UpdateZones(string newZone)
        {
            _previousZoneName = string.Copy(_currentZoneName);
            _currentZoneName = string.Copy(newZone);
        }

        public string Text()
        {
            return _previousZoneName;
        }
    }
}
