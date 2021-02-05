using ValueObjectGenerator;

namespace UseGenerator4
{
    [StringValueObject]
    public partial class FindUsagesInNotExistingGeneratedFileTest
    {
    }

    class Use1
    {
        private void Test()
        {
            new FindUsagesInNotExistingGeneratedFileTest("");
        }
    }
}