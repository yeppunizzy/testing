using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Maniac.AMF;
using Maniac.ManiacUtils;

namespace Maniac.Features;

internal class MovieMenu
{
	public static bool AutomationProcess = false;

	public static List<int> MovieIdList = new List<int>();

	private static void OnTick(object source, ElapsedEventArgs e)
	{
		AutomationProcess = true;
	}

	public static void SingleMovieMenu()
	{
		Utils.CLManiac("Username: ");
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName", new object[1] { Console.ReadLine() });
		int receiverid = (int)val;
		while (true)
		{
			Random rnd;
			int movieid;
			while (true)
			{
				rnd = new Random();
				int index = rnd.Next(Var.BotInformations.Count);
				string item = Var.BotInformations[index].Ticket;
				Console.Clear();
				Utils.CLWatermark();
				movieid = MovieUtil.CreateSingleMovie(item, receiverid, null);
				if (movieid > 0)
				{
					break;
				}
				Utils.CLCentered("error creating movie, retrying!");
				Thread.Sleep(2000);
			}
			Utils.CLCentered("Created movie with id -> " + movieid + "\n");
			Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
			{
				int num;
				while (true)
				{
					int index2 = rnd.Next(Var.WorkingProxies.Count);
					string proxy = Var.WorkingProxies[index2];
					try
					{
						num = MovieUtil.WatchMovie(bot.Ticket, bot.ActorID, movieid, bot.NebulaId, bot.NebulaToken, proxy);
					}
					catch
					{
						continue;
					}
					break;
				}
				if (num == 0)
				{
					Utils.CLCentered("Couldnt watch movie with -> " + bot.Username + "\n");
				}
				else
				{
					Utils.CLCentered("Successfully watched movie with -> " + bot.Username + "\n");
				}
			});
		}
	}

	public static void MultiMovieMenu()
	{
		int index = new Random().Next(Var.BotInformations.Count);
		string item = Var.BotInformations[index].Username;
		string item2 = Var.BotInformations[index].Password;
		int moviescreated = 0;
		Utils.CLManiac("Username: ");
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName", new object[1] { Console.ReadLine() });
		int receiverid = (int)val;
		Utils.CLManiac("Amount of movies to create: ");
		int Amount = Convert.ToInt32(Console.ReadLine());
		Utils.CLManiac("Automated Proxies amount: ");
		int amount = Convert.ToInt32(Console.ReadLine());
		Utils.CLManiac("timeout for proxies: ");
		int timeout = Convert.ToInt32(Console.ReadLine());
		System.Timers.Timer timer = new System.Timers.Timer(7200000.0);
		timer.Elapsed += OnTick;
		timer.Start();
		while (true)
		{
			Console.Clear();
			Utils.CLWatermark();
			if (AutomationProcess)
			{
				Utils.RefreshProxies(amount, item, item2, timeout);
				Console.Clear();
				Utils.CLWatermark();
				Utils.RefreshTickets();
				BotsMenu.VerifyBotsMenu();
				Console.Clear();
				Utils.CLWatermark();
				Utils.CLCentered("Everything has been refreshed! See you again in 2 hours <3!\n");
				Thread.Sleep(3000);
				Console.Clear();
				Utils.CLWatermark();
				AutomationProcess = false;
			}
			moviescreated = 0;
			MovieIdList.Clear();
			Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot, ParallelLoopState state)
			{
				while (true)
				{
					if (moviescreated > Amount)
					{
						state.Stop();
					}
					int index3 = new Random().Next(Var.WorkingProxies.Count);
					_ = Var.WorkingProxies[index3];
					try
					{
						int num2 = MovieUtil.CreateSingleMovie(bot.Ticket, receiverid, null);
						if (num2 > 0)
						{
							Utils.CLCentered("[" + moviescreated + "/" + Amount + "] Created movie with id -> " + num2 + "\n");
							MovieIdList.Add(num2);
							moviescreated++;
							break;
						}
					}
					catch
					{
					}
				}
			});
			Parallel.ForEach(MovieIdList, delegate(int movieid)
			{
				Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
				{
					int num;
					while (true)
					{
						int index2 = new Random().Next(Var.WorkingProxies.Count);
						string proxy = Var.WorkingProxies[index2];
						try
						{
							num = MovieUtil.WatchMovie(bot.Ticket, bot.ActorID, movieid, bot.NebulaId, bot.NebulaToken, proxy);
						}
						catch
						{
							continue;
						}
						break;
					}
					if (num == 0)
					{
						Utils.CLCentered("Couldnt watch movie with -> " + bot.Username + "\n");
					}
					else
					{
						Utils.CLCentered("Successfully watched movie with -> " + bot.Username + "\n");
					}
				});
			});
		}
	}

	public static void SlowConnectionMultiMode()
	{
		int index = new Random().Next(Var.BotInformations.Count);
		string item = Var.BotInformations[index].Username;
		string item2 = Var.BotInformations[index].Password;
		int moviescreated = 0;
		Utils.CLManiac("Username: ");
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GetActorIdFromName", new object[1] { Console.ReadLine() });
		int receiverid = (int)val;
		Utils.CLManiac("Amount of movies to create: ");
		int Amount = Convert.ToInt32(Console.ReadLine());
		Utils.CLManiac("Automated Proxies amount: ");
		int amount = Convert.ToInt32(Console.ReadLine());
		Utils.CLManiac("timeout for proxies: ");
		int timeout = Convert.ToInt32(Console.ReadLine());
		System.Timers.Timer timer = new System.Timers.Timer(7200000.0);
		timer.Elapsed += OnTick;
		timer.Start();
		while (true)
		{
			Console.Clear();
			Utils.CLWatermark();
			if (AutomationProcess)
			{
				Utils.RefreshProxies(amount, item, item2, timeout);
				Console.Clear();
				Utils.CLWatermark();
				Utils.RefreshTickets();
				BotsMenu.VerifyBotsMenu();
				Console.Clear();
				Utils.CLWatermark();
				Utils.CLCentered("Everything has been refreshed! See you again in 2 hours <3!\n");
				Thread.Sleep(3000);
				Console.Clear();
				Utils.CLWatermark();
				AutomationProcess = false;
			}
			moviescreated = 0;
			MovieIdList.Clear();
			Parallel.For(0, Amount, delegate(int i)
			{
				while (true)
				{
					int index3 = new Random().Next(Var.WorkingProxies.Count);
					_ = Var.WorkingProxies[index3];
					string item3 = Var.BotInformations[i].Ticket;
					try
					{
						if (Var.ErrorMode)
						{
							Utils.CLDebug("Trying to create a movie!\n");
						}
						int num2 = MovieUtil.CreateSingleMovie(item3, receiverid, null);
						if (num2 > 0)
						{
							Utils.CLCentered("[" + moviescreated + "/" + Amount + "] Created movie with id -> " + num2 + "\n");
							MovieIdList.Add(num2);
							moviescreated++;
							break;
						}
						if (Var.ErrorMode)
						{
							Utils.CLDebug("Movie had a id of 0!\n");
						}
					}
					catch
					{
						if (Var.ErrorMode)
						{
							Utils.CLDebug("Proxy didnt work or no response from msp!\n");
						}
					}
				}
			});
			Utils.CLCentered("Finished creating movies!\n");
			Parallel.ForEach(MovieIdList, delegate(int movieid)
			{
				Parallel.ForEach(Var.BotInformations, delegate((string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string Username, string Password) bot)
				{
					int num;
					while (true)
					{
						int index2 = new Random().Next(Var.WorkingProxies.Count);
						string proxy = Var.WorkingProxies[index2];
						try
						{
							num = MovieUtil.WatchMovie(bot.Ticket, bot.ActorID, movieid, bot.NebulaId, bot.NebulaToken, proxy);
						}
						catch
						{
							continue;
						}
						break;
					}
					if (num == 0)
					{
						Utils.CLCentered("Couldnt watch movie with -> " + bot.Username + "\n");
					}
					else
					{
						Utils.CLCentered("Successfully watched movie with -> " + bot.Username + "\n");
					}
				});
			});
		}
	}
}
