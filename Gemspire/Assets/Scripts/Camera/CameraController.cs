using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Bounds")]
    [SerializeField] private float cameraXMin;
    [SerializeField] private float cameraXMax;
    [SerializeField] private float cameraZMin;
    [SerializeField] private float cameraZMax;

    [Header("Camera Zoom")]
    //[SerializeField, Range(1f, 10f)] private float minZoom = 1f;
    //[SerializeField, Range(1f, 10f)] private float maxZoom = 5f;

    [SerializeField, Range(10f, 60f)] private float minZoom = 20f; // FOV minimum
    [SerializeField, Range(10f, 60f)] private float maxZoom = 60f; // FOV maximum

    [SerializeField, Range(1f, 50f)] private float smoothSpeed = 20f;
    [SerializeField, Range(1f, 10f)] private float zoomStrenght = 5f;

    private Camera cam;
    private Vector3 touchStart;
    private float targetZoom;
    private float zoomVelocity;
    private Vector3 cameraOffset;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Camera component missing on this GameObject!");
            enabled = false;
            return;
        }

        //targetZoom = cam.orthographicSize;
        targetZoom = cam.fieldOfView;

        cameraOffset = cam.transform.position;
    }

    void Update()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                HandleMobileInput();
                break;
            case RuntimePlatform.WindowsEditor:
                HandleDevInput();
                break;
        }
    }

    private void HandleMobileInput()
    {
        switch (Input.touchCount)
        {
            case <= 0:
                return;
            case 1:
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandlePanStart(touch.position);
                        break;
                    case TouchPhase.Moved:
                        HandlePanMove(touch.position);
                        break;
                    case TouchPhase.Ended:
                        HandlePanEnd();
                        break;
                }
                break;
            case 2:
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);
                Vector2 touchOnePos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPos = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePos - touchTwoPos).magnitude;
                float touchMagnitude = (touchOne.position - touchTwo.position).magnitude;

                float difference = (touchMagnitude - prevMagnitude) / 100f;
                HandleZoom(difference);
                break;
        }
    }

    private void HandleDevInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandlePanStart(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            HandlePanMove(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            HandlePanEnd();
        }
        HandleZoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void HandlePanStart(Vector2 position)
    {
        if (cam == null) return;
        //touchStart = cam.ScreenToWorldPoint(position);

        Vector3 pos = new Vector3(position.x, position.y, cameraOffset.y);
        touchStart = cam.ScreenToWorldPoint(pos);
    }

    private void HandlePanMove(Vector2 delta)
    {
        /*//if (EventSystem.current.IsPointerOverGameObject()) return;
        if (cam == null) return;
        Vector3 direction = touchStart - cam.ScreenToWorldPoint(delta);
        direction.y = 0;
        transform.position += direction;*/

        if (cam == null) return;
        Vector3 pos = new Vector3(delta.x, delta.y, cameraOffset.y);
        Vector3 direction = touchStart - cam.ScreenToWorldPoint(pos);
        direction.y = 0; // ne változzon a magasság
        transform.position += direction;
    }

    private void HandlePanEnd()
    {
        if (cam == null) return;
        Vector3 camPos = cam.transform.position;
        camPos.x = Mathf.Clamp(camPos.x, cameraXMin, cameraXMax);
        camPos.z = Mathf.Clamp(camPos.z, cameraZMin, cameraZMax);
        camPos.y = cameraOffset.y;
        transform.position = camPos;
    }

    private void HandleZoom(float zoomInput)
    {
        if (cam == null) return;

        if (zoomInput != 0)
        {
            targetZoom -= zoomInput * zoomStrenght;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        //cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothSpeed * Time.deltaTime);
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref zoomVelocity, smoothSpeed * Time.deltaTime);
    }   
}
