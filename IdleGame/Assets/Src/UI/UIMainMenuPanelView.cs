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
        protected Button _LoadBtn_Button = null;
        public Button LoadBtn_Button
        {
             get
             {
                 if (_LoadBtn_Button == null)
                 {
                     _LoadBtn_Button = transform.Find("LoadBtn").gameObject.GetComponent<Button>();
                 }
                 return _LoadBtn_Button;
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
    }
}
