using UnityEngine;

public class MonsterTypeChooser : MonoBehaviour
{
    public Texture[] MonsterTypes; // Expecting 4 textures (Monster, Backstabbed, Burnt, Blood)
    public GameObject[] Monsters;

    public bool Monster;
    public bool Backstabbed;
    public bool Burnt;
    public bool Blood;
    public FinalTaskTracker finalTaskTracker;

    private void Awake()
    {
        ApplyRandomMonsterTextures();
    }

    private void ApplyRandomMonsterTextures()
    {
        if (MonsterTypes == null || MonsterTypes.Length == 0)
        {
            Debug.LogWarning("No monster textures assigned!");
            return;
        }

        int chosenIndex = Random.Range(0, MonsterTypes.Length);
        Texture chosenTexture = MonsterTypes[chosenIndex];
        SetMonsterState(chosenIndex);

        // Apply texture to all monsters objects (Cube Remesh)
        foreach (GameObject monster in Monsters)
        {
            if (monster == null) continue;

            Renderer renderer = monster.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = null;
                renderer.material.mainTexture = chosenTexture;
            }
            else
            {
                Debug.LogWarning($"{monster.name} has no Renderer component!");
            }
        }
    }

    private void SetMonsterState(int index)
    {
        Monster = Backstabbed = Burnt = Blood = false;

        switch (index)
        {
            case 0:
                Monster = true;
                break;
            case 1:
                Backstabbed = true;
                break;
            case 2:
                Burnt = true;
                break;
            case 3:
                Blood = true;
                if (finalTaskTracker.Candles1to3 == false && finalTaskTracker.RedVial == false)
                {
                    finalTaskTracker.bloody = true;
                }
                break;
            default:
                Debug.LogWarning("Unexpected texture index; no state set.");
                break;
        }
    }
}

