using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

class Program
{
	// PInvoke for FindWindow
	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	// PInvoke for SetForegroundWindow
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetForegroundWindow(IntPtr hWnd);

	// PInvoke for SendInput
	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

	[DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	// PInvoke for mouse_event to simulate mouse click
	[DllImport("user32.dll")]
	private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

	private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
	private const uint MOUSEEVENTF_LEFTUP = 0x04;

	// Define the input structure
	[StructLayout(LayoutKind.Sequential)]
	struct INPUT
	{
		public uint type;
		public InputUnion u;
	}

	[StructLayout(LayoutKind.Explicit)]
	struct InputUnion
	{
		[FieldOffset(0)] public MOUSEINPUT mi;
		[FieldOffset(0)] public KEYBDINPUT ki;
		[FieldOffset(0)] public HARDWAREINPUT hi;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct KEYBDINPUT
	{
		public ushort wVk;
		public ushort wScan;
		public uint dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	struct MOUSEINPUT
	{
		public int dx;
		public int dy;
		public uint mouseData;
		public uint dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	struct HARDWAREINPUT
	{
		public uint uMsg;
		public ushort wParamL;
		public ushort wParamH;
	}

	private const uint INPUT_KEYBOARD = 1;
	private const uint KEYEVENTF_KEYUP = 0x0002;
	private const ushort VK_W = 0x57;  // Virtual key code for 'W'
	private const ushort VK_S = 0x53; // virtual key code for 'S'
	private const ushort VK_MENU = 0x12;  // Alt key
	private const ushort VK_ENTER = 0x0D; // Enter key

	static void PressAltEnter()
	{

		INPUT[] inputs = new INPUT[4];

		inputs[0].type = INPUT_KEYBOARD;
		inputs[0].u.ki = new KEYBDINPUT
		{
			wVk = VK_MENU,
			dwFlags = 0
		};

		// Press Enter down
		inputs[1].type = INPUT_KEYBOARD;
		inputs[1].u.ki = new KEYBDINPUT
		{
			wVk = VK_ENTER,
			dwFlags = 0
		};

		// Release Enter
		inputs[2].type = INPUT_KEYBOARD;
		inputs[2].u.ki = new KEYBDINPUT
		{
			wVk = VK_ENTER,
			dwFlags = KEYEVENTF_KEYUP
		};

		// Release Alt
		inputs[3].type = INPUT_KEYBOARD;
		inputs[3].u.ki = new KEYBDINPUT
		{
			wVk = VK_MENU,
			dwFlags = KEYEVENTF_KEYUP
		};

		// Send the input sequence
		SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
	}

		public static void Disclaimer()
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("[✖] If your game crashes, this program will not continue");
			Console.ResetColor();
			Console.WriteLine();
			Console.WriteLine();
		}
	

		public static void Header()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("## ##   ### ##   ##  ###    ####    ## ##   ### ###  ### ##    ## ##              ##     ##  ###  #### ##   ## ##         ##   ##     ### ###  ##  ###  ");
			Console.WriteLine("##   ##   ##  ##  ##   ##     ##    ##   ##   ##  ##   ##  ##  ##   ##              ##    ##   ##  # ## ##  ##   ##       #      ##     ##  ##  ##  ##  ");
			Console.WriteLine("##        ##  ##  ##   ##     ##    ####      ##       ##  ##  ####               ## ##   ##   ##    ##     ##   ##      ####  ## ##    ##      ## ##    ");
			Console.WriteLine("##        ## ##   ##   ##     ##     #####    ## ##    ## ##    #####             ##  ##  ##   ##    ##     ##   ##      ####  ##  ##   ## ##   ## ##    ");
			Console.WriteLine("##        ## ##   ##   ##     ##        ###   ##       ## ##       ###            ## ###  ##   ##    ##     ##   ##      ####  ## ###   ##      ## ###   ");
			Console.WriteLine("##   ##   ##  ##  ##   ##     ##    ##   ##   ##  ##   ##  ##  ##   ##            ##  ##  ##   ##    ##     ##   ##            ##  ##   ##      ##  ##   ");
			Console.WriteLine("## ##   #### ##   ## ##     ####    ## ##   ### ###  #### ##   ## ##            ###  ##   ## ##    ####     ## ##   ####     ###  ##  ####     ##  ###  ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("[Alpha v1.0] Developer Edition");
			Console.ResetColor();
		}

		public static void MapSelect(string menuOption)
		{
			int xPos = 0;
			int yPos = 0;

			// Display map options
			Console.WriteLine("[1] Factory");
			Console.WriteLine("[2] Interchange");
			Console.WriteLine("[3] Customs");
			Console.WriteLine("[4] Reserve");
			Console.WriteLine("[5] Woods");
			Console.WriteLine("[6] Lighthouse");
			Console.WriteLine("[7] Shoreline");
			Console.WriteLine();

			// Adjust the coordinates based on the map selected
			switch (menuOption)
			{
				case "1": // Factory
					xPos = 671;
					yPos = 391;  // Updated to a reasonable Y value
					break;
				case "2": // Interchange
					xPos = 871;
					yPos = 504;
					break;
				case "3": // Customs
					xPos = 671;
					yPos = 391;
					break;
				case "4": // Reserve
					xPos = 631;
					yPos = 465;
					break;
				case "5": // Woods
					xPos = 692;
					yPos = 267;
					break;
				case "6": // Lighthouse
					xPos = 455;
					yPos = 429;
					break;
				case "7": // Shoreline
					xPos = 509;
					yPos = 568;
					break;
				default:
					Console.WriteLine("Invalid option.");
					return;
			}

			// Move cursor and click at the specified coordinates
			SetCursorPos(xPos, yPos);
			mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);
			mouse_event(MOUSEEVENTF_LEFTUP, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);

			Console.WriteLine($"Map selected: {menuOption}, cursor moved to: ({xPos}, {yPos})");
		}

		public static void Menu()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			while (true)
			{
				Console.WriteLine("[1] Auto-AFK ");
				Console.WriteLine("[2] KD Dropper");
				Console.WriteLine("Please select either [1] or [2]:");
				var input = Console.ReadLine();

				if (input == "1")
				{
					AutoAFK().Wait();  // Call Auto-AFK function
					break;
				}
				else if (input == "2")
				{
					KDDropper().Wait();
					break;
				}
				else
				{
					Console.WriteLine("Invalid option. Please select either [1] or [2].");
				}
				Console.ResetColor();
			}
		}

		public static async Task AutoAFK()
		{
			string windowTitle = "EscapeFromTarkov";

			IntPtr hWnd = FindWindow(null, windowTitle);

			if (hWnd == IntPtr.Zero)
			{
				Console.WriteLine("Window not found!");
				return;
			}

			while (true)
			{
			PressAltEnter();

				if (SetForegroundWindow(hWnd))
				{
					Console.WriteLine("~~Tarkov Window has been found~~");

					SendKeyDown(VK_W);
					Console.WriteLine("'W' key down sent, moving forward...");

					await Task.Delay(2000); // 2second delay 

					SendKeyUp(VK_W);
					Console.WriteLine("'W' key up sent, stopped moving forward");

					SendKeyDown(VK_S);
					Console.WriteLine(" 'S' Key up sent, moving backward");

					await Task.Delay(2000);
					SendKeyUp(VK_S);
					Console.WriteLine("'S' Key down sent, stopped moving backward");
				}
				else
				{
					Console.WriteLine("Failed to activate the window.");
				}

				await Task.Delay(20000); // 60 second delay
			}
		}

		public static async Task KDDropper()
		{

		string windowTitle = "EscapeFromTarkov";

		IntPtr hWnd = FindWindow(null, windowTitle);

		Console.WriteLine("~~Tarkov Window has been found~~");
		Console.WriteLine();
		Console.WriteLine("Select a map: ");
		Console.WriteLine("[1] Factory");
		Console.WriteLine("[2] Interchange");
		Console.WriteLine("[3] Customs");
		Console.WriteLine("[4] Reserve");
		Console.WriteLine("[5] Woods");
		Console.WriteLine("[6] Lighthouse");
		Console.WriteLine("[7] Shoreline");
		Console.WriteLine();
		string mapOption = Console.ReadLine();


		if (hWnd == IntPtr.Zero)
		{
			Console.WriteLine("Window not found!");
			return;
		}

		if (SetForegroundWindow(hWnd))
		{
			await Task.Delay(10000); // 60 second delay
			MapSelect(mapOption);

		}
	}

	static void Main(string[] args)
	{
		Disclaimer();
		Header();
		Menu();
	}

	// Simulate key press down
	private static void SendKeyDown(ushort keyCode)
	{
		INPUT[] inputs = new INPUT[1];
		inputs[0].type = INPUT_KEYBOARD;
		inputs[0].u.ki = new KEYBDINPUT
		{
			wVk = keyCode,
			dwFlags = 0, // 0 for key down
			time = 0,
			dwExtraInfo = IntPtr.Zero
		};
		SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
	}

	// Simulate key release
	private static void SendKeyUp(ushort keyCode)
	{
		INPUT[] inputs = new INPUT[1];
		inputs[0].type = INPUT_KEYBOARD;
		inputs[0].u.ki = new KEYBDINPUT
		{
			wVk = keyCode,
			dwFlags = KEYEVENTF_KEYUP, // KEYEVENTF_KEYUP for key release
			time = 0,
			dwExtraInfo = IntPtr.Zero
		};
		SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
	}
}
