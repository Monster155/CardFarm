using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dajjsand.View.Menu.SelectLevel
{
    public class LevelItem : MonoBehaviour
    {
        public event Action<int> OnClick;

        [SerializeField] private TMP_Text _levelNumText;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _lockGroup;
        [SerializeField] private GameObject[] _stars;

        private int _levelNum;

        public void Init(int levelNum, int starsCount, bool isUnlocked)
        {
            _levelNum = levelNum;
            _levelNumText.text = (_levelNum + 1).ToString();

            for (int i = 0; i < starsCount; i++)
                _stars[i].SetActive(true);
            for (int i = starsCount; i < _stars.Length; i++)
                _stars[i].SetActive(false);
            
            _lockGroup.SetActive(!isUnlocked);
        }

        private void Start()
        {
            _button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick() => OnClick?.Invoke(_levelNum);
    }
}