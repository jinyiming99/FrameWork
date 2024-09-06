using System.Collections.Generic;
using com.VRDemo.gamemessage;
using UnityEngine;

namespace Src.PlayerPositionSync
{
    public class PlayerPositionSyncTools
    {
        private const float BlockSize = 3;

        public static Block GetBlockByVector3(Vector3 position)
        {
            return new Block()
            {
                X = Mathf.CeilToInt(position.x / BlockSize),
                Y = Mathf.CeilToInt(position.z / BlockSize)
            };
        }

        public static Block GetRightBlock(Block block)
        {
            return new Block()
            {
                X = block.X + 1,
                Y = block.Y
            };
        }

        public static Block GetLeftBlock(Block block)
        {
            return new Block()
            {
                X = block.X - 1,
                Y = block.Y
            };
        }

        public static Block GetUpBlock(Block block)
        {
            return new Block()
            {
                X = block.X,
                Y = block.Y + 1
            };
        }

        public static Block GetDownBlock(Block block)
        {
            return new Block()
            {
                X = block.X,
                Y = block.Y - 1
            };
        }

        public static List<Block> GetAllAdjacentBlocks(Block block)
        {
            var upBlock = GetUpBlock(block);
            var downBlock = GetDownBlock(block);

            var rightBlock = GetRightBlock(block);
            var leftBlock = GetLeftBlock(block);

            var upRightBlock = GetRightBlock(upBlock);
            var upLeftBlock = GetLeftBlock(upBlock);

            var downRightBlock = GetRightBlock(downBlock);
            var downLeftBlock = GetLeftBlock(downBlock);

            return new List<Block>()
            {
                upBlock,
                downBlock,
                rightBlock,
                leftBlock,
                upRightBlock,
                upLeftBlock,
                downRightBlock,
                downLeftBlock
            };
        }
    }
}