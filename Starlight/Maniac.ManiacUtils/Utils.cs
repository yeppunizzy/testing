using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Maniac.AMF;
using Maniac.Features;

namespace Maniac.ManiacUtils;

internal class Utils
{
	public static Random RandomString = new Random();

	public static string RandomizedString(int Length)
	{
		return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz", Length)
			select s[RandomString.Next(s.Length)]).ToArray());
	}

	public static void CalculateCPM()
	{
		while (true)
		{
			Var.cpm_builder = 0;
			Thread.Sleep(1000);
			Var.cpm_final = Var.cpm_builder;
			Var.cpm_final *= 60;
		}
	}

	public static void KillThreads()
	{
		while (Var.RunningThreads > 0)
		{
			try
			{
				if (Var.RunningThreads <= 0)
				{
					continue;
				}
				foreach (Thread activeThread in Var.ActiveThreads)
				{
					activeThread.Abort();
					Var.RunningThreads--;
				}
			}
			catch
			{
			}
		}
	}

	public static void CLManiac(string txt)
	{
		string text = "[Starlight] " + txt;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("{0," + (Console.WindowWidth / 2 + text.Length / 2) + "}", text);
	}

	public static void CLDebug(string txt)
	{
		string text = "[DEBUG] " + txt;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("{0," + (Console.WindowWidth / 2 + text.Length / 2) + "}", text);
	}

	public static void CLCentered(string txt)
	{
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write("{0," + (Console.WindowWidth / 2 + txt.Length / 2) + "}", txt);
	}

	public static void CLWatermark()
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		string[] source = "\r\n\r\n\r\n\r\n███████╗████████╗ █████╗ ██████╗ ██╗     ██╗ ██████╗ ██╗  ██╗████████╗\r\n██╔════╝╚══██╔══╝██╔══██╗██╔══██╗██║     ██║██╔════╝ ██║  ██║╚══██╔══╝\r\n███████╗   ██║   ███████║██████╔╝██║     ██║██║  ███╗███████║   ██║   \r\n╚════██║   ██║   ██╔══██║██╔══██╗██║     ██║██║   ██║██╔══██║   ██║   \r\n███████║   ██║   ██║  ██║██║  ██║███████╗██║╚██████╔╝██║  ██║   ██║   \r\n╚══════╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝   \r\n                                                                      \r\n\r\n\r\n                                                        ".Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
		int num = source.Max((string line) => line.Length);
		string leadingSpaces = new string(' ', (Console.WindowWidth - num) / 2);
		Console.WriteLine(string.Join(Environment.NewLine, source.Select((string line) => leadingSpaces + line)));
	}

	public static string Base64Encode(string plainText)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
	}

	public static string Base64Decode(string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	public static void ScrapeProxies(int timeout)
	{
		Var.Proxies.Clear();
		Console.Clear();
		CLWatermark();
		CLManiac("Proxy list path (drag and drop): ");
		string path = Console.ReadLine();
		if (File.Exists(path))
		{
			string[] array = File.ReadAllLines(path);
			Var.Proxies.AddRange(array);
			CLCentered("\n");
			CLManiac("Successfully loaded " + array.Length + " Proxies!\n");
			string[] array2 = array;
			foreach (string text in array2)
			{
				CLCentered("Successfully added -> " + text + "\n");
			}
			CLCentered("Finished Loading " + array.Length + " proxies\n");
			Thread.Sleep(3000);
		}
		else
		{
			StreamReader streamReader = new StreamReader(new WebClient
			{
				Proxy = null
			}.OpenRead("https://api.proxyscrape.com/v2/?request=getproxies&protocol=http&timeout=" + timeout + "&country=all&ssl=all&anonymity=all"));
			while (!streamReader.EndOfStream)
			{
				Var.Proxies.Add(streamReader.ReadLine());
			}
		}
	}

	public static void RefreshTickets()
	{
		Var.BotsLogged = 0;
		string oldTitle = Console.Title;
		Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
		{
			while (true)
			{
				int index = new Random().Next(Var.WorkingProxies.Count);
				try
				{
					(string, int, string, string, bool, string, string) item = BotUtils.LoginForBotsAuto(bot.Username, bot.Password, Var.WorkingProxies[index]);
					if (item.Item5)
					{
						Var.BotsLogged++;
						Console.Title = oldTitle + " | Bots loaded: " + Var.BotsLogged;
						CLCentered("Successfully logged into -> " + bot.Username + "\n");
						Var.BotInformations2.Add(item);
					}
					break;
				}
				catch
				{
					CLCentered("[Error] Proxy didnt work -> " + Var.WorkingProxies[index] + " retrying!\n");
				}
			}
		});
		Var.BotInformations.Clear();
		Var.BotInformations.AddRange(Var.BotInformations2);
		Var.BotInformations2.Clear();
		CLCentered("Sucessfully refreshed Tickets!\n");
		Thread.Sleep(3000);
	}

	public static void RefreshProxies(int Amount, string Username, string Password, int timeout)
	{
		UtilsMenu.ProxyWorked = 0;
		Var.WorkingProxies.Clear();
		string oldTitle = Console.Title;
		ScrapeProxies(timeout);
		Parallel.ForEach(Var.Proxies, delegate(string Proxy, ParallelLoopState state)
		{
			if (UtilsMenu.ProxyWorked > Amount)
			{
				state.Stop();
			}
			else
			{
				object obj = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { Username, Password, null, null, null, "MSP1-Standalone:XXXXXX" }, Proxy);
				object obj2 = (dynamic)obj == null || ((dynamic)obj).ToString().Contains("ERROR") || !((((dynamic)obj)["loginStatus"]["status"] == "Success" || ((dynamic)obj)["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0 || false || false;
				if ((!(((dynamic)obj2) ? true : false) || 1 == 0) && (!(((dynamic)obj2 | false) ? true : false) || 1 == 0))
				{
					UtilsMenu.ProxyWorked++;
					CLCentered("[" + UtilsMenu.ProxyWorked + "/" + Amount + "] Working Proxy -> " + Proxy + "\n");
					Var.WorkingProxies.Add(Proxy);
					try
					{
						File.AppendAllText("ManiacScrapedProxies.txt", Proxy + "\n");
					}
					catch
					{
					}
					Console.Title = oldTitle + " | Proxies: " + UtilsMenu.ProxyWorked;
				}
				else
				{
					CLCentered("[INVALID] Proxy -> " + Proxy + "\n");
				}
			}
		});
		CLCentered("Sucessfully refreshed Proxies!\n");
		Thread.Sleep(3000);
	}
}
