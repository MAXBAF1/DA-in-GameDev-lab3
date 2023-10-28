using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class GSParser : MonoBehaviour
{
    private const string Uri =
        "https://sheets.googleapis.com/v4/spreadsheets/1Zl0liKO-8ItIr42ZbGk0uwqAZKKmZbynLrQ6nrbrmyc/values/Лист2?key=AIzaSyDz9MHdX-tqBz3QH2XlvOIwXNjs3QDGKrs";

    public List<float[]> Data { get; } = new();

    public IEnumerator ParseGS()
    {
        var currResp = UnityWebRequest.Get(Uri);

        yield return currResp.SendWebRequest();
        var rawResp = currResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);

        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList.Skip(1);
            var count = 0;
            var arr = new float[3];

            foreach (var row in selectRow)
            {
                if (float.TryParse(row, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out var number))
                {
                    arr[count] = number;
                    count++;
                }

                if (count == 3) Data.Add(arr);
            }
        }
    }
}