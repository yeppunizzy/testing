using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using FluorineFx.AMF3;
using Maniac.ManiacUtils;

namespace Maniac.AMF;

internal class Checksum
{
	public static string createChecksum(object[] arguments)
	{
		return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(fromArray(arguments) + (getTicketValue(arguments) ?? "v1n3g4r") + "$CuaS44qoi0Mp2qp"))).Replace("-", "").ToLower();
	}

	public static string fromArray(object[] arguments)
	{
		string text = "";
		foreach (object obj in arguments)
		{
			text += fromObjectInner(obj);
		}
		return text;
	}

	private static string fromObject(object obj)
	{
		string[] array = (from C in obj.GetType().GetProperties()
			select C.Name).ToArray();
		Array.Sort(array);
		string text = "";
		string[] array2 = array;
		foreach (string name in array2)
		{
			text += fromObjectInner(obj.GetType().GetProperty(name).GetValue(obj, null));
		}
		return text;
	}

	private static string fromObjectInner(object Obj)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		if (!(Obj is TicketHeader) && Obj != null)
		{
			if (!(Obj is int) && !(Obj is string))
			{
				if (!(Obj is bool))
				{
					if (!(Obj is DateTime))
					{
						if (!(Obj is byte[]))
						{
							if (!(Obj is object[]))
							{
								if (!(Obj is ArrayCollection))
								{
									if (Obj is long)
									{
										return fromNumber((long)Obj);
									}
									if (Obj == null)
									{
										return "";
									}
									return fromObject(Obj);
								}
								return fromArrayCollection((ArrayCollection)Obj);
							}
							return fromArray((object[])Obj);
						}
						return fromByteArray((byte[])Obj);
					}
					return $"{((DateTime)Obj).Year}{((DateTime)Obj).AddMonths(-1).Month}{((DateTime)Obj).Day}";
				}
				if (!Convert.ToBoolean(Obj))
				{
					return "False";
				}
				return "True";
			}
			return Obj.ToString();
		}
		return "";
	}

	private static string fromArrayCollection(ArrayCollection o)
	{
		string text = "";
		foreach (object item in o)
		{
			text += fromObjectInner(item);
		}
		return text;
	}

	private static string fromNumber(long num)
	{
		string text = num.ToString();
		int num2 = text.IndexOf('.');
		if (num2 >= 0 && text.Length > num2 + 5)
		{
			return text.Substring(0, num2 + 5);
		}
		return text;
	}

	public static string fromByteArray(byte[] o)
	{
		if (o.Length <= 20)
		{
			return BitConverter.ToString(o.ToArray()).Replace("-", "").ToLower();
		}
		MemoryStream memoryStream = new MemoryStream(o);
		List<byte> list = new List<byte>();
		for (int i = 0; i < 20; i++)
		{
			memoryStream.Position = o.Length / 20 * i;
			list.Add(new BinaryReader(memoryStream).ReadByte());
		}
		return BitConverter.ToString(list.ToArray()).Replace("-", "").ToLower();
	}

	private static string getTicketValue(object[] o)
	{
		foreach (object obj in o)
		{
			if (obj is TicketHeader)
			{
				return ((TicketHeader)obj).Ticket.Split(',').Last().Substring(0, 5);
			}
		}
		return null;
	}
}
