using UnityEngine;

namespace Tools.AnimationCreate
{
    public class AnimationCreater : MonoBehaviour
    {

        [Header("路径点")] public BezierCurve _curve = new BezierCurve();
        [Header("移动速度")]
        public float _speed = 1;
        [Header("帧数")]
        public int frame = 30;

        [Header("是否旋转")] public bool isFollow = true;

        private const int STEP = 2000;

 
    }
}