using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace GameFrameWork.UI
{
    public interface IExpressionComponent
    {
        void Do(UIComponentStates state);
    }
    [System.Serializable]
    public abstract class CodeExpressionBase<T> :IExpressionComponent
    {
        protected Tween _tween;
        public float duration = 0.15f;
        public Graphic graphic;
        public T normalValue ;
        public T hightLightValue=default(T);
        public T pressValue=default(T);
        public T disableValue=default(T);
        public T selectedValue=default(T);

        protected T GetValue(UIComponentStates states)
        {
            switch (states)
            {
                case UIComponentStates.normal:
                    return normalValue;
                case UIComponentStates.press:
                    return pressValue;
                case UIComponentStates.hightLight:
                    return hightLightValue;
                case UIComponentStates.disable:
                    return disableValue;
                case UIComponentStates.selected:
                    return selectedValue;
            }

            return default(T);
        }

        public abstract void Do(UIComponentStates states);

        public virtual void Release()
        {
            graphic.DOKill(true);

        }
    }
    [System.Serializable]
    public class ScaleAnimation : CodeExpressionBase<Vector3>
    {
        
        public ScaleAnimation()
        {
            duration = 0.15f;
            normalValue = Vector3.one;
            hightLightValue = Vector3.one;
            pressValue = Vector3.one;
            disableValue = Vector3.one;
            selectedValue = Vector3.one;
        }
        public override void Do(UIComponentStates states)
        {
            if (graphic != null)
                _tween = graphic.transform.DOScale(GetValue(states), duration).SetEase(Ease.Linear);
        }

        public override void Release()
        {
            graphic.transform.DOKill(true);
        }
    }
    [System.Serializable]
    public class ColorAnimation : CodeExpressionBase<Color>
    {
        public ColorAnimation()
        {
            duration = 0.15f;
            normalValue = Color.white;
            hightLightValue = Color.white;
            pressValue = Color.white;
            disableValue = Color.white;
            selectedValue = Color.white;
        }
        public override void Do(UIComponentStates states)
        {
            if (graphic != null)
                _tween = graphic.DOColor(GetValue(states), duration).SetEase(Ease.Linear);
        }
        public override void Release()
        {
            graphic.DOKill(true);
        }
    }
    [System.Serializable]
    public class OffsetAnimation: CodeExpressionBase<Vector3>
    {
        public OffsetAnimation()
        {
            duration = 0.15f;
            normalValue = Vector3.zero;
            hightLightValue = Vector3.zero;
            pressValue = Vector3.zero;
            disableValue = Vector3.zero;
            selectedValue = Vector3.zero;
        }
        public override void Do(UIComponentStates states)
        {
            if (graphic != null)
                _tween = graphic.transform.DOLocalMove(GetValue(states), duration).SetEase(Ease.Linear);
        }
        public override void Release()
        {
            graphic.transform.DOKill(true);
        }
    }
    [System.Serializable]
    public class UIExpressionComponent : MonoBehaviour , ICustomComponentStateChange
    {
        public ScaleAnimation[] _scaleAnimations;
        public ColorAnimation[] _colorAnimations;
        public OffsetAnimation[] _offsetAnimations;

        public void Release()
        {
            foreach (var scaleAnimation in _scaleAnimations)
            {
                scaleAnimation.Release();
            }
            foreach (var colorAnimation in _colorAnimations)
            {
                colorAnimation.Release();
            }
            foreach (var offsetAnimation in _offsetAnimations)
            {
                offsetAnimation.Release();
            }
        }
        public void OnStateChange(UIComponentStates state)
        {
            DoExpression(_scaleAnimations, state);
            DoExpression(_colorAnimations, state);
            DoExpression(_offsetAnimations, state);
        }

        private void DoExpression(IExpressionComponent[] array, UIComponentStates state)
        {
            if (array == null)
                return;
            foreach (var expressionComponent in array)
            {
                expressionComponent.Do(state);
            }
        }
        
    }
}