using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings;


[assembly: RegisterConfigurableSeverity(
    ThreadSafeInterfaceImplementedByNonThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Thread-safe interface implemented by type that is not annotated with [ThreadSafe].",
    "Thread-safe interface implemented by type that is not annotated with [ThreadSafe].",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ThreadSafeClassInheritedByNonThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Thread-safe class inherited by type that is not annotated with [ThreadSafe].",
    "Thread-safe class inherited by type that is not annotated with [ThreadSafe].",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    CallToNonThreadSafeStaticMethodInThreadSafeType.Id,
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
    FieldOfNonThreadSafeTypeInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Field of type that is not thread-safe, in type annotated with [ThreadSafe]",
    "Field of type that is not thread-safe, in type annotated with [ThreadSafe]",
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
    AutoPropertyOfNonThreadSafeTypeInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Auto property of type that is not thread-safe, in type annotated with [ThreadSafe]",
    "Auto property of type that is not thread-safe, in type annotated with [ThreadSafe]",
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
    ParameterOfNonThreadSafeTypeInThreadSafeMethod.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Parameter of type that is not thread-safe in a thread-safe method",
    "Parameter of type that is not thread-safe in a thread-safe method",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ReadOnlyInterfaceImplementedByNonReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Readonly interface implemented by type that is not annotated with [ReadOnly].",
    "Readonly interface implemented by type that is not annotated with [ReadOnly].",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ReadOnlyClassInheritedByNonReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Readonly class inherited by type that is not annotated with [ReadOnly].",
    "Readonly class inherited by type that is not annotated with [ReadOnly].",
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
    FieldOfNonReadOnlyTypeInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Field of mutable type in type annotated with [ReadOnly]",
    "Field of mutable type in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    MutableAutoPropertyInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Mutable auto property in type annotated with [ReadOnly]",
    "Mutable auto property in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    AutoPropertyOfNonReadOnlyTypeInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Auto property of type that is not readonly, in type annotated with [ReadOnly]",
    "Auto property of type that is not readonly, in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Auto property assigned outside of constructor in type annotated with [ReadOnly]",
    "Auto property assigned outside of constructor in type annotated with [ReadOnly]",
    Severity.WARNING,
    false
)]

namespace AgentHeisenbug.Highlightings {
    [ConfigurableSeverityHighlighting(ThreadSafeInterfaceImplementedByNonThreadSafeType.Id, CSharpLanguage.Name)]
    public class ThreadSafeInterfaceImplementedByNonThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ThreadSafeInterfaceImplementedByNonThreadSafeType";

        public ThreadSafeInterfaceImplementedByNonThreadSafeType([NotNull] ITreeNode element, string interfaceName, string typeName) : base(
            element,
            "Interface '{0}' is thread-safe, but type '{1}' is not annotated with [ThreadSafe].",
            interfaceName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(ThreadSafeClassInheritedByNonThreadSafeType.Id, CSharpLanguage.Name)]
    public class ThreadSafeClassInheritedByNonThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ThreadSafeClassInheritedByNonThreadSafeType";

        public ThreadSafeClassInheritedByNonThreadSafeType([NotNull] ITreeNode element, string baseClassName, string typeName) : base(
            element,
            "Class '{0}' is thread-safe, but type '{1}' is not annotated with [ThreadSafe].",
            baseClassName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(CallToNonThreadSafeStaticMethodInThreadSafeType.Id, CSharpLanguage.Name)]
    public class CallToNonThreadSafeStaticMethodInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.CallToNonThreadSafeStaticMethodInThreadSafeType";

