using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebAPI : MonoBehaviour
{
    string cookie;
    [SerializeField] UnityEngine.UI.Button start;

    public static Reference callback;
    
    void Start()
    {
        cookie = "";
        GetData();
    }

    public void GetData()
    {
        StartCoroutine(IEGetData());
    }

    IEnumerator IEGetData()
    {
        start.interactable = false;

        using (UnityWebRequest uwr = new UnityWebRequest())
        {
            uwr.url = "https://localhost:44364/api/data";
            //uwr.url = "https://localhost:44364/api/weatherforecast";
            uwr.method = UnityWebRequest.kHttpVerbGET;

            uwr.SetRequestHeader("Cookie", cookie);

            /*
            byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"Email\": \"" + inputLoginEmail.text + "\",\"password\": \"" + inputLoginPassword.text + "\",\"remeberme\": false }");
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.SetRequestHandler("Content-Type", "application/json");
            */

            uwr.downloadHandler = new DownloadHandlerBuffer();

            yield return uwr.SendWebRequest();

            if(uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                callback = new Reference();
            }
            else
            {
                Debug.Log(uwr.downloadHandler.text);
                cookie = uwr.GetResponseHeader("Set-Cookie");
                callback = JsonToObject<Reference>(uwr.downloadHandler.text);
            }

            start.interactable = true;
        }
    }

    public static T JsonToObject<T>(string jsonText)
    {
        DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
        T obj = (T)s.ReadObject(ms);
        ms.Dispose();
        return obj;
    }
}

public struct Reference
{
    public int gm_GreyNum { get; set; }
    public int gm_BlackNum { get; set; }
    public int gm_Angle1 { get; set; }
    public int gm_Angle2 { get; set; }
    public int gm_Angle3 { get; set; }

    public float gm_UnionTime { get; set; }
    public float gm_FadedMusicTime { get; set; }

    public int ui_UniomNum { get; set; }
    public float ui_UnionFreezeTime { get; set; }
    public float ui_FreezeTimet1 { get; set; }
}