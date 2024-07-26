#define GETTEAMS

using System;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
enum EGet
{
    GetUsers,
    GetActiveUsersCountWithBots,
    GetUserInfo,
    GetChannels,
    GetTeams
}

enum EGetUsersParams
{
    active,
    inactive
}
class Program
{
    public static string GetToken()
    {
        return "token";
    }
    public static string GetUserId()
    {
        return "token";
    }
    public static string GetUsersURL()
    {
        return "address/api/v4/users";
    }
    public static string GetChannelsURL()
    {
        return "address/api/v4/channels";
    }
    public static string GetUsersCountURL()
    {
        return "address/api/v4/users/stats";
    }
    public static string GetUserInfoURL()
    {
        string user_id = GetUserId();
        return "address/api/v4/users/" + user_id;
    }
    public static string GetTeamsURL()
    {
        return "address/api/v4/teams";
    }

    public static string GetUsersParamActiv(string param)
    {
        int paramLength = param.Length;
        if (paramLength != 0)
            param = param.Insert(paramLength, "&active=true");
        else
            param = param.Insert(0, "?active=true");
        return param;
    }
    public static string GetUsersParamInactiv(string param)
    {
        int paramLength = param.Length;
        if (paramLength != 0)
            param = param.Insert(paramLength, "&inactive=true");
        else
            param = param.Insert(0, "?inactive=true");
        return param;
    }
    static async Task SendRequest(string url, string param, string token)
    {
        using var client = new HttpClient();
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url + param))
        {
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using HttpResponseMessage response = await client.SendAsync(requestMessage);
            PrintResponse(response);
        }
    }
    static async void PrintResponse(HttpResponseMessage response)
    {
        Console.WriteLine($"Status: {response.StatusCode}\n");
        //заголовки
        Console.WriteLine("Headers");
        foreach (var header in response.Headers)
        {
            Console.Write($"{header.Key}:");
            foreach (var headerValue in header.Value)
            {
                Console.WriteLine(headerValue);
            }
        }
        // содержимое ответа
        Console.WriteLine("\nContent");
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
    }
    static async Task Main(string[] args)
    {
        string url = "";
        string param = "";
        string token = GetToken();

        List<EGetUsersParams> array_of_params = new List<EGetUsersParams>();
#if GETUSERS
        //оставляю только 2 параметра?
        EGet request_type = EGet.GetUsers;
        array_of_params.Add(EGetUsersParams.inactive);
#endif
#if GETCOUNT
        //что вообще по параметрам ?
        EGet request_type = EGet.GetActiveUsersCountWithBots;
#endif
#if GETUSERINFO
        EGet request_type = EGet.GetUserInfo;
#endif
#if GETCHANNELS
        EGet request_type = EGet.GetChannels;
        url = GetChannelsURL();
#endif
#if GETTEAMS
    EGet request_type = EGet.GetTeams;
    url = GetTeamsURL();
#endif
        if (request_type == EGet.GetUsers)
        {
            url = GetUsersURL();
            if (array_of_params.Contains(EGetUsersParams.active))
                param = GetUsersParamActiv(param);
            else
                param = GetUsersParamInactiv(param);
        }
        else if (request_type == EGet.GetActiveUsersCountWithBots)
        {
            url = GetUsersCountURL();
        }
        else if (request_type == EGet.GetUserInfo)
        {
            url = GetUserInfoURL();
        }
        await SendRequest(url, param, token);
    }
}