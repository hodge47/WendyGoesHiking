namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriPadEXTurboControllerMacProfile : Xbox360DriverMacProfile
	{
		public HoriPadEXTurboControllerMacProfile()
		{
			Name = "Hori Pad EX Turbo Controller";
			Meta = "Hori Pad EX Turbo Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x000c,
				},
			};
		}
	}

	// @endcond
}


