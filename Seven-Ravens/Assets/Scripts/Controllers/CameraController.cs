using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rokemon
{
    public class CameraController : MonoBehaviour
    {
        // reference to player transform 
        private Transform _playerTransform;

        // reference to left map boundary vector
        private Vector3 _leftBoundary;

        // reference to right boundary vector
        private Vector3 _rightBound;

        private void Start()
        {
            _playerTransform = FindObjectOfType<PlayerController>().transform;

            float orthographicHeight = Camera.main.orthographicSize;

            float orthographicWidth = orthographicHeight * Camera.main.aspect;

            Tilemap backgroundTilemap = GameObject.FindGameObjectWithTag("BackGround").GetComponent<Tilemap>();
            backgroundTilemap.CompressBounds();

            _leftBoundary = backgroundTilemap.localBounds.min + new Vector3(orthographicWidth, orthographicHeight, 0f);
            _rightBound = backgroundTilemap.localBounds.max + new Vector3(-orthographicWidth, -orthographicHeight, 0f);

            PlayerController.Instance.SetBounds(backgroundTilemap.localBounds.min, backgroundTilemap.localBounds.max);
        }

        // updates camera position to track player movement
        void LateUpdate()
        {
            transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _leftBoundary.x, _rightBound.x), Mathf.Clamp(transform.position.y, _leftBoundary.y, _rightBound.y), transform.position.z);
        }
    }
}
