using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class shellSpawn : MonoBehaviour {

    //Green shell
    [SerializeField]
    Object greenMissile;
    [SerializeField]
    Transform greenShellCD;
    private Image greenShellLoadingBar;
    bool isGreenAvailable;
    [SerializeField]
    float greenShellCDTime = 20;

    //Red shell
    [SerializeField]
    Object redMissile;
    [SerializeField]
    Transform redShellCD;
    private Image redShellLoadingBar;
    bool isRedAvailable;
    [SerializeField]
    float redShellCDTime = 40;

    private GameObject missileManager;
    
    void Start()
    {
        greenShellLoadingBar = greenShellCD.GetComponent<Image>();
        greenShellLoadingBar.fillAmount = 1.0f;
        redShellLoadingBar = redShellCD.GetComponent<Image>();
        redShellLoadingBar.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGreenAvailable)
        {
            missileManager = Instantiate(greenMissile, transform.position, transform.rotation) as GameObject;
            greenShellLoadingBar.fillAmount = 0.0f;
            isGreenAvailable = false;
        }
        if (Input.GetMouseButtonDown(1) && isRedAvailable)
        {
            missileManager = Instantiate(redMissile, transform.position, transform.rotation) as GameObject;
            redShellLoadingBar.fillAmount = 0.0f;
            isRedAvailable = false;
        }

        if (!isGreenAvailable) isGreenAvailable = checkCoolDown(greenShellLoadingBar, greenShellCDTime);
        if (!isRedAvailable) isRedAvailable = checkCoolDown(redShellLoadingBar, redShellCDTime);
    }

    bool checkCoolDown (Image shellToLoad, float shellCoolDown)
    {
        bool isLoaded = false;
        float addFilledAmount = Time.deltaTime / shellCoolDown;
        float currentLoading = shellToLoad.fillAmount;
        shellToLoad.fillAmount = currentLoading + addFilledAmount;
        if (shellToLoad.fillAmount >= 1.0f)
        {
            shellToLoad.fillAmount = 1.0f;
            isLoaded = true;
        }
        return isLoaded;
    }
}
