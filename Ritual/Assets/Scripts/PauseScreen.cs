using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : MonoBehaviour
{
    [Header("GameObject to Activate/Deactivate")]
    public GameObject targetObject;

    [Header("Toggle Behavior")]
    public bool toggle = true; 

    private InputAction tabAction;
    private bool isPaused = false;

    void OnEnable()
    {
        
        tabAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/tab");
        tabAction.performed += OnTabPressed;
        tabAction.Enable();
    }

    void OnDisable()
    {
        tabAction.performed -= OnTabPressed;
        tabAction.Disable();
    }

    private void OnTabPressed(InputAction.CallbackContext context)
    {
        if (targetObject == null)
        {
            Debug.LogWarning("ActivateOnTab: No target object assigned!", this);
            return;
        }

        if (toggle)
        {
            bool newState = !targetObject.activeSelf;
            targetObject.SetActive(newState);

            if (newState)
                PauseGame();
            else
                ResumeGame();
        }
        else
        {
            targetObject.SetActive(true);
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }
}
