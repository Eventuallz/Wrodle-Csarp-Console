using System.Threading;
using System.Security;
using Wordle_200422;

bool exit = false;
while (exit == false)
{
    Console.Clear();
    // Importing the words from a file.
    String str = File.ReadAllText(@"word_list.txt");

    String[] words = str.Split("\n");

    // Method to get a random word.
    int WordNum()
    {
        Random random = new Random();
        return random.Next(0, words.Length);
    }

    // Defining word to be guessed. 
    string? word = words[WordNum()];

    bool gameOver = true;
    
    // Counting number of tries.
    var counter = 0;

    // Menu.
    Console.WriteLine();
    Console.Write("[S]tart Game | [E]xit >".Padding(3));
    ConsoleKeyInfo choice = Console.ReadKey(true);
    switch (choice.Key)
    {
        case ConsoleKey.S:
        {
            gameOver = false;
            Console.Clear();
            Console.WriteLine();
            Console.Write("Guess word".Padding(3));
        }
            break;

        case ConsoleKey.E:
            exit = true;
            return;

        default:
            Console.WriteLine("Wrong input".Padding(3));
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;
            break;
    }

    (int, int) remainingLine = Console.GetCursorPosition();
    (int, int) position = (0,0);
    // Game field.
    while (!gameOver)
    {
        try
        {
            Console.SetCursorPosition(remainingLine.Item1, remainingLine.Item2);
            Console.Write($"Remaining tries: {5 - counter}".Padding(8));
            Console.WriteLine();
            if (position != (0, 0))
                Console.SetCursorPosition(position.Item1,position.Item2);
            // Input.
            string? guess = Console.ReadLine();
            
            if (guess.Length > 5)
            {
                Console.WriteLine();
                Console.Write("Word can't be longer than 5 characters.".Padding(3));
                Console.CursorVisible = false;
                Console.ReadKey();
                ClearLine(1);
                counter--;
                Console.CursorVisible = true;
            }

            else if (guess.Length < 5)
            {
                Console.WriteLine();
                Console.Write("Word can't be shorter than 5 characters.".Padding(3));
                Console.CursorVisible = false;
                Console.ReadKey();
                ClearLine(1);
                counter--;
                Console.CursorVisible = true;
            }
            
            else if (!words.Contains(guess))
            {
                Console.WriteLine();
                Console.CursorVisible = false;
                Console.Write("Word was not found in the word list.".Padding(3));
                Console.ReadKey();
                ClearLine(1);
                counter--;
                Console.CursorVisible = true;
            }
            
            else
            {
                // Array for checking character correctness.
                // 2: letter is true and it is in the correct place.
                // 1: letter is true, but it is not in the correct place.
                // 0: letter is not found in the word.
                int[] proof = new int[guess.Length];

                WordOutput(guess, proof);

                void WordOutput(string? guessedWord, int[] checkNum)
                {
                    if (guessedWord == word)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Well done! You guessed the word!".Padding(3));
                        Console.CursorVisible = false;
                        Console.ReadKey();
                        gameOver = true;
                        Console.CursorVisible = true;
                    }

                    for (int i = 0; i < checkNum.Length; i++)
                    {
                        for (int n = 0; n < checkNum.Length; n++)
                        {
                            // Checking letter value.
                            checkNum[n] = CheckLetter(n);
                        }

                        Console.Write("|");
                        // Checking the returned value and displaying the color indicator for the player.
                        switch (checkNum[i])
                        {
                            case 0:
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                break;

                            case 1:
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                break;

                            case 2:
                                Console.BackgroundColor = ConsoleColor.Green;
                                break;
                        }

                        // Printing the guess with the colors from above.
                        Console.Write($" {guessedWord[i]} ");

                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write("|");
                    Console.WriteLine();
                }

                // Checking if the letter inside the word is the right word and returning the correct value.
                int CheckLetter(int i)
                {
                    if (word[i] == guess[i])
                        return 2;

                    else if (word.Contains(guess[i]))
                        return 1;

                    else
                        return 0;
                }
            }

            counter++;
            if (counter == 5 && guess != word)
            {
                Console.WriteLine();
                Console.WriteLine($"Number of tries exceeded.\nThe correct word was: {word}".Padding(3));
                Console.CursorVisible = false;
                Console.ReadKey();
                gameOver = true;
                Console.CursorVisible = true;
            }
            
            position = Console.GetCursorPosition();

            // Additional Method for clearing current line. For aesthetics only.
            static void ClearLine(int line)
            {
                var currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, currentLineCursor);
                for (var i = 0; i < Console.WindowWidth; i++)
                    Console.Write(" ");
                Console.SetCursorPosition(0, currentLineCursor - line - 1);
                for (var i = 0; i < Console.WindowWidth; i++)
                    Console.Write(" ");
                Console.SetCursorPosition(0, currentLineCursor - line - 1);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (SecurityException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}