namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HORIFightingCommanderControllerMacProfile : Xbox360DriverMacProfile
	{
		public HORIFightingCommanderControllerMacProfile()
		{
			Name = "HORI Fighting Commander Controller";
			Meta = "HORI Fighting Commander Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x0086,
				},
			};
		}
	}

	// @endcond
}


