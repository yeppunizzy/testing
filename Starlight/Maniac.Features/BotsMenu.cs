using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Maniac.AMF;
using Maniac.ManiacUtils;

namespace Maniac.Features;

internal class BotsMenu
{
	public static void LoginBotsFeature()
	{
		string oldtitle = Console.Title;
		string text = "n";
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Botfile path (drag and drop): ");
		Var.Bots = File.ReadAllLines(Console.ReadLine());
		Utils.CLCentered("\n");
		if (Var.WorkingProxies.Count > 0)
		{
			Utils.CLManiac("Use imported proxies? (y/n): ");
			text = Console.ReadLine();
		}
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Successfully loaded " + Var.Bots.Length + " Bots!\n");
		string[] bots = Var.Bots;
		if (text.ToLower() == "n")
		{
			string[] array = bots;
			foreach (string text2 in array)
			{
				(string, int, string, string, bool, string, string) item = BotUtils.LoginForBots(text2.Split(':')[0], text2.Split(':')[1]);
				if (item.Item5)
				{
					Var.BotsLogged++;
					Console.Title = oldtitle + " | Bots loaded: " + Var.BotsLogged;
					Utils.CLCentered("Successfully logged into -> " + text2.Split(':')[0] + "\n");
					Var.BotInformations.Add(item);
				}
			}
		}
		else
		{
			Parallel.ForEach(bots, delegate(string userpass)
			{
				while (true)
				{
					int index = new Random().Next(Var.WorkingProxies.Count);
					try
					{
						(string, int, string, string, bool, string, string) item2 = BotUtils.LoginForBots(userpass.Split(':')[0], userpass.Split(':')[1], Var.WorkingProxies[index]);
						if (item2.Item5)
						{
							Var.BotsLogged++;
							Console.Title = oldtitle + " | Bots loaded: " + Var.BotsLogged;
							Utils.CLCentered("Successfully logged into -> " + userpass.Split(':')[0] + "\n");
							Var.BotInformations.Add(item2);
						}
						break;
					}
					catch
					{
						Utils.CLCentered("[Error] Proxy didnt work -> " + Var.WorkingProxies[index] + " retrying!\n");
					}
				}
			});
		}
		Utils.CLManiac("Succesfully logged into " + Var.BotsLogged + " bots!\n");
		Thread.Sleep(3000);
	}

	public static void LoginAndExportTickets()
	{
		string oldtitle = Console.Title;
		string text = "n";
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Botfile path (drag and drop): ");
		Var.Bots = File.ReadAllLines(Console.ReadLine());
		Utils.CLCentered("\n");
		if (Var.WorkingProxies.Count > 0)
		{
			Utils.CLManiac("Use imported proxies? (y/n): ");
			text = Console.ReadLine();
		}
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Successfully loaded " + Var.Bots.Length + " Bots!\n");
		string[] bots = Var.Bots;
		if (text.ToLower() == "n")
		{
			string[] array = bots;
			foreach (string text2 in array)
			{
				(string, int, string, string, bool, string, string) item = BotUtils.LoginForBotsET(text2.Split(':')[0], text2.Split(':')[1]);
				if (item.Item5)
				{
					Var.BotsLogged++;
					Console.Title = oldtitle + " | Bots loaded: " + Var.BotsLogged;
					Utils.CLCentered("Successfully logged into -> " + text2.Split(':')[0] + "\n");
					Var.BotInformations.Add(item);
				}
			}
		}
		else
		{
			Parallel.ForEach(bots, delegate(string userpass)
			{
				while (true)
				{
					int index = new Random().Next(Var.WorkingProxies.Count);
					try
					{
						(string, int, string, string, bool, string, string) item2 = BotUtils.LoginForBotsET(userpass.Split(':')[0], userpass.Split(':')[1], Var.WorkingProxies[index]);
						if (item2.Item5)
						{
							Var.BotsLogged++;
							Console.Title = oldtitle + " | Bots loaded: " + Var.BotsLogged;
							Utils.CLCentered("Successfully logged into -> " + userpass.Split(':')[0] + "\n");
							Var.BotInformations.Add(item2);
						}
						break;
					}
					catch
					{
						Utils.CLCentered("[Error] Proxy didnt work -> " + Var.WorkingProxies[index] + " retrying!\n");
					}
				}
			});
		}
		Utils.CLManiac("Succesfully logged into " + Var.BotsLogged + " bots!\n");
		Thread.Sleep(3000);
	}

