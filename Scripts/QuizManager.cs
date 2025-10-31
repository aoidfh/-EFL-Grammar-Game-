using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quiz_List
{
    public int Level;
    public int theme;
    public List<Quiz> quiz_List = new List<Quiz>();
}

[System.Serializable]
public class Quiz
{
    public int number;
    public string quiz;
    public string answer;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;
    public string answer5;
    public string answer6;
    public string answer7;

    public Quiz(int number = 0, string quiz = "", string answer = "", string answer1 = "", string answer2 = "", string answer3 = "", string answer4 = "",
                string answer5 = "", string answer6 = "", string answer7 = "")
    {
        this.number = number;
        this.quiz = quiz;
        this.answer = answer;
        this.answer1 = answer1;
        this.answer2 = answer2;
        this.answer3 = answer3;
        this.answer4 = answer4;
        this.answer5 = answer5;
        this.answer6 = answer6;
        this.answer7 = answer7;
    }
}

public enum Type
{
    Meteor,
    Truck
}

public class QuizManager : MonoBehaviour
{
    public WebGL_Json webGL_Json;
    public AnswerList answerList;
    public UI_Manager ui_Manager;
    public Meteor_PlayerCtrl player_Ctrl;
    public MoveManager moveManager;
    public Meteor_SpawnManager meteor_SpawnManager;
    public Meteor_Manager[] meteor_Managers;
   
    public List<Quiz> quizList = new List<Quiz>();    
    public List<int> countList = new List<int>();
    public string[] check_Answer = new string[5];
    public Text[] truckText = new Text[5];
    public Text[] MonsterText_Theme1 = new Text[5];
    public Text[] MonsterText_Theme2 = new Text[5];

    public Text quiz;
    public int quizNum;

    public Meteor_SpawnManager spawn;   

    public float game_Time;

    public AudioSource quizPanelAudio_Source;

    public void QuizManager_Start()
    {       
        if (webGL_Json.gameName == "/Space rush.json")
        {
            meteor_SpawnManager.MeteorSpawnManager_Start();

            for (int i = 0; i < webGL_Json.avoid_List.data.Count; i++)
            {
                ui_Manager.QuestionAnswerList(webGL_Json.avoid_List.data[i].Question, webGL_Json.avoid_List.data[i].Correct);

                Quiz tempQuiz = new Quiz();
                tempQuiz.number = webGL_Json.avoid_List.data[i].No;
                tempQuiz.quiz = ui_Manager.QuestionBlank(webGL_Json.avoid_List.data[i].Question);
                tempQuiz.answer = webGL_Json.avoid_List.data[i].Correct;
                tempQuiz.answer1 = webGL_Json.avoid_List.data[i].Incorrect1;
                tempQuiz.answer2 = webGL_Json.avoid_List.data[i].Incorrect2;
                tempQuiz.answer3 = webGL_Json.avoid_List.data[i].Incorrect3;
                tempQuiz.answer4 = webGL_Json.avoid_List.data[i].Incorrect4;

                quizList.Add(tempQuiz);
            }
        }
        else if (webGL_Json.gameName == "/Pick a truck.json")
        {
            for (int i = 0; i < webGL_Json.truck_List.data.Count; i++)
            {
                ui_Manager.QuestionAnswerList(webGL_Json.truck_List.data[i].Question, webGL_Json.truck_List.data[i].Correct);

                Quiz tempQuiz = new Quiz();
                tempQuiz.number = webGL_Json.truck_List.data[i].No;
                tempQuiz.quiz = ui_Manager.QuestionBlank(webGL_Json.truck_List.data[i].Question);
                tempQuiz.answer = webGL_Json.truck_List.data[i].Correct;
                tempQuiz.answer1 = webGL_Json.truck_List.data[i].Incorrect1;
                tempQuiz.answer2 = webGL_Json.truck_List.data[i].Incorrect2;

                quizList.Add(tempQuiz);
            }

            if (webGL_Json.theme == 1)
            {
                Debug.Log(GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(0).gameObject);
                GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(0).gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(false);            
            }
            else
            {
                GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.GetChild(3).GetChild(0).GetChild(0).gameObject.SetActive(false);            
            }
        }

        ui_Manager.time_Sldier.maxValue = 15;
        //ui_Manager.isRestart += Restart;

        foreach (Quiz item in quizList)
        {
            count = 2;
            if (item.answer2 != "" && item.answer2 != null)
            {
                count++;
            }
            if (item.answer3 != "" && item.answer3 != null)
            {
                count++;
            }
            if (item.answer4 != "" && item.answer4 != null)
            {
                count++;
            }

            countList.Add(count);
        }

        ui_Manager.time_Sldier.maxValue = game_Time;
        ui_Manager.time_Sldier.value = ui_Manager.time_Sldier.maxValue;
    }

