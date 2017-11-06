using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class TextSystem : MonoBehaviour {
    public string filename = null;

    public static Dictionary<string, List<string>> textDictionary = new Dictionary<string, List<string>>();
    public static Dictionary<string, bool> showDictionary = new Dictionary<string, bool>();
    static string textPath;

    // Use this for initialization
    void Awake () {
        textPath = Application.dataPath + "/Resources/Texts/";
        if (filename != null) { Construct(textPath + filename); };
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Construct(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.UTF8);
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
            }
            else if (line.StartsWith("END::"))
            {
                if (name != null)
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
                contentList.Add(line);
            }
        }
    }

    public static void Clear()
    {
        textDictionary.Clear();
        showDictionary.Clear();
    }
}