	public static void TicketLogin()
	{
		string title = Console.Title;
		Console.Clear();
		Utils.CLWatermark();
		Utils.CLManiac("Ticketfile path (drag and drop): ");
		Var.Bots = File.ReadAllLines(Console.ReadLine());
		Utils.CLManiac("Successfully loaded " + Var.Bots.Length + " Bots!\n");
		string[] bots = Var.Bots;
		foreach (string text in bots)
		{
			(string, int, string, string, bool, string, string) item = (text.Split('•')[0], int.Parse(text.Split('•')[1]), text.Split('•')[2], text.Split('•')[3], true, text.Split('•')[4], text.Split('•')[5]);
			Var.BotsLogged++;
			Console.Title = title + " | Bots loaded: " + Var.BotsLogged;
			Utils.CLCentered("Successfully logged into -> " + text.Split('•')[4] + "\n");
			Var.BotInformations.Add(item);
		}
		Utils.CLManiac("Succesfully logged into " + Var.BotsLogged + " bots!\n");
		Thread.Sleep(3000);
	}

	public static void ClaimLevelUps()
	{
		Console.Clear();
		Utils.CLWatermark();
		foreach (var botInformation in Var.BotInformations)
		{
			for (int i = 0; i < 100; i++)
			{
				try
				{
					dynamic val = amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetLevelUps", new object[1]
					{
						new TicketHeader
						{
							Ticket = TicketUtil.headerTicket(botInformation.Ticket)
						}
					});
					if ((int)val.GetType().GetProperty("Count").GetValue(val, null) == 0)
					{
						Utils.CLManiac("Claimed all level up rewards for -> " + botInformation.Username + "\n");
						break;
					}
					int num = val[0]["LevelUpGiftSelectId"];
					string input = val[0]["Gifts"][0]["ColorScheme"].ToString();
					string input2 = val[0]["Gifts"][1]["ColorScheme"].ToString();
					string input3 = val[0]["Gifts"][2]["ColorScheme"].ToString();
					int count = Regex.Matches(input, "0x").Count;
					int count2 = Regex.Matches(input2, "0x").Count;
					int count3 = Regex.Matches(input3, "0x").Count;
					if (val[0]["CollectAllGifts"] == true)
					{
						dynamic val2 = amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.ClaimAllLevelUpGifts", new object[3]
						{
							new TicketHeader
							{
								Ticket = TicketUtil.headerTicket(botInformation.Ticket)
							},
							num,
							new object[3]
							{
								BotUtils.getlevelupcolor(count),
								BotUtils.getlevelupcolor(count2),
								BotUtils.getlevelupcolor(count3)
							}
						});
						if (val2["Description"] == "Success")
						{
							Utils.CLManiac("Claimed level up for -> " + botInformation.Username + "\n");
						}
						else
						{
							Utils.CLManiac("ERROR! -> " + botInformation.Username + "\n");
						}
					}
					else
					{
						dynamic val3 = amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.ClaimSingleLevelUpGift", new object[4]
						{
							new TicketHeader
							{
								Ticket = TicketUtil.headerTicket(botInformation.Ticket)
							},
							num,
							1,
							BotUtils.getlevelupcolor(count)
						});
						if (val3["Description"] == "Success")
						{
							Utils.CLManiac("Claimed level up for -> " + botInformation.Username + "\n");
						}
						else
						{
							Utils.CLManiac("ERROR! -> " + botInformation.Username + "\n");
						}
					}
					continue;
				}
				catch
				{
					Utils.CLManiac("Something broke 0.0, no worries it will try to continue normally!\n");
					continue;
				}
			}
		}
		Utils.CLManiac("Claimed level ups for all bots!\n");
		Thread.Sleep(3000);
	}

	public static void ActivateBotsMenu()
	{
		Console.Clear();
		Utils.CLWatermark();
		Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
		{
			BotUtils.ActivateBot(bot.Ticket, bot.ActorID, bot.Username);
			Utils.CLCentered("Verified -> " + bot.Username + "\n");
		});
		Utils.CLCentered("Finished verifying all bots!\n");
		Thread.Sleep(3000);
	}

	public static void VerifyBotsMenu()
	{
		Console.Clear();
		Utils.CLWatermark();
		Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
		{
			BotUtils.VerifyBot(bot.Ticket, bot.ActorID, bot.Username);
			Utils.CLCentered("Verified -> " + bot.Username + "\n");
		});
		Utils.CLCentered("Finished verifying all bots!\n");
		Thread.Sleep(3000);
	}
}
