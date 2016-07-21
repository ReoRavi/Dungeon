using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Net.Json;

public enum RESTType { GET, PUT, POST, DELETE };

public class HTTPManager : MonoBehaviour
{
    // PHP URL 주소
    public string url;
    // DB 이름
    public string dbName;
    // PHP Key
    public string key;
    // DB 컬럼
    public string[] columns;
    // DB 데이터
    public string[] datas;
    // REST 종류
    public RESTType restType;

    public string HTTP_REQUEST()
    {
        switch (restType)
        {
            case RESTType.GET:
                return HTTP_GET(url + "/" + dbName + "/" + key);

            case RESTType.PUT:
                return HTTP_PUT(url + "/" + dbName + "/" + PlayerPrefs.GetString("UserCode"), columns, datas);

            case RESTType.POST:
                return HTTP_POST(url + "/" + dbName + "/" + key, columns, datas);

            case RESTType.DELETE:
                return HTTP_DELETE(url + "/" + dbName + "/" + key);

            default:
                return "Error!";
        }
    }

    private string HTTP_GET(string url)
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

        //Debug.Log(col["UserCode"].GetValue());

        return result;
    }

    private string HTTP_PUT(string url, string[] columns, string[] datas)
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

        return result;
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

    private string HTTP_DELETE(string url)
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

        return result;
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

    public string[] GetUserDataFromJson(string jsonFile, string columnName)
    {
        string[] splitedJson = jsonFile.Split('}');
        string[] data = new string[splitedJson.Length - 1];

        JsonTextParser parser = new JsonTextParser();

        for (int index = 0; index < splitedJson.Length - 1; index++)
        {
            string jsonData = splitedJson[index] + "}";
            JsonObject jsonObject = parser.Parse(jsonData);

            JsonObjectCollection col = (JsonObjectCollection)jsonObject;

            data[index] = col[columnName].GetValue().ToString();
        }

        return data;
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
}
