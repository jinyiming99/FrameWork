namespace GameFrameWork.UI
{
    public interface IUIBase
    {
        int GetLayerID { get; } 
        void Show(params object[] args);
        void Hide();
        void Release();
    }
}