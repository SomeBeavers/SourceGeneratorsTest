namespace MetaJson
{
    [Serialize]
    public partial class RenameMemberWithOpenedGeneratedFileTest_Generated
    {
        [Serialize]
        public string Name { get; set; }

        public void Test() { }
    }

    public class RenameMemberWithOpenedGeneratedFileTest
    {
        private void UseGenerated()
        {
            new RenameMemberWithOpenedGeneratedFileTest_Generated().Test();
            var RenameMemberWithOpenedGeneratedFileTest_name = new RenameMemberWithOpenedGeneratedFileTest_Generated().get_name;
        }
    }
}