        public CallToNonThreadSafeStaticMethodInThreadSafeType([NotNull] ITreeNode element, string methodName) : base(
            element,
            "Method '{0}' is not declared to be thread-safe.",
            methodName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInThreadSafeType.Id, CSharpLanguage.Name)]
    public class MutableFieldInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableFieldInThreadSafeType";

        public MutableFieldInThreadSafeType([NotNull] ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ThreadSafe] class should be readonly.",
            fieldName
        ) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class FieldOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.FieldOfNonThreadSafeTypeInThreadSafeType";

        public FieldOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' of field '{0}' in a [ThreadSafe] type should be thread-safe.",
            fieldName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableAutoPropertyInThreadSafeType.Id, CSharpLanguage.Name)]
    public class MutableAutoPropertyInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableAutoPropertyInThreadSafeType";

        public MutableAutoPropertyInThreadSafeType([NotNull] ITreeNode element, string propertyName) : base(
            element,
            "Setter of auto property '{0}' in a [ThreadSafe] class should be private.",
            propertyName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public class AutoPropertyOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyOfNonThreadSafeTypeInThreadSafeType";

        public AutoPropertyOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string propertyName, string typeName) : base(
            element,
            "Type '{1}' of auto property '{0}' in a [ThreadSafe] type should be thread-safe.",
            propertyName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType.Id, CSharpLanguage.Name)]
    public class AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType";

        public AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType([NotNull] ITreeNode element, string propertyName, string @static) : base(
            element,
            "{1}auto property '{0}' in a [ThreadSafe] class should only be assigned in a {1}constructor.",
            propertyName, @static
        ) {}
    }

    [ConfigurableSeverityHighlighting(ParameterOfNonThreadSafeTypeInThreadSafeMethod.Id, CSharpLanguage.Name)]
    public class ParameterOfNonThreadSafeTypeInThreadSafeMethod : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ParameterOfNonThreadSafeTypeInThreadSafeMethod";

        public ParameterOfNonThreadSafeTypeInThreadSafeMethod([NotNull] ITreeNode element, string paramterName, string typeName) : base(
            element,
            "Type '{1}' of parameter '{0}' in a thread-safe method should be thread-safe unless method is [Pure].",
            paramterName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(ReadOnlyInterfaceImplementedByNonReadOnlyType.Id, CSharpLanguage.Name)]
    public class ReadOnlyInterfaceImplementedByNonReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ReadOnlyInterfaceImplementedByNonReadOnlyType";

        public ReadOnlyInterfaceImplementedByNonReadOnlyType([NotNull] ITreeNode element, string interfaceName, string typeName) : base(
            element,
            "Interface '{0}' is readonly, but type '{1}' is not annotated with [ReadOnly].",
            interfaceName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(ReadOnlyClassInheritedByNonReadOnlyType.Id, CSharpLanguage.Name)]
    public class ReadOnlyClassInheritedByNonReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ReadOnlyClassInheritedByNonReadOnlyType";

        public ReadOnlyClassInheritedByNonReadOnlyType([NotNull] ITreeNode element, string baseClassName, string typeName) : base(
            element,
            "Class '{0}' is readonly, but type '{1}' is not annotated with [ReadOnly].",
            baseClassName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableFieldInReadOnlyType.Id, CSharpLanguage.Name)]
    public class MutableFieldInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableFieldInReadOnlyType";

        public MutableFieldInReadOnlyType([NotNull] ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ReadOnly] type should be readonly.",
            fieldName
        ) {}
    }

    [ConfigurableSeverityHighlighting(FieldOfNonReadOnlyTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public class FieldOfNonReadOnlyTypeInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.FieldOfNonReadOnlyTypeInReadOnlyType";

        public FieldOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' of field '{0}' in a [ReadOnly] type should be readonly.",
            fieldName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(MutableAutoPropertyInReadOnlyType.Id, CSharpLanguage.Name)]
    public class MutableAutoPropertyInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableAutoPropertyInReadOnlyType";

        public MutableAutoPropertyInReadOnlyType([NotNull] ITreeNode element, string propertyName) : base(
            element,
            "Setter of auto property '{0}' in a [ReadOnly] class should be private.",
            propertyName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyOfNonReadOnlyTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public class AutoPropertyOfNonReadOnlyTypeInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyOfNonReadOnlyTypeInReadOnlyType";

        public AutoPropertyOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string propertyName, string typeName) : base(
            element,
            "Type '{1}' of auto property '{0}' in a [ReadOnly] type should be readonly.",
            propertyName, typeName
        ) {}
    }

    [ConfigurableSeverityHighlighting(AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType.Id, CSharpLanguage.Name)]
    public class AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType";

        public AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType([NotNull] ITreeNode element, string propertyName, string @static) : base(
            element,
            "{1}auto property '{0}' in a [ReadOnly] class should only be assigned in a {1}constructor.",
            propertyName, @static
        ) {}
    }
}

