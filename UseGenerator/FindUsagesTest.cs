using ValueObjectGenerator;

namespace UseGeneratorFindUsagesTest
{
    [StringValueObject]
    public partial class FindUsagesTest
    {
    }

    class Use1
    {
        private void Test()
        {
            var someNamedVar = new FindUsagesTest("").Value;
        }
    }
}