    int count =0;
    List<Text> trcuk_Text = new List<Text>();
    public List<Text> temp_Trcuk = new List<Text>();
    public List<Text> temp_AnswerList = new List<Text>();
    public void StartQuiz(int index)
    {
        quizPanelAudio_Source.Play();

        if (meteor_SpawnManager != null)
        {
            ui_Manager.state = State.Play;

            player_Ctrl.fail_Index = 0;

            for (int i = 0; i < 5; i++)
            {
                meteor_SpawnManager.meteor_Manager[i].isMove = true;
            }
        }

        //Quiz_Number.text = index+1 + "/" + quizList.Count;
        if ((index+1).ToString().Length == 1)
        {
            ui_Manager.currentQuiz_Count.text = "0" + (index + 1).ToString();
        }
        else
        {
            ui_Manager.currentQuiz_Count.text = (index + 1).ToString();
        }

        if (quizList.Count <= 9)
        {
            ui_Manager.totalQuiz_Count.text = "0" + quizList.Count;
        }
        else
        {
            ui_Manager.totalQuiz_Count.text = quizList.Count.ToString();
        }
        
        quizNum = index;
        List<string> tempList = new List<string>();

        tempList.Add(quizList[index].answer);
        tempList.Add(quizList[index].answer1);
      
        if (quizList[index].answer2 != "" && quizList[index].answer2 != null)
        {
            tempList.Add(quizList[index].answer2);
            
        }
        if (quizList[index].answer3 != "" && quizList[index].answer3 != null)
        {
            tempList.Add(quizList[index].answer3);

        }
        if (quizList[index].answer4 != "" && quizList[index].answer4 != null)
        {
            tempList.Add(quizList[index].answer4);
        }

        if (spawn != null)
        {
            spawn.MeteorSpawn();
        }
        
        if (webGL_Json.gameName == "/Pick a truck.json")
        {
            trcuk_Text.Clear();
            temp_Trcuk.Clear();
            for (int j = 0; j < 3; j++)
            {
                trcuk_Text.Add(truckText[j]);
            }
            int randdata = -1;
            for (int i = 0; i < countList[quizNum]; i++)
            {
                int check = 0;
                int tempNum =0;
                if (countList[quizNum] == 2)
                {
                    int truck_TempText = UnityEngine.Random.Range(0, trcuk_Text.Count);
                    if(randdata == truck_TempText)
                    {
                        while (true)
                        {
                            if (randdata == truck_TempText)
                            {
                                randdata = UnityEngine.Random.Range(0, trcuk_Text.Count);
                            }
                            else
                            {
                                truck_TempText = randdata;

                                break;
                            }
                        }
                    }
                    else
                    {
                        randdata = truck_TempText;
                    }
                                                         
                    int temp = UnityEngine.Random.Range(0, tempList.Count);

                    trcuk_Text[truck_TempText].text = tempList[temp];
                    check_Answer[truck_TempText] = tempList[temp];

                    if (check == 0)
                    {
                        tempNum = truck_TempText;
                    }
                                       
                    temp_Trcuk.Add(trcuk_Text[truck_TempText]);                   
                    tempList.RemoveAt(temp);
                 // trcuk_Text.RemoveAt(truck_TempText);
                }
                else
                {
                    int temp = UnityEngine.Random.Range(0, tempList.Count);
                    check_Answer[i] = tempList[temp];
                    truckText[i].text = tempList[temp];
                    tempList.RemoveAt(temp);
                }


                //int temp = UnityEngine.Random.Range(0, tempList.Count);
                //check_Answer[i] = tempList[temp];
                //truckText[i].text = tempList[temp];
                //tempList.RemoveAt(temp);
            }
        }
        else
        {
            for (int i = 0; i < countList[quizNum]; i++)
            {
                int temp = UnityEngine.Random.Range(0, tempList.Count);

                if (webGL_Json.theme == 1)
                {
                    MonsterText_Theme1[i].text = tempList[temp];
                }
                else
                {
                    MonsterText_Theme2[i].text = tempList[temp];
                }
                check_Answer[i] = tempList[temp];
                tempList.RemoveAt(temp);
            }
        }
     
        ui_Manager.TextAlgnment(quiz, quizList[index].quiz);
        
        //if (type == Type.Truck)
        //{
        //    Invoke("SetQuizTime", 3f);
        //}
        //else
        //{
        //quiz.text = quizList[index].quiz;
        //}
    }
   
    void SetQuizTime()
    {
        quiz.text = quizList[quizNum].quiz;
    }

    //void Restart()
    //{
    //    quizNum = 0;
    //    StartQuiz(quizNum);
    //}
}
