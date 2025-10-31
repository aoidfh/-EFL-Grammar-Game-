using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum State
{
    Play,
    Pause
}

public class UI_Manager : MonoBehaviour
{   
    [Header("GameState")]
    public State state;
    public float gamePlayTime;

    [Header("GameManager")]
    public WebGL_Json webGL_Json;
    public SkiQuiz_Manager skiQuiz_Manager;
    public Bubble_QuizManager bubble_QuizManager;    
    public Spawn_Manager spawn_Manager;
    public QuizManager quizManager;
    public Meteor_SpawnManager spawn;
    public Bubble_QuizManager bubble;    
    public GameStart animStart;
    public Send_Data send_Data;
    public MoveManager truckMove_Manager;

    [Header("GameObject")]
    public GameObject canvas;
    public GameObject[] back_Ground = new GameObject[2];
    public GameObject[] pub_Title = new GameObject[2];
    public GameObject[] Anim_Title = new GameObject[2];
    public GameObject[] operate_Obj;
    public GameObject start_Obj; 
    public GameObject title_Obj;
    public GameObject end_Obj;
    
    public GameObject[] info_Guide = new GameObject[2];   

    public GameObject info_Btn;
    public GameObject success_Panel;
    public GameObject fail_Panel;
    public GameObject game_Name;
    public GameObject game_Lesson;
    public GameObject backBtn;   
    public GameObject[] backBnt_Title;
    public GameObject[] Mobile_Key;
    public GameObject fullScreen_Btn;
    public GameObject pup_Count;
    
    [Header("GameBtn")]
    public Button start_Btn;
    public Button exit_Btn;

    [Header("GameSlider")]
    public Slider time_Sldier;

    [Header("Text")]
    public Text score_EndTitle;
    public Text Score_parsent;
    public Text answerText;
    public Text title;
    public Text lesson;
    public Text totalQuiz_Count;
    public Text currentQuiz_Count;
    public Text time;
    [Header("Action")]
    public Action isGameStart;
    public Action isNextQuiz;
    public Action isEndGame;
    public Action isRestart;
    public Action isFail;
    public Action GameSet;

    [Header("Animator")]
    public Animator clock;   

    public int isAnswer;
    public bool gameTimeCoroutine;
    public bool mobile = false;
    public bool count_Audio =false;
    
    public List<string> answerList = new List<string>();
    public int font_Size;
    string font_Color = "blue";
    public int theme_Index;

    public int quiz_Count =0;
    public int correct_Score =0;
    public int inCorrect_Score = 0;
    public int estimationSec = 0;

    public AudioSource audio_Click;
    public AudioSource BGMAudio_Source;   

    public GameObject pup_Guide;
    public GameObject[] eff_Particle;
    private void Start()
    {             
        //isGameStart += GameStart;
        isNextQuiz += NextQuiz;
        isEndGame += EndGame;
        //isRestart += SetGameStart;
        PlatformPC();
    }

    private void Update()
    {      
        if (pup_Guide.activeSelf || backBtn.activeSelf)
        {
            for (int i = 0; i < eff_Particle.Length; i++)
            {
                eff_Particle[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < eff_Particle.Length; i++)
            {
                eff_Particle[i].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isEndGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetGameStart(1);
        }
    }

    public void UI_ManagerStart()
    {
        TitleText(title);
        lesson.text = webGL_Json.lesson;                
        SetGameStart(webGL_Json.theme);

        if (skiQuiz_Manager != null)
        {
            skiQuiz_Manager.Quiz_Start();
        }

        if (bubble_QuizManager != null)
        {
            bubble_QuizManager.Bubble_Start();
            spawn_Manager.Spawn_ManagerStart();
            spawn_Manager.Start_SetQuiz();
        }

        if (quizManager != null)
        {
            quizManager.QuizManager_Start();
        }

        time_Sldier.maxValue = gamePlayTime;
        time.text = time_Sldier.maxValue.ToString();

        canvas.GetComponent<AudioSource>().Play();
        exit_Btn.gameObject.SetActive(true);
    }

    public void GameTimeStartCoroutine()
    {
        if (!gameTimeCoroutine)        
            gameTimeCoroutine = true;
        else        
            gameTimeCoroutine = false;

        estimationSec = (int)time_Sldier.maxValue - (int)time_Sldier.value;
        time_Sldier.value = gamePlayTime;
        
        if (gameTimeCoroutine)
        {
            StartCoroutine("GameTime");            
        }
        else
        {
            StopCoroutine("GameTime");           
        }
    }  

    IEnumerator GameTime()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.1f);

        while (time_Sldier.value > 0)
        {
            time_Sldier.value -= 0.1f;
            time.text = time_Sldier.value.ToString("0");
            if (int.Parse(time.text) <= 9)
            {
                time.text = "0" + time.text;
            }

            yield return seconds;
        }
      
        time_Sldier.value = gamePlayTime;
        isFail();

        GameTimeStartCoroutine();
    }

