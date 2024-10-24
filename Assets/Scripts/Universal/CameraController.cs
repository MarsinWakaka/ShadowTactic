using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 1.0f;
    public float maxZoomDistance = 10.0f;

    public CinemachineVirtualCamera virtualCamera;
    private float DistanceSum;

    Coroutine _coroutine;

    private void Start()
    {
        DistanceSum = maxZoomDistance + virtualCamera.m_Lens.OrthographicSize;//×ÜºÍ
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(_coroutine == null)
                _coroutine = StartCoroutine(ZoomOutCamera());
        }
    }

    private IEnumerator ZoomOutCamera()
    {
        float targetDistance = DistanceSum - virtualCamera.m_Lens.OrthographicSize;

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
