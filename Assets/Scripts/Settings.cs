using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings _instance;

    [SerializeField] int smCoin;
    [SerializeField] int deathTotal;
    [SerializeField] int winTotal;
    [SerializeField] int levelsSpawned;

    public static Settings Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        smCoin = PlayerPrefs.GetInt("smCoin");
        deathTotal = PlayerPrefs.GetInt("deathTotal");
        winTotal = PlayerPrefs.GetInt("winTotal");
        levelsSpawned = PlayerPrefs.GetInt("levelsSpawned");
    }
}
