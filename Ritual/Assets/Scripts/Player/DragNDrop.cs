using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float followSmoothness = 20f;

    private DropScript selectedDrop;
    private GameObject tempFollower;
    private bool isFollowing = false;
    private float followDepth = 0f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();

        if (isFollowing && tempFollower != null)
            FollowMouseInCameraSpace();
    }

    private void HandleClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (!isFollowing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("DD"))
                {
                    selectedDrop = hit.collider.GetComponent<DropScript>();
                    if (selectedDrop == null || selectedDrop.targetDragObject == null)
                        return;

                    GameObject target = selectedDrop.targetDragObject;

                    var renderers = target.GetComponentsInChildren<MeshRenderer>(true);
                    foreach (var r in renderers) r.enabled = false;

                    Vector3 worldPos = target.transform.position;
                    Quaternion worldRot = target.transform.rotation;
                    Vector3 worldScale = target.transform.lossyScale;

                    followDepth = Vector3.Distance(mainCamera.transform.position, worldPos);

                    tempFollower = Instantiate(target);
                    tempFollower.transform.position = worldPos;
                    tempFollower.transform.rotation = worldRot;
                    tempFollower.transform.localScale = worldScale;

                    foreach (var comp in tempFollower.GetComponents<MonoBehaviour>()) Destroy(comp);
                    foreach (var col in tempFollower.GetComponents<Collider>()) Destroy(col);
                    foreach (var mr in tempFollower.GetComponentsInChildren<MeshRenderer>(true)) mr.enabled = true;

                    isFollowing = true;
                }
            }
        }
        else
        {
            StartCoroutine(StopFollowingAfterDelay(2f));
        }
    }

    private void FollowMouseInCameraSpace()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, followDepth);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        tempFollower.transform.position = Vector3.Lerp(tempFollower.transform.position, worldPos, Time.deltaTime * followSmoothness);
    }

    private IEnumerator StopFollowingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tempFollower != null)
            Destroy(tempFollower);

        if (selectedDrop != null && selectedDrop.targetDragObject != null)
        {
            var renderers = selectedDrop.targetDragObject.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in renderers) r.enabled = true;
        }

        selectedDrop = null;
        isFollowing = false;
    }
}
