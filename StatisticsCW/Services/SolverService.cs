using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Genbox.WolframAlpha;
using Genbox.WolframAlpha.Objects;
using StatisticsCW.Enum;

namespace StatisticsCW.Services;

public class SolverService
{
    private WolframAlphaClient _client;
    
    public SolverService(string appId)
    {
        _client = new WolframAlphaClient(appId);
    }

    public Image Image(string query, PodId podId = PodId.Input)
    {
        var response = _client.FullResultAsync(query).Result;
        
        foreach (var pod in response.Pods)
        {
            if (pod.Id.Equals(podId.ToString()))
            {
                return pod.SubPods[0].Image;
            }
        }

        if (response.Pods.Count == 0) throw new ArgumentException("Wrong Query! 0 answers.");
        
        return response.Pods[0].SubPods[0].Image;
    }
    
    public string PlainText(string query, PodId podId = PodId.Input, string title = "Result")
    {
        var response = _client.FullResultAsync(query).Result;
        
        foreach (var pod in response.Pods)
        {
            if (pod.Id.Equals(podId.ToString()) && pod.Title.Equals(title))
            {
                return pod.SubPods[0].Plaintext;
            }
        }

        if (response.Pods.Count == 0) throw new ArgumentException("Wrong Query! 0 answers.");
        
        return response.Pods[0].SubPods[0].Plaintext;
    }

    public string ShortAnswer(string query)
    {
        return _client.ShortAnswerAsync(query).Result;
    }

    public string OperandFromResult(string result)
    {
        var split = result.Split('=');

        return split[^1].Trim();
    }
}