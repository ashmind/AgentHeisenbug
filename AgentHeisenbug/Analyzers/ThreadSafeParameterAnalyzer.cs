using System.Linq;
using AgentHeisenbug.Analyzers.Helpers;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers {
    //[ElementProblemAnalyzer(new[] { typeof(IParameterDeclaration) }, HighlightingTypes = new[] { typeof(MutableFieldOrPropertyInThreadSafeType) })]
    //public class ThreadSafeParameterAnalyzer : IElementProblemAnalyzer {
    //    private readonly AnalyzerPreconditions preconditions;
    //    private readonly ReferencedTypeHelper referenceHelper;

    //    public ThreadSafeParameterAnalyzer(AnalyzerPreconditions preconditions, ReferencedTypeHelper referenceHelper) {
    //        this.preconditions = preconditions;
    //        this.referenceHelper = referenceHelper;
    //    }

    //    public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
    //        var parameter = (IParameterDeclaration)element;
    //        if (!this.preconditions.MustBeThreadSafe(parameter))
    //            return;

    //        if (!this.referenceHelper.GetThreadSafety(parameter.Type).Instance) {
    //            consumer.AddHighlighting(new FieldOfNonThreadSafeTypeInThreadSafeType(
    //                parameter,
    //                "Type '{0}' of parameter '{1}' in a [ThreadSafe] type should be thread-safe.",
    //                parameter.Type.GetPresentableName(CSharpLanguage.Instance), parameter.DeclaredName)
    //            );
    //        }
    //    }
    //}
}
