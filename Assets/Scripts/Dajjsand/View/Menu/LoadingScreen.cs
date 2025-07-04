using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dajjsand.View.Menu
{
    public class LoadingScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _loadingAdditionalText;
        [SerializeField] private float _animDelay = 1f;
        [SerializeField] private int _maxDotsCount = 3;
        [Space]
        [SerializeField] private RectTransform _markerContainer;
        [SerializeField] private RectTransform _loadingMarker;

        private Coroutine _loadingCoroutine;

        private void OnEnable()
        {
            _loadingCoroutine = StartCoroutine(LoadingAnimationCoroutine());
            _loadingMarker.sizeDelta = new Vector2(0, _loadingMarker.sizeDelta.y);
        }

        private void OnDisable()
        {
            StopCoroutine(_loadingCoroutine);
        }

        public void UpdateProgress(float percent)
        {
            _loadingMarker.sizeDelta = new Vector2(percent * _markerContainer.rect.width, _loadingMarker.sizeDelta.y);
        }
        
        private IEnumerator LoadingAnimationCoroutine()
        {
            int count = 0;
            while (true)
            {
                yield return new WaitForSeconds(_animDelay);

                count = count >= _maxDotsCount ? 0 : count + 1;

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < count; i++)
                    sb.Append('.');

                _loadingAdditionalText.text = sb.ToString();
            }
        }
    }
}