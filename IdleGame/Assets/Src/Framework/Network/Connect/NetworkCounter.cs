namespace GameFrameWork.Network
{
    public static class NetworkCounter
    {
        public static int s_count = 0;
        public static int GetCount()
        {
            return s_count++;
        }
    }
}