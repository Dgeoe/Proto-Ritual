using UnityEngine.InputSystem;
using UnityEngine;

public class CandleClick : MonoBehaviour
{
    public int candleID; // 0 to 5
    public CandleRitual ritualManager;
    public Camera mainCamera;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            CheckClick(Mouse.current.position.ReadValue());
    }

    void CheckClick(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                ritualManager.ToggleCandle(candleID);
            }
        }
    }
}

