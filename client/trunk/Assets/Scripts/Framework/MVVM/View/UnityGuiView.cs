using System;
using UnityEngine;

namespace Framework.MVVM
{
    public abstract class UnityGuiView : IView<ViewModelBase>
    {
        public virtual string ViewName
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual string ViewModelTypeName
        {
            get
            {
                return string.Empty;
            }
        }

        public GameObject GameObject
        {
            get;
            private set;
        }

        private Transform _transform;
        public Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = GameObject.transform;
                }
                return _transform;
            }
        }

        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                {
                    _canvasGroup = GameObject.GetComponent<CanvasGroup>();
                }
                return _canvasGroup;
            }
        }

        private bool _isInitialized;

        public bool DestoryOnHide
        {
            get;
            set;
        }

        protected readonly PropertyBinder<ViewModelBase> binder = new PropertyBinder<ViewModelBase>();
        public readonly BindableProperty<ViewModelBase> viewModelProperty = new BindableProperty<ViewModelBase>();

        public Action RevealedAction
        {
            get;
            set;
        }

        public Action HiddenAction
        {
            get;
            set;
        }

        public ViewModelBase BindingContext
        {
            get
            {
                return viewModelProperty.Value;
            }
            set
            {
                if (!_isInitialized)
                {
                    OnInitialize();
                    _isInitialized = true;
                }
                viewModelProperty.Value = value;
            }
        }

        protected virtual void OnInitialize()
        {
            GameObject = GameObject.Find(ViewName);
            GameObject.AddComponent<CanvasGroup>();
            viewModelProperty.OnValueChanged += OnBindingContextChanged;
        }

        public void Reveal(bool immediate = true, Action action = null)
        {
            if (action != null)
            {
                RevealedAction += action;
            }
            OnAppear();
            OnReveal(immediate);
            OnRevealed();
        }

        public void Hide(bool immediate = true, Action action = null)
        {
            if (action != null)
            {
                HiddenAction += action;
            }
            OnHide(immediate);
            OnHidden();
            OnDisappear();
        }

        public virtual void OnAppear()
        {
            GameObject.SetActive(true);
        }

        private void OnReveal(bool immediate)
        {
            BindingContext.OnStartReveal();
            if (immediate)
            {
                Transform.localScale = Vector3.one;
                CanvasGroup.alpha = 1;
            }
            else
            {
                StartAnimatedReveal();
            }
        }

        public virtual void OnRevealed()
        {
            BindingContext.OnFinishReveal();
            if (RevealedAction != null)
            {
                RevealedAction();
            }
        }

        private void OnHide(bool immediate)
        {
            BindingContext.OnStartHide();
            if (immediate)
            {
                Transform.localScale = Vector3.zero;
                CanvasGroup.alpha = 0;
            }
            else
            {
                StartAnimatedHide();
            }
        }

        public virtual void OnHidden()
        {
            if (HiddenAction != null)
            {
                HiddenAction();
            }
        }

        public virtual void OnDisappear()
        {
            GameObject.SetActive(false);
            BindingContext.OnFinishHide();
            if (DestoryOnHide)
            {
                UnityEngine.Object.Destroy(GameObject);
            }
        }

        public virtual void OnDestory()
        {
            if (BindingContext.IsRevealed)
            {
                Hide(true);
            }
            BindingContext.OnDestory();
            BindingContext = null;
            viewModelProperty.OnValueChanged = null;
        }

        protected virtual void StartAnimatedReveal()
        {
            CanvasGroup.interactable = false;
            Transform.localScale = Vector3.one;

            ////huandong
            //CanvasGroup.DOFade(1, 0.2f).SetDelay(0.2f).OnComplete(() =>
            //{
            //    canvasGroup.interactable = true;
            //});
        }

        protected virtual void StartAnimatedHide()
        {
            CanvasGroup.interactable = false;
            //canvasGroup.DOFade(0, 0.2f).SetDelay(0.2f).OnComplete(() =>
            //{
            //    transform.localScale = Vector3.zero;
            //    canvasGroup.interactable = true;
            //});
        }

        protected virtual void OnBindingContextChanged(ViewModelBase oldValue, ViewModelBase newValue)
        {
            binder.UnBind(oldValue);
            binder.Bind(newValue);
        }
    }
}
