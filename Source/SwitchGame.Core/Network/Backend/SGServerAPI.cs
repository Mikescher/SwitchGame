using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoSAMFramework.Portable;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.Extensions;
using MonoSAMFramework.Portable.Language;
using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.Network.REST;
using SwitchGame.Shared.DeviceBridge;
using SwitchGame.Shared.Network.Backend.QueryResult;
using SwitchGame.Shared.Resources;
using SwitchGame.Shared.SaveData;

namespace SwitchGame.Shared.Network.Backend
{
	public class SGServerAPI : SAMRestAPI, ISGServerAPI
	{
		private const int RETRY_LOGERROR           = 4;

		private readonly ISGOperatingSystemBridge bridge;

		public SGServerAPI(ISGOperatingSystemBridge b) : base(SGConstants.SERVER_URL, SGConstants.SERVER_SECRET)
		{
			bridge = b;
		}

		public async Task LogClient(UserSettings profile, SAMLogEntry entry)
		{
			try
			{
				var ps = new RestParameterSet();
				//ps.AddParameterInt("userid", profile.OnlineUserID, false);
				//ps.AddParameterHash("password", profile.OnlinePasswordHash, false);
				ps.AddParameterString("app_version", SGConstants.Version.ToString(), false);
				ps.AddParameterString("screen_resolution", bridge.DeviceResolution.FormatAsResolution(), false);
				ps.AddParameterString("exception_id", entry.Type, false);
				ps.AddParameterCompressed("exception_message", entry.MessageShort, false);
				ps.AddParameterCompressed("exception_stacktrace", entry.MessageLong, false);
				ps.AddParameterCompressed("additional_info", bridge.FullDeviceInfoString, false);

				var response = await QueryAsync<QueryResultLogClient>("log-client", ps, RETRY_LOGERROR);

				if (response == null)
				{
					SAMLog.Warning("Log_Upload_LC_NULL", "response == null");
				}
				else if (response.result == "error")
				{
					SAMLog.Warning("Log_Upload_LC_ERR", response.errormessage);
				}
			}
			catch (RestConnectionException e)
			{
				// well, that sucks
				// probably no internet
				SAMLog.Warning("Backend::LC_RCE", e); // probably no internet
			}
			catch (Exception e)
			{
				SAMLog.Error("Backend::LC_E", e);
			}
		}
	}
}
