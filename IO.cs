using System.Text.RegularExpressions;

namespace ConsoleCalculatorGIT
{
    internal static class IO
    {
        // This class handles all input and output between the console and the end user
        // It validates the input before returning it to the calling method,
        // and formats the output before writing to the console.
        // It is marked as static because it will not handle instanced data.

        // Usage examples:
        // IO.Write("1 {+} 2 {*} 3 {/} 4 {=} 5") // Will print the sequence and highlight the operators;
        // if (IO.Confirm("Vill du skriva in ett nytt värde?")) { */ Do something */ };
        // switch (IO.Menu("Visa meny.", "Alt 1", "Alt 2", "Alt 3")) { /* case 0: ... */ };

        // Set up the default color scheme to use
        public const ConsoleColor DefaultForegroundColor = ConsoleColor.Gray;
        public const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
        public const ConsoleColor DefaultHighlightColor = ConsoleColor.Blue;
        public const ConsoleColor DefaultSystemColor = ConsoleColor.DarkGray;
        public const ConsoleColor DefaultSystemHighlightColor = ConsoleColor.Yellow;
        public const ConsoleColor DefaultErrorColor = ConsoleColor.Red;

        // Set up default input keys
        public const ConsoleKey DefaultYesKey = ConsoleKey.J;
        public const ConsoleKey DefaultNoKey = ConsoleKey.N;

        // Set up menu behaviour (Note: Is set to static readonly and not const in order to disable unreachable code warning)
        public static readonly bool WrapArroundMenu = true;

        // Public methods

        public static void Write() => Write("\n", false);

        public static void Write(string text, bool newLine = true) =>
            Write(text, DefaultForegroundColor, DefaultBackgroundColor, DefaultHighlightColor, newLine);

        public static void Write(string text, ConsoleColor foregroundColor, bool newLine = true) =>
            Write(text, foregroundColor, DefaultBackgroundColor, DefaultHighlightColor, newLine);

        public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor,
                                 bool newLine = true) =>
            Write(text, foregroundColor, backgroundColor, DefaultHighlightColor, newLine);

