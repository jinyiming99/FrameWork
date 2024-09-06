using Game.Status;
using GameFrameWork;
using GameFrameWork.Pool;

namespace Game
{
    public class GameFSM : FSMMachine<IState,GameStatusEnum>
    {
        public GameFSM() : base()
        {
            AddState(GameStatusEnum.MainMenu,ClassPool<GameMenuStatus>.Spawn());
            m_nextState = m_dic[GameStatusEnum.MainMenu];
        }
    }
}