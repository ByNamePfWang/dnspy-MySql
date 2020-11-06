using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace HeatMeterPrePay.Properties
{
	// Token: 0x02000073 RID: 115
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000552F8 File Offset: 0x000534F8
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040006F2 RID: 1778
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
