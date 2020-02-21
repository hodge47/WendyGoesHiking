namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriFightStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriFightStickMacProfile()
		{
			Name = "Hori Fight Stick";
			Meta = "Hori Fight Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x000d,
				},
			};
		}
	}

	// @endcond
}


