namespace MetaJson
{
    [Serialize]
    public partial class RenameMemberWithNotExistingGeneratedFileTest_Generated
    {
        [Serialize]
        public string Name { get; set; }

        public void Test() { }
    }

    public class RenameMemberWithNotExistingGeneratedFileTest
    {
        private void UseGenerated()
        {
            new RenameMemberWithNotExistingGeneratedFileTest_Generated().Test();
            var RenameMemberWithNotExistingGeneratedFileTest_name = new RenameMemberWithNotExistingGeneratedFileTest_Generated()._text;
        }
    }
}