namespace GameFrameWork
{
    public interface IGame
    {
        void Init();

        void Start();


        void Resume();


        void Release();


        void Update();

        void Pause();
    }
}