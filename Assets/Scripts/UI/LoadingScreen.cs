using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _screen;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void ShowLoadingScreen()
        {
            gameObject.SetActive(true);
            _screen.alpha = 1f;
        }

        public void HideLoadingScreen()
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            while (_screen.alpha > 0f)
            {
                _screen.alpha -= 0.01f;
                yield return new WaitForSeconds(0.015f);
            }

            gameObject.SetActive(false);
            yield break;
        }
    }
}