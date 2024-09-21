using UnityEngine;
using UnityEngine.UI;
 /// <summary>
 /// unity inputfield的输入文字数量上限
 /// </summary>
[RequireComponent(typeof(InputField))]
public class InputFieldLimit : MonoBehaviour
{
    public int maxLength = 10; // 设置最大长度
 
    private InputField inputField;
 
    void Awake()
    {
        inputField = GetComponent<InputField>();
        inputField.onValidateInput += OnValidateInput;
    }
 
    private char OnValidateInput(string text, int charIndex, char addedChar)
    {
        if (inputField.text.Length >= maxLength)
        {
            return '\0'; // 返回空字符，表示不接受添加的字符
        }
        return addedChar; // 返回添加的字符，表示接受它
    }
}