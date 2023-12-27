using System;
using System.Net;
using System.Threading;
using WebSocketSharp;

namespace Maniac.ManiacUtils;

internal class WSUtil
{
	public static string webSocketPath = "";

	public static void SetWebsocketPath(string server)
	{
		webSocketPath = new WebClient
		{
			Proxy = null
		}.DownloadString((server.ToLower() == "us") ? "https://presence-us.mspapis.com/getServer" : "https://presence.mspapis.com/getServer");
	}

	public static void SetWebsocketPathPastebin(string server)
	{
		webSocketPath = new WebClient
		{
			Proxy = null
		}.DownloadString((server.ToLower() == "us") ? "https://presence-us.mspapis.com/getServer" : "https://pastebin.com/raw/djqsHXZD");
	}

	public static WebSocket ConnectBotToWebSocket(string NebulaId, string NebulaToken, int length)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			WebSocket val = new WebSocket("ws://" + webSocketPath.Replace('-', '.') + ":10843/" + webSocketPath.Replace('.', '-') + "/?transport=websocket", Array.Empty<string>());
			val.Connect();
			val.Send("42[\"10\",{\"messageType\":10,\"messageContent\":{\"version\":3,\"applicationId\":\"APPLICATION_WEB\",\"country\":\"" + Var.server + "\",\"username\":\"" + NebulaId + "\",\"access_token\":\"" + NebulaToken + "\"}}]");
			Thread.Sleep(length);
			return val;
		}
		catch (Exception)
		{
		}
		return null;
	}
}
