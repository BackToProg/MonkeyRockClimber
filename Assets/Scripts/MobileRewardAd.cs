// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using GoogleMobileAds.Api;
// using System;
//
// public class MobileRewardAd : MonoBehaviour
// {
//     public static MobileRewardAd instance = null;
//     string IdRewardedVideoAndroid = "ca-app-pub-1049448472911337/7858183220";
//     public RewardBasedVideoAd rewardBasedVideoAd;
//     // Use this for initialization
//     private void Awake()
//     {
//         if (instance == null) instance = this;
//         else if (instance != this) Destroy(gameObject);
//         DontDestroyOnLoad(gameObject);
//     }
//     void Start()
//     {
//         try { MyAdvertisement.ShowFullNormal(); } catch { }
//         rewardBasedVideoAd = RewardBasedVideoAd.Instance;
//         RequestRewardedVideo();
//     }
//     public void RequestRewardedVideo()
//     {
// #if UNITY_ANDROID
//         string adUnitId = IdRewardedVideoAndroid;
// #elif UNITY_IPHONE
//         string adUnitId = IdRewardedVideoIOs;
// #else
//         string adUnitId = "unexpected_platform";
// #endif
//         AdRequest request = new AdRequest.Builder().Build();
//         rewardBasedVideoAd.LoadAd(request, adUnitId);
//         rewardBasedVideoAd.OnAdLoaded += OnAdLoaded;
//         rewardBasedVideoAd.OnAdFailedToLoad += OnAdFailedToLoad;
//         rewardBasedVideoAd.OnAdClosed += OnAdClose;
//         rewardBasedVideoAd.OnAdRewarded += OnAdRewarded;
//     }
//
//     private void OnAdLoaded(object sender, EventArgs args)
//     {
//
//     }
//
//     private void OnAdFailedToLoad(object sender, EventArgs args)
//     {
//
//     }
//
//     private void OnAdClose(object sender, EventArgs args)
//     {
//         rewardBasedVideoAd.OnAdLoaded -= OnAdLoaded;
//         rewardBasedVideoAd.OnAdFailedToLoad -= OnAdFailedToLoad;
//         rewardBasedVideoAd.OnAdClosed -= OnAdClose;
//         rewardBasedVideoAd.OnAdRewarded -= OnAdRewarded;
//         RequestRewardedVideo();
//     }
//
//     private void OnAdRewarded(object sender, EventArgs args)
//     {
//         GameManager.ins.Revival();    
//     }
// }
