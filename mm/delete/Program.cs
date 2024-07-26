#define REMOVEUSERFROMCHANNEL
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
enum EDeletions
{
    DiactivateUser,
    RemoveUserFromChannel
}
enum EParamsDiactivate
{
    Diactivate,
    DeletePermanently
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
    public static string GetDeactivateUsersURL()
    {
        string user_id = GetUserId();
        return "address/api/v4/users/" + user_id;
    }
    public static string GetChannelId()
    {
        return "idch";
    }
    public static string GetRemoveUserFromChannel()
    {
        string user_id = GetUserId();
        string channel_id = GetChannelId();
        return "address/api/v4/channels/" + channel_id + "/members/" + user_id;
    }
    public static string DeleteUserParamPerm(string param)
    {
        int paramLength = param.Length;
        if (paramLength != 0)
            param = param.Insert(paramLength, "&permanent=true");
        else
            param = param.Insert(0, "?permanent=true");
        return param;
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
    static async Task SendRequest(string url, string param, string token)
    {
        using var client = new HttpClient();
        using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url + param);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using HttpResponseMessage response = await client.SendAsync(requestMessage);
        PrintResponse(response);
    }
    static async Task Main()
    {
        string token = GetToken();
        string param = "";
        string url = "";
        List<EParamsDiactivate> array_of_params = new List<EParamsDiactivate>();
        EDeletions request_type = EDeletions.DiactivateUser;
#if DIACTIVATEUSER
        array_of_params.Add(EParamsDiactivate.Diactivate);
        url = GetDeactivateUsersURL();
#endif
#if DELETEUSER
        array_of_params.Add(EParamsDiactivate.DeletePermanently);
        url = GetDeactivateUsersURL();
#endif
#if REMOVEUSERFROMCHANNEL
        url = GetRemoveUserFromChannel();
#endif
        if (request_type == EDeletions.DiactivateUser)
            if (array_of_params.Contains(EParamsDiactivate.DeletePermanently))
                param = DeleteUserParamPerm(param);
        await SendRequest(url, param, token);
    }
}