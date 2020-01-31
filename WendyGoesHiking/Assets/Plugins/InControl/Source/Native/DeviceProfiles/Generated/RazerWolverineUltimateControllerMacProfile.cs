namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class RazerWolverineUltimateControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerWolverineUltimateControllerMacProfile()
		{
			Name = "Razer Wolverine Ultimate Controller";
			Meta = "Razer Wolverine Ultimate Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1532,
					ProductID = 0x0a14,
				},
			};
		}
	}

	// @endcond
}


