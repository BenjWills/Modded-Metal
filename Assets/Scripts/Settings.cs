using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Settings : MonoBehaviour
{
    public static Settings _instance;

    [SerializeField] int smCoin;
    [SerializeField] int deathTotal;
    [SerializeField] int winTotal;
    [SerializeField] int levelsSpawned;
    [SerializeField] float bestTime;
    [SerializeField] bool isFullscreen;
    [SerializeField] float brightness;

    Volume postProcessing;
    public LiftGammaGain liftGammagGain;

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

        postProcessing = this.gameObject.GetComponentInChildren<Volume>();
        postProcessing.profile.TryGet(out liftGammagGain);

        if (!PlayerPrefs.HasKey("smCoin"))
        {
            PlayerPrefs.SetInt("smCoin", 0);
        }
        if (!PlayerPrefs.HasKey("deathTotal"))
        {
            PlayerPrefs.SetInt("deathTotal", 0);
        }
        if (!PlayerPrefs.HasKey("winTotal"))
        {
            PlayerPrefs.SetInt("winTotal", 0);
        }
        if (!PlayerPrefs.HasKey("levelsSpawned"))
        {
            PlayerPrefs.SetInt("levelsSpawned", 0);
        }
        if (!PlayerPrefs.HasKey("BestTime"))
        {
            PlayerPrefs.SetInt("BestTime", 9999);
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
        bestTime = PlayerPrefs.GetFloat("BestTime");
        if (PlayerPrefs.GetInt("Fullscreen") != 0)
        {
            isFullscreen = true;
        }
        else
        {
            isFullscreen = false;
        }
        brightness = PlayerPrefs.GetFloat("Brightness");
    }
}
