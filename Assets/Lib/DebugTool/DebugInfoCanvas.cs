using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugInfoCanvas : DebugInfo
{
    public Text propertyText;
    public Text logText;

    protected override void DrawInfos()
    {
        if (propertyText == null)
            return;

        propertyText.text = "";

        foreach (var pair in infos)
            propertyText.text += pair.Key + pair.Value.content.ToString() + "\n";        
    }

    protected override void DrawLogs()
    {
        logText.text = "";
        for (int i = nextIndex; i != logIndex;)
        {
            i = i + 1 == logs.Length ? 0 : i + 1;
            var log = logs[i];

            if (log == null)
                continue;
            logText.text += log.message + "\n";
            if (log.logType == LogType.Exception)
                logText.text += @"<color=red>" + log.stackTrace + @"</color>";
        }
    }
}
