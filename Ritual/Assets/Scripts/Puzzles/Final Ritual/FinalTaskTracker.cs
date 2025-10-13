using UnityEngine;
using UnityEngine.InputSystem;

public class FinalTaskTracker : MonoBehaviour
{
    public bool Correct1, Correct2, Correct3, Correct4;
    public bool Candles1to3;
    public bool RedVial;
    public bool two;
    public bool bloody;

    public bool knifeHeld;
    public bool birdHeld;
    public bool hairHeld;
    public bool vialHeld;

    [SerializeField] private Camera mainCamera;
    public FlashbangWin flashbang;
    public GameOver gameover;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Logic()
    {
        //How it Works!!!
        //When all Correct1–4 are true, the ritual begins.
        //Clicking on an object on the “Ritual” layer does a raycast.

        //If it has a Sacrifices script:
        //Clicking on a Knife, Hair, Vial, or DeadBird picks it up (sets held bool).
        //Clicking on a Bowl checks which held item you have and runs the ritual logic.
        //Depending on Candles1to3, RedVial, and bloody, the script verifies the correct offering:

        //If correct wooooo you win oh yeha baby

        //If wrong wtf dude jumpscare time
    }

    void Update()
    {
        if (!Correct1 || !Correct2 || !Correct3 || !Correct4)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();
    }

    private void HandleClick()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Ritual")))
        {
            Sacrifices s = hit.collider.GetComponent<Sacrifices>();
            if (s == null) return;

            if (s.Knife) { ClearHeld(); knifeHeld = true; Debug.Log("Picked up Knife"); return; }
            if (s.DeadBird) { ClearHeld(); birdHeld = true; Debug.Log("Picked up Dead Bird"); return; }
            if (s.Hair) { ClearHeld(); hairHeld = true; Debug.Log("Picked up Hair"); return; }
            if (s.Vial) { ClearHeld(); vialHeld = true; Debug.Log("Picked up Vial"); return; }

            if (s.Bowl)
            {
                CheckPlacement();
            }
        }
    }

    private void CheckPlacement()
    {
        if (Candles1to3 && knifeHeld)
        {
            Debug.Log("Yay – Knife correctly placed in bowl!");
            flashbang.TriggerFlashbang();

        }
        else if (!Candles1to3 && RedVial && hairHeld)
        {
            Debug.Log("Yay – Hair correctly placed in bowl!");
            flashbang.TriggerFlashbang();
        }
        else if (!Candles1to3 && !RedVial && bloody && vialHeld)
        {
            Debug.Log("Yay – Vial correctly placed in bowl!");
            flashbang.TriggerFlashbang();
        }
        else if (!Candles1to3 && !RedVial && !bloody && birdHeld)
        {
            Debug.Log("Yay – Dead bird correctly placed in bowl!");
            flashbang.TriggerFlashbang();
        }
        else
        {
            Debug.Log("Fail – Wrong offering placed!");
            gameover.OnTimerEnd();
        }

        ClearHeld();
    }

    private void ClearHeld()
    {
        knifeHeld = birdHeld = hairHeld = vialHeld = false;
    }
}
