using System.Text.RegularExpressions;

namespace ConsoleCalculatorGIT
{
    /// <summary>Contains methods that can be used to interact with the user using the <see cref="Console"/> class.
    ///          Has methods for writing to the console, reading a string or a double from the console and
    ///          printing a navigatable menu. The purpose of this class is to simplify error handling,
    ///          color management and menu operations.</summary>
    internal static class IO
    {
        // ###################
        // ## Configuration ##
        // ###################

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

        // ####################
        // ## Public methods ##
        // ####################

        /// <summary>Write a new line to the console.</summary>
        public static void Write() => Write("\n", false);

        /// <summary>Writes the specified text to the console using the default foreground-, background- and highlight colors.</summary>
        /// <param name="text">The text to write.</param>
        /// <param name="newLine">Set to true to write the current line terminator at the end of the text.</param>
        public static void Write(string text, bool newLine = true) =>
            Write(text, DefaultForegroundColor, DefaultBackgroundColor, DefaultHighlightColor, newLine);

        /// <summary>Writes the specified text to the console in a specified color, while using the default background- and highlight colors.</summary>
        /// <param name="text">The text to write.</param>
        /// <param name="newLine">Set to true to write the current line terminator at the end of the text.</param>
        /// <param name="foregroundColor">The color of the text.</param> 
        public static void Write(string text, ConsoleColor foregroundColor, bool newLine = true) =>
            Write(text, foregroundColor, DefaultBackgroundColor, DefaultHighlightColor, newLine);

        /// <summary>Writes the specified text to the console in a specified foreground- and background color, while using the default highlight color.</summary>
        /// <param name="text">The text to write.</param>
        /// <param name="newLine">Set to true to write the current line terminator at the end of the text.</param>
        /// <param name="foregroundColor">The color of the text.</param> 
        /// <param name="backgroundColor">The background color of the text.</param>
        public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor,
            bool newLine = true) =>
            Write(text, foregroundColor, backgroundColor, DefaultHighlightColor, newLine);

