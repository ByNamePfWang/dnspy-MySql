using System;
using System.Configuration;
using System.Windows.Forms;

namespace HeatMeterPrePay.Util
{
	// Token: 0x0200004C RID: 76
	public static class ConfigUtil
	{
		// Token: 0x060004F3 RID: 1267 RVA: 0x000515E4 File Offset: 0x0004F7E4
		public static string GetConnectionStringsConfig(string connectionName)
		{
			string executablePath = Application.ExecutablePath;
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(executablePath);
			return configuration.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0005161C File Offset: 0x0004F81C
		public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
		{
			string executablePath = Application.ExecutablePath;
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(executablePath);
			bool flag = false;
			if (configuration.ConnectionStrings.ConnectionStrings[newName] != null)
			{
				flag = true;
			}
			if (flag)
			{
				configuration.ConnectionStrings.ConnectionStrings.Remove(newName);
			}
			ConnectionStringSettings settings = new ConnectionStringSettings(newName, newConString, newProviderName);
			configuration.ConnectionStrings.ConnectionStrings.Add(settings);
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("ConnectionStrings");
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0005168C File Offset: 0x0004F88C
		public static string GetAppConfig(string strKey)
		{
			string executablePath = Application.ExecutablePath;
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(executablePath);
			foreach (string a in configuration.AppSettings.Settings.AllKeys)
			{
				if (a == strKey)
				{
					return configuration.AppSettings.Settings[strKey].Value.ToString();
				}
			}
			return null;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000516FC File Offset: 0x0004F8FC
		public static void UpdateAppConfig(string newKey, string newValue)
		{
			string executablePath = Application.ExecutablePath;
			Configuration configuration = ConfigurationManager.OpenExeConfiguration(executablePath);
			bool flag = false;
			foreach (string a in configuration.AppSettings.Settings.AllKeys)
			{
				if (a == newKey)
				{
					flag = true;
				}
			}
			if (flag)
			{
				configuration.AppSettings.Settings.Remove(newKey);
			}
			configuration.AppSettings.Settings.Add(newKey, newValue);
			configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}
