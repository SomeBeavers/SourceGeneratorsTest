using System;
using System.Collections.Generic;
using AutoNotify;

namespace GeneratedDemo
{
    public partial class ExampleViewModel
    {
        [AutoNotify(PropertyName = "Count")] private int _amount = 5;

        [AutoNotify] private string _text = "private field text";
    }

    public static class TypingTest
    {
        public static void Run()
        {
            var vm = new ExampleViewModel();

            var text = vm.Text;
            Console.WriteLine($"Text = {text}");

            var count = vm.Count;
            Console.WriteLine($"Count = {count}");
        }
    }
}
