using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HeatMeterPrePay
{
	// Token: 0x0200006B RID: 107
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class HeatMeterPrePay : ApplicationSettingsBase
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0005497F File Offset: 0x00052B7F
		public static HeatMeterPrePay Default
		{
			get
			{
				return HeatMeterPrePay.defaultInstance;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00054986 File Offset: 0x00052B86
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00054998 File Offset: 0x00052B98
		[DebuggerNonUserCode]
		[DefaultSettingValue("0")]
		[UserScopedSetting]
		public string areaNum
		{
			get
			{
				return (string)this["areaNum"];
			}
			set
			{
				this["areaNum"] = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000549A6 File Offset: 0x00052BA6
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x000549B8 File Offset: 0x00052BB8
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("1.0.0")]
		public string versionNum
		{
			get
			{
				return (string)this["versionNum"];
			}
			set
			{
				this["versionNum"] = value;
			}
		}

		// Token: 0x040006D9 RID: 1753
		private static HeatMeterPrePay defaultInstance = (HeatMeterPrePay)SettingsBase.Synchronized(new HeatMeterPrePay());
	}
}
