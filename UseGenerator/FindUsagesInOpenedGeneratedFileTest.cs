using ValueObjectGenerator;

namespace UseGenerator5
{
    [StringValueObject]
    public partial class FindUsagesInOpenedGeneratedFileTest
    {
    }

    class Use1
    {
        private void Test()
        {
            var someNamedVar = new FindUsagesInOpenedGeneratedFileTest("").Value;
        }
    }
}