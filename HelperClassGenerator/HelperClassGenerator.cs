using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HelperClassGenerator
{
    [Generator]
    public class HelperClassGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif 

            // Register a syntax receiver that will be created for each generation pass.
            // In this case, to detect specific attribute param.
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <summary>
        /// Created on demand before each generation pass.
        /// </summary>
        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<MethodInfo> methodCandidates{ get; } = new List<MethodInfo>();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation.
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
				// TODO: Do not allow duplicated method name
				
                if (syntaxNode is InvocationExpressionSyntax invocationExpressionSyntax)
                {
                    ExpressionSyntax expressionSyntax = invocationExpressionSyntax.Expression;
                    IEnumerable<IdentifierNameSyntax> identifierNameSyntaxes = expressionSyntax.DescendantNodes().OfType<IdentifierNameSyntax>();

                    if(identifierNameSyntaxes != null && identifierNameSyntaxes.Count() >= 2)
                    {
                        List<IdentifierNameSyntax> list = identifierNameSyntaxes.ToList();

                        if(list[0].ToString() == "CInternalHelper")
                        {
                            // We found a method of interest, perform dissection
                            MethodInfo methodInfo = new MethodInfo();
                            methodInfo.MethodName = list[1].ToString();

                            if(invocationExpressionSyntax.ArgumentList != null && invocationExpressionSyntax.ArgumentList.Arguments != null &&
                               invocationExpressionSyntax.ArgumentList.Arguments.Count() > 0)
                            // Ensure there are arguments
                            {
                                // Save the argument types
                                methodInfo.ArgumentList = new List<string>();
                                var argumentList = invocationExpressionSyntax.ArgumentList.Arguments.AsEnumerable();
                                
                                foreach(var item in argumentList)
                                {
                                    if (item.Expression.Kind() == SyntaxKind.StringLiteralExpression)
                                    {
                                        methodInfo.ArgumentList.Add("string");
                                    }
                                    else if (item.Expression.Kind() == SyntaxKind.NumericLiteralExpression)
                                    {
                                        methodInfo.ArgumentList.Add("numeric");
                                    }
                                    else
                                    {
                                        methodInfo.ArgumentList.Add("unknown");
                                    }

                                    methodInfo.ArgumentCount++;
                                }
                            }

                            methodCandidates.Add(methodInfo);
                        }
                    }
                }
            }
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Compilation compilation = context.Compilation;

            // Retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            HashSet<string> methodIncluded = new HashSet<string>();

            // Function to add into file to create the class
            StringBuilder classBuilder = new StringBuilder();

            classBuilder.AppendLine(@"
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoHelperClassGenerator
{         
      public static class CInternalHelper
      {");

            foreach (MethodInfo inputMethodName in receiver.methodCandidates)
            {
                if (!methodIncluded.Contains(inputMethodName.MethodName))
                    methodIncluded.Add(inputMethodName.MethodName);
                else
                    continue;

                foreach(var x in Helpers.MethodImplementationReference)
                {
                    if(Helpers.MeasureSimilarity(EditDistanceAlgorithm.MinDistance(x.Key, inputMethodName.MethodName), Math.Max(x.Key.Length, inputMethodName.MethodName.Length)) >= 0.7)
                    {
                        // Ensure arguments are correct
                        if(inputMethodName.ArgumentCount ==  x.Value.ArgumentCount)
                        {
                            if(inputMethodName.ArgumentCount > 0)
                            {
                                for(int a = 0; a < inputMethodName.ArgumentCount; a++)
                                {
                                    if(inputMethodName.ArgumentList[a] != x.Value.ArgumentList[a])
                                    {
                                        break;
                                    }
                                }

                                classBuilder.AppendLine();
                                classBuilder.AppendLine(string.Format(x.Value.MethodImplementation, inputMethodName.MethodName));
                                break;
                            }
                            else
                            // No argument
                            {
                                classBuilder.AppendLine();
                                classBuilder.AppendLine(string.Format(x.Value.MethodImplementation, inputMethodName.MethodName));
                                break;
                            }

                        }
                    }
                }
            }

            classBuilder.AppendLine(@"     }"); // Closing for class
            classBuilder.AppendLine(@"     
}");                                            // Closing for namespace

            context.AddSource($"CInternalHelper.cs", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }
    }
}
