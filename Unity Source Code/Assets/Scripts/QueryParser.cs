using UnityEngine;
using Database;
using TMPro;
using System;

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
        try
        {
            keys = currentDatabase.GetKeys(input.text);

            var result = await currentDatabase.CustomFetch(input.text, keys);
            graph.transform.GetComponent<GraphManager>().BuildGraph(result);
        }
        catch (Exception e)
        {     
            Debug.LogException(e);
            input.text = "";
            input.placeholder.GetComponent<TMP_Text>().text = "PLEASE ENTER VALID QUERY";
            input.placeholder.GetComponent<TMP_Text>().color = Color.red;
        }
  
    }

}