        /// <summary>Writes the specified text to the console in specified foreground-, highlight- and background colors.</summary>
        /// <param name="text">The text to write.</param>
        /// <param name="newLine">Set to true to write the current line terminator at the end of the text.</param>
        /// <param name="foregroundColor">The color of the text.</param> 
        /// <param name="backgroundColor">The background color of the text.</param>
        /// <param name="highlightColor">The color of the text that is highlighted (by enclosing it in {curly braces}).</param>
        public static void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor,
                                 ConsoleColor highlightColor, bool newLine = true)
        {
            // Split the input into sequences of different colors (enables sequence highlighting)
            var sequences = ExtractSequences(text, foregroundColor, highlightColor);

            // Iterate through all sequences and write them to the console
            foreach (var (Text, Color) in sequences)
            {
                Console.ForegroundColor = Color;
                Console.BackgroundColor = backgroundColor;
                Console.Write(Text);
            }

            if (newLine) Console.WriteLine();
        }

        /// <summary>Wait for the user to acknowledge something. The method returns when the user presses any key.</summary>
        /// <param name="text">The prompt message to display.</param>
        public static void Wait(string text = "")
        {
            WriteSystemText(text);

            Console.ReadKey(true);

            // Write a line terminator if a text was supplied
            if (text != "") Console.WriteLine();
        }

        /// <summary>Ask the user to press any of the specifed keys. Will not return until the user has pressed
        ///          any of the specified keys. All other key presses will be suppressed.</summary>
        /// <param name="text">The prompt message to display.</param>
        /// <param name="acceptedKeys">The key or keys to expect. Must specify at least one key.</param>
        /// <returns>The <see cref="ConsoleKey"/> value of the key that was pressed.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ConsoleKey ExpectKey(string text, params ConsoleKey[] acceptedKeys)
        {
            // Check for parameter error
            if (acceptedKeys.Length == 0) throw new ArgumentNullException(nameof(acceptedKeys));

            Write(text);

            // Stay inside a loop indefinitely until we are satisfied with the input
            while (true)
            {
                // Write the available key options
                WriteKeyOptions(acceptedKeys);

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

        /// <summary>Asks the user to confirm using the <see cref="DefaultYesKey"/> key or the <see cref="DefaultNoKey"/> key.
        ///          Will not return until either of those keys is pressed. All other keys will be suppressed.</summary>
        /// <param name="text">The prompt message to display.</param>
        /// <returns>A boolean that is true if the user selected yes, otherwise false.</returns>
        public static bool Confirm(string text) => ExpectKey(text, DefaultYesKey, DefaultNoKey) == DefaultYesKey;

        /// <summary>Clear the console and set the background color to the <see cref="DefaultBackgroundColor"/> value.</summary>
        public static void Clear() => Clear(DefaultBackgroundColor);

        /// <summary>Clear the console and set the background color.</summary>
        /// <param name="color">The <see cref="ConsoleColor"/> value to set the background to.</param>
        public static void Clear(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
        }

        /// <summary>Prints a menu with custom options that the user can navigate using the up/down keyboard keys.
        ///          Refreshes only the currently selected and the previously selected option in order to avoid flickering
        ///          in the console. Returns only when the user selects an option using the enter key.</summary>
        /// <param name="text">The mesage to display above the menu.</param>
        /// <param name="defaultIndex">The index of the menu item to be initially selected.</param>
        /// <param name="menuItems">Add the text of the menu options to print. The array must contain at least one member.</param>
        /// <returns>The zero-based index of the selected menu item.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int Menu(string text, int defaultIndex, params string[] menuItems)
        {
            // Check for errors regarding the defaultIndex parameter
            if (defaultIndex < 0) throw new ArgumentOutOfRangeException(nameof(defaultIndex));
            if (defaultIndex >= menuItems.Length) throw new ArgumentOutOfRangeException(nameof(defaultIndex));

            // Check for errors regarding the menuItems parameter
            if (menuItems.Length == 0) throw new ArgumentNullException(nameof(menuItems));

            // Clear the console and write the message and a navigation explanation
            Clear();
            Write(text);
            Console.CursorVisible = false;

            WriteSystemText("Tryck på {upp}- eller {nedpil} för att navigera i menyn.\n" +
                            "Tryck på {enter} för att välja alternativ.\n\n");

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
                    case ConsoleKey.Escape:
                        index = menuItems.Length - 1;
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

        /// <summary>Ask the user to enter a string into the console, then return the validated result.
        ///          Will print an error message if the users input is not accepted.
        ///          Will also continue to run until the user has entered an acceptable input.</summary>
        /// <param name="text">The prompt message to display.</param>
        /// <param name="acceptEmpty">(Optional) Set to true if the user is allowed to enter an empty string. Default is false.</param>
        /// <returns>A string that contains the users input.</returns>
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

        /// <summary>Ask the user to enter a number into the console, then return the validated result.
        ///          Will print an error message if the users input is not accepted.
        ///          Will also continue to run until the user has entered an acceptable input.</summary>
        /// <param name="text">The prompt message to display.</param>
        /// <returns>A double that contains the users input.</returns>
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
                // Do not throw errors, print them to the user instead and let them try again
                try
                {
                    if (input == null) throw new NullReferenceException();

                    double value = Convert.ToDouble(input.Replace('.', ','));
                    return value;
                }
                catch (OverflowException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Värdet överskrider datatypens kapacitet.", DefaultErrorColor);
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Kunde inte konvertera till ett nummer.", DefaultErrorColor);
                }
                catch (NullReferenceException)
                {
                    Console.SetCursorPosition(Left, Top);
                    Write("Ogiltig input - Värdet kan inte vara null.", DefaultErrorColor);
                }
            }
        }

        // #####################
        // ## Private methods ##
        // #####################

        /// <summary>Write a text using the default system color scheme.</summary>
        /// <param name="text">The text to write.</param>
        private static void WriteSystemText(string text)
        {
            if (text != "") Write(text, DefaultSystemColor, DefaultBackgroundColor, DefaultSystemHighlightColor, false);
        }

        /// <summary>Write out the option of keys that the user is expected to press.</summary>
        /// <param name="acceptedKeys">An array containing the keys.</param>
        private static void WriteKeyOptions(ConsoleKey[] acceptedKeys)
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

        /// <summary>Used by <see cref="GetString(string, bool)"/> and <see cref="GetDouble(string)"/>.
        ///          Will print the specified text to the console and make sure that it is correctly formatted
        ///          (i.e. must end with either a question mark or a colon).</summary>
        /// <param name="text"></param>
        private static void WriteGetText(string text)
        {
            string checkedText = text.TrimEnd(); // Remove the trailing white space

            if (checkedText.Length > 0 && (checkedText[^1] != ':' && checkedText[^1] != '?' && checkedText[^1] != '='))
                checkedText += ": ";

            if (checkedText.Length > 0 && checkedText[^1] != ' ') checkedText += " ";

            Write(checkedText, false);
        }

        /// <summary>Used by the <see cref="ExpectKey(string, ConsoleKey[])"/> method.
        ///          Writes the users input using the default system color scheme.</summary>
        /// <param name="input">The input from the user.</param>
        private static void WriteValidInput(string input) =>
            Write(": {" + input + "}", DefaultSystemColor, DefaultBackgroundColor, DefaultForegroundColor);

        /// <summary>Used by the <see cref="ExpectKey(string, ConsoleKey[])"/> method.
        ///          Writes the users input using the default error color scheme.</summary>
        /// <param name="input">The input from the user.</param>
        private static void WriteInvalidInput(string input) =>
            Write(": {" + input + "}", DefaultSystemColor, DefaultBackgroundColor, DefaultErrorColor);

        /// <summary>This method is using regular expressions in order to separate sequences of chars that
        ///          are encapsulated inside curly braces, in order to highlight them in another color.
        ///          Also checks for escape characters (i.e. {{word}} should be written as {word}
        ///          in the default color).</summary>
        /// <param name="text">The input text to analyze and separate.</param>
        /// <param name="defaultColor">The foreground color of the text that is not highlighted.</param>
        /// <param name="highlightColor">The foreground color of the text that is highlighted.</param>
        /// <returns>An array of a tuple that contains the text and the color of the sequence of the originial input text.</returns>
        private static (string Text, ConsoleColor Color)[] ExtractSequences(string text,
            ConsoleColor defaultColor, ConsoleColor highlightColor)
        {
            var regex = new Regex("{[^}]+}+|[^{}]+");
            // {[^}]*}+ checks for a sequence that begins with { and takes everything that comes next
            //          until a } is encountered
            // |        OR:
            // [^{}]+   checks for a sequence that does not contain { and ends with }

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