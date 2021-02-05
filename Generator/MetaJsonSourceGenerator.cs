using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MetaJson
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor ClassNotSerializable = new(
            "MJ-001",
            "Class Not Serializable",
            "Class '{0}' is not found or not set as serializable",
            "MetaJson.Serialization",
            DiagnosticSeverity.Error,
            true);
    }

    [Generator]
    public class MetaJsonSourceGenerator : ISourceGenerator
    {
        private static Timer aTimer;

        public void Execute(GeneratorExecutionContext context)
        {
            var serializableClasses = new List<SerializableClass>();
            var variables = new List<Variable>();
            var serializeInvocations = new List<SerializeInvocation>();
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(tree);
                var walk = new FindClassesAndInvocationsWalker(semanticModel, context);
                walk.Visit(tree.GetRoot());
                serializableClasses.AddRange(walk.SerializableClasses);
                variables.AddRange(walk.AllVariables);
                serializeInvocations.AddRange(walk.SerializeInvocations);
            }

            foreach (var serializableClass in serializableClasses)
            {
                // Create methods!
                var sb = new StringBuilder();
                var name = $@"MetaJsonSerializer_{serializableClass.Name}";
                sb.AppendLine($@"
                using System;
                using System.Text;

                namespace MetaJson
                {{
                   
                    public static class {name}
                    {{"
                );

                const string SPC = "    ";
                foreach (var invocation in serializeInvocations)
                {
                    var invocationTypeStr = invocation.TypeArg.ToString();

                    sb.Append(
                        $@"{SPC}{SPC}public static string Serialize<T>({invocationTypeStr} obj) where T: {invocationTypeStr}");
                    sb.Append(@"
                        {
                ");
                    GenerateMethodBody(sb, invocation, serializableClasses, context);
                    sb.Append(@"
                        }

                ");
                }

                // Class footer
                sb.Append(@"
                    }"
                );

                for (var i = 0; i < serializableClasses.Count; i++)
                {
                    var publicStaticVoidDonothing = "{public static void DoNothing() {}}";
                    sb.Append($"public static class DummySymbol_{serializableClass.Name}{i} {publicStaticVoidDonothing}");
                }


                sb.Append($"public partial class {serializableClass.Name}");
                sb.AppendLine();
                sb.Append("{");
                sb.AppendLine();
                sb.Append($"//{variables.Count}");
                sb.AppendLine();
                foreach (var serializableClassVariable in variables)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append($"public string {serializableClassVariable.Name}");
                    sb.Append(" {get;set;}");
                }

                foreach (var method in serializableClass.Methods)
                {
                    if (!method.Name.Contains("Test"))
                    {
                        sb.Append($"public string {method.Name.ToLower()}");
                        sb.Append(" {get;set;} ");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.Append($"public void Created_{method.Name.ToLower()} ()");
                        sb.Append(" {");
                        sb.Append($"{method.Name}();");
                        sb.Append(" }");
                        sb.AppendLine();
                    }
                }

                sb.Append("}");


                sb.Append("}");

                var generatedFileSource = sb.ToString();

                context.AddSource(name, SourceText.From(generatedFileSource, Encoding.UTF8));
            }
        }


        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private JsonNode BuildTree(ITypeSymbol symbol, string csObj, List<SerializableClass> knownClasses,
            GeneratorExecutionContext context)
        {
            if (symbol.Kind != SymbolKind.NamedType)
                return null;


            // primitive types
            switch (symbol.SpecialType)
            {
                case SpecialType.System_Int32:
                    return new NumericNode(csObj);
                case SpecialType.System_String:
                    return new StringNode(csObj);
            }


            // if is serializable class
            var invocationTypeStr = symbol.ToString();
            var foundClass = knownClasses.FirstOrDefault(c => c.Type.ToString().Equals(invocationTypeStr));
            if (foundClass != null)
            {
                var objectNode = new ObjectNode();
                foreach (var sp in foundClass.Properties)
                {
                    var value = BuildTree(sp.Symbol.Type, $"{csObj}.{sp.Name}", knownClasses, context);
                    objectNode.Properties.Add((sp.Name, value));
                }

                return objectNode;
            }

            // list, dictionnary, ...

            // fallback on list
            INamedTypeSymbol enumerable = null;
            if (symbol.MetadataName.Equals("IList`1"))
                enumerable = symbol as INamedTypeSymbol;
            else
                enumerable = symbol.AllInterfaces.FirstOrDefault(i => i.MetadataName.Equals("IList`1"));
            if (enumerable != null)
            {
                var listType = enumerable.TypeArguments.First();

                var listNode = new ListNode(csObj);
                listNode.ElementType = BuildTree(listType, $"{csObj}[i]", knownClasses, context);
                return listNode;
            }

            // fallback on enumerables?


            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ClassNotSerializable,
                symbol.Locations.First(), invocationTypeStr));
            return null;
        }


        private void GenerateMethodBody(StringBuilder sb, SerializeInvocation invocation,
            List<SerializableClass> knownClasses, GeneratorExecutionContext context)
        {
            // Create nodes
            var treeContext = new TreeContext();
            treeContext.IndentCSharp(+3);
            var ct = treeContext.CSharpIndent;

            var nodes = new List<MethodNode>();
            nodes.Add(new CSharpLineNode($"{ct}StringBuilder sb = new StringBuilder();"));

            var jsonTree = BuildTree(invocation.TypeArg, "obj", knownClasses, context);
            nodes.AddRange(jsonTree.GetNodes(treeContext));

            ct = treeContext.CSharpIndent;
            nodes.Add(new CSharpLineNode($"{ct}return sb.ToString();"));

            // Merge json nodes
            var mergedNodes = new List<MethodNode>();
            var streak = new List<PlainJsonNode>();
            foreach (var node in nodes)
                if (node is PlainJsonNode js)
                {
                    streak.Add(js);
                }
                else
                {
                    if (streak.Count > 0)
                    {
                        mergedNodes.Add(MergeJsonNodes(streak));
                        streak.Clear();
                    }

                    mergedNodes.Add(node);
                }

            if (streak.Count > 0) mergedNodes.Add(MergeJsonNodes(streak.OfType<PlainJsonNode>()));

            nodes = mergedNodes;

            // Convert json to C#
            for (var i = 0; i < nodes.Count; ++i)
                if (nodes[i] is PlainJsonNode js)
                {
                    CSharpNode cs =
                        new CSharpLineNode($"{js.CSharpIndent}sb.Append(\"{js.Value.Replace("\"", "\\\"")}\");");
                    nodes.RemoveAt(i);
                    nodes.Insert(i, cs);
                }

            // Output as string
            foreach (var node in mergedNodes)
                if (node is CSharpNode cs)
                    sb.Append(cs.CSharpCode);
        }

        private PlainJsonNode MergeJsonNodes(IEnumerable<PlainJsonNode> nodes)
        {
            var combined = string.Join("", nodes.Select(n => n.Value));
            return new PlainJsonNode(nodes.First().CSharpIndent, combined);
        }
    }

    internal class SerializableClass
    {
        public string Name { get; set; }
        public ClassDeclarationSyntax Declaration { get; set; }

        public List<SerializableProperty> Properties { get; set; } = new();

        //public List<Variable> Variables { get; set; } = new List<Variable>();
        public List<Method> Methods { get; set; } = new();
        public INamedTypeSymbol Type { get; set; }
    }

    internal class Variable
    {
        public string Name { get; set; }
    }

    internal class Method
    {
        public string Name { get; set; }
        public IMethodSymbol Symbol { get; set; }
    }

    internal class SerializableProperty
    {
        public string Name { get; set; }
        public IPropertySymbol Symbol { get; set; }
    }

    internal class SerializeInvocation
    {
        public InvocationExpressionSyntax Invocation { get; set; }
        public ITypeSymbol TypeArg { get; set; }
    }

    internal class ClassWalkerState
    {
        public SerializableClass CurrentClass { get; set; } = null;
        public bool IsSerializable => CurrentClass != null;
    }
}