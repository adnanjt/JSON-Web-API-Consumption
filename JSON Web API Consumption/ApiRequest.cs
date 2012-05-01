﻿using System;
using System.Net;

namespace JSON_Web_API_Consumption
{
	public class ApiRequest
	{
		readonly static WebClient WebClient = new WebClient();

		public static string GetJson(Uri uri)
		{
			return WebClient.DownloadString(uri);
		}
	}
}
