using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace UnityLib.API
{
    public class GoogleSheet : MonoBehaviour
    {
        public event Action OnDataChanged;

        public string BaseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
        public string SheetID = "1_pF8x7nJlyLDTzhdiaXardRE0R5eo7WNwSLrnq2c7Tw";
        public string APIKey = "AIzaSyBYNS6lZWyycwivcMgEsyoOMEtdJsTsCDo";
        public string sheetName = "sheet1";
        public string range = "B2:K26";
        public string dateTimeRenderOption = "FORMATTED_STRING";
        public string majorDimension = "COLUMNS";

        public string TestQuery;
        public float monitorInterval = 30;

        public string cache { get; private set; }

        void Start()
        {
            StartCoroutine(StartMonitor());
        }

        IEnumerator StartMonitor()
        {
            while (true)
            {
                //print("looking for google sheet changes");
                Get(CreateRequest());
                yield return new WaitForSeconds(monitorInterval);
            }
        }

        public string CreateRequest()
        {
            var url = new StringBuilder(BaseURL);
            url.Append(SheetID);
            url.AppendFormat("/values/{0}", sheetName);
            url.AppendFormat("!{0}?", range);

            if (dateTimeRenderOption != "")
                AddParam(url, "dateTimeRenderOption", dateTimeRenderOption);

            if (majorDimension != "")
                AddParam(url, "majorDimension", majorDimension);

            url.AppendFormat("{0}={1}", "key", APIKey);

            return url.ToString();
        }

        StringBuilder AddParam(StringBuilder builder, string key, string value)
        {
            return builder.AppendFormat("{0}={1}&", key, value);
        }

        public void Test()
        {
            Get(@"https://sheets.googleapis.com/v4/spreadsheets/1_pF8x7nJlyLDTzhdiaXardRE0R5eo7WNwSLrnq2c7Tw/values/Sheet1!B3:K26?dateTimeRenderOption=FORMATTED_STRING&key=AIzaSyBYNS6lZWyycwivcMgEsyoOMEtdJsTsCDo&majorDimension=COLUMNS");
        }

        public void Get(string url)
        {
            StartCoroutine(StartGet(url));
        }

        IEnumerator StartGet(string url)
        {
            WWW www = new WWW(url);
            yield return www;

            if (www.error != null)
                Debug.Log(www.error);

            if (cache != www.text)
            {
                cache = www.text;
                if (OnDataChanged != null)
                    OnDataChanged();
            }
        }
    }
}