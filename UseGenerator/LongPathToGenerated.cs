﻿using ValueObjectGenerator;

namespace UseGenerator.Some.Long.Path.More.Than255.No.Navigation.Happens.In.Both.ReSharper.And.VisualStudio
{
    [StringValueObject]
    public partial class LongPathToGenerated
    {
        
    }

    class UseLongPathToGenerated
    {
        private void UseLongPathToGenerated1()
        {
            var value = new LongPathToGenerated("").Value;
        }
    }
}