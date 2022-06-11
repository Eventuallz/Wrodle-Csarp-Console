namespace Wordle_200422;

public static class Extensions
{
    public static string Padding(this string Input, int padding)
    {
        return Input.PadLeft(Input.Length + padding);
    }
}