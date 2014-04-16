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
    MutableFieldInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable field in type annotated with [ThreadSafe]",
    "Mutable field in type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    MutableAutoPropertyInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable auto property in type annotated with [ThreadSafe]",
    "Mutable auto property in type annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Auto property assigned outside of constructor in type annotated with [ThreadSafe]",
    "Auto property assigned outside of constructor in type annotated with [ThreadSafe]",
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
    AutoPropertyOfNonThreadSafeTypeInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Auto property of type that is not thread-safe, in type annotated with [ThreadSafe]",
    "Auto property of type that is not thread-safe, in type annotated with [ThreadSafe]",
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
    public class CallToNotThreadSafeStaticMethodInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "CallToNotThreadSafeStaticMethodInThreadSafeType";

        public CallToNotThreadSafeStaticMethodInThreadSafeType(ITreeNode element, string methodName) : base(
            element,
            "Method '{0}' is not declared to be thread-safe.",
            methodName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInThreadSafeType.Id, CSharpLanguage.Name)]
    public class MutableFieldInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "MutableFieldInThreadSafeType";

        public MutableFieldInThreadSafeType(ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ThreadSafe] class should be readonly.",
            fieldName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableAutoPropertyInThreadSafeType.Id, CSharpLanguage.Name)]
    public class MutableAutoPropertyInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "MutableAutoPropertyInThreadSafeType";

        public MutableAutoPropertyInThreadSafeType(ITreeNode element, string propertyName) : base(
            element,
            "Setter of auto property '{0}' in a [ThreadSafe] class should be private.",
            propertyName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType.Id, CSharpLanguage.Name)]
    public class AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType";

        public AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType(ITreeNode element, string propertyName, string @static) : base(
            element,
            "{1}auto property '{0}' in a [ThreadSafe] class should only be assigned in a {1}constructor.",
            propertyName, @static
        ) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class FieldOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "FieldOfNonThreadSafeTypeInThreadSafeType";

        public FieldOfNonThreadSafeTypeInThreadSafeType(ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' of field '{0}' in a [ThreadSafe] type should be thread-safe.",
            fieldName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class AutoPropertyOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AutoPropertyOfNonThreadSafeTypeInThreadSafeType";

        public AutoPropertyOfNonThreadSafeTypeInThreadSafeType(ITreeNode element, string propertyName, string typeName) : base(
            element,
            "Type '{1}' of auto property '{0}' in a [ThreadSafe] type should be thread-safe.",
            propertyName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInReadOnlyType.Id, CSharpLanguage.Name)]
    public class MutableFieldInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "MutableFieldInReadOnlyType";

        public MutableFieldInReadOnlyType(ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ThreadSafe] class should be readonly.",
            fieldName
        ) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfMutableTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public class FieldOfMutableTypeInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "FieldOfMutableTypeInReadOnlyType";

        public FieldOfMutableTypeInReadOnlyType(ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' of field '{0}' in a [ReadOnly] type should be read only.",
            fieldName, typeName
        ) {}
    }
}

