using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    public class Program
    {
        public static string ProcessCommand(string input)
        {
            try
            {
                List<string> numbers = new List<string>();
                List<char> operators = new List<char>();
                //char prevNonWhiteSpaceChar = '0';
                // Loop through the input string to extract numbers, operators, and parentheses
                // Remove whitespace
                input = input.Replace(" ", string.Empty);
                for (int i = 0; i < input.Length; i++)
                {

                    //if (char.IsWhiteSpace(input[i])) continue;
                    //prevNonWhiteSpaceChar = input[i];

                    // Handle negative numbers
                    if (input[i] == '-' && (i == 0 || "+-*/(".Contains(input[i - 1])))
                    {
                        i = ExtractNumber(input, i, numbers);
                    }
                    // Handle positive numbers or decimals
                    else if (char.IsDigit(input[i]) || (input[i] == '.' && i + 1 < input.Length && char.IsDigit(input[i + 1])))
                    {
                        i = ExtractNumber(input, i, numbers);
                    }
                    // Handle parentheses
                    else if (input[i] == '(')
                    {
                        i = ExtractExpression(input, i, numbers);
                    }
                    // Handle operators
                    else if ("+-*/".Contains(input[i]))
                    {
                        operators.Add(input[i]);
                    }
                }            

                // Evaluate Multiplication and Division first
                EvaluateOperators(numbers, operators, new char[] { '*', '/' });

                // Evaluate Addition and Subtraction
                EvaluateOperators(numbers, operators, new char[] { '+', '-' });

                // Return the result
                return numbers[0];
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e.Message;
            }
        }
        // I used Chat GPT to turn my code in to functions to save time and make it easier to read
        private static int ExtractNumber(string input, int startIndex, List<string> numbers)
        {
            int numberIndexRange = startIndex + 1; // Start after the sign // This is witch craft and you want to understand why it works for both - starting numbers

            // Loop to find the end of the number
            while (numberIndexRange < input.Length && (char.IsDigit(input[numberIndexRange]) || input[numberIndexRange] == '.'))
            {
                numberIndexRange++;
            }

            // Add the number, including any sign
            numbers.Add(input.Substring(startIndex, numberIndexRange - startIndex));
            return numberIndexRange - 1; // Return the last valid character index
        }
        private static int ExtractExpression(string input, int startIndex, List<string> numbers)
        {
            int expressionIndexRange = startIndex;
            int amountOfOpenBrackets = 1;

            while (amountOfOpenBrackets > 0 && expressionIndexRange < input.Length)
            {
                expressionIndexRange++;
                if (expressionIndexRange < input.Length && input[expressionIndexRange] == '(') amountOfOpenBrackets++;
                if (expressionIndexRange < input.Length && input[expressionIndexRange] == ')') amountOfOpenBrackets--;
            }

            // Extract the expression within parentheses and evaluate it using recursion
            numbers.Add(ProcessCommand(input.Substring(startIndex + 1, expressionIndexRange - startIndex - 1)));
            return expressionIndexRange; // Return the index of the closing bracket
        }
        private static void EvaluateOperators(List<string> numbers, List<char> operators, char[] targetOperators)
        {
            for (int i = 0; i < operators.Count; i++)
            {
                // Only evaluate if the operator is one of the target operators
                if (Array.Exists(targetOperators, op => op == operators[i]))
                {
                    double left = double.Parse(numbers[i]);
                    double right = double.Parse(numbers[i + 1]);

                    // Determine the operation
                    if (operators[i] == '*')
                    {
                        numbers[i] = (left * right).ToString();
                    }
                    else if (operators[i] == '/')
                    {
                        numbers[i] = (left / right).ToString();
                    }
                    else if (operators[i] == '+')
                    {
                        numbers[i] = (left + right).ToString();
                    }
                    else if (operators[i] == '-')
                    {
                        numbers[i] = (left - right).ToString();
                    }

                    // Remove the processed number and operator
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--; // Decrement to re-evaluate this index
                }
            }
        }
        static void Main(string[] args)
        {
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine(ProcessCommand(input));

            }
        }
    }
}
