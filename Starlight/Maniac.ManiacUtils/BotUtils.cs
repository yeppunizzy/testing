using System;
using System.IO;
using System.Net;
using Maniac.AMF;

namespace Maniac.ManiacUtils;

internal class BotUtils
{
	public static (string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string username, string password) LoginForBots(string Username, string Password, string Proxy = null)
	{
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { Username, Password, null, null, null, "MSP1-Standalone:XXXXXX" }, Proxy);
		dynamic val2 = val == null || val.ToString().Contains("ERROR") || !((val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0 || false || false;
		if ((!(val2 ? true : false) || 1 == 0) && (!((val2 | false) ? true : false) || 1 == 0))
		{
			int item = (int)val["loginStatus"]["actor"]["ActorId"];
			string item2 = (string)val["loginStatus"]["ticket"];
			string item3 = (string)val["loginStatus"]["nebulaLoginStatus"]["profileId"];
			string item4 = (string)val["loginStatus"]["nebulaLoginStatus"]["accessToken"];
			return (item2, item, item3, item4, true, Username, Password);
		}
		if (((string)val["loginStatus"]["status"]).ToUpper() == "ERROR")
		{
			Utils.CLManiac("You are rate limited, please change your ip and after click on any key on your keyboard!\n");
			Console.ReadKey();
			return LoginForBots(Username, Password);
		}
		return (null, 0, null, null, false, Username, Password);
	}

	public static (string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string username, string password) LoginForBotsAuto(string Username, string Password, string Proxy = null)
	{
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { Username, Password, null, null, null, "MSP1-Standalone:XXXXXX" }, Proxy);
		dynamic val2 = val == null || val.ToString().Contains("ERROR") || !((val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0 || false || false;
		if ((!(val2 ? true : false) || 1 == 0) && (!((val2 | false) ? true : false) || 1 == 0))
		{
			int item = (int)val["loginStatus"]["actor"]["ActorId"];
			string item2 = (string)val["loginStatus"]["ticket"];
			string item3 = (string)val["loginStatus"]["nebulaLoginStatus"]["profileId"];
			string item4 = (string)val["loginStatus"]["nebulaLoginStatus"]["accessToken"];
			return (item2, item, item3, item4, true, Username, Password);
		}
		if (((string)val["loginStatus"]["status"]).ToUpper() == "ERROR")
		{
			Utils.CLManiac("You are rate limited, please change your ip and after click on any key on your keyboard!\n");
			return LoginForBotsAuto(Username, Password);
		}
		return (null, 0, null, null, false, Username, Password);
	}

	public static (string Ticket, int ActorID, string NebulaId, string NebulaToken, bool Success, string username, string password) LoginForBotsET(string Username, string Password, string Proxy = null)
	{
		dynamic val = amf.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { Username, Password, null, null, null, "MSP1-Standalone:XXXXXX" }, Proxy);
		dynamic val2 = val == null || val.ToString().Contains("ERROR") || !((val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0 || false || false;
		if ((!(val2 ? true : false) || 1 == 0) && (!((val2 | false) ? true : false) || 1 == 0))
		{
			int item = (int)val["loginStatus"]["actor"]["ActorId"];
			string text = (string)val["loginStatus"]["ticket"];
			string text2 = (string)val["loginStatus"]["nebulaLoginStatus"]["profileId"];
			string text3 = (string)val["loginStatus"]["nebulaLoginStatus"]["accessToken"];
			try
			{
				File.AppendAllText("Tickets-" + Var.server + ".txt", text + "•" + item + "•" + text2 + "•" + text3 + "•" + Username + "•" + Password + "\n");
			}
			catch (Exception)
			{
			}
			return (text, item, text2, text3, true, Username, Password);
		}
		if (((string)val["loginStatus"]["status"]).ToUpper() == "ERROR")
		{
			Utils.CLManiac("You are rate limited, please change your ip and after click on any key on your keyboard!\n");
			return LoginForBotsET(Username, Password);
		}
		return (null, 0, null, null, false, Username, Password);
	}

	public static string getlevelupcolor(int amount)
	{
		string text = "";
		for (int i = 0; i < amount; i++)
		{
			text += "0x000000,";
		}
		return text.Remove(text.Length - 1, 1);
	}

	public static void ActivateBot(string Ticket, int ActorId, string Username)
	{
		byte[] array = new WebClient().DownloadData("https://cdn.discordapp.com/attachments/1106972095159742534/1108459652519301150/loo.png");
		amf.SendAMF("MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshotSmallAndBig", new object[7]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId,
			"moviestar",
			"fullSizeMovieStar",
			array,
			array,
			"jpg"
		});
		amf.SendAMF("MovieStarPlanet.WebService.Moderation.AMFModeration.LoginEvent", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			Username
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetPostLoginBundleStandalone", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.AMFAwardService.claimDailyAward", new object[4]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			"wheel",
			120,
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.LoadActorDetailsExtended", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.Session.AMFSessionServiceForWeb.GetChatPermissionInfo", new object[1]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			}
		});
		amf.SendAMF("MovieStarPlanet.WebService.Session.AMFSessionServiceForWeb.GetLevelThresholds", new object[0]);
		amf.SendAMF("MovieStarPlanet.WebService.Moderation.AMFModeration.LoginEvent", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			Username
		});
		amf.SendAMF("MovieStarPlanet.WebService.Achievement.AMFAchievementWebService.CheckLoginAchievements", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.WorldTheme.AMFWorldThemeService.GetWorldThemeInfo", new object[1]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			}
		});
		amf.SendAMF("MovieStarPlanet.WebService.Bonster.AMFBonsterService.GetBonsterCandyPrices", new object[0]);
		amf.SendAMF("MovieStarPlanet.WebService.Bonster.AMFBonsterService.GetBonsterTemplateList", new object[0]);
		amf.SendAMF("MovieStarPlanet.WebService.Spending.AMFSpendingService.GetEmoticonPackages", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadBlockedAndBlockingActors", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadBlockedAndBlockingActorsNeb", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetPostLoginBundleStandalone", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.LoadMovieStarListRevised", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			new object[1] { ActorId }
		});
		amf.SendAMF("MovieStarPlanet.WebService.Friendships.AMFFriendshipService.GetProfileTodosCount", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.Quest.AMFQuestService.GetAllQuestStatusForDownloadableClient", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.AnchorCharacter.AMFAnchorCharacterService.GetAnchorCharacterList", new object[0]);
		amf.SendAMF("MovieStarPlanet.WebService.MovieService.AMFMovieService.GetAutoSavedMovieId", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadMood", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadModeratorInformation", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.Activity.AMFActivityService.GetOfflineTodos", new object[4]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId,
			0,
			100
		});
		amf.SendAMF("MovieStarPlanet.WebService.Spending.AMFSpendingService.GetActiveSpecialsItems", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.Friendships.AMFFriendshipService.GetFriendListWithNameAndScore", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.NotificationCenter.AMFNotificationCenterService.GetNotificationCount", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.UpdateBehaviourStatusNew", new object[6]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId,
			0,
			"",
			-1,
			-1
		});
		amf.SendAMF("MovieStarPlanet.WebService.PiggyBank.AMFPiggyBankService.GetPiggyBank", new object[1]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			}
		});
		amf.SendAMF("MovieStarPlanet.WebService.Friendships.AMFFriendshipService.ApproveDefaultAnchorFriendship", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.UpdateGift", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.Awarding.AMFAwardingService.countAwardsLeft", new object[3]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			new object[1] { "wheel" },
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.GameStatsService.AMFGameStatsService.IncrementCount", new object[3]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			"WheelOfFortuneImpressions",
			1
		});
		amf.SendAMF("MovieStarPlanet.WebService.AMFMobileService.GetUserGameStatusData", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			Convert.ToInt32(Ticket.Split(',')[1])
		});
	}

	public static void VerifyBot(string Ticket, int ActorId, string Username)
	{
		byte[] array = new WebClient().DownloadData("https://cdn.discordapp.com/attachments/1106972095159742534/1108459652519301150/loo.png");
		amf.SendAMF("MovieStarPlanet.WebService.Snapshots.AMFGenericSnapshotService.CreateSnapshotSmallAndBig", new object[7]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId,
			"moviestar",
			"fullSizeMovieStar",
			array,
			array,
			"jpg"
		});
		amf.SendAMF("MovieStarPlanet.WebService.Moderation.AMFModeration.LoginEvent", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			Username
		});
		amf.SendAMF("MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetPostLoginBundleStandalone", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			ActorId
		});
		amf.SendAMF("MovieStarPlanet.WebService.AMFAwardService.claimDailyAward", new object[4]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(Ticket)
			},
			"wheel",
			120,
			ActorId
		});
	}
}
