#define SETSYSTEMROLE

using System;
using System.Data.Common;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
enum EPut
{
    UpdateUser,
    UpdateSystemLevelRole,
    UpdateTeamLevelRole,
    UpdateChannelLevelRole
}
enum ESystemLevelRole
{
    system_user,
    system_admin
}
enum ETeamLevelRole
{
    team_user,
    team_admin,
    both
}
enum EChannelLevelRole
{
    channel_user,
    channel_admin,
    both
}
class Program
{
    public static string GetToken()
    {
        return "token";
    }
    public static string GetUserUpdateURL()
    {
        string user_id = GetUserId();
        return "address/api/v4/users/" + user_id;
    }
    public static string GetSystemLevelRoleURL()
    {
        string user_id = GetUserId();
        return "address/api/v4/users/" + user_id;
    }
    public static string GetChannelLevelRoleURL()
    {
        string user_id = GetUserId();
        string channel_id = GetChannelId();
        return "address/api/v4/channels/" + channel_id + "/members/" + user_id + "/roles";
    }
    public static string GetTeamLevelRoleURL()
    {
        string team_id = GetTeamId();
        string user_id = GetUserId();
        return "address/api/v4/teams/" + team_id + "/members/" + user_id + "/roles";
    }
    public static string GetUserId()
    {
        return "userid";
    }
    public static string GetTeamId()
    {
        return "";
    }
    public static string GetChannelId()
    {
        return "";
    }
    static async Task SendRequest(string url, string param, string token, string jsonBody)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(url, content);
        PrintResponse(response);
    }
    public static string GetUpdateUserJsonBody()
    {
        string user_id = GetUserId();
        string email = "newuserafterput@example.com";
        string un = "newuserafterput";
        string ps = "newuserafterput";
        string jsonBody = "{\"id\": \"" + user_id + "\", \"email\": \"" + email + "\", \"username\": \"" + un + "\", \"password\": \"" + ps + "\"}";
        return jsonBody;
    }
    public static string GetSystemRoleJsonBody(ESystemLevelRole eSystemLevelRole)
    {
        string role = "";
        if (eSystemLevelRole == ESystemLevelRole.system_user)
            role = "system_user";
        if (eSystemLevelRole == ESystemLevelRole.system_admin)
            role = "system_admin";
        string jsonBody = "{\"role\": \"" + role + "\"}";
        return jsonBody;
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
        string jsonBody = "";
#if UPDATEUSER
        EPut request_type = EPut.UpdateUser;
        jsonBody = GetUpdateUserJsonBody();
#endif
#if SETSYSTEMROLE
        EPut request_type = EPut.UpdateSystemLevelRole;
        jsonBody = GetUpdateUserJsonBody();
#endif
#if SETTEAMROLE
#endif
#if SETCHANNELROLE
#endif
        if (request_type == EPut.UpdateUser)
            url = GetUserUpdateURL();
        if (request_type == EPut.UpdateSystemLevelRole)
            url = GetSystemLevelRoleURL();
        if (request_type == EPut.UpdateTeamLevelRole)
            url = GetTeamLevelRoleURL();
        if (request_type == EPut.UpdateChannelLevelRole)
            url = GetChannelLevelRoleURL();
        await SendRequest(url, param, token, jsonBody);
    }
}