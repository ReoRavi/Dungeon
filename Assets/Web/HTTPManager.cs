using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using UnityEngine.UI;
using System.Json;

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

        Debug.Log(result);

        return ReplaceJsonData(result);
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

        return ReplaceJsonData(result);
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

        return ReplaceJsonData(result);
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

        return ReplaceJsonData(result);
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
        if (jsonFile.Length == 0)
        {
            return null;
        }

        string[] splitedJson = jsonFile.Split('}');
        string[] data = new string[splitedJson.Length - 1];    

        for (int index = 0; index < splitedJson.Length - 1; index++)
        {
            string jsonData = splitedJson[index] + "}";
            JsonValue jsonObject = JsonValue.Parse(jsonData);

            data[index] = jsonObject[columnName].ToString();
    }

        return data;
    }

    // php에서 include를 사용하니 AP 라는 문자열이 추가로 리턴됨, 따로 처리해준다
    private string ReplaceJsonData(string jsonData)
    {
        string data = jsonData.Substring(jsonData.IndexOf("AP") + 2);
        Debug.Log(data);
        return data;
    }
}
