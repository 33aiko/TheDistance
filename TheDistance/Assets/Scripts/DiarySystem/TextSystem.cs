using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class TextSystem : MonoBehaviour {

	public Diary d; 
    public string filename = null;

    public static Dictionary<string, List<string>> textDictionary = new Dictionary<string, List<string>>();
    public static Dictionary<string, bool> showDictionary = new Dictionary<string, bool>();
    static string textPath;

    // Use this for initialization
    public void HandAwake (int EorN) {
        textPath = Application.dataPath + "/Resources/Texts/";
        if (filename != null) { Construct(textPath + filename, EorN); };
	}

	void Start(){
		d.Initiate ();
//		d.gameObject.SetActive (false);
	}
	

    public static void Construct(string path, int EricNatalie)
    {
        StreamReader sr = new StreamReader(path, Encoding.UTF8);
        int EorN=0;
        string line;
        string name = null;
        List<string> contentList = new List<string>();
        while ((line = sr.ReadLine()) != null)
        {
            //skip empty line
            if (line.Equals("") || line.Equals(" "))
            {
                continue;
            }

            //read content and name
            if (line.StartsWith("START::"))
            {
                name = line.Substring(7);
                EorN = 0;
                if (name.StartsWith("Eric::"))
                {
                    EorN = 1;
                }
                else if(name.StartsWith("Nata::"))
                {
                    EorN = 2;
                }
                
            }
            else if (line.StartsWith("END::"))
            {
                if (name != null && EorN==EricNatalie)
                {
                    List<string> addList = new List<string>(contentList);
                    textDictionary.Add(name, addList);
                    showDictionary.Add(name, false);
                    name = null;
                    contentList.Clear();
                }
            }
            else
            {
                if (EorN == EricNatalie)
                {
                    contentList.Add(line);
                }
            }
        }
    }

    public static void Clear()
    {
        textDictionary.Clear();
        showDictionary.Clear();
    }
}
