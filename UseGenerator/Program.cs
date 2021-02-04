using System;
using JsonSrcGen;
using ValueObjectGenerator;

namespace UseGenerator
{
    [Json]
    public class JsonArrayClass
    {
        public bool[] BooleanArray { get; set; }
    }

    [StringValueObject]
    public partial class UserName
    {
    }

    //[IntValueObject]
    public class ProductId
    {
    }

    public partial class PartialClassToDelete
    {
    }

    public static class Program
    {
        public static void UseJsonArray()
        {
            var _convert = new JsonConverter();

            var jsonClass = new JsonArrayClass
            {
                BooleanArray = new[] {true, false}
            };

            var json = _convert.ToJson(jsonClass);
        }

        public static void SampleStringValueObject()
        {
            var userName = new UserName("Ryota");
            Console.WriteLine($"userName.Value: {userName.Value}");

            var partialClassToDelete = new PartialClassToDelete("Test");
        }

        public static void SampleIntValueObject()
        {
            /*use ProductId*/
        }

        public static void Main(string[] args)
        {
        }
    }
}