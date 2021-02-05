using System;
using ValueObjectGenerator;

namespace UseGenerator3
{
    [StringValueObject]
    public partial class UserName
    {
    }

    public class NavigateToExistingFileTest
    {
        public static void SampleStringValueObject()
        {
            var userNameExistingFileTest = new UserName("Ryota");
            Console.WriteLine($": {userNameExistingFileTest.Value}");
        }
    }
}