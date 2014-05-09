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
    AccessToNonThreadSafeStaticMemberInThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Access to static member that is not thread-safe from type annotated with [ThreadSafe]",
    "Access to static member that is not thread-safe from type annotated with [ThreadSafe]",
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
    "Field using type that is not thread-safe in type annotated with [ThreadSafe]",
    "Field using type that is not thread-safe in type annotated with [ThreadSafe]",
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
    "Auto property using type that is not thread-safe in type annotated with [ThreadSafe]",
    "Auto property using type that is not thread-safe in type annotated with [ThreadSafe]",
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
    "Parameter using type that is not thread-safe in a thread-safe method",
    "Parameter using type that is not thread-safe in a thread-safe method",
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
    "Field using mutable type in type annotated with [ReadOnly]",
    "Field using mutable type in type annotated with [ReadOnly]",
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
    "Auto property using type that is not readonly in type annotated with [ReadOnly]",
    "Auto property using type that is not readonly in type annotated with [ReadOnly]",
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
    public static class HeisenbugHighlightings {
        public static ThreadSafeInterfaceImplementedByNonThreadSafeType ThreadSafeInterfaceImplementedByNonThreadSafeType([NotNull] ITreeNode element, string interfaceName, string typeName) {
            return new ThreadSafeInterfaceImplementedByNonThreadSafeType(element, interfaceName, typeName);
        }
        public static ThreadSafeClassInheritedByNonThreadSafeType ThreadSafeClassInheritedByNonThreadSafeType([NotNull] ITreeNode element, string baseClassName, string typeName) {
            return new ThreadSafeClassInheritedByNonThreadSafeType(element, baseClassName, typeName);
        }
        public static AccessToNonThreadSafeStaticMemberInThreadSafeType AccessToNonThreadSafeStaticMemberInThreadSafeType([NotNull] ITreeNode element, string memberKind, string memberName) {
            return new AccessToNonThreadSafeStaticMemberInThreadSafeType(element, memberKind, memberName);
        }
        public static MutableFieldInThreadSafeType MutableFieldInThreadSafeType([NotNull] ITreeNode element, string fieldName) {
            return new MutableFieldInThreadSafeType(element, fieldName);
        }
        public static FieldOfNonThreadSafeTypeInThreadSafeType FieldOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string fieldName, string typeName) {
            return new FieldOfNonThreadSafeTypeInThreadSafeType(element, fieldName, typeName);
        }
        public static MutableAutoPropertyInThreadSafeType MutableAutoPropertyInThreadSafeType([NotNull] ITreeNode element, string propertyName) {
            return new MutableAutoPropertyInThreadSafeType(element, propertyName);
        }
        public static AutoPropertyOfNonThreadSafeTypeInThreadSafeType AutoPropertyOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string propertyName, string typeName) {
            return new AutoPropertyOfNonThreadSafeTypeInThreadSafeType(element, propertyName, typeName);
        }
        public static AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType([NotNull] ITreeNode element, string propertyName, string @static) {
            return new AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType(element, propertyName, @static);
        }
        public static ParameterOfNonThreadSafeTypeInThreadSafeMethod ParameterOfNonThreadSafeTypeInThreadSafeMethod([NotNull] ITreeNode element, string paramterName, string typeName) {
            return new ParameterOfNonThreadSafeTypeInThreadSafeMethod(element, paramterName, typeName);
        }
        public static ReadOnlyInterfaceImplementedByNonReadOnlyType ReadOnlyInterfaceImplementedByNonReadOnlyType([NotNull] ITreeNode element, string interfaceName, string typeName) {
            return new ReadOnlyInterfaceImplementedByNonReadOnlyType(element, interfaceName, typeName);
        }
        public static ReadOnlyClassInheritedByNonReadOnlyType ReadOnlyClassInheritedByNonReadOnlyType([NotNull] ITreeNode element, string baseClassName, string typeName) {
            return new ReadOnlyClassInheritedByNonReadOnlyType(element, baseClassName, typeName);
        }
        public static MutableFieldInReadOnlyType MutableFieldInReadOnlyType([NotNull] ITreeNode element, string fieldName) {
            return new MutableFieldInReadOnlyType(element, fieldName);
        }
        public static FieldOfNonReadOnlyTypeInReadOnlyType FieldOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string fieldName, string typeName) {
            return new FieldOfNonReadOnlyTypeInReadOnlyType(element, fieldName, typeName);
        }
        public static MutableAutoPropertyInReadOnlyType MutableAutoPropertyInReadOnlyType([NotNull] ITreeNode element, string propertyName) {
            return new MutableAutoPropertyInReadOnlyType(element, propertyName);
        }
        public static AutoPropertyOfNonReadOnlyTypeInReadOnlyType AutoPropertyOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string propertyName, string typeName) {
            return new AutoPropertyOfNonReadOnlyTypeInReadOnlyType(element, propertyName, typeName);
        }
        public static AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType([NotNull] ITreeNode element, string propertyName, string @static) {
            return new AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType(element, propertyName, @static);
        }
    }

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

    [ConfigurableSeverityHighlighting(AccessToNonThreadSafeStaticMemberInThreadSafeType.Id, CSharpLanguage.Name)]
    public class AccessToNonThreadSafeStaticMemberInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AccessToNonThreadSafeStaticMemberInThreadSafeType";

        public AccessToNonThreadSafeStaticMemberInThreadSafeType([NotNull] ITreeNode element, string memberKind, string memberName) : base(
            element,
            "{0} '{1}' is not declared to be thread-safe.",
            memberKind, memberName
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
            "Type '{1}' used by field '{0}' in a [ThreadSafe] type should be thread-safe.",
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
            "Type '{1}' used by auto property '{0}' in a [ThreadSafe] type should be thread-safe.",
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
            "Type '{1}' used by parameter '{0}' in a thread-safe method should be thread-safe unless method is [Pure].",
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
            "Type '{1}' used by field '{0}' in a [ReadOnly] type should be readonly.",
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
            "Type '{1}' used by auto property '{0}' in a [ReadOnly] type should be readonly.",
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

