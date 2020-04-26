using System;
using System.Collections.Generic;
using System.Text;

namespace VkDonateApi
{
	public interface IRequest
	{
		Dictionary<string, string> GetParameters();
	}
}
