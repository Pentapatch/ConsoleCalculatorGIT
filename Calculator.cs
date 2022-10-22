namespace ConsoleCalculatorGIT
{
    /// <summary>This is the main program. Call the <see cref="Run"/> method from the entry point in order to run the calculator.</summary>
    internal static class Calculator
    {
        // ###########################################
        // ## Private fields, properties and consts ##
        // ###########################################

        private static List<string> history = new();

        /// <summary>Set this flag to true when the help section has automatically be shown once.</summary>
        private static bool helpShown = false;

        /// <summary>Set this flag to true in order to exit from all context menus.</summary>
        private static bool ExitToMain { get; set; } = false;

        private const string DefaultTitle = "S.A. Calculator by Dennis Hankvist";

        // ####################
        // ## Public methods ##
        // ####################

        public static void Run()
        {
            Console.Title = DefaultTitle;
            DisplayWelcomeMessage();
            MainMenu();
        }

        // #####################
        // ## Private methods ##
        // #####################

        /// <summary>Write a welcoming message to the console.</summary>
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

        /// <summary>Display the main menu.
        ///          Will never return from this method. The user can choose to exit the application.</summary>
        private static void MainMenu()
        {
            // Stay inside a loop until the user exits the application via the menu
            int index = 0; // <-- Store the index of the currently selected option outside of the loop
            while (true)
            {
                Console.Title = DefaultTitle;

                // Display a menu to let the user choose the next action
                index = IO.Menu("Välj vad du vill göra härnäst.", index,
                                "Starta ny beräkning",
                                "Visa historik",
                                "Visa hjälp",
                                "Avsluta programmet");

                switch (index)
                {
                    case 0: // Starta ny beräkning
                        NewCalculationPrompt();
                        break;
                    case 1: // Visa historik
                        ViewHistory(true);
                        break;
                    case 2: // Visa hjälp
                        ViewHelp();
                        break;
                    case 3: // Avsluta programmet
                        if(IO.Confirm("Är du säker på att du vill avsluta?")) Environment.Exit(0);
                        break;
                }

                ExitToMain = false; // Reset flag
            }
        }

        /// <summary>Print a help section to the console.</summary>
        private static void ViewHelp()
        {
            Console.Title = $"{DefaultTitle} - Hjälpavsnitt";

            // Reset the helpShown flag so that this method won't automatically be called again
            helpShown = true;

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

            IO.Write("DECIMALTAL", IO.DefaultHighlightColor);
            IO.Write("Kalkylatorn kan hantera decimaltal genom att använda {,} tecknet.",
                     IO.DefaultForegroundColor, IO.DefaultBackgroundColor, ConsoleColor.Cyan);
            IO.Write("Exempel: {2,25 * 12,75} är tillåtet\n",
                     ConsoleColor.DarkGray, IO.DefaultBackgroundColor, ConsoleColor.Cyan);

            IO.Wait("Tryck på valfri knapp för att fortsätta...");
        }

        private static void NewCalculationPrompt()
        {
            // Display the help section if it has not been viewed already
            if (!helpShown) ViewHelp();

            while (true)
            {
                Console.Title = $"{DefaultTitle} - Ny beräkning";

                // Let the end user enter their calculation
                IO.Clear();
                string test = IO.GetString("Skriv in ditt uttryck", true);

                // Check if we should abort (user left an empty answer)
                if (test == "") return;

                // Try to parse the expresion
                try
                {
                    // Tokenize the input
                    List<string> tokens = ExpressionParser.TokenizeInput(test);

                    // Parse the expression using the tokens
                    double result = ExpressionParser.ParseExpression(tokens);

                    // Print the result to the console
                    IO.Clear();
                    string resultString = ExpressionParser.FormatExpression(tokens) + " {=} " + result.ToString();

                    // Store the result in the history list
                    history.Add(resultString);

                    // Display a sub menu and let the user chose their next step
                    CalculationContextMenu($"Resultat: {resultString}");

                    // Exit the method
                    return;
                }
                catch (DivideByZeroException)
                {
                    IO.Write("Ogiltig inmatning: Det går inte att dividera med noll.", ConsoleColor.Red);
                }
                catch (Exception e)
                {
                    if (e.Message != "")
                        IO.Write($"Ogiltig inmatning: {e.Message}", ConsoleColor.Red);
                    else
                        IO.Write("Ogiltig inmatning", ConsoleColor.Red);
                }

                // Wait for the user to acknowledge the error
                IO.Wait("Tryck på valfri knapp för att försöka igen..");
            }
        }

        private static void CalculationContextMenu(string result)
        {
            int index = 0;
            while (!ExitToMain)
            {
                Console.Title = $"{DefaultTitle} - Beräkningsresultat";

                // Display the context menu and store the selected index
                index = IO.Menu(result, index, "Gör en ny beräkning",
                                               "Visa beräkningssteg",
                                               "Visa historik",
                                               "Gå tillbaka till huvudmenyn");

                // Select what to do based on what option the user chosed
                switch (index)
                {
                    case 0: // Gör en ny beräkning
                        NewCalculationPrompt();
                        return;
                    case 1: // Visa beräkningssteg
                        IO.Write("Den här funktionen är tyvärr inte implementerad ännu.", ConsoleColor.Red);
                        IO.Wait("Tryck på valfri knapp för att fortsätta..");
                        break;
                    case 2: // Visa historik
                        ViewHistory(false);
                        break;
                    case 3: // Gå tillbaka till huvudmenyn
                        ExitToMain = true;
                        return;
                }
            }
        }

        /// <summary>Displays the history list and displays a menu that lets the user choose to clear the list,
        ///          go back or go back to the main menu.</summary>
        /// <param name="fromMainMenu">Set to true if this method is called from within the main menu (will
        ///                            disable the return to main menu option).</param>
        private static void ViewHistory(bool fromMainMenu)
        {
            // Set up the default option texts
            const string DefaultGoBack = "Gå tillbaka";
            const string DefaultGoBackToMain = "Tillbaka till huvudmenyn";
            const string DefaultClearHistory = "Rensa historiken";

            int index = history.Count == 0 ? 0 : 1; // <-- Select "Go back" by default
            while (true)
            {
                Console.Title = $"{DefaultTitle} - Historik";

                // Create a string that contains all history in descending order
                string text = "Visar historik:\n";

                for (int i = history.Count - 1; i >= 0; i--)
                    text += $"{i + 1}) {history[i]}\n";

                if (history.Count == 0) text += "Det finns ingen historik att visa..\n";

                // Create a list of options
                List<string> options = new() { DefaultGoBack };
                if (!fromMainMenu) options.Add(DefaultGoBackToMain);
                if (history.Count > 0) options.Insert(0, DefaultClearHistory);

                // Display the menu
                index = IO.Menu(text, index, options.ToArray());
                switch (options[index])
                {
                    case DefaultGoBack:
                        return;
                    case DefaultGoBackToMain:
                        ExitToMain = true;
                        return;
                    case DefaultClearHistory:
                        if (IO.Confirm("Är du säker på att du vill rensa historiken"))
                        {
                            history.Clear();
                            index = 0; // Reset the index, because the number of options might changes at the next iteration
                        }
                        break;
                }
            }
        }

    }
}