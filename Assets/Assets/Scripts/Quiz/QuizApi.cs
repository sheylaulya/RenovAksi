using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class QuizAPI : MonoBehaviour
{
    public string apiURL;

    public IEnumerator GetQuestions(System.Action<QuizQuestion[]> callback)
    {
        UnityWebRequest request =
            UnityWebRequest.Get(apiURL);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json =
                request.downloadHandler.text.Trim();

            json = "{\"items\":" + json + "}";

            QuestionList data =
                    JsonUtility.FromJson<QuestionList>(json);

            Debug.Log("data null? " + (data == null));
            Debug.Log("items null? " + (data.items == null));

            if (data != null && data.items != null)
            {
                Debug.Log("Jumlah soal: " + data.items.Length);

                callback?.Invoke(data.items);
            }
            else
            {
                Debug.LogError("Gagal parse JSON");
            }
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}