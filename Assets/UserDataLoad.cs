using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Net.Json;

public class UserDataLoad : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        // 처음 접속이라면
        if (!PlayerPrefs.HasKey("firstAccess"))
        {
            PlayerPrefs.SetInt("firstAccess", 1);

            string[] columns = { "UserCode", "FriendCode", "NickName", "HighScore", "FirstAccessTime", "AccessTime", "LastAccessTime", "FriendCount" };
            string[] datas = { "0", "0", "\\\"Ravi\\\"", "0", "\\\"0000-00-00 00:00:00\\\"", "\\\"0000-00-00 00:00:00\\\"", "\\\"0000-00-00 00:00:00\\\"", "0" };

            PlayerPrefs.SetString("UserCode", HTTP_POST("http://ravi1237.com/Dungeon/UnityTest.php/Dungeon_UserData/FirstAccess", columns, datas));

            Debug.Log("Code : " + PlayerPrefs.GetString("UserCode"));
        }
    }

    public void FirstAccess()
    {
        string[] columns = { "UserCode", "FriendCode", "NickName", "HighScore", "FirstAccessTime", "AccessTime", "LastAccessTime", "FriendCount" };
        string[] datas = { "0", "0", "\\\"Ravi\\\"", "0", "\\\"0000-00-00 00:00:00\\\"", "\\\"0000-00-00 00:00:00\\\"", "\\\"0000-00-00 00:00:00\\\"", "0" };

        HTTP_POST("http://ravi1237.com/Dungeon/UnityTest.php/Dungeon_UserData", columns, datas);
    }

    public void FirstAccessGet()
    {
        HTTP_GET("http://ravi1237.com/Dungeon/UnityTest.php/Dungeon_UserData/1");
    }

    public void FirstAccessPut()
    {
        string[] columns = { "UserCode", "FriendCode", "NickName", "HighScore", "FirstAccessTime", "AccessTime", "LastAccessTime", "FriendCount" };
        string[] datas = { "1", "1237", "\\\"Ravi\\\"", "1", "\\\"1000-00-00 00:00:00\\\"", "\\\"1000-00-00 00:00:00\\\"", "\\\"1000-00-00 00:00:00\\\"", "1" };

        HTTP_PUT("http://ravi1237.com/Dungeon/UnityTest.php/Dungeon_UserData/1", columns, datas);
    }

    public void FirstAccessDelete()
    {
        HTTP_DELETE("http://ravi1237.com/Dungeon/UnityTest.php/Dungeon_UserData/0");
    }

    private void HTTP_GET(string url)
    {
        HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

        webRequest.Method = "GET";
        webRequest.ContentType = "application/json";

        string result = null;

        using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            result = reader.ReadToEnd();
        }

        JsonTextParser parser = new JsonTextParser();
        JsonObject jsonObject = parser.Parse(result);
        JsonObjectCollection col = (JsonObjectCollection)jsonObject;

        Debug.Log(col["UserCode"].GetValue());
    }

    private void HTTP_PUT(string url, string[] columns, string[] datas)
    {
        HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

        webRequest.Method = "PUT";
        webRequest.ContentType = "application/json";

        using (StreamWriter stream = new StreamWriter(webRequest.GetRequestStream()))
        {
            stream.WriteLine(CreateJsonFile(columns, datas));
        }

        // Pick up the response:
        string result = null;
        using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            result = reader.ReadToEnd();
        }

        Debug.Log(result);
    }

    private string HTTP_POST(string url, string[] columns, string[] datas)
    {
        HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

        webRequest.Method = "POST";
        webRequest.ContentType = "application/json";

        using (StreamWriter stream = new StreamWriter(webRequest.GetRequestStream()))
        {
            stream.WriteLine(CreateJsonFile(columns, datas));
        }

        string result = null;

        using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            result = reader.ReadToEnd();
        }

        Debug.Log(result);

        return result;
    }

    private void HTTP_DELETE(string url)
    {
        HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

        webRequest.Method = "DELETE";
        webRequest.ContentType = "application/json";

        string result = null;

        using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            result = reader.ReadToEnd();
        }

        Debug.Log(result);
    }

    private string CreateJsonFile(string[] column, string[] data)
    {
        if (column.Length != data.Length)
        {
            return "Error";
        }

        string jsonString = "{";

        for (int dataCount = 0; dataCount < data.Length; dataCount++)
        {
            jsonString += "\"";

            jsonString += column[dataCount];

            jsonString += "\"";

            jsonString += " : ";

            jsonString += "\"";

            jsonString += data[dataCount];

            jsonString += "\"";

            if (dataCount != data.Length - 1)
                jsonString += ", ";
        }

        jsonString += "}";

        return jsonString;
    }
}
