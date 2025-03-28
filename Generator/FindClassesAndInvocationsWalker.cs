﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Microsoft.CodeAnalysis.Operations;

namespace MetaJson
{
    class FindClassesAndInvocationsWalker : CSharpSyntaxWalker
    {
        public List<SerializableClass> SerializableClasses { get; set; } = new List<SerializableClass>();
        public List<Variable> AllVariables { get; set; } = new List<Variable>();
        public List<SerializeInvocation> SerializeInvocations { get; set; } = new List<SerializeInvocation>();

        private readonly SemanticModel _semanticModel;
        private readonly GeneratorExecutionContext _context;

        public FindClassesAndInvocationsWalker(SemanticModel semanticModel, GeneratorExecutionContext context)
        {
            _semanticModel = semanticModel;
            _context = context;
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var variableName = node.Variables.First().Identifier.ValueText;
            AllVariables.Add(new Variable(){Name = variableName});
            base.VisitVariableDeclaration(node);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            bool isSerializable = false;

            // Start by a quick check
            foreach (AttributeListSyntax attrList in node.AttributeLists)
            {
                foreach (AttributeSyntax attr in attrList.Attributes)
                {
                    string name = attr.Name.ToString();
                    if (name.Contains("Serialize"))
                    {
                        isSerializable = true;
                        break;
                    }
                }
                if (isSerializable)
                    break;
            }

            if (!isSerializable)
            {
                base.VisitClassDeclaration(node);
                return;
            }

            INamedTypeSymbol type = _semanticModel.GetDeclaredSymbol(node);

            // Proper check
            isSerializable = type.GetAttributes().Any(a => a.AttributeClass.ToString().Equals("MetaJson.SerializeAttribute"));
            if (!isSerializable)
            {
                base.VisitClassDeclaration(node);
                return;
            }

            List<IPropertySymbol> serializableProperties = type.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.GetAttributes().Any(a => a.AttributeClass.ToString().Equals("MetaJson.SerializeAttribute")))
                .ToList();

            // TODO: add methods and variables
            List<IMethodSymbol> methods = type.GetMembers().OfType<IMethodSymbol>().ToList();
            //methods[0].
            //List< VariableDeclarationSyntax > variables = type.GetMembers().OfType<VariableDeclarationSyntax>().ToList();




            SerializableClass sc = new SerializableClass()
            {
                Name = node.Identifier.ValueText,
                Declaration = node,
                Type = type
            };
            //foreach (var variable in variables)
            //{
            //    var myVariable = new Variable
            //    {
            //        Name = variable.Variables.First().Identifier.Value.ToString(),
              
            //    };
            //    sc.Variables.Add(myVariable);
            //}
            foreach (var method in methods)
            {
                if (!method.Name.Contains("."))
                {
                    var myMethod = new Method
                    {
                        Name = method.Name,
                        Symbol = method
                    };
                    sc.Methods.Add(myMethod);
                }
            }
            foreach (IPropertySymbol serializableProperty in serializableProperties)
            {
                // serializableProperty.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as PropertyDeclarationSyntax,

                SerializableProperty sp = new SerializableProperty()
                {
                    Name = serializableProperty.Name,
                    Symbol = serializableProperty,
                };

                sc.Properties.Add(sp);
            }

            SerializableClasses.Add(sc);

            base.VisitClassDeclaration(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is MemberAccessExpressionSyntax memberAccessSyntax
                && memberAccessSyntax.Expression.ToString().Contains("MetaJsonSerializer"))
            {
                // Calling MetaJsonSerializer static methods
                if (memberAccessSyntax.Name is GenericNameSyntax generic && generic.Identifier.ValueText.ToString().Equals("Serialize"))
                {
                    if (node.ArgumentList.Arguments.Count == 1 && generic.TypeArgumentList.Arguments.Count == 1)
                    {
                        TypeSyntax type = generic.TypeArgumentList.Arguments.First();
                        SymbolInfo argSymbol = _semanticModel.GetSymbolInfo(type);
                        SerializeInvocations.Add(new SerializeInvocation()
                        {
                            Invocation = node,
                            TypeArg = argSymbol.Symbol as ITypeSymbol
                        });
                    }
                    else
                    {
                        // error
                    }
                }
            }

            base.VisitInvocationExpression(node);
        }
    }
}
