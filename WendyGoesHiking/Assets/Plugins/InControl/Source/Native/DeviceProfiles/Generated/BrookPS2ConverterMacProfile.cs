namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class BrookPS2ConverterMacProfile : Xbox360DriverMacProfile
	{
		public BrookPS2ConverterMacProfile()
		{
			Name = "Brook PS2 Converter";
			Meta = "Brook PS2 Converter on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0c12,
					ProductID = 0x08f1,
				},
			};
		}
	}

	// @endcond
}


