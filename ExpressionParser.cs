namespace ConsoleCalculatorGIT
{
    internal static class ExpressionParser
    {
        // ###################
        // ## Configuration ##
        // ###################

        // Set up operators and symbols
        private const char AdditionOperator = '+';
        private const char SubtractionOperator = '-';
        private const char MultiplicationOperator = '*';
        private const char DivisionOperator = '/';
        private const char ModulusOperator = '%';
        private const char ExponentiationOperator = '^';

        /// <summary>Take in a math expression in the form of a string and transform it into a list of tokens to be further processed
        ///          using the <see cref="ParseExpression(List{string})"/> method.</summary>
        /// <param name="input"></param>
        /// <exception cref="Exception"></exception>
        public static List<string> TokenizeInput(string input)
        {
            // This method scans the user input, character by character, extracting only the
            // bits that are relevant into tokens for further processing.
            // White spaces and characters are ignored. Input validation is performed.

            //TODO: Add custom exceptions
            // Declare a new list of strings and add a blank item (the first token)
            List<string> tokens = new() { "" };

            // Loop through all characters and analyze them one at a time
            foreach (char currentChar in input)
            {
                bool appendToToken = true; // Flag that controls if we should append the current character to the latest added token

                // Check what the currently analyzed character contains
                switch (currentChar)
                {
                    case AdditionOperator:
                    case SubtractionOperator:
                    case MultiplicationOperator:
                    case DivisionOperator:
                    case ModulusOperator:
                    case ExponentiationOperator:
                        // The character is an operator:
                        // Make sure that a left hand value is provided
                        bool isNegativeNumber = false; // Flag that indicates that this character should be added to the current token instead of being treated as a separate token
                        if (tokens[^1] == "")
                        {
                            // Check if the operator is an subtraction (indicates a negative number)
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
                            appendToToken = false; // Do not append the operator to the last token
                        }
                        break;
                    default:
                        // The character is anything but an operator
                        // Exclude all characters that is Not a number or a decimal place
                        if (!char.IsNumber(currentChar) && currentChar != ',')
                            appendToToken = false; // Do not append the current character to the last token

                        // If the character is a decimal place: Make sure that the current token does not already contain one decimal place
                        if (currentChar == ',' && tokens[^1].Contains(','))
                            throw new Exception("En term får inte innehålla två decimal punkter.");
                        break;
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
            // Copy the list of tokens (because the parser works by replacing tokens that's been processed)
            List<string> localTokens = new(tokens);

            // Set up variables
            int level = 3;     // Holds the current level of operation that is being checked for (3 = Exponent, 2 = multiply/divide, 1 = addition/subtraction)

            // Stay in a loop until only one token remain (the final result)
            while (localTokens.Count > 1)
            {
                bool next = false; // Flag that indicates that there could be more operators of the current level (continue the while loop)

                // Loop through all of the tokens in order to find the first one with the highest order of operation
                string currentOperator = "";
                int index = -1;
                for (int i = 0; i < localTokens.Count; i++)
                {
                    string token = localTokens[i]; // Holds the current token

                    // Check if the current token is an operator of the current level (of order of operation)
                    if ((level == 3 && token == "^") || (level == 2 && (token == "*" || token == "/" || token == "%")) || (level == 1 && (token == "+" || token == "-")))
                    {
                        // Set the current operator
                        currentOperator = token;
                        index = i;

                        // Set the flag so we check if there are yet another operator token of the same level, before stepping down
                        next = true;

                        // No need to continue this for loop (restart again at the next iteration of the while loop)
                        break;
                    }
                }

                // Check if an operator of the current level was found
                if (next)
                {
                    // Operator was found: Perform the calculation
                    // Then replace the three affected tokens with the result of the computation
                    double left = Convert.ToDouble(localTokens[index - 1].Replace("_", ""));  // The left hand value
                    double right = Convert.ToDouble(localTokens[index + 1].Replace("_", "")); // The right hand value
                    double sum = 0d;

                    // Check for divide by zero error
                    if (currentOperator == "/" && right == 0)
                        throw new DivideByZeroException();

                    // Perform the calculation
                    if (currentOperator == ExponentiationOperator.ToString())
                        sum = Math.Pow(left, right);
                    if (currentOperator == ModulusOperator.ToString())
                        sum = left % right;
                    if (currentOperator == MultiplicationOperator.ToString())
                        sum = left * right;
                    if (currentOperator == DivisionOperator.ToString())
                        sum = left / right;
                    if (currentOperator == AdditionOperator.ToString())
                        sum = left + right;
                    if (currentOperator == SubtractionOperator.ToString())
                        sum = left - right;

                    // Remove the three currently calculated tokens
                    // Insert a new token that represents the computed sum of the three tokens currently affected
                    localTokens.RemoveRange(index - 1, 3);
                    localTokens.Insert(index - 1, sum.ToString());
                }
                else
                    // Not found: Decrement to a lower level (of order of operation)
                    level--;
            }

            return Convert.ToDouble(localTokens[0]);
        }

        public static string FormatExpression(List<string> tokens)
        {
            string result = "";

            for (int i = 0; i < tokens.Count; i++)
            {
                if (IsOperator(tokens[i]))
                    result += "{" + tokens[i] + "}";
                else
                    result += tokens[i];

                if (i != tokens.Count - 1) result += " ";
            }

            return result;
        }

        private static bool IsOperator(string token)
        {
            if (token.Length > 1) return false;

            return Convert.ToChar(token[..1]) switch
            {
                AdditionOperator or
                SubtractionOperator or
                MultiplicationOperator or
                DivisionOperator or
                ModulusOperator or
                ExponentiationOperator => true,
                _ => false,
            };
        }

    }
}