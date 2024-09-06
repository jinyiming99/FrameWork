using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public static class DeviceInfo
{
    public static string DeviceName
    {
        get
        {
            if (_deviceData is null)
            {
                LoadDeviceData();
            }
            return _deviceData._deviceName;
        }
    }
    [Serializable]
    private class DeviceInfoData
    {
        public string _deviceName;

        public void CreateDeviceInfoData()
        {
            string str = $"{DateTime.Now.Ticks}{SystemInfo.deviceUniqueIdentifier}";
            Debug.Log($"deciver id key = {str}");
            var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder builder = new StringBuilder();

            //Loop through each byte of the hashed data and format each one as a hexadecimal strings.
            for (int cnt = 0; cnt < data.Length; cnt++)
            {
                builder.Append(data[cnt].ToString("x2"));
            }

            _deviceName = builder.ToString();
            Debug.Log($"_deviceName = {_deviceName}");

        }
    }

    private static DeviceInfoData _deviceData = null;

    private static string _pathToDeviceNameFile = Application.persistentDataPath + "/device_name.txt";
    private static void LoadDeviceData()
    {
        Debug.Log($"LoadDeviceData path={_pathToDeviceNameFile}");
        if (!File.Exists(_pathToDeviceNameFile))
        {
            _deviceData = new DeviceInfoData();
            _deviceData.CreateDeviceInfoData();
            SaveDeviceData();
        }
        else
        {
            using (FileStream steam = new FileStream(_pathToDeviceNameFile, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(steam))
                {
                    var str = reader.ReadToEnd();
                    _deviceData = JsonUtility.FromJson<DeviceInfoData>(str);
                }
            }
        }

    }

    private static void SaveDeviceData()
    {
        Task.Factory.StartNew(() =>
        {
            using (FileStream steam = new FileStream(_pathToDeviceNameFile, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(steam))
                {
                    var str = JsonUtility.ToJson(_deviceData);
                    writer.Write(str);
                }
            }
        });

    }
}
