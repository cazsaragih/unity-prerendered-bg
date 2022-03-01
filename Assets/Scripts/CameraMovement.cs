using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    private Camera primaryCamera;
    private float rangeScreenPointX;
    private float rangeScreenPointY;
    private float rangeCamOffsetX;
    private float rangeCamOffsetY;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Vector2 minCamOffset;
    [SerializeField]
    private Vector2 maxCamOffset;
    [SerializeField]
    private Vector2 minScreenPoint;
    [SerializeField]
    private Vector2 maxScreenPoint;
    [SerializeField]
    private Camera secondaryCamera;

    [Header("Debug Mode")]
    [SerializeField]
    private bool isDebug;
    [SerializeField]
    private Vector3 playerScreenPos;
    [SerializeField]
    private float x, y;

    private void Awake()
    {
        primaryCamera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (playerTransform == null)
            Debug.LogError("playerTransform is not assigned!");

        rangeScreenPointX = maxScreenPoint.x - minScreenPoint.x;
        rangeScreenPointY = maxScreenPoint.y - minScreenPoint.y;

        rangeCamOffsetX = maxCamOffset.x - minCamOffset.x;
        rangeCamOffsetY = maxCamOffset.y - minCamOffset.y;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (playerTransform == null)
            return;
        
        if (isDebug)
        {
            playerScreenPos = secondaryCamera.WorldToScreenPoint(playerTransform.position);
            playerScreenPos = new Vector3(playerScreenPos.x / Screen.width, playerScreenPos.y / Screen.height, playerScreenPos.z);
            SetVanishingPoint(primaryCamera, new Vector2(x, y));
        }
        else
        {
            float playerOnScreenPosX = secondaryCamera.WorldToScreenPoint(playerTransform.position).x / Screen.width;
            float playerOnScreenPosY = secondaryCamera.WorldToScreenPoint(playerTransform.position).y / Screen.height;

            float ratioX = (playerOnScreenPosX - minScreenPoint.x) / rangeScreenPointX;
            float ratioY = (playerOnScreenPosY - minScreenPoint.y) / rangeScreenPointY;

            float currOffsetX = (ratioX * rangeCamOffsetX) + minCamOffset.x;
            float currOffsetY = (ratioY *    rangeCamOffsetY) + minCamOffset.y;

            currOffsetX = Mathf.Clamp(currOffsetX, maxCamOffset.x, minCamOffset.x);
            currOffsetY = Mathf.Clamp(currOffsetY, maxCamOffset.y, minCamOffset.y);

            SetVanishingPoint(primaryCamera, new Vector2(currOffsetX, currOffsetY));
        }
    }

    private void SetVanishingPoint(Camera cam, Vector2 perspectiveOffset)
    {
        Matrix4x4 m = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / m.m00;
        float h = 2 * cam.nearClipPlane / m.m11;

        float left = -w / 2 - perspectiveOffset.x;
        float right = left + w;
        float bottom = -h / 2 - perspectiveOffset.y;
        float top = bottom + h;

        cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
    }

    private static Matrix4x4 PerspectiveOffCenter(
        float left, float right,
        float bottom, float top,
        float near, float far)
    {
        float x = (2.0f * near) / (right - left);
        float y = (2.0f * near) / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0f * far * near) / (far - near);
        float e = -1.0f;

        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x; m[0, 1] = 0.0f; m[0, 2] = a; m[0, 3] = 0.0f;
        m[1, 0] = 0.0f; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0.0f;
        m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = e; m[3, 3] = 0.0f;

        return m;
    }

    private void ResetProjectionMatrix()
    {
        primaryCamera.ResetProjectionMatrix();
    }
}
