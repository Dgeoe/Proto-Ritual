using UnityEngine;

public class Note : MonoBehaviour
{
    //Use to leave comments in the inspector for when leaving a project for a long time

    public string ToDo;
    public string For;
    public string ThisDoes;

    public enum urgency {NotVery, aLittle, aLot, URGENT};
    public urgency HowImportant;
    public enum dropdown {Joe, Jake, Riona, Alex};
    public dropdown SincerlyFrom;
}
