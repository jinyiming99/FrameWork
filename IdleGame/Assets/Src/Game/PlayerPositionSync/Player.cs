using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public string UserID = "";
    public string TeamName = "";

    private void Awake()
    {
        UserID = DeviceInfo.DeviceName;
        Instance = this;
    }
}