namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HarmonixGuitarMacProfile : Xbox360DriverMacProfile
	{
		public HarmonixGuitarMacProfile()
		{
			Name = "Harmonix Guitar";
			Meta = "Harmonix Guitar on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0x1538,
				},
			};
		}
	}

	// @endcond
}


