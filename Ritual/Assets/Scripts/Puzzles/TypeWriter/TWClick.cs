using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

public class TWClick : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public MonsterTypeChooser monsterTypeChooser;

    public FinalTaskTracker finalTaskTracker;

    [Header("Audio")]
    public AudioSource correctSFX;
    public AudioSource wrongSFX;

    [Header("Victory Effects")]
    public GameObject hourglassLight3; 
    public AudioSource winChime;       


    public float clickResetDelay = 0.3f; //The time for naimtion click length
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

                // Trigger button animation please work!
                Animator anim = clickedObject.GetComponent<Animator>();
                if (anim != null)
                    StartCoroutine(PlayButtonClickAnimation(anim));

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
       
        pressedButtons.Add(buttonNumber);

        
        correctSFX?.Play();

        
        if (pressedButtons.Count == 4)
        {
            
            HashSet<int> playerSet = new HashSet<int>(pressedButtons);

            if (playerSet.SetEquals(correctButtons))
            {

                Debug.Log("All buttons correct!");
                finalTaskTracker.Correct3 = true;

                if (hourglassLight3 != null)
                    hourglassLight3.SetActive(true);

                if (winChime != null)
                    winChime.Play();
            }
            else
            {
                
                Debug.Log("Incorrect combination!");
                wrongSFX?.Play();
            }

            
            pressedButtons.Clear();
        }
    }



    private IEnumerator PlayButtonClickAnimation(Animator anim)
    {
        anim.SetBool("Clicked", true);
        yield return new WaitForSeconds(clickResetDelay);
        anim.SetBool("Clicked", false);
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

