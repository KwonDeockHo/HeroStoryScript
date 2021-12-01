using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MapObj
{
    public string roomtype { get; set; }
    public int number { get; set; }
    public string[] mapPos { get; set; }
}

public class mapObjSetting : MonoBehaviour
{
    public enum eDataType
    {
        NONE = -1,    // Open None
        Top,          // Open Top
        Bottom,       // Open Bottom
        Left,         // Open Left
        Right,        // Open Right
        TopRight,     // Open TopRight
        TopLeft,      // Open TopLeft
        BottomRight,  // Open BottomRight
        BottomLeft    // Open BottomLeft

    }
    public static mapObjSetting Instance;
    private string fileName = "mapFile.json";
    private string textfileName = "Resources/Text.txt";
    // 스테이지 정보를 담는 변수 
    public static int stage;
    
    // 메모장 데이터를 담는 변수
    public string Map;
    public string textMap;

    public List<MapObj> randMapObj;

    public GameObject[] objTemplete;

    public GameObject shopObject;
    public GameObject itemObject;
    public GameObject nextStageObject;
    public Chest[] chests;
    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        //Non Json, true/false : Txt
        randMapObj = ReadJson(fileName);
    }

    //Json 파일 입출력
    public List<MapObj> ReadJson(string filePath)
    {
        //var path = Path.Combine(Application.dataPath, filePath);

        //var fileContent = File.ReadAllText(path);
        //var mapobj = JsonConvert.DeserializeObject<List<MapObj>>(fileContent);
        
        // 윈도우 전용 셋업(Mac 필요 시 별도 처리 필요)
        var path = Path.Combine(Application.streamingAssetsPath, filePath);
        var fileContent = File.ReadAllText(path);
        var mapobj = JsonConvert.DeserializeObject<List<MapObj>>(fileContent);


        return mapobj;
    }

    public void LoadData(string path, eDataType dataType)
    {        // Json
        var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        var ta = Resources.Load(path) as TextAsset;

        switch (dataType)
        {
            case eDataType.NONE:

                break;
            case eDataType.Top:

                break;
            case eDataType.Bottom:
                break;
            case eDataType.Left:
                break;
            case eDataType.Right:
                break;
            case eDataType.TopRight:
                break;
            case eDataType.TopLeft:
                break;
            case eDataType.BottomRight:
                break;
            case eDataType.BottomLeft:
                break;

        }

    }
    //Txt 파일 입출력
    public string ReadTxt(string filePath)
    {
        var path = Path.Combine(Application.dataPath, filePath);

        FileInfo fileInfo = new FileInfo(path);
        string value = "";

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(path);
            value = reader.ReadToEnd();
            reader.Close();
        }

        else
            value = "파일이 없습니다.";

        return value;
    }
}
