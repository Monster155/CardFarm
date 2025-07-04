using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dajjsand.View.Game.Cards
{
    public class MergeBar : MonoBehaviour
    {
        [SerializeField] private GameObject _mergeBar;
        [SerializeField] private Image _barImage;

        private void Start()
        {
            StopMerge();
        }

        public void StartMerge()
        {
            _mergeBar.SetActive(true);
            _barImage.fillAmount = 0f;
        }

        public void UpdateProgress(float percentage)
        {
            _barImage.fillAmount = percentage;
        }

        public void FinishMerge()
        {
            _barImage.fillAmount = 1f;
            DOVirtual.DelayedCall(0.2f, () => _mergeBar.SetActive(false));
        }

        public void StopMerge()
        {
            _mergeBar.SetActive(false);
        }
    }
}