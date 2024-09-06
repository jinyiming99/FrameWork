using com.VRDemo.gamemessage;
using Game.Network;
using UnityEngine;

namespace Src.PlayerPositionSync
{
    public class UserBody
    {
        private const float MoveAnimationDistanceThreshold = 0.05f;
        private const float RotateAngleThreshold = 0.2f;
        public string UserId { get; set; }

        private Transform _transform;
        private Vector3 _targetPosition;
        private float _moveSpeed = 1.2f;
        private Vector3 _forward;

        private bool _isMoving = false;

        private readonly EstimateBodyHeight _estimateBodyHeight = new();

        private UserBodyPool _userBodyPool;

        private UserBodyAnimation _userBodyAnimation;
        public void Initialize()
        {
            _transform = _userBodyPool.Get().transform;
            _transform.name = UserId;
            _transform.localScale = Vector3.one;
            _userBodyAnimation = new UserBodyAnimation(_transform.GetComponent<Animator>());
        }

        public UserBody(string userId, UserBodyPool userBodyPool)
        {
            UserId = userId;
            _userBodyPool = userBodyPool;
        }

        public void SetPositionEffectiveImmediately(Vector3 position, float angle)
        {

            _transform.position = FixYAxisValue(position);

            _targetPosition = position;
            _forward = Vector3.up * angle;
            _transform.eulerAngles = _forward;
        }

        private Vector3 FixYAxisValue(Vector3 position)
        {
            //TODO: 这里需要改为使用更为可靠的y值数据源.
            return new Vector3(position.x, Player.Instance.transform.parent.parent.position.y, position.z);
        }

        public void SetPositionEffectiveImmediately(Position position)
        {
            SetPositionEffectiveImmediately(ProtoExtention.Conversion2Vector3(position), position.Direction);
        }

        public void SetTargetPosition(Vector3 targetPosition, float angle)
        {
            _targetPosition = FixYAxisValue(targetPosition);
            _forward = Vector3.up * angle;
        }


        public void SetTargetPosition(Position targetPosition)
        {
            SetTargetPosition(ProtoExtention.Conversion2Vector3(targetPosition), targetPosition.Direction);
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        public void Update()
        {
            BodyMove();
            BodyRotate();

            TryPlayAnimation();
        }

        private void BodyMove()
        {
            _transform.position = Vector3.Lerp(_transform.position, _targetPosition, Time.deltaTime * _moveSpeed);

            var currentPosition = _transform.position;
            var distance = Vector2.Distance(new Vector2(currentPosition.x, currentPosition.z), new Vector2(_targetPosition.x, _targetPosition.z));

            _isMoving = distance > MoveAnimationDistanceThreshold;
            if (!_isMoving)
            {
                _transform.position = _targetPosition;
            }

            _estimateBodyHeight.UpdateHeightData(_targetPosition.y);
        }

        private void BodyRotate()
        {
            var quaternionForward = Quaternion.Euler(_forward);
            _transform.rotation = Quaternion.Lerp(_transform.rotation, quaternionForward, Time.deltaTime * _moveSpeed);

            var angle = Mathf.Abs(Quaternion.Angle(_transform.rotation, quaternionForward));
            var isRotating = angle > RotateAngleThreshold;

            if (!isRotating)
            {
                _transform.rotation = quaternionForward;
            }
        }

        private void TryPlayAnimation()
        {
            bool isSquatting = _estimateBodyHeight.CheckSquatting(_targetPosition.y);
            if (isSquatting)
            {
                _userBodyAnimation.Squat();
            }
            else
            {
                if (_isMoving)
                {
                    _userBodyAnimation.Walk();
                }
                else
                {
                    _userBodyAnimation.Idle();
                }
            }
        }

        public void Clear()
        {
            _userBodyPool.Release(_transform.gameObject);
        }
    }
}