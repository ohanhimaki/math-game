int delayInMilliseconds = 1200; // Default delay: 1 second

if (args.Length > 0 && int.TryParse(args[0], out int parsedDelay))
{
    delayInMilliseconds = parsedDelay * 1000; // Convert seconds to milliseconds
}


Console.WriteLine("\nWELCOME TO THE MATH GAME AKSELI TEAM AND ANDREE!!!!");
int[,] nokiaTune = new int[,]
{
    { 659, 300 }, // E5
    { 784, 300 }, // G5
    { 880, 300 }, // A5
    { 784, 300 }, // G5
    { 659, 300 }, // E5
    { 523, 300 }, // C5
    { 587, 300 }, // D5
    { 659, 300 }, // E5
    { 587, 300 }, // D5
};


// Play each note in the Nokia Tune
for (int i = 0; i < nokiaTune.GetLength(0); i++)
{
    int frequency = nokiaTune[i, 0];
    int duration = nokiaTune[i, 1];

    
    Console.Beep(frequency, duration);
    
    System.Threading.Thread.Sleep(50); // Slight pause between notes
}

while (true)
{
    Console.WriteLine($"The numbers will appear with a delay of {delayInMilliseconds / 1000.00} seconds.\n");

    Random random = new Random();
    int sum = 0; // Initialize sum

    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(delayInMilliseconds); // Wait for the specified delay
        var min = 0 - sum;
        var max = 9 - sum;
        int number = random.Next(min, max); // Random number between -9 and 9
        sum += number;

        // Ensure the sum is between 0 and 9 (wrap around)
        sum = (sum % 10 + 10) % 10;

        // Overwrite previous number and play a beep sound
        Console.SetCursorPosition(0, Console.CursorTop - 1); // Move cursor up to overwrite the previous line
        Console.WriteLine($"Number: {number}      "); // Display number
        Console.Beep(1000, 200); // Beep at 1000 Hz for 200 ms
    }

    Console.WriteLine("\nNow, it's your turn to calculate the final sum!\n");

// Ask for the user's answer
    Console.Write("Your answer: ");
    
    // take first number
    var result = Console.ReadKey();
    

// Validate and provide feedback
    if (int.TryParse(result.KeyChar.ToString(), out int userSum) && userSum == sum)
    {
        Console.WriteLine("\nCorrect! Well done!");
        // delayInMilliseconds -= 100; // Decrease the delay by 100 ms
        
        delayInMilliseconds = delayInMilliseconds*9/10;
    }
    else
    {
        Console.WriteLine($"\nWrong. The correct sum is: {sum}");
        // delayInMilliseconds += 100; // Increase the delay by 100 ms
        // make it one step backwards
        delayInMilliseconds = delayInMilliseconds*10/9;
    }
}