using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Camera cam;

    [Header("Target to follow")]
    [SerializeField] private Transform target;

    [Header("Camera Bounds")]
    [SerializeField] private PolygonCollider2D boundaryPolygon;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;

        if (boundaryPolygon != null)
            targetPos = ConstrainToBounds(targetPos);

        transform.position = targetPos;
    }

    private Vector3 ConstrainToBounds(Vector3 pos)
    {
        float camH = cam.orthographicSize * 2f;
        float camW = camH * cam.aspect;
        float halfW = camW * 0.5f;
        float halfH = camH * 0.5f;

        Bounds b = boundaryPolygon.bounds;

        float minX = b.min.x + halfW;
        float maxX = b.max.x - halfW;
        float minY = b.min.y + halfH;
        float maxY = b.max.y - halfH;

        float x = (minX <= maxX) ? Mathf.Clamp(pos.x, minX, maxX) : b.center.x;
        float y = (minY <= maxY) ? Mathf.Clamp(pos.y, minY, maxY) : b.center.y;

        return new Vector3(x, y, pos.z);
    }

    public void SetFollowTarget(Transform newTarget) => target = newTarget;
    public void SetBoundaryPolygon(PolygonCollider2D polygon) => boundaryPolygon = polygon;
}
