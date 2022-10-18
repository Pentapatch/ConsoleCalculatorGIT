namespace ConsoleCalculatorGIT
{
    internal static class Calculator
    {
        private static List<string> history = new();

        private static bool ExitToMain { get; set; } = false;

        public static void Run()
        {
            Console.Title = "S.A. Calculator by Dennis Hankvist";
            DisplayWelcomeMessage();
            MainMenu();
        }

        private static void DisplayWelcomeMessage()
        {
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

                // Display a menu to let the user choose the next action
                switch (IO.Menu("Välj vad du vill göra härnäst.",
                                "Starta ny beräkning",
                                "Visa beräkningshistorik",
                                "Avsluta programmet"))
                {
                    case 0: // Starta ny beräkning

                        break;
                    case 1: // Visa beräkningshistorik

                        break;
                    case 2: // Avsluta programmet
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}