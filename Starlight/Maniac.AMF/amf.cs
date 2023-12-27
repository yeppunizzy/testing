using System;
using System.IO;
using System.Net;
using FluorineFx.IO;
using Maniac.ManiacUtils;
using Newtonsoft.Json.Linq;

namespace Maniac.AMF;

internal class amf
{
	public static string Endpoint = "";

	public static string sessionID = "";


    public static void GetEndpointForServer(string server)
    {
        WebClient webClient = new WebClient();
        JObject jObject = JObject.Parse(webClient.DownloadString("https://disco.mspapis.com/disco/v1/services/msp/" + server + "?services=mspwebservice"));
        Endpoint = (string?)jObject["Services"]![0]!["Endpoint"];
        webClient.Dispose();
    }

    public static object SendAMF(string method, object[] arguments, string proxy = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		AMFMessage val = new AMFMessage((ushort)3);
		val.AddHeader(new AMFHeader("sessionID", false, (object)Utils.RandomizedString(24)));
		val.AddHeader(new AMFHeader("id", false, (object)Checksum.createChecksum(arguments)));
		val.AddHeader(new AMFHeader("needClassName", false, (object)false));
		val.AddBody(new AMFBody(method, "/1", (object)arguments));
		MemoryStream memoryStream = new MemoryStream();
		AMFSerializer val2 = new AMFSerializer((Stream)memoryStream);
		val2.WriteMessage(val);
		((BinaryWriter)val2).Flush();
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Endpoint + "/Gateway.aspx?method=" + method);
		httpWebRequest.Headers["X-Forwarded-For"] = "127.0.0.1";
		httpWebRequest.Referer = "app:/cache/t1.bin/[[DYNAMIC]]/2";
		httpWebRequest.Accept = "text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5, application/x-mpegURL";
		httpWebRequest.ContentType = "application/x-amf";
		httpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; en) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/32.0";
		if (proxy != null)
		{
			httpWebRequest.Proxy = new WebProxy(proxy);
		}
		httpWebRequest.Method = "POST";
		byte[] array = memoryStream.ToArray();
		try
		{
			httpWebRequest.GetRequestStream().Write(array, 0, array.Length);
			HttpWebResponse obj = (HttpWebResponse)httpWebRequest.GetResponse();
			memoryStream = new MemoryStream();
			obj.GetResponseStream().CopyTo(memoryStream);
			return DecodeAMF(memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			return "ERROR! " + ex.ToString();
		}
	}

    public static dynamic DecodeAMF(byte[] body)
    {
        MemoryStream memoryStream = new MemoryStream(body);
        AMFDeserializer aMFDeserializer = new AMFDeserializer(memoryStream);
        AMFMessage aMFMessage = aMFDeserializer.ReadAMFMessage();
        object content = aMFMessage.Bodies[0].Content;
        memoryStream.Dispose();
        aMFDeserializer.Dispose();
        return content;
    }
}