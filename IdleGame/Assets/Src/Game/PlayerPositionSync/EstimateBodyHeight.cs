using System.Collections.Generic;
using System.Linq;

namespace Src.PlayerPositionSync
{
    public class EstimateBodyHeight
    {
        private const int SampleTargetSize = 500;
        private const byte SampleFrequency = 5;
        private const float SquattingThreshold = 0.7f;
        private const float DefaultBodyHeight = 1.7f;

        private readonly Queue<float> _bodyHeightQueue = new();

        private byte UpdateHeightTimes = 0;
        private int _sampleSize = 0;


        public EstimateBodyHeight()
        {
            _sampleSize = SampleTargetSize / SampleFrequency;
        }

        public void UpdateHeightData(float bodyHeight)
        {
            int index = UpdateHeightTimes % SampleFrequency;

            if (index == 0)
            {
                _bodyHeightQueue.Enqueue(bodyHeight);

                if (_bodyHeightQueue.Count > _sampleSize)
                {
                    _bodyHeightQueue.Dequeue();
                }

                UpdateHeightTimes = 0;
            }


            UpdateHeightTimes++;
        }

        public float GetBodyHeight()
        {
            return _bodyHeightQueue.Count > 0 ? _bodyHeightQueue.Max() : DefaultBodyHeight;
        }

        public bool CheckSquatting(float height)
        {
            return height < GetBodyHeight() * SquattingThreshold;
        }
    }
}