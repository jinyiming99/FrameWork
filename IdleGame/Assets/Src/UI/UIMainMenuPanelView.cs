using GameFrameWork.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMainMenuPanelView : MonoBehaviour
    {
        protected RectTransform _UIMainMenuPanel_RectTransform = null;
        public RectTransform UIMainMenuPanel_RectTransform
        {
             get
             {
                 if (_UIMainMenuPanel_RectTransform == null)
                 {
                     _UIMainMenuPanel_RectTransform = transform.Find("UIMainMenuPanel").gameObject.GetComponent<RectTransform>();
                 }
                 return _UIMainMenuPanel_RectTransform;
             }
       }
        protected Image _TextList_Image = null;
        public Image TextList_Image
        {
             get
             {
                 if (_TextList_Image == null)
                 {
                     _TextList_Image = transform.Find("TextList").gameObject.GetComponent<Image>();
                 }
                 return _TextList_Image;
             }
       }
        protected RectTransform _TextList_RectTransform = null;
        public RectTransform TextList_RectTransform
        {
             get
             {
                 if (_TextList_RectTransform == null)
                 {
                     _TextList_RectTransform = transform.Find("TextList").gameObject.GetComponent<RectTransform>();
                 }
                 return _TextList_RectTransform;
             }
       }
        protected CustomLoopView _TextList_CustomLoopView = null;
        public CustomLoopView TextList_CustomLoopView
        {
             get
             {
                 if (_TextList_CustomLoopView == null)
                 {
                     _TextList_CustomLoopView = transform.Find("TextList").gameObject.GetComponent<CustomLoopView>();
                 }
                 return _TextList_CustomLoopView;
             }
       }
        protected Image _Viewport1_Image = null;
        public Image Viewport1_Image
        {
             get
             {
                 if (_Viewport1_Image == null)
                 {
                     _Viewport1_Image = transform.Find("TextList/Viewport1").gameObject.GetComponent<Image>();
                 }
                 return _Viewport1_Image;
             }
       }
        protected RectTransform _Viewport1_RectTransform = null;
        public RectTransform Viewport1_RectTransform
        {
             get
             {
                 if (_Viewport1_RectTransform == null)
                 {
                     _Viewport1_RectTransform = transform.Find("TextList/Viewport1").gameObject.GetComponent<RectTransform>();
                 }
                 return _Viewport1_RectTransform;
             }
       }
        protected RectTransform _Content1_RectTransform = null;
        public RectTransform Content1_RectTransform
        {
             get
             {
                 if (_Content1_RectTransform == null)
                 {
                     _Content1_RectTransform = transform.Find("TextList/Viewport1/Content1").gameObject.GetComponent<RectTransform>();
                 }
                 return _Content1_RectTransform;
             }
       }
        protected CustomToggleGroup _Content1_CustomToggleGroup = null;
        public CustomToggleGroup Content1_CustomToggleGroup
        {
             get
             {
                 if (_Content1_CustomToggleGroup == null)
                 {
                     _Content1_CustomToggleGroup = transform.Find("TextList/Viewport1/Content1").gameObject.GetComponent<CustomToggleGroup>();
                 }
                 return _Content1_CustomToggleGroup;
             }
       }
        protected RectTransform _Node_RectTransform = null;
        public RectTransform Node_RectTransform
        {
             get
             {
                 if (_Node_RectTransform == null)
                 {
                     _Node_RectTransform = transform.Find("TextList/Viewport1/Content1/Node").gameObject.GetComponent<RectTransform>();
                 }
                 return _Node_RectTransform;
             }
       }
        protected RectTransform _GameObject1_RectTransform = null;
        public RectTransform GameObject1_RectTransform
        {
             get
             {
                 if (_GameObject1_RectTransform == null)
                 {
                     _GameObject1_RectTransform = transform.Find("TextList/Viewport1/Content1/Node/GameObject1").gameObject.GetComponent<RectTransform>();
                 }
                 return _GameObject1_RectTransform;
             }
       }
        protected CustomButton _GameObject1_CustomButton = null;
        public CustomButton GameObject1_CustomButton
        {
             get
             {
                 if (_GameObject1_CustomButton == null)
                 {
                     _GameObject1_CustomButton = transform.Find("TextList/Viewport1/Content1/Node/GameObject1").gameObject.GetComponent<CustomButton>();
                 }
                 return _GameObject1_CustomButton;
             }
       }
        protected Image _Image_Image = null;
        public Image Image_Image
        {
             get
             {
                 if (_Image_Image == null)
                 {
                     _Image_Image = transform.Find("TextList/Viewport1/Content1/Node/GameObject1/Image").gameObject.GetComponent<Image>();
                 }
                 return _Image_Image;
             }
       }
        protected RectTransform _Image_RectTransform = null;
        public RectTransform Image_RectTransform
        {
             get
             {
                 if (_Image_RectTransform == null)
                 {
                     _Image_RectTransform = transform.Find("TextList/Viewport1/Content1/Node/GameObject1/Image").gameObject.GetComponent<RectTransform>();
                 }
                 return _Image_RectTransform;
             }
       }
        protected Image _StartBtn_Image = null;
        public Image StartBtn_Image
        {
             get
             {
                 if (_StartBtn_Image == null)
                 {
                     _StartBtn_Image = transform.Find("StartBtn").gameObject.GetComponent<Image>();
                 }
                 return _StartBtn_Image;
             }
       }
        protected RectTransform _StartBtn_RectTransform = null;
        public RectTransform StartBtn_RectTransform
        {
             get
             {
                 if (_StartBtn_RectTransform == null)
                 {
                     _StartBtn_RectTransform = transform.Find("StartBtn").gameObject.GetComponent<RectTransform>();
                 }
                 return _StartBtn_RectTransform;
             }
       }
        protected CustomButton _StartBtn_CustomButton = null;
        public CustomButton StartBtn_CustomButton
        {
             get
             {
                 if (_StartBtn_CustomButton == null)
                 {
                     _StartBtn_CustomButton = transform.Find("StartBtn").gameObject.GetComponent<CustomButton>();
                 }
                 return _StartBtn_CustomButton;
             }
       }
        protected Image _LoadBtn_Image = null;
        public Image LoadBtn_Image
        {
             get
             {
                 if (_LoadBtn_Image == null)
                 {
                     _LoadBtn_Image = transform.Find("LoadBtn").gameObject.GetComponent<Image>();
                 }
                 return _LoadBtn_Image;
             }
       }
        protected RectTransform _LoadBtn_RectTransform = null;
        public RectTransform LoadBtn_RectTransform
        {
             get
             {
                 if (_LoadBtn_RectTransform == null)
                 {
                     _LoadBtn_RectTransform = transform.Find("LoadBtn").gameObject.GetComponent<RectTransform>();
                 }
                 return _LoadBtn_RectTransform;
             }
       }
        protected CustomButton _LoadBtn_CustomButton = null;
        public CustomButton LoadBtn_CustomButton
        {
             get
             {
                 if (_LoadBtn_CustomButton == null)
                 {
                     _LoadBtn_CustomButton = transform.Find("LoadBtn").gameObject.GetComponent<CustomButton>();
                 }
                 return _LoadBtn_CustomButton;
             }
       }
        protected Button _OptionBtn_Button = null;
        public Button OptionBtn_Button
        {
             get
             {
                 if (_OptionBtn_Button == null)
                 {
                     _OptionBtn_Button = transform.Find("OptionBtn").gameObject.GetComponent<Button>();
                 }
                 return _OptionBtn_Button;
             }
       }
        protected Image _OptionBtn_Image = null;
        public Image OptionBtn_Image
        {
             get
             {
                 if (_OptionBtn_Image == null)
                 {
                     _OptionBtn_Image = transform.Find("OptionBtn").gameObject.GetComponent<Image>();
                 }
                 return _OptionBtn_Image;
             }
       }
        protected RectTransform _OptionBtn_RectTransform = null;
        public RectTransform OptionBtn_RectTransform
        {
             get
             {
                 if (_OptionBtn_RectTransform == null)
                 {
                     _OptionBtn_RectTransform = transform.Find("OptionBtn").gameObject.GetComponent<RectTransform>();
                 }
                 return _OptionBtn_RectTransform;
             }
       }
        protected Button _ExitBtn_Button = null;
        public Button ExitBtn_Button
        {
             get
             {
                 if (_ExitBtn_Button == null)
                 {
                     _ExitBtn_Button = transform.Find("ExitBtn").gameObject.GetComponent<Button>();
                 }
                 return _ExitBtn_Button;
             }
       }
        protected Image _ExitBtn_Image = null;
        public Image ExitBtn_Image
        {
             get
             {
                 if (_ExitBtn_Image == null)
                 {
                     _ExitBtn_Image = transform.Find("ExitBtn").gameObject.GetComponent<Image>();
                 }
                 return _ExitBtn_Image;
             }
       }
        protected RectTransform _ExitBtn_RectTransform = null;
        public RectTransform ExitBtn_RectTransform
        {
             get
             {
                 if (_ExitBtn_RectTransform == null)
                 {
                     _ExitBtn_RectTransform = transform.Find("ExitBtn").gameObject.GetComponent<RectTransform>();
                 }
                 return _ExitBtn_RectTransform;
             }
       }
        protected Image _Viewport_Image = null;
        public Image Viewport_Image
        {
             get
             {
                 if (_Viewport_Image == null)
                 {
                     _Viewport_Image = transform.Find("Scroll View/Viewport").gameObject.GetComponent<Image>();
                 }
                 return _Viewport_Image;
             }
       }
        protected RectTransform _Viewport_RectTransform = null;
        public RectTransform Viewport_RectTransform
        {
             get
             {
                 if (_Viewport_RectTransform == null)
                 {
                     _Viewport_RectTransform = transform.Find("Scroll View/Viewport").gameObject.GetComponent<RectTransform>();
                 }
                 return _Viewport_RectTransform;
             }
       }
        protected RectTransform _Content_RectTransform = null;
        public RectTransform Content_RectTransform
        {
             get
             {
                 if (_Content_RectTransform == null)
                 {
                     _Content_RectTransform = transform.Find("Scroll View/Viewport/Content").gameObject.GetComponent<RectTransform>();
                 }
                 return _Content_RectTransform;
             }
       }
    }
}
