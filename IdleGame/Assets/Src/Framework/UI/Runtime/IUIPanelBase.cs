namespace GameFrameWork.UI
{
    public interface IUIPanelBase
    {
        int GetLayerID { get; } 
        void Show(params object[] args);
        void Hide();
        void Release();
    }
}