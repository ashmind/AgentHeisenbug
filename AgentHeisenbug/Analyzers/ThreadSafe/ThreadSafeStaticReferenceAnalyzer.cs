using System.Linq;
using AgentHeisenbug.Processing;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using AgentHeisenbug.Highlightings;

namespace AgentHeisenbug.Analyzers.ThreadSafe {
    [ElementProblemAnalyzer(new[] { typeof(IReferenceExpression) }, HighlightingTypes = new[] { typeof(AccessToNonThreadSafeStaticMemberInThreadSafeType) })]
    public class ThreadSafeStaticReferenceAnalyzer : ElementProblemAnalyzer<IReferenceExpression> {
        private enum MemberKind {
            Method,
            Property,
            Field,
            Constant,
            Unknown
        }

        [NotNull] private readonly AnalyzerPreconditions _preconditions;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeStaticReferenceAnalyzer(
            [NotNull] AnalyzerPreconditions preconditions,
            [NotNull] HeisenbugFeatureProvider featureProvider
        ) {
            _preconditions = preconditions;
            _featureProvider = featureProvider;
        }

        protected override void Run(IReferenceExpression element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_preconditions.MustBeThreadSafe(element))
                return;
            
            var resolved = element.Reference.Resolve();
            var member = resolved.DeclaredElement as ITypeMember;
            if (member == null || !member.IsStatic)
                return;

            var kind = GetMemberKind(member);
            if (kind == MemberKind.Unknown || kind == MemberKind.Constant)
                return;

            if (_featureProvider.GetFeatures(member).IsStaticAccessThreadSafe)
                return;

            consumer.AddHighlighting(new AccessToNonThreadSafeStaticMemberInThreadSafeType(
                element.NotNull(), kind.ToString(), member.ShortName
            ));
        }

        private MemberKind GetMemberKind(ITypeMember member) {
            var field = member as IField;
            if (field != null)
                return field.IsConstant ? MemberKind.Constant : MemberKind.Field;

            if (member is IProperty)
                return MemberKind.Property;

            if (member is IMethod)
                return MemberKind.Method;

            return MemberKind.Unknown;
        }
    }
}