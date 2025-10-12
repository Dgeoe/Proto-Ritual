using UnityEngine;

public class MonsterPositions : MonoBehaviour
{
    public GameObject[] Positions;
    public GameObject miniJumpScare;
    public GameObject JumpScare;
    private int howmany;
    private int holder = 0;
    private int pickMemory;
    private int randomPick;

    private void Start()
    {
        if (Positions != null)
        {
            RandomPosition();
        }
    }

    private void RandomPosition()
    {
        miniJumpScare.SetActive(false);  
        foreach (GameObject i in Positions)
        {
            i.SetActive(false);
        }
        howmany = Positions.Length; 
        randomPick = Random.Range(0, howmany - 1);
        if (holder == 0)
        {
            pickMemory = randomPick;
            holder++;
        }
        else
        {
            if (randomPick == pickMemory)
            {
                randomPick = Random.Range(0, howmany - 1);
                RandomPosition();
            }
            else
            {
                holder = 0;
            }
        }
        Positions[randomPick].SetActive(true);
    }

    private void MiniJumpscare()
    {
        miniJumpScare.SetActive(true);  
        //Get Player Position 
        //TP GameObject to be infront of that position
        //Set Salt Circle Spawn Limits
    }

    public void ForSeeingGlass()
    {
        int chance = Random.Range(0, 4);
        if (chance == 0)
        {
            MiniJumpscare();
        }
        else
        {
            RandomPosition();
        }
    }

    public void DisableOnSpace()
    {
        foreach (GameObject i in Positions)
        {
            i.SetActive(false);
        }
        miniJumpScare.SetActive(false);
    }
    public void Jumpscare()
    {

    }
}
