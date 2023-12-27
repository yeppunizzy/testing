using System;
using System.Net;
using Maniac.AMF;

namespace Maniac.ManiacUtils;

internal class MovieUtil
{
	public static int movieid;

	public static int CreateSingleMovie(string ticket, int receiverid, string proxy)
	{
		WebClient webClient = new WebClient();
		byte[] array = webClient.DownloadData("https://cdn.discordapp.com/attachments/1106972095159742534/1108087460564574288/cachedImage.png");
		byte[] array2 = webClient.DownloadData("https://snapshots.mspcdns.com/v1/snapshots/MSP_US_blob_movieactorclothesdata_0_32_478_193");
		byte[] array3 = webClient.DownloadData("https://snapshots.mspcdns.com/v1/snapshots/MSP_US_blob_moviedata_0_32_478_193.jpg?SMode=pqh1");
		string text = Utils.RandomizedString(14);
		dynamic val = amf.SendAMF("MovieStarPlanet.MobileServices.AMFMovieService.CreateMovieWithSnapshot", new object[9]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(ticket)
			},
			text,
			false,
			new Random().Next(29999, 217330),
			array2,
			array3,
			new object[8] { receiverid, receiverid, receiverid, receiverid, receiverid, receiverid, receiverid, receiverid },
			array,
			array
		});
		if (val == null)
		{
			CreateSingleMovie(ticket, receiverid, proxy);
			return 0;
		}
		if (val.ToString().Contains("ERROR"))
		{
			CreateSingleMovie(ticket, receiverid, proxy);
			return 0;
		}
		if (val["movieId"] > -1)
		{
			return val["movieId"];
		}
		CreateSingleMovie(ticket, receiverid, proxy);
		return 0;
	}

	public static int WatchMovie(string ticket, int actorid, int movieid, string nebulaid, string nebulatoken, string proxy)
	{
		WSUtil.ConnectBotToWebSocket(nebulaid, nebulatoken, 200);
		dynamic val = amf.SendAMF("MovieStarPlanet.MobileServices.AMFMovieService.MovieWatched", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketUtil.headerTicket(ticket)
			},
			movieid
		}, proxy);
		if ((!((val == null) ? true : false) || 1 == 0) && (!(val.ToString().Contains("ERROR") ? true : false) || 1 == 0) && (!((val["awardedFame"] > 10 && val["returnType"] == 2) ? true : false) || 1 == 0) && val.ContainsKey("description") && val["description"] == "RateLimited")
		{
			return 0;
		}
		return 1;
	}
}
