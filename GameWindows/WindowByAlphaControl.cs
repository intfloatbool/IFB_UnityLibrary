using UnityEngine;

namespace IFB_UnityLibrary.GameWindows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowByAlphaControl : GameWindowBase
    {
        [SerializeField] private float _showTime = 0.5f;
        
        private CanvasGroup _canvasGroup;

        private bool _isShowProcess;

        private float _showVelocity;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show()
        {
            if(IsDestroyed)
                return;
            
            _canvasGroup.alpha = 0.1f;
            _canvasGroup.interactable = true;
            _isShowProcess = true;
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            if(IsDestroyed)
                return;
            
            _isShowProcess = false;
        }

        public override void UpdateWindow()
        {
            if(!IsActive)
                return;
            
            base.UpdateWindow();

            float targetAlpha = _isShowProcess ? 1f : 0f;

            float alphaToApply = Mathf.SmoothDamp(_canvasGroup.alpha, targetAlpha, ref _showVelocity, _showTime);

            alphaToApply = Mathf.Clamp01(alphaToApply);
            
            _canvasGroup.alpha = alphaToApply;

            if (alphaToApply <= 0.09f)
            {
                _canvasGroup.interactable = false;
                gameObject.SetActive(false); 
            }
        }
    }
    
}