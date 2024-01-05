using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager Instance;

    [SerializeField] private string androidID = "5512312";
    [SerializeField] private string iosID = "5512313";

    [SerializeField] string skippableVideoPlacementID = "Interstitial_Android";
    [SerializeField] string forcedVideoPlacementID = "Rewarded_Android";

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);

        if (Application.platform == RuntimePlatform.Android)
            Advertisement.Initialize(androidID);
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            Advertisement.Initialize(iosID);
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Debug.Log("Platform is Editor");
            Advertisement.Initialize(androidID);
        }
    }

    void Update()
    {
        //AdStartManualInput();
    }

    void AdStartManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowSkippableAd();
    }

    public void ShowForcedAd()
    {
        //Debug.Log("input to show ad");
        Advertisement.Show(forcedVideoPlacementID);
    }

    public void ShowSkippableAd()
    {
        //Debug.Log("input to show ad");
        Advertisement.Show(skippableVideoPlacementID);
    }


    public void OnUnityAdsDidError(string message)
    {
        //Debug.Log("AD ERROR");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (placementId == forcedVideoPlacementID)
            {
                //Debug.Log("Ad Finished");
            }
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Debug.Log("Ad Started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        //Debug.Log("Ad Ready");
    }


}
