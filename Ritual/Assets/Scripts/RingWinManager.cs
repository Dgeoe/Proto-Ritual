using UnityEngine;

public class RingWinManager : MonoBehaviour
{
    [Header("References")]
    public SeeingGlass seeingGlass;
    public Transform outerRing;
    public Transform middleRing;
    public Transform innerRing;
    public RingRitual ringRitual;
    public AudioSource ritualCompleteSound;

    [Header("Hourglass Light")]
    public GameObject hourglassLight4; // Drag the GameObject here in the Inspector

    private bool ritualComplete = false;

    void Update()
    {
        if (seeingGlass == null || outerRing == null || middleRing == null || innerRing == null || ringRitual == null)
            return;

        if (ritualComplete) return;

        int setIndex = seeingGlass.chosenSetIndex;

        switch (setIndex)
        {
            case 0:
                if (CheckRings(180f, 90f, 0f)) CompleteRitual();
                break;
            case 1:
                if (CheckRings(90f, 270f, 90f)) CompleteRitual();
                break;
            case 2:
                if (CheckRings(90f, 90f, 180f)) CompleteRitual();
                break;
            case 3:
                if (CheckRings(0f, 180f, 90f)) CompleteRitual();
                break;
        }
    }

    bool CheckRings(float outerZ, float middleZ, float innerZ)
    {
        outerZ = NormalizeAngle(outerZ);
        middleZ = NormalizeAngle(middleZ);
        innerZ = NormalizeAngle(innerZ);

        float outerAngle = outerRing.localEulerAngles.z;
        float middleAngle = middleRing.localEulerAngles.z;
        float innerAngle = innerRing.localEulerAngles.z;

        bool outerCorrect = Mathf.Abs(Mathf.DeltaAngle(outerAngle, outerZ)) < 1f;
        bool middleCorrect = Mathf.Abs(Mathf.DeltaAngle(middleAngle, middleZ)) < 1f;
        bool innerCorrect = Mathf.Abs(Mathf.DeltaAngle(innerAngle, innerZ)) < 1f;

        return outerCorrect && middleCorrect && innerCorrect;
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    void CompleteRitual()
    {
        ritualComplete = true;
        Debug.Log("Ring Ritual Complete");

        // Play ritual complete sound
        if (ritualCompleteSound != null)
            ritualCompleteSound.Play();

        // Activate Hourglass Light 4
        if (hourglassLight4 != null)
            hourglassLight4.SetActive(true);

        // Notify ring ritual
        ringRitual.MarkRitualCompleted();
    }
}
