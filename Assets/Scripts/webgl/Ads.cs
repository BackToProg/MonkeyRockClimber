using System;
using UnityEngine;

namespace webgl
{
    public class Ads : MonoBehaviour
    {
        public static Ads Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public void ShowReward(Action onComplete)
        {
        }

        public void ShowInterstitial()
        {
        }
    }
}