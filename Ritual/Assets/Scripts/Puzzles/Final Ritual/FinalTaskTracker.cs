using UnityEngine;

public class FinalTaskTracker : MonoBehaviour
{
    public bool Correct1, Correct2, Correct3, Correct4;
    public bool Candles1to3;
    public bool RedVial;
    public bool two;
    public bool bloody;
    private void Logic()
    {
        //If you have lit any of the first three candles, place the knife in the bowl.
        //If you have not lit any of the first three candles and you have mixed red into your goblet, place the lock of hair in the bowl.
        //If you have not lit any of the first three candles, and you have not mixed red into your goblet and the monster is bloody, place the blood vial in the bowl.
        //If none of this is the case, place the dead bird in the bowl
    }

    void Update()
    {
        if (Correct1 && Correct2 && Correct3 && Correct4)
        {
            //Enable Drag And Drop Here
        }
        else
        {
            return;
        }
    }

    public void Check()
    {
        if (RedVial)
        {
            //hair
        }
        else if (bloody)
        {
            //bloodvial
        }
        else if (Candles1to3)
        {
            //Knif e
        }
        else
        {
            //dead bird 
        }
    }
}
