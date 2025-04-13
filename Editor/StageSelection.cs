using UnityEngine;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    public Dropdown stageDropdown;

    void Start()
    {
        stageDropdown.onValueChanged.AddListener(OnStageChanged);
    }

    void OnStageChanged(int index)
    {

        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in player) 
        { 
            if (player != null)
            {
                Destroy(g);
            }
        }
    }
}
