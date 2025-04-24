using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MMScreen : MonoBehaviour
{
    [SerializeField] TextMeshPro bestTime;
    [SerializeField] TextMeshPro wins;
    [SerializeField] TextMeshPro deaths;
    [SerializeField] TextMeshPro levelsSpawned;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bestTime.text = "Best: " + PlayerPrefs.GetFloat("BestTime").ToString();
        wins.text = "Wins: " + PlayerPrefs.GetInt("winTotal").ToString();
        deaths.text = "Deaths: " + PlayerPrefs.GetInt("deathTotal").ToString();
        levelsSpawned.text = "Levels Spawned: " + PlayerPrefs.GetInt("levelsSpawned").ToString();
    }
}
