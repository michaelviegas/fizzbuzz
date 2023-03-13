namespace FizzBuzz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TwistedFizzBuzz.TwistedFizzBuzz.SetTokens(new Dictionary<int, string> 
            { 
                { 3, "Fizz" }, 
                { 5, "Buzz" } 
            });

            foreach (var item in TwistedFizzBuzz.TwistedFizzBuzz.GetOutput(1, 100))
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}