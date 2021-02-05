namespace MetaJson
{
    [Serialize]
    public partial class RenameMemberWithClosedGeneratedFileTest_Generated
    {
        [Serialize]
        public string Name { get; set; }

        public void Test() { }
    }

    public class RenameMemberWithClosedGeneratedFileTest
    {
        private void UseGenerated()
        {
            new RenameMemberWithClosedGeneratedFileTest_Generated().Test();
        }
    }
}