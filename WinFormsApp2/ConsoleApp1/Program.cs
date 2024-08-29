using StackExchange.Redis;
using System;

class Program
{
    static void Main(string[] args)
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        IDatabase db = redis.GetDatabase();

        db.StringSet("key", "nour");

        string value = db.StringGet("key");
        Console.WriteLine(value);

        Console.ReadLine();
    }
}
