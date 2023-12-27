using System.Collections.Generic;
using System.Threading;

namespace Maniac.ManiacUtils;

internal class Var
{
	public static string server = "";

	public static int actor = 0;

	public static int BotsLogged = 0;

	public static List<(string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password)> BotInformations = new List<(string, int, string, string, bool, string, string)>();

	public static List<(string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password)> BotInformations2 = new List<(string, int, string, string, bool, string, string)>();

	public static string[] Bots = new string[0];

	public static string[] Proxy = new string[0];

	public static List<string> Proxies = new List<string>();

	public static List<string> WorkingProxies = new List<string>();

	public static List<string> FreshWorkingProxies = new List<string>();

	public static List<int> PetList = new List<int>();

	public static int cpm_builder = 0;

	public static int cpm_final = 0;

	public static int Threads = 0;

	public static List<Thread> ActiveThreads = new List<Thread>();

	public static bool ErrorMode = false;

	public static string ProxyUser = "";

	public static string ProxyPass = "";

	public static int ProxyAmount;

	public static bool scraping = false;

	public static string OldTitle = "";

	public static int RunningThreads { get; set; }
}
