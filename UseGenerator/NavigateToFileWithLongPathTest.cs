using ValueObjectGenerator;

namespace UseGenerator.Some.Long.Path.More.Than255.No.Navigation.Happens.In.Both.ReSharper.And.VisualStudioUseGenerator.Some.Long.Path.More.Than255.No.Navigation.Happens.In.Both.ReSharper.And.VisualStudio
{
    [StringValueObject]
    public partial class NavigateToFileWithLongPathTest
    {
        
    }

    class UseLongPathToGenerated
    {
        private void UseLongPathToGenerated1()
        {
            var longPathValue = new NavigateToFileWithLongPathTest("").Value;
        }
    }
}