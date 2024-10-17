using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Drawing;

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

	// used to access screen metrics
	[DllImport("user32.dll")]
	private static extern int GetSystemMetrics(int nIndex);

	const int SM_CXSCREEN = 0;
	const int SM_CYSCREEN = 1;

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
	private const ushort VK_MENU = 0x12;  // Alt key
	private const ushort VK_ENTER = 0x0D; // Enter key
	private const ushort VK_ESCAPE = 0x1B; // escape key

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

	public static void SimulateEscapeKey()
	{
		INPUT[] inputs = new INPUT[2];

		inputs[0].type = INPUT_KEYBOARD;
		inputs[0].u.ki = new KEYBDINPUT
		{
			wVk = VK_ESCAPE,
			dwFlags = 0
		};

		inputs[1].type = INPUT_KEYBOARD;
		inputs[1].u.ki = new KEYBDINPUT
		{
			wVk = VK_ESCAPE,
			dwFlags = KEYEVENTF_KEYUP
		};

		SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
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
		Console.WriteLine("[Alpha v1.3]");
		Console.ResetColor();
	}

	public static void Disclaimer()
	{
		Console.ForegroundColor = ConsoleColor.DarkRed;
		Console.WriteLine("[✖] If your game crashes, this program will not continue");
		Console.ResetColor();
		Console.WriteLine();
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.ResetColor();
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine("[✖] This application does not touch your game memory");
		Console.WriteLine();
	}

	public static void Menu()
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		while (true)
		{
			Console.WriteLine("[1] Auto-AFK ");
			Console.WriteLine("[2] KD Dropper");
			var input = Console.ReadLine();

			if (input == "1")
			{
				AutoAFK().Wait();  // Call Auto-AFK function
				break;
			}
			if (input == "2")
			{
				KDDropper().Wait();
				break;
			}
			else
			{
				Console.WriteLine("Invalid option. Please select either [1] [2] or [3] .");
			}
			Console.ResetColor();
		}
	}

	public static void MapSelect(string menuOption)
	{
		int xPos = 0;
		int yPos = 0;

		// Adjust the coordinates based on the map selected
		switch (menuOption)
		{
			case "1": // Factory
				xPos = 671;
				yPos = 391;
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

	public static async Task AutoAFK()
	{
		string windowTitle = "EscapeFromTarkov";
		int screenWidth = GetSystemMetrics(SM_CXSCREEN);
		int screenHeight = GetSystemMetrics(SM_CYSCREEN);

		IntPtr hWnd = FindWindow(null, windowTitle);

		PressAltEnter();

		if (hWnd == IntPtr.Zero)
		{
			Console.WriteLine("Window not found!");
			return;
		}
		while (true)
		{
			if (SetForegroundWindow(hWnd) && screenWidth == 1920)
			{
				Console.WriteLine("~~Tarkov Window has been found~~");
				int xPos = 979;
				int yPos = 730;

				SetCursorPos(xPos, yPos);
				await Task.Delay(5000);
				mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);
				mouse_event(MOUSEEVENTF_LEFTUP, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);
				SimulateEscapeKey();
			}
			if (SetForegroundWindow(hWnd) && screenWidth == 2560)
			{
				int xPos = 1305;
				int yPos = 973;

				SetCursorPos(xPos, yPos);
				await Task.Delay(5000);
				mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);
				mouse_event(MOUSEEVENTF_LEFTUP, (uint)xPos, (uint)yPos, 0, UIntPtr.Zero);
				SimulateEscapeKey();
			}
			if (!SetForegroundWindow(hWnd))
			{
				Console.WriteLine("Failed to find Tarkov, its probably not running.");
			}
			await Task.Delay(900000);
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
}
