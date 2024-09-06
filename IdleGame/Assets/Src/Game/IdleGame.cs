using GameFrameWork;
using GameFrameWork.DebugTools;
using GameFrameWork.Network;
using GameFrameWork.Network.Interface;
using GameFrameWork.Pool;

namespace Game
{
    public class IdleGame : Game<GameFSM,GameData>
    {
        private static EventManager<EventArg> _gameEvent = new EventManager<EventArg>();
        public static EventManager<EventArg> GameEvent => _gameEvent;
        private NetworkObject _networkObject;
        
        public override void Init()
        {
            base.Init();
            
    
        }
        
        

        public override void Start()
        {
            base.Start();
        }

        public override void Resume()
        {
            base.Resume();
        }
        

        public override void Release()
        {
            base.Release();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Pause()
        {
            base.Pause();
        }
    }
}