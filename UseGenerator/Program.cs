using System;
using JsonSrcGen;
using ValueObjectGenerator;
using CSV;

namespace UseGenerator
{
    [Json]
    public class JsonArrayClass
    {
        public bool[] BooleanArray { get; set; }
    }

    [StringValueObject]
    public partial class UserName
    {
    }

    //[IntValueObject]
   public partial class ProductId
    {
    }

    [IntValueObject]
    public partial class CategoryId
    {
    }

    [LongValueObject]
    public partial class ConsumeId
    {
    }

    [FloatValueObject]
    public partial class Scale
    {
    }

    [DoubleValueObject]
    public partial class Rate
    {
    }

    public static class Program
    {

        public static void UseJsonArray()
        {
            var _convert = new JsonConverter();

            var jsonClass = new JsonArrayClass()
            {
                BooleanArray = new bool[] { true, false }
            };

            var json = _convert.ToJson(jsonClass);
        }

        public static void SampleStringValueObject()
        {
            var userName = new UserName("Ryota");
            Console.WriteLine($"userName.Value: {userName.Value}");
        }

        public static void SampleIntValueObject()
        {
            /*use ProductId*/
        }


        public static void Run()
        {
            Cars.All.ToList().ForEach(c => WriteLine($"{c.Brand}\t{c.Model}\t{c.NYear}\t{c.Cc}"));
        }


        public static void SampleLongValueObject()
        {
            Console.WriteLine("---LongValueObject Sample---");
            var consumeId = new ConsumeId(1L);
            var otherConsumeId = consumeId;

            Console.WriteLine("var consumeId = new ConsumeId(1L);");
            Console.WriteLine("var otherConsumeId = consumeId;");
            Console.WriteLine();
            Console.WriteLine($"consumeId: {consumeId}");
            Console.WriteLine($"consumeId.Value: {consumeId.Value}");
            Console.WriteLine();
            Console.WriteLine($"consumeId == otherConsumeId: {consumeId == otherConsumeId}");
            Console.WriteLine($"consumeId.Equals(otherConsumeId): {consumeId.Equals(otherConsumeId)}");
            Console.WriteLine($"consumeId == new ConsumeId(1L): {consumeId == new ConsumeId(1L)}");
            Console.WriteLine($"consumeId.Equals(new ConsumeId(1L)): {consumeId.Equals(new ConsumeId(1L))}");
            Console.WriteLine();
            Console.WriteLine($"consumeId == new ConsumeId(2L): {consumeId == new ConsumeId(2L)}");
            Console.WriteLine($"consumeId.Equals(new ConsumeId(2L): {consumeId.Equals(new ConsumeId(2L))}");
            Console.WriteLine($"consumeId.Equals(null): {consumeId.Equals(null)}");
            Console.WriteLine($"consumeId.Equals(1L): {consumeId.Equals(1L)}");
            Console.WriteLine();
        }

        public static void SampleFloatValueObject()
        {
            Console.WriteLine("---FloatValueObject Sample---");
            var scale = new Scale(0.5F);
            var otherScale = scale;

            Console.WriteLine("var scale = new Scale(0.5F);");
            Console.WriteLine("var otherScale = scale;");
            Console.WriteLine();
            Console.WriteLine($"scale: {scale}");
            Console.WriteLine($"scale.Value: {scale.Value}");
            Console.WriteLine();
            Console.WriteLine($"scale == otherScale: {scale == otherScale}");
            Console.WriteLine($"scale.Equals(otherScale): {scale.Equals(otherScale)}");
            Console.WriteLine($"scale == new Scale(1L): {scale == new Scale(1L)}");
            Console.WriteLine($"scale.Equals(new Scale(1L)): {scale.Equals(new Scale(1L))}");
            Console.WriteLine();
            Console.WriteLine($"scale == new Scale(2L): {scale == new Scale(2L)}");
            Console.WriteLine($"scale.Equals(new Scale(2L): {scale.Equals(new Scale(2L))}");
            Console.WriteLine($"scale.Equals(null): {scale.Equals(null)}");
            Console.WriteLine($"scale.Equals(1L): {scale.Equals(0.5F)}");
            Console.WriteLine();
        }

        public static void SampleDoubleValueObject()
        {
            Console.WriteLine("---DoubleValueObject Sample---");
            var rate = new Rate(0.25);
            var otherRate = rate;

            Console.WriteLine("var rate = new Rate(0.25);");
            Console.WriteLine("var otherRate = rate;");
            Console.WriteLine();
            Console.WriteLine($"rate: {rate}");
            Console.WriteLine($"rate.Value: {rate.Value}");
            Console.WriteLine();
            Console.WriteLine($"rate == otherRate: {rate == otherRate}");
            Console.WriteLine($"rate.Equals(otherRate): {rate.Equals(otherRate)}");
            Console.WriteLine($"rate == new Rate(0.25): {rate == new Rate(0.25)}");
            Console.WriteLine($"rate.Equals(new Rate(0.25)): {rate.Equals(new Rate(0.25))}");
            Console.WriteLine();
            Console.WriteLine($"rate == new Rate(0.1): {rate == new Rate(0.1)}");
            Console.WriteLine($"rate.Equals(new Rate(0.1): {rate.Equals(new Rate(0.1))}");
            Console.WriteLine($"rate.Equals(null): {rate.Equals(null)}");
            Console.WriteLine($"rate.Equals(0.25): {rate.Equals(0.25)}");
            Console.WriteLine();
        }

        public static void SampleCustomizedPropertyName()
        {
            Console.WriteLine("---CustomizedPropertyName Sample---");
            Console.WriteLine("var fieldName = new CustomizedPropertyName(\"CustomizedPropertyName\");");

            var fieldName = new CustomizedPropertyName("CustomizedPropertyName");
            Console.WriteLine($"fieldName.StringValue : {fieldName.StringValue}");
        }

        public static void Main(string[] args)
        {
            SampleStringValueObject();
            SampleIntValueObject();
            SampleLongValueObject();
            SampleFloatValueObject();
            SampleDoubleValueObject();
            SampleCustomizedPropertyName();
        }
    }
}