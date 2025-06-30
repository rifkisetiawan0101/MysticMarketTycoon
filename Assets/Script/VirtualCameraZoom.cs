using UnityEngine;
using Cinemachine;

public class VirtualCameraZoom : MonoBehaviour {
    public CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float zoomSpeed = 50;
    [SerializeField] private float maxZoom = 2000;
    [SerializeField] private float minZoom = 1400;

    void Start() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update() {
        ScrollZoom();
    }

    private void ScrollZoom() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0 && !PersistentManager.Instance.isUIOpen) {
            float newSize = virtualCamera.m_Lens.OrthographicSize - scrollInput * zoomSpeed;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom); 
        }
    }
}
