using System;
using ValueObjectGenerator;

namespace UseGenerator01
{
    [StringValueObject]
    public partial class UserName
    {
    }

    public class RenameGeneratedUsageWithClosedGeneratedFileTest
    {
        public static void SampleStringValueObject()
        {
            var renameTestVar3 = new UserName("Ryota");
            Console.WriteLine($": {renameTestVar3.Value}");
        }
    }
}