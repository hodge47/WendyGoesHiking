namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class XboxOneSMacProfile : NativeInputDeviceProfile
	{
		// We need this explicit profile instead of inheriting XboxOneDriverMacProfile
		// because the Guide button is currently broken on Mac and we need to make it Passive.
		//
		public XboxOneSMacProfile()
		{
			Name = "Microsoft Xbox One S Controller";
			Meta = "Microsoft Xbox One S Controller on Mac";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.XboxOne;

			IncludePlatforms = new[]
			{
				"OS X"
			};

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x045e,
					ProductID = 0x02ea,
				},
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button( 11 )
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button( 12 )
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button( 13 )
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button( 14 )
				},
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button( 0 )
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button( 1 )
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button( 2 )
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button( 3 )
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button( 8 )
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button( 9 )
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button( 6 )
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button( 7 )
				},
				new InputControlMapping
				{
					Handle = "View",
					Target = InputControlType.View,
					Source = Button( 5 )
				},
				new InputControlMapping
				{
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = Button( 4 )
				},
				new InputControlMapping
				{
					Handle = "Guide",
					Target = InputControlType.System,
					Source = Button( 10 ),
					Passive = true, // gets stuck with Xbox One S controller.
				}
			};

			AnalogMappings = new[]
			{
				LeftStickLeftMapping( 0 ),
				LeftStickRightMapping( 0 ),
				LeftStickUpMapping( 1 ),
				LeftStickDownMapping( 1 ),

				RightStickLeftMapping( 2 ),
				RightStickRightMapping( 2 ),
				RightStickUpMapping( 3 ),
				RightStickDownMapping( 3 ),

				LeftTriggerMapping( 4 ),
				RightTriggerMapping( 5 ),
			};
		}
	}

	// @endcond
}
