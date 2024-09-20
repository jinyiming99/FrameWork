using GameFrameWork.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITestNode : MonoBehaviour, ICustomLoopNode<string>
    {
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Text _content;
        [SerializeField]
        private Text _age;
        public void SetData(string data)
        {
            _name.text = data;
        }
    }

    public class TestNodeData
    {
        public string Name;
        public int Age;
        public string content;
    }
}