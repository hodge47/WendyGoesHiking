namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MicrosoftAdaptiveControllerMacProfile : Xbox360DriverMacProfile
	{
		public MicrosoftAdaptiveControllerMacProfile()
		{
			Name = "Microsoft Adaptive Controller";
			Meta = "Microsoft Adaptive Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x045e,
					ProductID = 0x0b0a,
				},
			};
		}
	}

	// @endcond
}


