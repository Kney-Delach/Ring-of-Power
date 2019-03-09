﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace LevelManagement
{
    // splash screen menu 
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        // reference to the ScreenFader component
        [SerializeField]
        private ScreenFader _screenFader;

        // reference to splash text
        [SerializeField]
        private Text _splashText;

        // reference to splash image
        [SerializeField]
        private GameObject _splashImage; 

        // reference to wait delay in seconds
        [FormerlySerializedAs("delay")]
        [SerializeField]
        private float _delay = 1f;

        // reference to pressed status
        private bool _pressed = false;

        // assign the ScreenFader component
        private void Awake()
        {
            _screenFader = GetComponent<ScreenFader>();
        }
        
        // fade on upon start
        private void Start()
        {
            _screenFader.FadeOn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_pressed)
            {
                _pressed = true;
                FadeAndLoad();
            }
        }

        // fade off the ScreenFader and load the MainMenu
        public void FadeAndLoad()
        {
            StartCoroutine(FadeAndLoadRoutine());
        }

        // coroutine to fade off the ScreenFader and load the main menu
        private IEnumerator FadeAndLoadRoutine()
        {
            yield return new WaitForSeconds(_delay);

            _screenFader.FadeOff();

            LevelLoader.LoadMainMenuLevel();

            _splashText.text = "";
            _splashImage.active = false;

            yield return new WaitForSeconds(_screenFader.FadeOffDuration);

            Object.Destroy(gameObject);
        }
    }
}