    public void Click(int index)
    {
        if (state == State.Play)
        {
            GameTimeStartCoroutine();
        }

        string answer = quizManager.check_Answer[index];
        string student_answer = quizManager.quizList[quizManager.quizNum].answer;

        if (answer == student_answer && state == State.Play)
        {
            state = State.Pause;
            
            answerText.text = answerList[quizManager.quizNum];

            truckMove_Manager.truckAudio_Source.clip = truckMove_Manager.truck_Audio[0];
            truckMove_Manager.truckAudio_Source.Play();
            //성공          
            correct_Score++;
            isAnswer = index;
            success_Panel.SetActive(true);
            Invoke("NextQuizBtn", 4f);

            Application.ExternalCall("Set_Struct", webGL_Json.projectAuthoringCode, quizManager.quizNum+1, quizManager.quizNum + 1, 1,
            answer, student_answer, "Y", "N", webGL_Json.quiz_Count, estimationSec, 1);
        }
        else if (answer != student_answer && state == State.Play)           
        {
            state = State.Pause;            
            isAnswer = index;
            answerText.text = answerList[quizManager.quizNum];
            fail_Panel.SetActive(true);

            truckMove_Manager.truckAudio_Source.clip = truckMove_Manager.truck_Audio[1];
            truckMove_Manager.truckAudio_Source.Play();
            //실패               
            Invoke("NextQuizBtn", 4f);

            Application.ExternalCall("Set_Struct", webGL_Json.projectAuthoringCode, quizManager.quizNum+1, quizManager.quizNum + 1, 1,
            answer, student_answer, "N", "N", webGL_Json.quiz_Count, estimationSec, 1);
        }   
    }

    public void SetGameStart(int theme)
    {
        theme_Index = theme;

        //GameObject.Find("Game_Lesson").transform.GetChild(0).GetComponent<Text>().text = webGL_Json.;
       
        GameSet?.Invoke();
        if (theme_Index == 1)
        {
            pub_Title[0].SetActive(true);
            back_Ground[0].SetActive(true);

            pub_Title[1].SetActive(false);
            back_Ground[1].SetActive(false);
        }
        else
        {
            pub_Title[1].SetActive(true);
            back_Ground[1].SetActive(true);

            pub_Title[0].SetActive(false);
            back_Ground[0].SetActive(false);
        }
       
        //타이틀 화면 켜기
        title_Obj.SetActive(true);
        game_Name.SetActive(true);
        game_Lesson.SetActive(true);

        start_Obj.SetActive(false);
        end_Obj.SetActive(false);

        //게임 가이드 띄움
        Invoke("SetInfo_Panel", 3f);
        //Invoke("SetInfo_Panel", 1f);

        //스코어 초기화
        time_Sldier.value = gamePlayTime;

        //for (int i = 0; i < start_Star.Length; i++)
        //{
        //    start_Star[i].SetActive(false);
        //}        
    }

    void SetInfo_Panel()
    {
        if (webGL_Json.level == 1)
        {
            info_Guide[0].SetActive(true);
        }
        else
        {
            info_Guide[0].SetActive(true);            
        }

        game_Name.SetActive(false);
        game_Lesson.SetActive(false);
    }

    public void GameStart()
    {
        Count_Audio();
        BGMAudio_Source.volume = 0.4f;

        if (Time.timeScale == 1)
        {
            if (webGL_Json.level == 1)
            {
                info_Guide[0].SetActive(false);
            }
            else
            {
                info_Guide[0].SetActive(false);
            }
            
            title_Obj.SetActive(false);
            start_Obj.SetActive(true);

            if (animStart != null)
            {                
                animStart.AnimStart();
            }

            state = State.Play;


            if (spawn != null)
            {
                //spawn.SetWallTime();
            }

            if (quizManager != null)
            {
                quizManager.StartQuiz(quizManager.quizNum);
            }
        }
        else
        {
            Time.timeScale = 1;
            if (webGL_Json.level == 1)
            {
                info_Guide[0].SetActive(false);
            }
            else
            {
                info_Guide[0].SetActive(false);
            }
        }     
    }    
    public void InfoBtn()
    {
        Time.timeScale = 0;
      
        if (webGL_Json.level == 1)
        {
            info_Guide[0].SetActive(true);
        }
        else
        {
            info_Guide[0].SetActive(true);
        }

        Count_Audio();
    }

    public void NextQuizBtn()
    {
        isNextQuiz();       
    }

    public void NextQuiz()
    {        
        state = State.Play;
        
        if (quizManager.quizNum + 1 >= quizManager.quizList.Count)
        {
            isEndGame();
          
            return;
        }
           
        quizManager.StartQuiz(quizManager.quizNum + 1);

        if (success_Panel.activeSelf)
        {
            success_Panel.SetActive(false);
        }
        else
        {
            fail_Panel.SetActive(false);
        }
    }
    
    public void EndGame()
    {
        state = State.Pause;

        GameTimeStartCoroutine();

        if (success_Panel.activeSelf)
        {
            success_Panel.SetActive(false);
        }
        else
        {
            fail_Panel.SetActive(false);
        }

        exit_Btn.gameObject.SetActive(false);
        start_Obj.SetActive(false);
        end_Obj.SetActive(true);

        send_Data.Send_Student_Lesson_Result();
    }
 
