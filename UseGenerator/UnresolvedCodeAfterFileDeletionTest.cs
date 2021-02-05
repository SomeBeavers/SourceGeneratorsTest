namespace UseGenerator8
{
    public partial class PartialClassToDelete
    {
    }

    public class UnresolvedCodeAfterFileDeletionTest
    {
        private void Test()
        {
            var partialClassToDelete = new PartialClassToDelete("Test");
        }
    }
}