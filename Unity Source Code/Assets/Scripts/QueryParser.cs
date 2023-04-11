using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using UnityEngine.UI;
using TMPro;
using System;
using System.Security.Cryptography;
using GraphFoundation;

public class QueryParser : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField input;
    public Neo4jDatabase currentDatabase;
    private string[] keys;
    private GameObject graph;
    void Start()
    {
        graph = GameObject.FindGameObjectWithTag("Graph");
        input = gameObject.GetComponent<TMP_InputField>();
        currentDatabase = GameObject.FindGameObjectWithTag("NodeSpawner").GetComponent<Neo4jConnection>().currentDatabase;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public async void Listen()
    {
        if (input.text == "") 
        {

            input.placeholder.GetComponent<TMP_Text>().text = "PLEASE ENTER VALID QUERY";
            input.placeholder.GetComponent<TMP_Text>().color = Color.red;
        }
        else
        {
            string keyString = input.text.ToLower().Split(new string[] { "return" }, StringSplitOptions.None)[1];
            keys = keyString.Split(new string[] { "," }, StringSplitOptions.None);
            foreach (string key in keys)
            {
                key.Trim();
            }
            ClearGraph();
            var x = await currentDatabase.CustomFetch(input.text, keys);
            for (int index = 0; index < x.Count; index++)
            {

                int id = (int)x[index].Item1.Id; // get Node ID (elementID puts some weird pre-fix in front of it, stringparsing could solve this)
                var labels = x[index].Item1.Labels; // get Node labels
                var _properties = x[index].Item1.Properties; // get Node Properties. Since its of type Dictionary one needs to iterate over the key-value pairs

                Debug.Log(id);
            }
        }
    }

    public void ClearGraph()
    {
        while (graph.transform.childCount > 0)
        {
            DestroyImmediate(graph.transform.GetChild(0).gameObject);
        }
    }
}