        public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor,
                                 ConsoleColor highlightColor, bool newLine = true)
        {
            // Split the input into sequences of different colors (enables sequence highlighting)
            var sequences = ExtractSequences(text, foregroundColor, highlightColor);

            // Iterate through all sequences and write them to the console
            foreach (var sequence in sequences)
            {
                Console.ForegroundColor = sequence.Color;
                Console.BackgroundColor = backgroundColor;
                Console.Write(sequence.Text);
            }

            if (newLine) Console.WriteLine();
        }

        private static void WriteSystemText(string text)
        {
            // Write a system text with default color scheme
            if (text != "") Write(text, DefaultSystemColor, DefaultBackgroundColor, DefaultSystemHighlightColor, false);
        }

        public static void Wait(string text = "")
        {
            WriteSystemText(text);

            Console.ReadKey(true);

            // Write a line terminator if a text was supplied
            if (text != "") Console.WriteLine();
        }

        public static ConsoleKey ExpectKey(string text, params ConsoleKey[] acceptedKeys)
        {
            // Check for parameter error
            if (acceptedKeys.Length == 0) throw new Exception("Parameter acceptedKeys must contain at least one member");

            Write(text);

            // Stay inside a loop indefinitely until we are satisfied with the input
            while (true)
            {
                // Write the available key options
                WriteKeyOptions(text, acceptedKeys);

                // Wait for the user to press any key, then check if the key was valid
                ConsoleKey key = Console.ReadKey(true).Key;
                if (acceptedKeys.Contains(key))
                {
                    WriteValidInput(key.ToString());
                    return key;
                }
                else
                {
                    WriteInvalidInput(key.ToString());
                }
            }
        }

        public static bool Confirm(string text) => ExpectKey(text, DefaultYesKey, DefaultNoKey) == DefaultYesKey;

        public static void Clear() => Clear(DefaultBackgroundColor);

        public static void Clear(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
        }

        public static int Menu(string text, int defaultIndex, params string[] menuItems)
        {
            // This method prints a navigatable menu which lets the end user choose
            // between options that is passed in to the method.
            // The method refreshes the currently- and last selected options instead
            // of rewriting the entire menu in order to avoid glitching.
            // The index of the selected menu option is returned when selected.

            // Clear the console and write the message and a navigation explanation
            Clear();
            Write(text);
            Console.CursorVisible = false;

            WriteSystemText("Tryck på {upp}- eller {nedpil} för att navigera i menyn.\n" +
                              "Tryck på {enter} för att välja alternativ.\n\n");

            // Check for errors regarding the parameter defaultIndex
            if (defaultIndex < 0) throw new ArgumentOutOfRangeException("The index must be 0 or greater.");
            if (defaultIndex >= menuItems.Length) throw new ArgumentOutOfRangeException("The index must be less than or equal to the number of menu options.");

            // Write the menu items
            int index = defaultIndex;
            int[] consoleIndices = new int[menuItems.Length]; // Store the Y-position of the start of the options

            for (int i = 0; i < menuItems.Length; i++)
            {
                consoleIndices[i] = Console.CursorTop;
                Write(menuItems[i], i == index ? DefaultBackgroundColor : DefaultForegroundColor,
                                    i == index ? DefaultForegroundColor : DefaultBackgroundColor, true);
            }

            // Stay inside a loop until the user presses enter
            while (true)
            {
                // Wait for the user to press a key
                bool exitLoop = false; // <-- Using a flag here because we cannot break out of a switch and loop in one statement
                int lastIndex = index;
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        index--;
                        break;
                    case ConsoleKey.DownArrow:
                        index++;
                        break;
                    case ConsoleKey.Enter:
                        exitLoop = true;
                        break;
                }

                // If the user pressed enter: Break the loop
                if (exitLoop) break;

                // Check if the index is out of bounds and either wrap arround or stay there (based on the configuration)
                if (index < 0)
                {
                    if (WrapArroundMenu)
                        index = menuItems.Length - 1;
                    else
                        index = 0;
                }
                if (index > menuItems.Length - 1)
                {
                    if (WrapArroundMenu)
                        index = 0;
                    else
                        index = menuItems.Length - 1;
                }

                // Rewrite the last selected menu option (in order to avoid to rewrite the entire menu)
                Console.SetCursorPosition(0, consoleIndices[lastIndex]);
                Write(menuItems[lastIndex], false);

                // Rewrite the newly selected menu option (--""--)
                Console.SetCursorPosition(0, consoleIndices[index]);
                Write(menuItems[index], DefaultBackgroundColor, DefaultForegroundColor, false);
            }

            // Restore the console and return the selected index
            Clear();
            Console.CursorVisible = true;
            return index;
        }

        public static string GetString(string text, bool acceptEmpty = false)
        {
            // Stay inside a loop until we are satisfied with the input from the end user
            while (true)
            {
                WriteGetText(text);

                // Store the cursor location of the starting point of the answer
                var (Left, Top) = Console.GetCursorPosition();

                // Wait for the user to enter their input
                string? input = Console.ReadLine();

                // Validate and return the input
                if (input != null && !((acceptEmpty || input == "") && !acceptEmpty)) // <-- Note: This could probably be made clearer
                    return input;
                else
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input", DefaultErrorColor);
                }
            }
        }

        public static double GetDouble(string text)
        {
            // Stay inside a loop until we are satisfied with the input from the end user
            while (true)
            {
                WriteGetText(text);

                // Store the cursor location of the starting point of the answer
                var (Left, Top) = Console.GetCursorPosition();

                // Wait for the user to enter their input
                string? input = Console.ReadLine();

                // Validate and return the input
                try
                {
                    if (input == null) throw new NullReferenceException();

                    double value = Convert.ToDouble(input.Replace('.', ','));
                    return value;
                }
                catch (OverflowException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Värdet överskrider datatypens kapacitet", DefaultErrorColor);
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Kunde inte konvertera till ett nummer", DefaultErrorColor);
                }
                catch (NullReferenceException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Värdet kan inte vara null", DefaultErrorColor);
                }
            }
        }

        // Private methods

        private static void WriteKeyOptions(string text, ConsoleKey[] acceptedKeys)
        {
            // Create a string that displays the option of keys that are available to the user
            string keyOptionString = "Väntar på ditt svar (";
            for (int i = 0; i < acceptedKeys.Length; i++)
            {
                keyOptionString += "{" + acceptedKeys[i].ToString() + "}"; // The curly braces is for highlighting the key

                if (i != acceptedKeys.Length - 1)
                    keyOptionString += "/";
                else
                    keyOptionString += ")";
            }

            WriteSystemText(keyOptionString);
        }

        private static void WriteGetText(string text)
        {
            string check = text.TrimEnd();

            if (check.Length > 0 && (check[^1] != ':' && check[^1] != '?' && check[^1] != '='))
                check += ": ";

            if (check.Length > 0 && check[^1] != ' ') check += " ";

            Write(check, false);
        }

        private static void WriteValidInput(string input) =>
            Write(": {" + input + "}", DefaultSystemColor, DefaultBackgroundColor, DefaultForegroundColor);

        private static void WriteInvalidInput(string input) =>
            Write(": {" + input + "}", DefaultSystemColor, DefaultBackgroundColor, DefaultErrorColor);

        private static (string Text, ConsoleColor Color)[] ExtractSequences(string text,
            ConsoleColor defaultColor, ConsoleColor highlightColor)
        {
            // This method is using regular expressions in order to separate sequences of chars that
            // are encapsulated inside curly braces, in order to highlight them in another color.
            // The method also checks for escape characters (i.e. {{word}} should be written as {word}
            // in the default color).

            var regex = new Regex("{[^}]+}+|[^{}]+");
            // {[^}]*}+ checks for a sequence that begins with { and takes everything that comes next
            //          until a } is encountered
            // |        OR:
            // [^{}]+   checks for a sequence that does not contain { or }

            // Find the sequences that match with this pattern
            MatchCollection matches = regex.Matches(text);

            // Create an array of a tuple containing the sequence and the color of that sequence
            var result = new (string, ConsoleColor)[matches.Count];

            // Set the value of each of the array members
            for (int i = 0; i < matches.Count; i++)
            {
                string sequence = matches[i].Value;
                ConsoleColor color = defaultColor;

                // Check if the sequence should be a highlighted word
                if (sequence.Contains("{{"))
                {
                    // Escaped sequence: Should be printed at default color and include a set of curly braces
                    sequence = sequence.Replace("{{", "{").Replace("}}", "}"); // Remove one set of curly braces
                }
                else if (sequence.Contains('{'))
                {
                    // Highlighted sequence
                    sequence = sequence.Replace("{", "").Replace("}", ""); // Remove the curly braces
                    color = highlightColor;
                }

                result[i] = (sequence, color);
            }

            return result;
        }

    }
}