using System.Linq;
using AgentHeisenbug.Highlightings.ThreadSafe;
using AgentHeisenbug.Processing;
using AgentHeisenbug.Processing.FeatureTypes;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers {
    [ElementProblemAnalyzer(new[] { typeof(IReferenceExpression) }, HighlightingTypes = new[] { typeof(AccessToNonThreadSafeStaticMemberInThreadSafeType) })]
    public class ThreadSafeStaticReferenceAnalyzer : ElementProblemAnalyzer<IReferenceExpression> {
        private enum MemberKind {
            Method,
            Property,
            Field,
            Constant,
            Unknown
        }

        [NotNull] private readonly IAnalyzerPrecondition<ThreadSafe> _precondition;
        [NotNull] private readonly HeisenbugFeatureProvider _featureProvider;

        public ThreadSafeStaticReferenceAnalyzer(
            [NotNull] IAnalyzerPrecondition<ThreadSafe> precondition,
            [NotNull] HeisenbugFeatureProvider featureProvider
        ) {
            _precondition = precondition;
            _featureProvider = featureProvider;
        }

        protected override void Run(IReferenceExpression element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer) {
            if (!_precondition.Applies(element))
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

            consumer.AddHighlighting(new AccessToNonThreadSafeStaticMemberInThreadSafeType(element.NotNull(), member, kind.ToString()));
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