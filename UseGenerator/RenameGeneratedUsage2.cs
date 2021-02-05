using System;
using ValueObjectGenerator;

namespace UseGenerator11
{
    [StringValueObject]
    public partial class UserName
    {
    }

    public class RenameGeneratedUsage2
    {
        public static void SampleStringValueObject()
        {
            var userNameProgram2 = new UserName("Ryota");
            Console.WriteLine($": {userNameProgram2.Value}");
        }
    }
}