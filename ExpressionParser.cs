namespace ConsoleCalculatorGIT
{
    internal static class ExpressionParser
    {
        // ###################
        // ## Configuration ##
        // ###################

        // Set up operators
        private const char AdditionOperator = '+';
        private const char SubtractionOperator = '-';
        private const char MultiplicationOperator = '*';
        private const char DivisionOperator = '/';
        private const char ModulusOperator = '%';
        private const char ExponentiationOperator = '^';

        // ####################
        // ## Public methods ##
        // ####################

        /// <summary>Takes in a math expression in the form of a string and transform it into a list of tokens to be further processed
        ///          using the <see cref="ParseExpression(List{string})"/> method. It also validates the expressions and throws exceptions
        ///          if anything is out of order (does not check against division by zero).</summary>
        /// <param name="input"></param>
        /// <exception cref="Exception"></exception>
        public static List<string> TokenizeInput(string input)
        {
            // This method scans the user input, character by character, extracting only the
            // bits that are relevant, into tokens for further processing.
            // White spaces and characters are ignored. Input validation is performed.

            //TODO: Add custom exceptions or separate the validation part to its own method

            // Declare a new list of strings and add an empty string (the first token)
            List<string> tokens = new() { "" };

            // Loop through all characters and analyze them one at a time
            foreach (char currentChar in input)
            {
                bool appendToToken = true; // Flag that controls if we should append the current character to
                                           // the latest added token

                // Check if the currently analyzed character is an operator
                if (IsOperator(currentChar.ToString()))
                {
                    // The character is an operator:
                    bool isNegativeNumber = false; // Flag that dictates whether this character should be added to
                                                   // the current token instead of being treated as a separate token
                    
                    // Make sure that a left hand value is provided
                    if (tokens[^1] == "")
                    {
                        // Check if the operator is an subtraction (could represent a negative number)
                        if (currentChar == SubtractionOperator && !tokens[^1].Contains(SubtractionOperator))
                            // Is a negative number: Do not add as an operator
                            isNegativeNumber = true;
                        else
                            // Is not a negative number: raise exception
                            throw new Exception("En operator saknar en term på vänster sida.");
                    }

                    // Add the operator as a new token, then add a new empty token
                    if (!isNegativeNumber)
                    {
                        tokens.Add(currentChar.ToString());
                        tokens.Add("");
                        appendToToken = false; // <-- Do not append the operator to the newest token
                    }
                }
                else
                {
                    // The character is anything but an operator
                    // Exclude all characters that is Not a number or a decimal place
                    if (!char.IsNumber(currentChar) && currentChar != ',')
                        appendToToken = false; // <-- Do not append the current character to the newest token

                    // If the character is a decimal place: Make sure that the current token does not already contain one decimal place
                    if (currentChar == ',' && tokens[^1].Contains(','))
                        throw new Exception("En term får inte innehålla två decimalpunkter.");
                }

                // Append the character to the latest token
                if (appendToToken) tokens[^1] += currentChar.ToString();
            }

            // Make sure that the last token is set (otherwise it is a faulty input)
            if (tokens[^1] == "")
                throw new Exception("En operator saknar en term på höger sida.");

            // Return the list of tokens
            return tokens;
        }

        /// <summary>Take in a list of math tokens and evaluate them in order to preduce the sum of the expression.
        ///          Use the <see cref="TokenizeInput(string)"/> method in order to create the tokens.</summary>
        /// <param name="tokens"></param>
        /// <returns>The sum of the math expression as a double.</returns>
        /// <exception cref="DivideByZeroException"></exception>
        public static double ParseExpression(List<string> tokens)
        {
            // Copy the list of tokens (because the parser works by replacing tokens that's been processed with
            // the sum of that computation, until only one token remain)
            List<string> localTokens = new(tokens);

            // Set up a variable that holds the current level of operation that is being checked for
            // (3 = Exponent, 2 = multiply/divide, 1 = addition/subtraction)
            int level = 3; // <-- Initialize to the highest value possible

            // Stay in a loop until only one token remain (the final result)
            while (localTokens.Count > 1)
            {
                bool performCalculation = false; // Flag that indicates that there could be more operators
                                                 // of the current level (continue the while loop)
                char? currentOperator = null;

                // ¤¤ STEP 1: Find the operator with the highest precedence ¤¤
                // Loop through all of the tokens in order to find the first one with the highest precedence
                // Perform the calculation then repeat this process. Decrement the level if no further operator
                // of this level is found.
                int index = -1;
                for (int i = 0; i < localTokens.Count; i++)
                {
                    string token = localTokens[i]; // Holds the current token
                    currentOperator = GetOperator(token);

                    // Check if the current token is an operator of the current level (of order of operation)
                    if (IsNextOperator(currentOperator, level))
                    {
                        // Set the index of the operator to calculate
                        index = i;

                        // Set the flag so we check if there are yet another operator token of the same level, before stepping down
                        performCalculation = true;

                        // No need to continue this for loop (restart again at the next iteration of the while loop)
                        break;
                    }
                }

                // ¤¤ STEP 2: Perform the calculation of the selected operator or decrement the level ¤¤
                // Check whether an operator to compute was found under this iteration of the while loop
                if (performCalculation)
                {
                    // Operator was found: Perform the calculation
                    // Then replace the three affected tokens with the result of the computation
                    double leftTerm = Convert.ToDouble(localTokens[index - 1]);
                    double rightTerm = Convert.ToDouble(localTokens[index + 1]);
                    double sum = 0d;

                    // Check for divide by zero error
                    // Note: This should be moved to the Tokenizer method
                    if (currentOperator == DivisionOperator && rightTerm == 0)
                        throw new DivideByZeroException();

                    // Perform the calculation
                    // Note: This part could probably be accomplished by using some design pattern
                    if (currentOperator == ExponentiationOperator)
                        sum = Math.Pow(leftTerm, rightTerm);
                    if (currentOperator == ModulusOperator)
                        sum = leftTerm % rightTerm;
                    if (currentOperator == MultiplicationOperator)
                        sum = leftTerm * rightTerm;
                    if (currentOperator == DivisionOperator)
                        sum = leftTerm / rightTerm;
                    if (currentOperator == AdditionOperator)
                        sum = leftTerm + rightTerm;
                    if (currentOperator == SubtractionOperator)
                        sum = leftTerm - rightTerm;

                    // Remove the three affected tokens
                    // Insert a new token that represents the computed sum
                    localTokens.RemoveRange(index - 1, 3);
                    localTokens.Insert(index - 1, sum.ToString());
                }
                else
                {
                    // Operator was not found: Decrement to a lower level (of operator precedence)
                    level--;
                }
            }

            // Convert and return the last remaning token (holds the sum of the whole expression)
            return Convert.ToDouble(localTokens[0]);
        }

        // #####################
        // ## Private methods ##
        // #####################

        /// <summary>Formats the list of tokens into a string.</summary>
        /// <param name="tokens">The list of tokens to format.</param>
        /// <returns>A formatted string that represents the expression.</returns>
        public static string FormatExpression(List<string> tokens)
        {
            string result = "";

            for (int i = 0; i < tokens.Count; i++)
            {
                // Check whether to highlight the token or not
                if (IsOperator(tokens[i]))
                    result += "{" + tokens[i] + "}";
                else
                    result += tokens[i];

                // Add a space if it is not the last token in the list
                if (i != tokens.Count - 1) result += " ";
            }

            return result;
        }

        /// <summary>Check if the current operator matches with the current level (of order of operation).</summary>
        /// <param name="mathOperator">The current operator token represented as a char.</param>
        /// <param name="level">The current level (of order of operation).</param>
        /// <returns>A boolean that indicates if the current operator is to be processed.</returns>
        private static bool IsNextOperator(char? mathOperator, int level) => ((level, mathOperator)) switch
        {
            (3, ExponentiationOperator) or
            (2, MultiplicationOperator) or
            (2, DivisionOperator) or
            (2, ModulusOperator) or
            (1, AdditionOperator) or
            (1, SubtractionOperator) => true,
            _ => false,
        };

        /// <summary>Check whether the specified token is an operator token.</summary>
        /// <param name="token">The token to check against.</param>
        /// <returns>A boolean that indicates if the specified token is a math operator.</returns>
        private static bool IsOperator(string token) => GetOperator(token) switch
        {
            AdditionOperator or
            SubtractionOperator or
            MultiplicationOperator or
            DivisionOperator or
            ModulusOperator or
            ExponentiationOperator => true,
            _ => false,
        };

        /// <summary>Convert the string token into nullable char.</summary>
        /// <param name="token">The token to convert.</param>
        /// <returns>A representation of the math operator if succesfull, null otherwise.</returns>
        private static char? GetOperator(string token)
        {
            if (token.Length > 1) return null;
            return Convert.ToChar(token[..1]);
        }

    }
}