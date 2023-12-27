using System;
using System.Security.Cryptography;
using System.Text;

namespace Maniac.ManiacUtils;

internal class TicketUtil
{
	public static int markingID = new Random().Next(0, 1000);

	public static string headerTicket(string ticket)
	{
		return ticket + getMarkingId();
	}

	public static string getMarkingId()
	{
		markingID++;
		byte[] bytes = Encoding.UTF8.GetBytes(markingID.ToString());
		return BitConverter.ToString(MD5.Create().ComputeHash(bytes)).Replace("-", "").ToLower() + BitConverter.ToString(bytes).Replace("-", "").ToLower();
	}
}
