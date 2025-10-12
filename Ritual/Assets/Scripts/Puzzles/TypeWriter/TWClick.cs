using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

public class TWClick : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public MonsterTypeChooser monsterTypeChooser;

    [Header("Audio")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

    [SerializeField] private string correctButtonsDisplay = "";

    private int typewriterLayerMask;
    private HashSet<int> correctButtons = new HashSet<int>();
    private HashSet<int> pressedButtons = new HashSet<int>();

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        typewriterLayerMask = LayerMask.GetMask("Typewriter");

        if (monsterTypeChooser == null)
        {
            Debug.LogError("MonsterTypeChooser not found in scene!");
            return;
        }

        SetCorrectButtons();
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, typewriterLayerMask))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (int.TryParse(Regex.Replace(clickedObject.name, @"\D", ""), out int buttonNumber))
                {
                    ProcessButtonPress(buttonNumber);
                }
                else
                {
                    Debug.LogWarning($"Clicked object {clickedObject.name} has no valid number!");
                }
            }
        }
    }

    private void ProcessButtonPress(int buttonNumber)
    {
        if (correctButtons.Contains(buttonNumber))
        {
            if (!pressedButtons.Contains(buttonNumber))
            {
                pressedButtons.Add(buttonNumber);
                correctSFX?.Play();
                Debug.Log($" Correct button {buttonNumber} pressed!");

                if (pressedButtons.Count == correctButtons.Count)
                {
                    Debug.Log(" YAY!");
                }
            }
        }
        else
        {
            wrongSFX?.Play();
            pressedButtons.Clear();
            Debug.Log($"Wrong button {buttonNumber}!");
        }
    }

    private void SetCorrectButtons()
    {
        correctButtons.Clear();

        if (monsterTypeChooser.Monster)
        {
            correctButtons = new HashSet<int> { 6, 4, 9, 1 };
            correctButtonsDisplay = "6, 4, 9, 1";
        }
        else if (monsterTypeChooser.Backstabbed)
        {
            correctButtons = new HashSet<int> { 7, 1, 8, 9 };
            correctButtonsDisplay = "7, 1, 8, 9";
        }
        else if (monsterTypeChooser.Burnt)
        {
            correctButtons = new HashSet<int> { 2, 8, 7, 5 };
            correctButtonsDisplay = "2, 8, 7, 5";
        }
        else if (monsterTypeChooser.Blood)
        {
            correctButtons = new HashSet<int> { 4, 2, 5, 6 };
            correctButtonsDisplay = "4, 2, 5, 6";
        }
        else
        {
            correctButtonsDisplay = "None";
            Debug.LogWarning("No monster type set!");
        }

        pressedButtons.Clear();
    }
}
