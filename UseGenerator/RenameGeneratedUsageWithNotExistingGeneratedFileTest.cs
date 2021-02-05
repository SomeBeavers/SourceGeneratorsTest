using System;
using ValueObjectGenerator;

namespace UseGenerator10
{
    [StringValueObject]
    public partial class UserName
    {
    }

    public class RenameGeneratedUsageWithNotExistingGeneratedFileTest
    {
        public static void SampleStringValueObject()
        {
            var userNameProgram1 = new UserName("Ryota");
            Console.WriteLine($": {userNameProgram1.Value}");
        }
    }
}