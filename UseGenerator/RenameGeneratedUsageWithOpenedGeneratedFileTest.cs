using System;
using ValueObjectGenerator;

namespace UseGenerator02
{
    [StringValueObject]
    public partial class UserName
    {
    }
    public class RenameGeneratedUsageWithOpenedGeneratedFileTest
    {
        public static void SampleStringValueObject()
        {
            var userNameProgram3 = new UserName("Ryota");
            Console.WriteLine($": {userNameProgram3.Value}");
        }
    }
}