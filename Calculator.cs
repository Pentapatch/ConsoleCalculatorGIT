namespace ConsoleCalculatorGIT
{
    internal static class Calculator
    {
        // Private fields, properties and consts
        
        private static List<string> history = new();

        private static bool helpShown = false;

        private static bool ExitToMain { get; set; } = false;

        private const string DefaultTitle = "S.A. Calculator by Dennis Hankvist";

        // Public methods

        public static void Run()
        {
            Console.Title = DefaultTitle;
            DisplayWelcomeMessage();
            MainMenu();
        }

        private static void DisplayWelcomeMessage()
        {
            // This method writes a welcome message to the end user
            IO.Clear();
            IO.Write(@" {_____}   {___}   _____       _            _       _             ");
            IO.Write(@"{/  ___|} {/ _ \} /  __ \     | |          | |     | |            ");
            IO.Write(@"{\ `--.} {/ /_\ \}| /  \/ __ _| | ___ _   _| | __ _| |_ ___  _ __ ");
            IO.Write(@" {`--. \}{|  _  |}| |    / _` | |/ __| | | | |/ _` | __/ _ \| '__|");
            IO.Write(@"{/\__/ /}{| | | |}| \__/\ (_| | | (__| |_| | | (_| | || (_) | |   ");
            IO.Write(@"{\____}(_){_| |_}(_)____/\__,_|_|\___|\__,_|_|\__,_|\__\___/|_|   ");
            IO.Write("");
            IO.Write("Välkommen till Dennis Hankvist's semi-avancerade konsollkalkylator!");
            IO.Write("");
            IO.Write("En betygsgrundande inlämningsuppgift inom Programmering Grund.");
            IO.Write("TUC Yrkeshögskola, Systemutvecklare.NET, HT2022");
            IO.Wait("Tryck på valfri knapp för att fortsätta...");
        }

        private static void MainMenu()
        {
            // Stay inside a loop until the end user exits the application via the menu
            int index = 0;
            while (true)
            {
                Console.Title = DefaultTitle;

                // Display a menu to let the user choose the next action
                index = IO.Menu("Välj vad du vill göra härnäst.", index,
                                "Starta ny beräkning",
                                "Visa beräkningshistorik",
                                "Visa hjälp",
                                "Avsluta programmet");

                switch (index)
                {
                    case 0: // Starta ny beräkning
                        NewCalculation();
                        break;
                    case 1: // Visa beräkningshistorik

                        break;
                    case 2: // Visa hjälp
                        ViewHelp();
                        break;
                    case 3: // Avsluta programmet
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }

                ExitToMain = false; // Reset flag
            }
        }

        private static void ViewHelp()
        {
            // This method writes a help section to the end user

            Console.Title = $"{DefaultTitle} - Hjälpavsnitt";

            helpShown = true; // Do not automatically show this if already shown once

            IO.Clear();
            IO.Write("MULTI-UTTRYCK", IO.DefaultHighlightColor);
            IO.Write("Kalkylatorn kan hantera flera matematiska operatorer i samma uttryck (expression).");
            IO.Write("Exempel: {2 + 2 * 3 / 4 % 5} är tillåtet\n",
                     ConsoleColor.DarkGray, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Write("OPERATORER SOM STÖDS", IO.DefaultHighlightColor);
            IO.Write("{+} addition, {-} subtraktion, {*} multiplicering, {/} dividering, {%} modulus samt {^} exponent\n",
                     IO.DefaultForegroundColor, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Write("NEGATIVA TAL", IO.DefaultHighlightColor);
            IO.Write("Kalkylatorn hanterar även negativa tal");
            IO.Write("Exempel: {-2 - -2} är tillåtet\n",
                     ConsoleColor.DarkGray, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Write("WHITESPACE", IO.DefaultHighlightColor);
            IO.Write("Kalkylatorn ignorerar all white space.");
            IO.Write("Exempel: {7--2*5} är tillåtet\n",
                     ConsoleColor.DarkGray, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Write("AVBRYTA EN BERÄKNING", IO.DefaultHighlightColor);
            IO.Write("Lämna ett tomt svar och tryck på {enter} för att avbryta en beräkning.\n",
                     IO.DefaultForegroundColor, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Wait("Tryck på valfri knapp för att fortsätta...");
        }

        private static void NewCalculation()
        {
            // Display the help section if it has not been viewed already
            if (!helpShown) ViewHelp();

            Console.Title = $"{DefaultTitle} - Ny beräkning";

            // Let the end user enter their calculation
            IO.Clear();
            string test = IO.GetString("Skriv in ditt uttryck", true);

            // Check if we should abort (left an empty answer)
            if (test == "") return;


            IO.Wait();
        }

    }
}