using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Maniac.AMF;
using Maniac.Features;
using Maniac.ManiacUtils;

namespace Maniac;

internal class Program
{

	private static void Main(string[] args)
	{
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Server: ");
		Var.server = Console.ReadLine();
		amf.GetEndpointForServer(Var.server);
		while (true)
		{
			Console.Clear();
			Utils.CLWatermark();
			Utils.CLCentered("[1] Msp websocket url | [2] Pastebin websocket url\n");
			Utils.CLManiac("Option: ");
			string text = Console.ReadLine();
			if (text == "1")
			{
				WSUtil.SetWebsocketPath(Var.server);
				break;
			}
			if (text == "2")
			{
				WSUtil.SetWebsocketPathPastebin(Var.server);
				break;
			}
			Utils.CLCentered("Wrong input!");
			Thread.Sleep(2000);
		}
		Console.Clear();
		while (true)
		{
			Console.Clear();
			Utils.CLWatermark();
			Utils.CLCentered("[1] Bots Menu | [2] Movie Menu | [3] Utils Menu\n");
			Utils.CLCentered("Option: ");
			switch (Console.ReadLine())
			{
			case "1":
				while (true)
				{
					Console.Clear();
					Utils.CLWatermark();
					if (Var.WorkingProxies.Count == 0)
					{
						Utils.CLCentered("Looks like you dont have any proxies imported! Please import or scrape proxies before trying to use the bots menu!");
						Thread.Sleep(5000);
						break;
					}
					Utils.CLCentered("[1] Login Bots | [2] Login and extract Tickets | [3] Quick login with Tickets\n");
					Utils.CLCentered("[4] Claim level ups | [5] Activate Bots | [6] Validate Bots | [7] go back\n\n");
					Utils.CLManiac("Option: ");
					switch (Console.ReadLine())
					{
					default:
						return;
					case "7":
						break;
					case "1":
						BotsMenu.LoginBotsFeature();
						continue;
					case "2":
						BotsMenu.LoginAndExportTickets();
						continue;
					case "3":
						BotsMenu.TicketLogin();
						continue;
					case "4":
						BotsMenu.ClaimLevelUps();
						continue;
					case "5":
						BotsMenu.ActivateBotsMenu();
						continue;
					case "6":
						BotsMenu.VerifyBotsMenu();
						continue;
					}
					break;
				}
				break;
			case "2":
				while (true)
				{
					Console.Clear();
					Utils.CLWatermark();
					if (Var.BotInformations.Count == 0)
					{
						Utils.CLCentered("Looks like you dont have any bots imported! Please import or login bots before trying to bot movies!");
						Thread.Sleep(5000);
						break;
					}
					if (Var.WorkingProxies.Count == 0)
					{
						Utils.CLCentered("Looks like you dont have any proxies imported! Please import or scrape proxies before trying to bot movies!");
						Thread.Sleep(5000);
						break;
					}
					Utils.CLCentered("[1] Single Mode | [2] Multi Mode | [3] Slow Connection Multi mode | [4] Go back\n");
					Utils.CLManiac("Option: ");
					switch (Console.ReadLine())
					{
					default:
						return;
					case "4":
						break;
					case "1":
						MovieMenu.SingleMovieMenu();
						continue;
					case "2":
						MovieMenu.MultiMovieMenu();
						continue;
					case "3":
						MovieMenu.SlowConnectionMultiMode();
						continue;
					}
					break;
				}
				break;
			case "3":
			{
				string text2;
				while (true)
				{
					Console.Clear();
					Utils.CLWatermark();
					Utils.CLCentered("[1] Proxy scraper | [2] import Proxy list | [3] Killer Proxy Mode | [4] Enable Debug/Error testing | [5] Go back\n");
					Utils.CLManiac("Option: ");
					text2 = Console.ReadLine();
					switch (text2)
					{
					case "1":
						UtilsMenu.ProxyScraperMenu();
						continue;
					case "2":
						UtilsMenu.ProxyImportMenu();
						continue;
					case "3":
					{
						Console.Clear();
						Utils.CLWatermark();
						Utils.CLCentered("[INFO] You have to use an account so the proxies can be validated! (use a bot account)\n");
						Utils.CLCentered("[INFO] Recommended amount -> 50 to 100\n");
						Utils.CLCentered("[INFO] Recommended timeout -> 5000 to 10000\n\n\n");
						Utils.CLManiac("Username: ");
						Var.ProxyUser = Console.ReadLine();
						Utils.CLManiac("Password: ");
						Var.ProxyPass = Console.ReadLine();
						Utils.CLManiac("Amount: ");
						Var.ProxyAmount = Convert.ToInt32(Console.ReadLine());
						Utils.CLManiac("Timeout: ");
						int timeout = Convert.ToInt32(Console.ReadLine());
						Utils.CLCentered("Threads: ");
						Var.Threads = int.Parse(Console.ReadLine());
						new Thread(Utils.CalculateCPM).Start();
						int num2 = 0;
						Utils.ScrapeProxies(timeout);
						Var.OldTitle = Console.Title;
						Var.scraping = true;
						while (num2 < Var.Threads)
						{
							Thread thread = new Thread(UtilsMenu.KillerProxyScraperMenu);
							thread.Start();
							Var.ActiveThreads.Add(thread);
							num2++;
							Var.RunningThreads++;
							if (num2 >= Var.Threads)
							{
								break;
							}
						}
						break;
					}
					}
					if (!(text2 == "4"))
					{
						break;
					}
					Var.ErrorMode = true;
					Utils.CLDebug("Enabled debug mode! You will now receive more informations when using some functions!");
					Thread.Sleep(3000);
				}
				if (text2 == "5")
				{
					break;
				}
				return;
			}
			default:
				Utils.CLManiac("Invalid option!");
				Thread.Sleep(1000);
				break;
			}
		}
	}
}
