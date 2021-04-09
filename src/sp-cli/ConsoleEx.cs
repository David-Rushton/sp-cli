using System;


/// <summary>
/// Ideally these would be extension methods.
/// Unfortunately C# doesn't support extending static classes.
/// </summary>
public static class ConsoleEx
{
    const string AnsiEscapeCode = "\u001b";

    static (int Left, int Top) _position = (0, 0);


    public static void SaveCursorPosition() => Console.Write($"{AnsiEscapeCode}[s");
    //     // WhereAmI();
    //     _position = Console.GetCursorPosition();
    // }

    public static void RestoreCursorPosition() => Console.Write($"{AnsiEscapeCode}[u");
    // {
    //     // WhereAmI();
    //     //Console.Write($"restoring: {_position.Top}");
    //     Console.SetCursorPosition(_position.Left, _position.Top);

    //     // WhereAmI();
    // }


#region CSI
// https://en.wikipedia.org/wiki/ANSI_escape_code#CSI_(Control_Sequence_Introducer)_sequences

    public static void ClearFromCursor() => Console.Write($"{AnsiEscapeCode}[0J");

    public static void ClearToCursor() => Console.Write($"{AnsiEscapeCode}[1J");

    public static void Clear() => Console.Write($"{AnsiEscapeCode}[3J");

#endregion CSI

    /// <summary>
    /// Creates requested number of empty lines after the cursor.
    /// If there are not enough lines after the cursor the screen will scroll up to accommodate as
    /// many lines as possible.
    /// </summary>
    /// <param name="count"></param>
    public static void ClearNextLines(byte count)
    {
        var startingPosition = Console.GetCursorPosition();

        // advance the console
        Console.Write( new string('\n', count) );

        // reset the position as best we can.
        // the starting position may have scrolled up.
        // it may have advanced off the screen.
        var top = Console.GetCursorPosition().Top - count;
        Console.SetCursorPosition
        (
            startingPosition.Left,
            top < 0 ? 0 : top
        );
    }


static int _seed = 0;

    static void WhereAmI()
    {
        Console.Write($"{_seed++} t {Console.GetCursorPosition().Top}");
        Console.ReadKey();
    }
}
