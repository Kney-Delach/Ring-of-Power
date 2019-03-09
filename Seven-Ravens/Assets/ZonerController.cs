using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class ZonerController : MonoBehaviour
    {
        // static reference to player tag to be checked on trigger in every instance 
        private static string _playerTag = "Player";

        // if triggered by player, trigger scene switch
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == _playerTag)
            {
                // TODO: Fade scene to zone tag'd scene 
            }
        }
    }
}
