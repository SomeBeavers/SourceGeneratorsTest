using System;
using System.Collections.Generic;
using JsonSrcGen;
using MetaJson;
using ValueObjectGenerator;

namespace MetaJson
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
    public partial class ProductId
    {
    }

    public partial class PartialClassToDelete
    {
    }

    [Serialize]
    public partial class Book
    {
        [Serialize]
        public string Name { get; set; }
    }

    public static class Program
    {
        public static void UseJsonArray()
        {
            var _convertProgram = new JsonConverter();

            var jsonClassProgram = new JsonArrayClass
            {
                BooleanArray = new[] {true, false}
            };

            var json = _convertProgram.ToJson(jsonClassProgram);
        }

        public static void SampleStringValueObject()
        {
            var userNameProgram = new UserName("Ryota");
            Console.WriteLine($": {userNameProgram.Value}");

            var partialClassToDelete = new PartialClassToDelete("Test");
        }

        public static void SampleIntValueObject()
        {
            /*use ProductId*/
        }

        public static void Main(string[] args)
        {
            var someNamedName = new Book().get_name;
        }
    }
}