using System.Collections;
using Cinemachine;
using UnityEngine;
using Utilities;

namespace Universal
{
    public class CameraController : MonoBehaviour
    {
        public float zoomSpeed = 1.0f;
        public float maxZoomDistance = 10.0f;
        [Header("虚拟相机方案")]
        public CinemachineVirtualCamera virtualCamera;
        // [Header("硬跟踪方案")]
        // private Transform _mainCamera;
        // private Transform _playerTrans;
        // private Vector3 _offset = Vector3.back * 10;

        private void Start()
        {
            _distanceSum = maxZoomDistance + virtualCamera.m_Lens.OrthographicSize;//总和
            virtualCamera.Follow = GameManager.Instance.GetPlayer().transform;
            // _mainCamera = Camera.main.transform;
            // _playerTrans = GameManager.Instance.GetPlayer().transform;
        }

        private void Update()
        {
            // _mainCamera.position = _playerTrans.position + _offset;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(_coroutine == null)
                    _coroutine = StartCoroutine(ZoomOutCamera());
            }
        }

        Coroutine _coroutine;
        private float _distanceSum;
        private IEnumerator ZoomOutCamera()
        {
            float targetDistance = _distanceSum - virtualCamera.m_Lens.OrthographicSize;

            float initialDistance = virtualCamera.m_Lens.OrthographicSize;
            float timer = 0;

            while (timer < 1f)
            {
                timer += Time.deltaTime * zoomSpeed;
                float newDistance = Mathf.Lerp(initialDistance, targetDistance, timer);
                virtualCamera.m_Lens.OrthographicSize = newDistance;
                yield return null;
            }
            _coroutine = null;
        }
    }
}
