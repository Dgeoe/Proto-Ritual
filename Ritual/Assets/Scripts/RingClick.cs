using UnityEngine;
using UnityEngine.InputSystem;

public class RingClick : MonoBehaviour
{
    public int ringID;
    public RingRitual ringRitualManager;
    public Camera mainCamera;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckClick(Mouse.current.position.ReadValue());
        }
    }

    void CheckClick(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!ringRitualManager.IsInRingMode)
                    ringRitualManager.EnterRingMode();
            }
        }
    }
}
