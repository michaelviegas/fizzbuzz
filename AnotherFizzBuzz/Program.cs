
namespace AnotherFizzBuzz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string>
            {
                { 5, "Fizz" },
                { 9, "Buzz" },
                { 27, "Bar" }
            });

            foreach (var item in TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(-20, 127))
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}