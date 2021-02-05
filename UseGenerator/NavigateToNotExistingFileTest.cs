using System;
using ValueObjectGenerator;

namespace UseGenerator2
{
    [StringValueObject]
    public partial class UserName
    {
    }

    public class NavigateToNotExistingFileTest
    {
        public static void SampleStringValueObject()
        {
            var userNameNotExistingFileTest = new UserName("Ryota");
            Console.WriteLine($": {userNameNotExistingFileTest.Value}");
        }
    }
}