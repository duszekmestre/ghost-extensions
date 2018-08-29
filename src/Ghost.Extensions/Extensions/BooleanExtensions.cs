namespace Ghost.Extensions.Extensions
{
    public static class BooleanExtensions
    {
        public static int AsInt(this bool input, int trueValue = 1, int falseValue = 0)
        {
            return input ? trueValue : falseValue;
        }
    }
}
