using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon {
    public class TransformReference : MonoBehaviour
    {
        [SerializeField]
        private GameObject _transformedObject; 
        public GameObject TransformedObject { get { return _transformedObject ; } }
    }
}