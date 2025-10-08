using UnityEngine;

public class TempShapeScript : MonoBehaviour
{
    public Transform outerRing;
    public Transform middleRing;
    public Transform innerRing;
    public RingRitual ringRitualManager;

    private string currentShapeName;
    private RingRotation targetRotation;
    private bool ritualCompleted = false;

    //ANOTHER RING RITUAL SCRIPT PLEASE GOD MAKE IT STOP

    [System.Serializable]
    public struct RingRotation
    {
        public float outerZ;
        public float middleZ;
        public float innerZ;

        public RingRotation(float outer, float middle, float inner)
        {
            outerZ = outer;
            middleZ = middle;
            innerZ = inner;
        }
    }

    private RingRotation[] shapes = new RingRotation[4];
    private string[] shapeNames = new string[4];

    void Awake()
    {
        shapes[0] = new RingRotation(-180f, 90f, 0f);
        shapes[1] = new RingRotation(90f, -90f, 90f);
        shapes[2] = new RingRotation(90f, 90f, -180f);
        shapes[3] = new RingRotation(0f, -180f, 90f);

        shapeNames[0] = "Diamond";
        shapeNames[1] = "Hourglass";
        shapeNames[2] = "Trident";
        shapeNames[3] = "Helix";

        int randomIndex = Random.Range(0, shapes.Length);
        targetRotation = shapes[randomIndex];
        currentShapeName = shapeNames[randomIndex];

        Debug.Log($"Target Shape: {currentShapeName}");
    }

    void Update()
    {
        if (ritualCompleted || ringRitualManager == null || !ringRitualManager.IsInRingMode)
            return;

        if (CheckIfRingsAligned())
        {
            ritualCompleted = true;
            SnapRingsToTarget();
            DisableRingRotation();
            ringRitualManager.ExitRingMode();
            Debug.Log("Ring Ritual has been completed");
        }
    }

    private bool CheckIfRingsAligned()
    {
        float outerCurrent = NormalizeAngle(outerRing.localEulerAngles.z);
        float middleCurrent = NormalizeAngle(middleRing.localEulerAngles.z);
        float innerCurrent = NormalizeAngle(innerRing.localEulerAngles.z);

        float outerTarget = NormalizeAngle(targetRotation.outerZ);
        float middleTarget = NormalizeAngle(targetRotation.middleZ);
        float innerTarget = NormalizeAngle(targetRotation.innerZ);

        return Mathf.Approximately(outerCurrent, outerTarget) &&
               Mathf.Approximately(middleCurrent, middleTarget) &&
               Mathf.Approximately(innerCurrent, innerTarget);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    private void SnapRingsToTarget()
    {
        if (outerRing != null)
            outerRing.localEulerAngles = new Vector3(0f, 0f, NormalizeAngle(targetRotation.outerZ));
        if (middleRing != null)
            middleRing.localEulerAngles = new Vector3(0f, 0f, NormalizeAngle(targetRotation.middleZ));
        if (innerRing != null)
            innerRing.localEulerAngles = new Vector3(0f, 0f, NormalizeAngle(targetRotation.innerZ));
    }

    private void DisableRingRotation()
    {
        if (outerRing != null)
        {
            RingRotate rr = outerRing.GetComponent<RingRotate>();
            if (rr != null) rr.enabled = false;
        }
        if (middleRing != null)
        {
            RingRotate rr = middleRing.GetComponent<RingRotate>();
            if (rr != null) rr.enabled = false;
        }
        if (innerRing != null)
        {
            RingRotate rr = innerRing.GetComponent<RingRotate>();
            if (rr != null) rr.enabled = false;
        }
    }
}
