namespace PaperNest_API.Utils
{
    public static class TextEditorUtil
    {
        public static string ReadAndEditMultilineText(string initialText)
        {
            Console.WriteLine("Mulai editing teks (Ctrl+S untuk menyimpan, Ctrl+C untuk membatalkan):");
            List<string> lines = new List<string>(initialText.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            int currentLine = 0;
            int cursorX = 3; // Start at column 3 to account for "1: "
            int cursorY = 0;

            // Display initial text with line numbers
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {lines[i]}");
            }
            Console.SetCursorPosition(cursorX, cursorY);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentLine > 0)
                        {
                            currentLine--;
                            cursorY--;
                            cursorX = Math.Min(cursorX, lines[currentLine].Length + 3); // Keep cursor within line length
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentLine < lines.Count - 1)
                        {
                            currentLine++;
                            cursorY++;
                            cursorX = Math.Min(cursorX, lines[currentLine].Length + 3);
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursorX > 3)
                        {
                            cursorX--;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (cursorX <= lines[currentLine].Length + 2) // Changed to <=
                        {
                            cursorX++;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.Enter:
                        string lineAfterCursor = lines[currentLine].Substring(cursorX - 3);
                        lines[currentLine] = lines[currentLine].Substring(0, cursorX - 3);
                        lines.Insert(currentLine + 1, lineAfterCursor);

                        // Update display
                        Console.SetCursorPosition(0, cursorY);
                        Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                        Console.SetCursorPosition(0, cursorY);
                        Console.Write($"{currentLine + 1}: {lines[currentLine]}");

                        for (int i = currentLine + 1; i < lines.Count; i++)
                        {
                            Console.SetCursorPosition(0, i);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, i);
                            Console.Write($"{i + 1}: {lines[i]}");
                        }

                        currentLine++;
                        cursorY++;
                        cursorX = 3;
                        Console.SetCursorPosition(cursorX, cursorY);
                        break;
                    case ConsoleKey.Backspace:
                        if (cursorX > 3)
                        {
                            int pos = cursorX - 4;
                            string line = lines[currentLine];
                            lines[currentLine] = line.Remove(pos, 1);

                            // Redraw the current line
                            Console.SetCursorPosition(0, cursorY);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, cursorY);
                            Console.Write($"{currentLine + 1}: {lines[currentLine]}");

                            cursorX--;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        else if (currentLine > 0) // Handle backspace at the beginning of a line
                        {
                            // Move the content of the current line to the end of the previous line
                            string previousLineContent = lines[currentLine - 1];
                            lines[currentLine - 1] += lines[currentLine];
                            lines.RemoveAt(currentLine);
                            currentLine--;
                            cursorY--;
                            cursorX = previousLineContent.Length + 3; // Position cursor at the end of the previous line

                            // Redraw the previous line and all subsequent lines
                            for (int i = currentLine; i < lines.Count; i++)
                            {
                                Console.SetCursorPosition(0, i);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, i);
                                Console.Write($"{i + 1}: {lines[i]}");
                            }
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (cursorX <= lines[currentLine].Length + 2)
                        {
                            string line = lines[currentLine];
                            lines[currentLine] = line.Remove(cursorX - 3, 1);

                            Console.SetCursorPosition(0, cursorY);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, cursorY);
                            Console.Write($"{currentLine + 1}: {lines[currentLine]}");
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                    case ConsoleKey.S when (key.Modifiers & ConsoleModifiers.Control) != 0:
                        Console.WriteLine("\nSaving...");
                        return string.Join(Environment.NewLine, lines);
                    case ConsoleKey.C when (key.Modifiers & ConsoleModifiers.Control) != 0:
                        Console.WriteLine("\nCanceled.");
                        return initialText;
                    default:
                        if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || key.KeyChar == ' ')
                        {
                            int pos = cursorX - 3;  // Adjust for line number prefix
                            string line = lines[currentLine];

                            // Insert the character
                            if (pos >= 0 && pos <= line.Length)
                            {
                                lines[currentLine] = line.Insert(pos, key.KeyChar.ToString());

                                // Redraw the current line
                                Console.SetCursorPosition(0, cursorY);
                                Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                                Console.SetCursorPosition(0, cursorY);
                                Console.Write($"{currentLine + 1}: {lines[currentLine]}");

                                // Move cursor forward one position
                                cursorX++;
                                Console.SetCursorPosition(cursorX, cursorY);
                            }
                        }
                        break;
                }
            }
        }
    }
}