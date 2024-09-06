using System;

namespace GameFrameWork.Network
{
    public class NetworkException : Exception
    {
        public NetworkException(NetworkErrorEnum e,string msg) :base(msg)
        {
            err = e;
        }
        private NetworkErrorEnum err;
        public NetworkErrorEnum Err => err;
    }
}