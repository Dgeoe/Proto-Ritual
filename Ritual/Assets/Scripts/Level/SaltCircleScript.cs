using UnityEngine;

[ExecuteAlways] //Praying this doesnt fuck up might need to del ltr
[RequireComponent(typeof(SphereCollider))]
public class SaltCircleScript : MonoBehaviour
{
    public Transform player;
    public float pushForce = 10f;
    [Range(1f, 25f)]
    public float circleScale = 5f;

    private SphereCollider circleCollider;
    private float radius;

    void Start()
    {
        //the goal is here is 2 things
        //1. Define a circle area the player cant leave
        //2. Scale my img renderer and the collider via script for easy testing later
        circleCollider = GetComponent<SphereCollider>();
        circleCollider.isTrigger = true; 
        radius = circleCollider.radius * transform.localScale.x;
    }

    void FixedUpdate()
    {
        transform.localScale = new Vector3(circleScale, circleScale, 1f);
        if (player == null) return;

        Vector3 center = transform.position;
        Vector3 playerPos = player.position;

        Vector3 toPlayer = playerPos - center;
        float distance = toPlayer.magnitude;

        if (distance > radius)
        {
            // Push the player back inside the circle ***
            // *** This valeu HAS to match the force of the players forward movemnt!!!
            Vector3 pushDir = toPlayer.normalized;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-pushDir * pushForce, ForceMode.VelocityChange);
            }
        }
    }

    // How big is the circle ??
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>()?.radius * transform.localScale.x ?? 1f);
    }
    #endif
}

