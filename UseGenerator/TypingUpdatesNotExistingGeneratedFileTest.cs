using MetaJson;
using System;
using System.Collections.Generic;

namespace MetaJson
{
    [Serialize]
    partial class Person
    {
        [Serialize]
        public string Name { get; set; }
    }

    class TypingUpdatesNotExistingGeneratedFileTest
    {
        private void Test()
        {
            var serializedName = new Person().get_name;
        }
    }
}
