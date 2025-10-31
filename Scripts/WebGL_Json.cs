using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System.Text;

[System.Serializable]
public class Ski
{
    public int No;
    public string Question;
    public string Correct;
    public string Incorrect1;
    public string Incorrect2;
}

[System.Serializable]
public class Ski_List
{
    public string projectAuthoringCode;
    public string title;
    public string lesson;
    public int level;
    public int theme;
    public List<Ski> data = new List<Ski>();
}

[System.Serializable]
public class Avoid
{
    public int No;
    public string Question;
    public string Correct;
    public string Incorrect1;
    public string Incorrect2;
    public string Incorrect3;
    public string Incorrect4;
}

[System.Serializable]
public class Avoid_List
{
    public string projectAuthoringCode;
    public string title;
    public string lesson;
    public int level;
    public int theme;
    public List<Avoid> data = new List<Avoid>();
}

[System.Serializable]
public class Truck
{
    public int No;
    public string Question;
    public string Correct;
    public string Incorrect1;
    public string Incorrect2;
}

[System.Serializable]
public class Truck_List
{
    public string projectAuthoringCode;
    public string title;
    public string lesson;
    public int level;
    public int theme;
    public List<Truck> data = new List<Truck>();
}

[System.Serializable]
public class Bubble
{
    public int No;
    public string Sentence;
    public string Correct1;
    public string Correct2;
    public string Correct3;
    public string Correct4;
    public string Correct5;
    public string Correct6;
    public string Correct7;
    public string Correct8;
    public string Answer;
}

[System.Serializable]
public class Bubble_List
{
    public string projectAuthoringCode;
    public string title;
    public string lesson;
    public int level;
    public int theme;
    public List<Bubble> data = new List<Bubble>();
}

public class WebGL_Json : MonoBehaviour
{
    public UI_Manager ui_Manager;
    public Send_Data send_Data;
    public List<Quiz_List> temp = new List<Quiz_List>();
    public List<Quiz> tempQuizList = new List<Quiz>();

    JsonData itemData;
    public string gameName;

    public Ski_List ski_List;
    public Avoid_List avoid_List;
    public Truck_List truck_List;
    public Bubble_List bubble_List;

    public string title;
    public string lesson;
    public string projectAuthoringCode;
    public int theme = -1;
    public int level = -1;
    public int quiz_Count;

    public void Start()
    {
        StartCoroutine(LoadBase());
    }

    public void WebGL_Start()
    {
        StartCoroutine(LoadBase());
    }

    public IEnumerator LoadBase()
    {
        string JsonString;
        string filePath;

        //filePath = Path.Combine(Application.dataPath + gameName);
        filePath = Application.streamingAssetsPath + gameName;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW reader = new WWW(filePath);
            //while (!reader.isDone) { }
            yield return reader;
            JsonString = reader.text;
        }
        else
        {
            JsonString = File.ReadAllText(filePath);
        }

        if (gameName == "/Ski adventure.json")
        {
            ski_List = JsonUtility.FromJson<Ski_List>(JsonString);
            title = ski_List.title;
            lesson = ski_List.lesson;
            theme = ski_List.theme;
            level = ski_List.level;
            projectAuthoringCode = ski_List.projectAuthoringCode;
            ui_Manager.quiz_Count = ski_List.data.Count;
        }
        else if (gameName == "/Space rush.json")
        {
            avoid_List = JsonUtility.FromJson<Avoid_List>(JsonString);
            title = avoid_List.title;
            lesson = avoid_List.lesson;
            theme = avoid_List.theme;
            level = avoid_List.level;
            projectAuthoringCode = avoid_List.projectAuthoringCode;
            ui_Manager.quiz_Count = avoid_List.data.Count;
        }
        else if (gameName == "/Pick a truck.json")
        {
            truck_List = JsonUtility.FromJson<Truck_List>(JsonString);
            title = truck_List.title;
            lesson = truck_List.lesson;
            theme = truck_List.theme;
            level = truck_List.level;
            projectAuthoringCode = truck_List.projectAuthoringCode;
            ui_Manager.quiz_Count = truck_List.data.Count;
        }
        else
        {
            bubble_List = JsonUtility.FromJson<Bubble_List>(JsonString);
            title = bubble_List.title;
            lesson = bubble_List.lesson;
            theme = bubble_List.theme;
            level = bubble_List.level;
            projectAuthoringCode = bubble_List.projectAuthoringCode;
            ui_Manager.quiz_Count = bubble_List.data.Count;
        }

        //send_Data.StartAnswerList();
        ui_Manager.UI_ManagerStart();
    }
}