using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SimpleJSON;

public class GameSystem : MonoBehaviour
{

    public TextAsset asset;

    // Start is called before the first frame update
    void Start()
    {

        var jsonstring = asset.text;

        var obj = JSON.Parse(jsonstring);

        print(obj.Count  + " | "+ obj[obj.Count-1]);
    }

}

