namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPVersusControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPVersusControllerMacProfile()
		{
			Name = "PDP Versus Controller";
			Meta = "PDP Versus Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf904,
				},
			};
		}
	}

	// @endcond
}


