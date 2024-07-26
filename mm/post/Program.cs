#define ADDUSERTOCHANNEL

using System;
using System.Data.Common;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

enum EPost
{
    AddUser,
    AddUserToTeam,
    CreateTeam,
    CreateChannel,
    AddUserToChannel
}


class Program
{
    public static string GetToken()
    {
        return "token";
    }
    public static string GetAddUserURL()
    {
        return "address/api/v4/users";
    }
    public static string GetTeamId()
    {
        return "teamid";
    }
    public static string GetUserId()
    {
        return "userid";
    }
    public static string GetChannelId()
    {
        return "channelid";
    }
    public static string GetCreateTeamURL()
    {
        return "address/api/v4/teams";
    }
    public static string GetAddToTeamURL()
    {
        string team_id = GetTeamId();
        return "address/api/v4/teams/" + team_id + "/members";
    }
    public static string GetCreateChannelURL()
    {
        return "address/api/v4/channels";
    }
    public static string GetAddUserToChannelURL()
    {
        string channel_id = GetChannelId();
        return "address/api/v4/channels/" + channel_id + "/members";
    }
    public static string GetAddUserJsonBody()
    {
        string email = "DepoGoga@example.com";
        string nu = "Depo";
        string ps = "password1225";
        string jsonBody = "{\"email\": \"" + email + "\", \"username\": \"" + nu + "\", \"password\": \"" + ps + "\"}";
        return jsonBody;
    }
    public static string GetCreateTeamJsonBody()
    {
        string name = "flowers"; //: "string",
        string display_name = "flowers";  //: "string",
        string type = "O"; //"string"
        string jsonBody = "{\"name\": \"" + name + "\", \"display_name\": \"" + display_name + "\", \"type\": \"" + type + "\"}";
        return jsonBody;
    }
    public static string GetAddToTeamJsonBody()
    {
        string team_id = GetTeamId();
        string user_id = GetUserId();
        string jsonBody = "{\"team_id\": \"" + team_id + "\", \"user_id\": \"" + user_id + "\"}";
        return jsonBody;
    }
    public static string GetCreateChannelJsonBody()
    {
        string team_id = "9reytkxbrjdmtxtkmrbwxkctjh"; //"//string
        string name = "flowers12";//: "string",
        string display_name = "MFFlowers"; //: "string",
        string purpose = "Main For Flowers"; //: "string",
        string header = "What is a channel header ? idk";//: "string",
        string type = "O";//: "string"
        string jsonBody = "{\"team_id\": \"" + team_id + "\", \"name\": \"" + name + "\", \"display_name\": \"" + display_name + "\", \"purpose\": \""
                             + purpose + "\", \"header\": \"" + header + "\", \"type\": \"" + type + "\"}";
        return jsonBody;
    }
    public static string GetAddUserToChannelJsonBody()
    {
        string user_id = GetUserId();
        string jsonBody = "{\"user_id\": \"" + user_id + "\"}";
        return jsonBody;
    }
    static async Task SendRequest(string url, string param, string token, string jsonBody)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(url, content);
        PrintResponse(response);
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
    static async Task Main()
    {
        string token = GetToken();
        string param = "";
        string url = "";
        string jsonBody = "";
#if ADDUSER
        url = GetAddUserURL();
        jsonBody = GetAddUserJsonBody();
#endif
#if ADDUSERTOTEAM
        url = GetAddToTeamURL();
        jsonBody = GetAddToTeamJsonBody();
#endif
#if CREATETEAM
        url = GetCreateTeamURL();
        jsonBody = GetCreateTeamJsonBody();
#endif
#if CREATECHANNEL
        url = GetCreateChannelURL();
        jsonBody = GetCreateChannelJsonBody();
#endif
#if ADDUSERTOCHANNEL
        url = GetAddUserToChannelURL();
        jsonBody = GetAddUserToChannelJsonBody();
#endif
        // #if REMOVEUSERFROMCHANNEL
        //         url = GetRemoveUserFromChannelURL();
        //         jsonBody = GetCreateChannelJsonBody();
        // #endif

        await SendRequest(url, param, token, jsonBody);
    }
}