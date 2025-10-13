using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SphereCollider))]
public class SaltCircleScript : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerMovement playerMovement;

    [Header("Settings")]
    [Range(1f, 25f)]
    public float circleScale = 4f;

    private SphereCollider circleCollider;
    private float radius;

    void Start()
    {
        circleCollider = GetComponent<SphereCollider>();
        circleCollider.isTrigger = true;
        UpdateRadius();
    }

    void Update()
    {
        transform.localScale = new Vector3(circleScale, circleScale, circleScale);
        UpdateRadius();

        if (player == null || playerMovement == null)
            return;

        KeepPlayerInside();
    }

    private void UpdateRadius()
    {
        radius = circleCollider.radius * transform.localScale.x;
    }

    private void KeepPlayerInside()
    {
        Vector3 center = transform.position;
        Vector3 playerPos = player.position;

        Vector3 toPlayer = playerPos - center;
        float distance = toPlayer.magnitude;

        if (distance > radius)
        {
            Vector3 clampedPos = center + toPlayer.normalized * radius;

            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 correction = (clampedPos - playerPos) * Time.deltaTime * 15f;
                controller.Move(correction);

            }
            else
            {
                player.position = clampedPos;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float gizmoRadius = GetComponent<SphereCollider>()?.radius * transform.localScale.x ?? 1f;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
#endif
}