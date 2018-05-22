using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoSAMFramework.Portable.LogProtocol;
using SwitchGame.Shared.SaveData;

namespace SwitchGame.Shared.Network.Backend
{
	public interface ISGServerAPI
	{
		Task LogClient(UserSettings profile, SAMLogEntry entry);
	}

	#pragma warning disable 1998
	public class DummySGServerAPI : ISGServerAPI
	{
		public async Task LogClient(UserSettings profile, SAMLogEntry entry)
		{
			//
		}
	}
#pragma warning restore 1998
}