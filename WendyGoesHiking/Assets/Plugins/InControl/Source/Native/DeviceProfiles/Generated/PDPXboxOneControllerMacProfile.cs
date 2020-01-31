namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PDPXboxOneControllerMacProfile()
		{
			Name = "PDP Xbox One Controller";
			Meta = "PDP Xbox One Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a4,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02cb,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x013a,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0162,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x561a,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0161,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0163,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02ab,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0160,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a8,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a2,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x015b,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a5,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02ad,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02c0,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a7,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02a6,
				},
			};
		}
	}

	// @endcond
}


