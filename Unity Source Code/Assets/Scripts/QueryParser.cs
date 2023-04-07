using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using UnityEngine.UI;
using TMPro;
using System;
using System.Security.Cryptography;

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

    public void Listen()
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
            //currentDatabase.CustomFetch(input.text, keys);
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
