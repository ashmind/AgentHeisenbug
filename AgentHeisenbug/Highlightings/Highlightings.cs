using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;


[assembly: RegisterConfigurableSeverity(
    CallToNotThreadSafeStaticMethodInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Call to static method that is not thread-safe from type annotated with [ThreadSafe]",
    "Call to static method that is not thread-safe from type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    MutableFieldOrPropertyInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable property or field in type annotated with [ThreadSafe]",
    "Mutable property or field in type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    FieldOfNonThreadSafeTypeInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Field of type that is not thread-safe, in type annotated with [ThreadSafe]",
    "Field of type that is not thread-safe, in type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ExposingNotThreadSafeTypeInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Exposing type that is not thread-safe from type annotated with [ThreadSafe]",
    "Exposing type that is not thread-safe from type annotated with [ThreadSafe]",
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
    FieldOfMutableTypeInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Field of mutable type in type annotated with [ReadOnly]",
    "Field of mutable type in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]

namespace AgentHeisenbug.Highlightings {
    [ConfigurableSeverityHighlighting(CallToNotThreadSafeStaticMethodInThreadSafeType.Id, CSharpLanguage.Name)]
    public class CallToNotThreadSafeStaticMethodInThreadSafeType : ThreadSafetyHighligtingBase {
        public const string Id = "CallToNotThreadSafeStaticMethodInThreadSafeType";

        [StringFormatMethod("messageFormat")]
        public CallToNotThreadSafeStaticMethodInThreadSafeType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldOrPropertyInThreadSafeType.Id, CSharpLanguage.Name)]
    public class MutableFieldOrPropertyInThreadSafeType : ThreadSafetyHighligtingBase {
        public const string Id = "MutableFieldOrPropertyInThreadSafeType";

        [StringFormatMethod("messageFormat")]
        public MutableFieldOrPropertyInThreadSafeType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class FieldOfNonThreadSafeTypeInThreadSafeType : ThreadSafetyHighligtingBase {
        public const string Id = "FieldOfNonThreadSafeTypeInThreadSafeType";

        [StringFormatMethod("messageFormat")]
        public FieldOfNonThreadSafeTypeInThreadSafeType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(ExposingNotThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class ExposingNotThreadSafeTypeInThreadSafeType : ThreadSafetyHighligtingBase {
        public const string Id = "ExposingNotThreadSafeTypeInThreadSafeType";

        [StringFormatMethod("messageFormat")]
        public ExposingNotThreadSafeTypeInThreadSafeType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInReadOnlyType.Id, CSharpLanguage.Name)]
    public class MutableFieldInReadOnlyType : ThreadSafetyHighligtingBase {
        public const string Id = "MutableFieldInReadOnlyType";

        [StringFormatMethod("messageFormat")]
        public MutableFieldInReadOnlyType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfMutableTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public class FieldOfMutableTypeInReadOnlyType : ThreadSafetyHighligtingBase {
        public const string Id = "FieldOfMutableTypeInReadOnlyType";

        [StringFormatMethod("messageFormat")]
        public FieldOfMutableTypeInReadOnlyType(ITreeNode element, string messageFormat, params object[] args) : base(element, messageFormat, args) {}
    }
}