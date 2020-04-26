using System;
using System.Collections.Generic;
using System.Text;

namespace VkDonateApi
{
	public class RequestException : Exception
	{
		public RequestException(string text) : base(text) { }
	}
}
