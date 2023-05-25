using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Genbox.WolframAlpha;
using Genbox.WolframAlpha.Objects;

namespace StatisticsCW.Services;

public class SolverService
{
    private WolframAlphaClient _client;
    
    public SolverService(string appId)
    {
        _client = new WolframAlphaClient(appId);
    }

    public List<Image> Images(string query)
    {
        var results = _client.FullResultAsync(query).Result;

        var images = new List<Image>();
        
        foreach (var pod in results.Pods)
        {
            images.AddRange(pod.SubPods.Select(subPod => subPod.Image));
        }

        return images;
    }
}