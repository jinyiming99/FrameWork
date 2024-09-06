using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.Pool;
using GameFrameWork;
using com.VRDemo.gamemessage;
using Game.Network;

namespace Src.PlayerPositionSync
{
    public class SyncUserBodyManager : MonoBehaviour
    {
        [SerializeField] private float UpdateCD = 0.2f;

        public float MoveSpeed = 2.5f;
        private Dictionary<string, UserBody> _userBodies;
        private bool _isStart = false;
        private FloatTimeCounter _timer = new();
        private Block _oldLocationBlock;

        private UserBodyPool _userBodyPool;

        public void Begin()
        {
            AddListener();
            _userBodies = DictionaryPool<string, UserBody>.Get();
            _isStart = true;
            _userBodyPool = new UserBodyPool();

        }

        public void Stop()
        {
            RemoveListener();
            foreach (var body in _userBodies)
            {
                body.Value.Clear();
            }

            DictionaryPool<string, UserBody>.Release(_userBodies);
            _userBodyPool.Dispose();
            _isStart = false;
        }

        private void AddListener()
        {
            //TODO:换成新的事件。
            //PositionUpdateRes.OnPositionUpdateReply += PositionUpdateResOnPositionUpdateReply;
        }

        private void RemoveListener()
        {
            //PositionUpdateRes.OnPositionUpdateReply -= PositionUpdateResOnPositionUpdateReply;
        }

        private void PositionUpdateResOnPositionUpdateReply(RepeatedField<UpdateBody> updateBodies)
        {
            UpdateUserBodyData(updateBodies);
        }

        private void UpdateUserBodyData(RepeatedField<UpdateBody> newUserBodiesInfo)
        {
            if (newUserBodiesInfo != null)
            {
                var updatedUserBodyId = ListPool<string>.Get();
                var selfUserId = Player.Instance.UserID;

                foreach (var updateBody in newUserBodiesInfo)
                {
                    if (updateBody.Userid.Equals(selfUserId) || string.IsNullOrEmpty(updateBody.Userid))
                    {
                        continue;
                    }

                    if (!_userBodies.TryGetValue(updateBody.Userid, out var currentUserBody))
                    {
                        currentUserBody = new UserBody(updateBody.Userid, _userBodyPool);
                        currentUserBody.Initialize();
                        currentUserBody.SetMoveSpeed(MoveSpeed);
                        currentUserBody.SetPositionEffectiveImmediately(updateBody.Pos);
                        _userBodies.Add(updateBody.Userid, currentUserBody);
                    }

                    currentUserBody.SetTargetPosition(updateBody.Pos);
                    updatedUserBodyId.Add(currentUserBody.UserId);
                }

                var differenceId = _userBodies.Keys.Except(updatedUserBodyId);

                foreach (var userId in differenceId)
                {
                    _userBodies[userId].Clear();
                    _userBodies.Remove(userId);
                }
            }
        }

        private void Update()
        {
            if (_isStart)
            {
                TriggerUserBodiesUpdate();
                ReportSelfPosition();
            }
        }

        private void TriggerUserBodiesUpdate()
        {
            foreach (var userBody in _userBodies.Values)
            {
                userBody.Update();
            }
        }

        private void ReportSelfPosition()
        {
            if (_timer.GetPassTime() > UpdateCD)
            {
                GenerateSelfPositionMessageAndSend();
                _timer.ResetCounter();
            }
        }

        private void GenerateSelfPositionMessageAndSend()
        {
            var playerTransform = Player.Instance.transform;

            var position = playerTransform.position;
            var directionAngle = playerTransform.eulerAngles.y;

            var currentBlock = PlayerPositionSyncTools.GetBlockByVector3(position);
            var allAdjacentBlocks = PlayerPositionSyncTools.GetAllAdjacentBlocks(currentBlock);


            RepeatedField<Block> blocks = new() { currentBlock };
            blocks.AddRange(allAdjacentBlocks);

            var pos = ProtoExtention.Conversion2NetVector3(position);
            pos.Direction = directionAngle;

            var msg = new ReportPositionRequest
            {
                Userid = Player.Instance.UserID,
                Pos = pos,
                OldLocationBlock = _oldLocationBlock ?? currentBlock,
                NewLocationBlock = currentBlock,
            };

            msg.BlockUserList.AddRange(blocks);

            _oldLocationBlock = currentBlock;

            //TODO:换成新的发送消息。
            //Winmain.SendMessage(MessageType.PositionReport, msg);
        }
    }
}