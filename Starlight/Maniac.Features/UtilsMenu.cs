using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Maniac.AMF;
using Maniac.ManiacUtils;

namespace Maniac.Features;

internal class UtilsMenu
{
	public static int ProxyWorked;

	public static int validProxyCount = 0;

	private static ConcurrentBag<string> validProxies = new ConcurrentBag<string>();

	public static void KillerProxyScraperMenu()
	{
		string oldTitle = Var.OldTitle;
		while (true)
		{
			try
			{
				string text = "";
				if (Var.Proxies.Count != 0)
				{
					int index = new Random().Next(Var.Proxies.Count);
					text = Var.Proxies[index];
					Var.Proxies.Remove(text);
				}
				try
				{
					dynamic val = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6]
					{
						Var.ProxyUser,
						Var.ProxyPass,
						null,
						null,
						null,
						"MSP1-Standalone:XXXXXX"
					}, text);
					bool flag = val == null || val.ToString().Contains("ERROR");
					if ((bool)(!flag && (val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated")))
					{
						Interlocked.Increment(ref validProxyCount);
						Utils.CLCentered($"[{validProxyCount}/{Var.ProxyAmount}] Working Proxy -> {text}\n");
						Var.WorkingProxies.Add(text);
						Var.cpm_builder++;
						try
						{
							File.AppendAllText("ScrapedProxies.txt", text + "\n");
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
						}
						if (Var.Proxies.Count == 0)
						{
							Console.Title = $"{oldTitle} | Proxies: {validProxyCount} | CPM: {Var.cpm_final} | Queue: All proxies are queued";
							continue;
						}
						Console.Title = $"{oldTitle} | Proxies: {validProxyCount} | CPM: {Var.cpm_final} | Queue: {Var.Proxies.Count}";
					}
					else
					{
						Var.cpm_builder++;
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}
	}

	public static void ProxyScraperMenu()
	{
		string oldTitle = Console.Title;
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLCentered("[INFO] You have to use an account so the proxies can be validated! (use a bot account)\n");
		Utils.CLCentered("[INFO] Recommended amount -> 50 to 100\n");
		Utils.CLCentered("[INFO] Recommended timeout -> 5000 to 10000\n\n\n");
		Utils.CLManiac("Username: ");
		string Username = Console.ReadLine();
		Utils.CLManiac("Password: ");
		string Password = Console.ReadLine();
		Utils.CLManiac("Amount: ");
		int Amount = Convert.ToInt32(Console.ReadLine());
		Utils.CLManiac("Timeout: ");
		int timeout = Convert.ToInt32(Console.ReadLine());
		do
		{
			Utils.ScrapeProxies(timeout);
			Parallel.ForEach(Var.Proxies, delegate(string Proxy, ParallelLoopState state)
			{
				if (ProxyWorked > Amount)
				{
					state.Stop();
				}
				else
				{
					object obj = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { Username, Password, null, null, null, "MSP1-Standalone:XXXXXX" }, Proxy);
					object obj2 = (dynamic)obj == null || ((dynamic)obj).ToString().Contains("ERROR") || !((((dynamic)obj)["loginStatus"]["status"] == "Success" || ((dynamic)obj)["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0 || false || false;
					if ((!(((dynamic)obj2) ? true : false) || 1 == 0) && (!(((dynamic)obj2 | false) ? true : false) || 1 == 0))
					{
						ProxyWorked++;
						Utils.CLCentered("[" + ProxyWorked + "/" + Amount + "] Working Proxy -> " + Proxy + "\n");
						Var.WorkingProxies.Add(Proxy);
						try
						{
							File.AppendAllText("ScrapedProxies.txt", Proxy + "\n");
						}
						catch
						{
						}
						Console.Title = oldTitle + " | Proxies: " + ProxyWorked;
					}
					else
					{
						Utils.CLCentered("[INVALID] Proxy -> " + Proxy + "\n");
					}
				}
			});
		}
		while (ProxyWorked < Amount);
		Utils.CLCentered("Finished scraping " + Amount + " proxys\n");
		Thread.Sleep(3000);
	}

	public static void ProxyImportMenu()
	{
		string title = Console.Title;
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Proxy list path (drag and drop): ");
		Var.Proxy = File.ReadAllLines(Console.ReadLine());
		Utils.CLCentered("\n");
		Utils.CLManiac("Successfully loaded " + Var.Proxy.Length + " Proxies!\n");
		string[] proxy = Var.Proxy;
		foreach (string text in proxy)
		{
			ProxyWorked++;
			Console.Title = title + " | Proxies: " + ProxyWorked;
			Utils.CLCentered("Successfully added -> " + text + "\n");
			Var.WorkingProxies.Add(text);
		}
		Utils.CLCentered("Finished Loading " + Var.Proxy.Length + " proxies\n");
		Thread.Sleep(3000);
	}
}
