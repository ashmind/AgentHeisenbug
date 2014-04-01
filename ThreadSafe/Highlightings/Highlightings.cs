using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using ThreadSafety.Highlightings;


[assembly: RegisterConfigurableSeverity(
    CallToStaticMethodNotThreadSafe.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Call to static method that is not thread-safe from type annotated with [ThreadSafe]",
    "Call to static method that is not thread-safe from type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    MutableFieldOrPropertyNotThreadSafe.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable property or field in type annotated with [ThreadSafe]",
    "Mutable property or field in type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    MutableFieldInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable field in type annotated with [ReadOnly]",
    "Mutable field in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ExposingNotThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Exposing type that is not thread-safe from type annotated with [ThreadSafe]",
    "Exposing type that is not thread-safe from type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]

namespace ThreadSafety.Highlightings {
    [ConfigurableSeverityHighlighting(CallToStaticMethodNotThreadSafe.Id, CSharpLanguage.Name)]
    public class CallToStaticMethodNotThreadSafe : ThreadSafetyHighligtingBase {
        public const string Id = "CallToStaticMethodNotThreadSafe";

        [StringFormatMethod("messageFormat")]
        public CallToStaticMethodNotThreadSafe(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldOrPropertyNotThreadSafe.Id, CSharpLanguage.Name)]
    public class MutableFieldOrPropertyNotThreadSafe : ThreadSafetyHighligtingBase {
        public const string Id = "MutableFieldOrPropertyNotThreadSafe";

        [StringFormatMethod("messageFormat")]
        public MutableFieldOrPropertyNotThreadSafe(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInReadOnlyType.Id, CSharpLanguage.Name)]
    public class MutableFieldInReadOnlyType : ThreadSafetyHighligtingBase {
        public const string Id = "MutableFieldInReadOnlyType";

        [StringFormatMethod("messageFormat")]
        public MutableFieldInReadOnlyType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(ExposingNotThreadSafeType.Id, CSharpLanguage.Name)]
    public class ExposingNotThreadSafeType : ThreadSafetyHighligtingBase {
        public const string Id = "ExposingNotThreadSafeType";

        [StringFormatMethod("messageFormat")]
        public ExposingNotThreadSafeType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }
}