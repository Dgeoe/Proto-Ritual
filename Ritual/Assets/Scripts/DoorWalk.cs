using UnityEngine;

public class DoorWalk : MonoBehaviour
{
    private float speed = .4f;
    private float distance = 4f;
    private Vector3 StartSpot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartSpot = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z <= StartSpot.z - distance)
        {
            transform.position = StartSpot;
        }
    }
}
