using System;
using System.Threading.Tasks;
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

    [Obsolete("Use ImageAsync instead")]
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
    
    public async Task<Image> ImageAsync(string query, PodId podId = PodId.Input)
    {
        var response = await _client.FullResultAsync(query);

        foreach (var pod in response.Pods)
        {
            if (pod.Id.Equals(podId.ToString()))
            {
                return pod.SubPods[0].Image;
            }
        }

        if (response.Pods.Count == 0)
            throw new ArgumentException("Wrong Query! 0 answers.");

        return response.Pods[0].SubPods[0].Image;
    }

    [Obsolete("Use PlainTextAsync instead")]
    public string PlainText(string query, PodId podId = PodId.Input)
    {
        var response = _client.FullResultAsync(query).Result;
        var title = PodTitle.Get(podId);

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

    public async Task<string> PlainTextAsync(string query, PodId podId = PodId.Input)
    {
        var response = await _client.FullResultAsync(query);
        var title = PodTitle.Get(podId);

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

    [Obsolete("Use ShortAnswerAsync instead")]
    public string ShortAnswer(string query)
    {
        return _client.ShortAnswerAsync(query).Result;
    }
    
    public async Task<string> ShortAnswerAsync(string query)
    {
        return await _client.ShortAnswerAsync(query);
    }

    public string OperandFromResult(string result)
    {
        var split = result.Split('=');

        return split[^1].Trim();
    }
}