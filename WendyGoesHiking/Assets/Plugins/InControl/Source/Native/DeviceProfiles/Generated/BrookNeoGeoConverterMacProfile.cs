namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class BrookNeoGeoConverterMacProfile : Xbox360DriverMacProfile
	{
		public BrookNeoGeoConverterMacProfile()
		{
			Name = "Brook NeoGeo Converter";
			Meta = "Brook NeoGeo Converter on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0c12,
					ProductID = 0x07f4,
				},
			};
		}
	}

	// @endcond
}


