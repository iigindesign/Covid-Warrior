using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class Question : MonoBehaviour
{
    private QuestionData _questionData;
    private GameObject player;
    private bool generating;

    void Start()
    {
      player = GameObject.Find("player");
      _questionData = new QuestionData();
    }

    public void generateQuestion(string type, string level) {
      if (!generating) {
        string ans_correct = player.GetComponent<Player>().getCorrect();
        _questionData.type = type;
        _questionData.level = level;
        StartCoroutine(Download(_questionData.type, _questionData.level, ans_correct, result => {
          _questionData = result;
          Debug.Log(_questionData.Stringify());
        }));
      }
    }

    public void answeredQuestion(int correct) {
      //>0 indicates answered correct, <0 incorrect
      player.GetComponent<Player>().answered(_questionData.id, correct);
      string info = "{\"questionID\":\"" + _questionData.id +"\", \"correct\":" + correct + "}";
      StartCoroutine(Answer(info, result => {
        Debug.Log(result);
      }));
    }

    public string getQuestion() {
      return _questionData.question;
    }

    public string getA() {
      return _questionData.a;
    }

    public string getB() {
      return _questionData.b;
    }

    public string getC() {
      return _questionData.c;
    }

    public string getD() {
      return _questionData.d;
    }

    public string correctAnswer() {
      return _questionData.correct;
    }

    IEnumerator Download(string type, string level, string correct, System.Action<QuestionData> callback = null)
    {
      generating = true;
      string url;
      if (correct != null) {
        url = "https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/getQuestion?select_type=" + type + "&select_level=" + level + "&answered=" + correct;
      }
      else {
        url = "https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/getQuestion?select_type=" + type + "&select_level=" + level;
      }
      using (UnityWebRequest request = UnityWebRequest.Get(url))
      {
          yield return request.SendWebRequest();

          if (request.isNetworkError || request.isHttpError)
          {
              Debug.Log(request.error);
              if (callback != null)
              {
                  callback.Invoke(null);
              }
          }
          else
          {
            if (request.downloadHandler.text == "null")
            {
              callback.Invoke(null);
            }
            else if (callback != null)
            {
                callback.Invoke(QuestionData.Parse(request.downloadHandler.text.Substring(1, request.downloadHandler.text.Length - 2)));
            }
          }
      }
      generating = false;
    }

    IEnumerator Answer(string info, System.Action<string> callback = null)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(info);
        using (UnityWebRequest request = UnityWebRequest.Put("https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/answered", bodyRaw))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if(callback != null)
                {
                    callback.Invoke("false");
                }
            }
            else
            {
                if(callback != null)
                {
                    callback.Invoke(request.downloadHandler.text);
                }
            }
        }
    }
}