    public void TitleText(Text titleText)
    {
        try
        {
            string[] str = webGL_Json.title.Split('^');
            if (str.Length >= 2)
            {
                titleText.text = str[0] + "\n" + str[1];
            }
            else
            {
                titleText.text = webGL_Json.title;
            }
        }
        catch (Exception)
        {
            titleText.text = webGL_Json.title;
        }
    
    }
    public void BackBtnTitle()
    {
        Count_Audio();

        if (webGL_Json.level == 2)
        {
            backBnt_Title[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 60);
            backBnt_Title[1].SetActive(false);
        }
    }


    //텍스트 줄바꿈시 텍스트 위치조정
    //게임 종류 버블:0, 나머지:1
    public void TextAlgnment(Text quiztext,string text)
    {       
        try
        {
            string[] str = text.Split('^');

            if (str.Length >= 2)
            {
                quiztext.alignment = TextAnchor.MiddleLeft;
                quiztext.verticalOverflow = VerticalWrapMode.Truncate;
                quiztext.text = str[0] + "\n" + str[1];           
            }
            else
            {
                quiztext.alignment = TextAnchor.MiddleCenter;
                quiztext.verticalOverflow = VerticalWrapMode.Truncate;
                quiztext.text = text;
            }                      
        }
        catch (Exception)
        {
            quiztext.alignment = TextAnchor.MiddleCenter;
            quiztext.verticalOverflow = VerticalWrapMode.Truncate;
            quiztext.text = text;
        }                                
    }
    public void Restart()
    {
        isRestart();               
    }

    public void QuestionAnswerList(string tempQuestion, string answer)
    {
        string question = "";      
        try
        {            
            string[] str = tempQuestion.Split('^');

            if (str.Length >= 2)
            {
                tempQuestion = str[0] + "\n" + str[1];
            }
        }
        catch (Exception)
        {}

        try
        {
            int firstIndexOf = tempQuestion.LastIndexOf('[');
            question = tempQuestion.Substring(0, firstIndexOf);
            question += "<color=" + font_Color + ">";
            int secondIndexOf = tempQuestion.LastIndexOf(']');

            question += tempQuestion.Substring(firstIndexOf + 1, secondIndexOf - firstIndexOf - 1);
            question += "</color>";
            question += tempQuestion.Substring(secondIndexOf + 1);
        }
        catch (Exception)
        {
            question = "<color=" + font_Color + ">" +answer +"</color>"; 
        }
              
        answerList.Add(question);
    }
    public void QuestionAnswerList()
    {
        string question = "";
      
        //foreach(Bubble item in webGL_Json.bubble_List.data)
        //{
        //    question = "";
        //    question = item.Answer;
           
        //    answerList.Add(question);
        //}

        for (int i = 0; i < webGL_Json.bubble_List.data.Count; i++)
        {
            question = "";
            question = webGL_Json.bubble_List.data[i].Answer;
          
            answerList.Add(question);
        }
    }
    public string QuestionBlank(string tempQuestion)
    {
        string question = "";
        try
        {
            int firstIndexOf = tempQuestion.LastIndexOf('[');
            question = tempQuestion.Substring(0, firstIndexOf);
            int secondIndexOf = tempQuestion.LastIndexOf(']');

            question += "<b>____</b>";

            question += tempQuestion.Substring(secondIndexOf + 1);
        }
        catch (Exception)
        {
            question = tempQuestion;            
        }
       

        return question;
    }

    public void GameTimeScale()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;         
                           
            BackBtnTitle();
        }
        else
        {
            Time.timeScale = 1;            
        }
    }
 
    public void PlatformPC()
    {
        operate_Obj[0].SetActive(true);
        if (skiQuiz_Manager != null)
        {
            skiQuiz_Manager.quiz_Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 84);
        }
        if (spawn != null)
        {
            spawn.quiz_Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(640, 84);
        }       
    }

    public void PlatformMobile()
    {
        mobile = true;

        //if (skiQuiz_Manager != null)
        //{
        //    skiQuiz_Manager.quiz_Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-160, 84);
        //}
        //if (spawn != null)
        //{
        //    spawn.quiz_Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 84);
        //}

        operate_Obj[1].SetActive(true);        
        Invoke("KeyActive", 2f);
    }

    public void KeyActive()
    {
        if (Mobile_Key.Length >= 2)
        {          
            if (webGL_Json.theme == 1)
            {
                Mobile_Key[0].SetActive(true);
                Mobile_Key[1].SetActive(false);
            }
            else
            {
                Mobile_Key[1].SetActive(true);
                Mobile_Key[0].SetActive(false);                
            }        
        }
    }

    public void ClickAudio_Play()
    {
        audio_Click.Play();      
    }

    public void Count_Audio()
    {
        if (count_Audio)
        {
            count_Audio = false;
            pup_Count.GetComponent<AudioSource>().Pause();
        }
        else
        {
            count_Audio = true;
            pup_Count.GetComponent<AudioSource>().Play();
        }
    }
    
    public void GameExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }   
}
