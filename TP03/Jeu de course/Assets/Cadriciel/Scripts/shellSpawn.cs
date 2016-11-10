using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

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
        //gestion inputs
        // -greenshell on left trigger/left clic
        // -redshell on right trigger/right clic
        float triggerPosition = CrossPlatformInputManager.GetAxis("shellTrigger");
        bool greenShellInput = (triggerPosition < -0.5)? true : false;
        bool redShellInput = (triggerPosition > 0.5) ? true : false;

        if (greenShellInput && isGreenAvailable)
        {
            missileManager = Instantiate(greenMissile, transform.position, transform.rotation) as GameObject;
            greenShellLoadingBar.fillAmount = 0.0f;
            isGreenAvailable = false;
        }
        if (redShellInput && isRedAvailable)
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

    public void refillGreenShell()
    {
        greenShellLoadingBar.fillAmount = 1.0f;
        isGreenAvailable = true;
    }

    public void refillRedShell()
    {
        redShellLoadingBar.fillAmount = 1.0f;
        isRedAvailable = true;
    }
}
