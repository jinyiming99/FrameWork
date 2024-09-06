using System;

namespace GameFrameWork.Base
{
    public abstract class FrameWorkArgBase : EventArgs ,IReference
    {
        public abstract void Release();
    }
}