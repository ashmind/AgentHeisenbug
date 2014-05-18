using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using AgentHeisenbug.Highlightings.ReadOnly;
using AgentHeisenbug.Highlightings.ThreadSafe;

// ReSharper disable PartialTypeWithSinglePart


[assembly: RegisterConfigurableSeverity(
    ThreadSafeInterfaceInNonThreadSafeType.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Thread-safe interface implemented by type that is not annotated with [ThreadSafe]",
    "Thread-safe interface implemented by type that is not annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    ThreadSafeBaseClassInNonThreadSafeClass.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Thread-safe base class in class not annotated with [ThreadSafe]",
    "Thread-safe base class in class not annotated with [ThreadSafe]",
    Severity.WARNING,
    false
)]
[assembly: RegisterConfigurableSeverity(
    NonThreadSafeBaseClassInThreadSafeClass.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Non-readonly base class in class annotated with [ReadOnly]",
    "Non-readonly base class in class annotated with [ReadOnly]",
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
    NonReadOnlyBaseClassInReadOnlyClass.Id,
    null,
    HighlightingGroupIds.ConstraintViolation,
    "Non-readonly base class in class annotated with [ReadOnly]",
    "Non-readonly base class in class annotated with [ReadOnly]",
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


namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(ThreadSafeInterfaceInNonThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class ThreadSafeInterfaceInNonThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ThreadSafeInterfaceInNonThreadSafeType";

        private ThreadSafeInterfaceInNonThreadSafeType([NotNull] ITreeNode element, string interfaceName, string typeName) : base(
            element,
            "Base interface '{0}' is thread-safe, but type '{1}' is not annotated with [ThreadSafe].",
            interfaceName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(ThreadSafeBaseClassInNonThreadSafeClass.Id, CSharpLanguage.Name)]
    public partial class ThreadSafeBaseClassInNonThreadSafeClass : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ThreadSafeBaseClassInNonThreadSafeClass";

        private ThreadSafeBaseClassInNonThreadSafeClass([NotNull] ITreeNode element, string baseClassName, string typeName) : base(
            element,
            "Base class '{0}' is thread-safe, but class '{1}' is not annotated with [ThreadSafe].",
            baseClassName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(NonThreadSafeBaseClassInThreadSafeClass.Id, CSharpLanguage.Name)]
    public partial class NonThreadSafeBaseClassInThreadSafeClass : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.NonThreadSafeBaseClassInThreadSafeClass";

        private NonThreadSafeBaseClassInThreadSafeClass([NotNull] ITreeNode element, string baseClassName, string typeName) : base(
            element,
            "Base class '{0}' is not thread-safe, but class '{1}' is annotated with [ThreadSafe].",
            baseClassName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(AccessToNonThreadSafeStaticMemberInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class AccessToNonThreadSafeStaticMemberInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AccessToNonThreadSafeStaticMemberInThreadSafeType";

        private AccessToNonThreadSafeStaticMemberInThreadSafeType([NotNull] ITreeNode element, string memberKind, string memberName) : base(
            element,
            "{0} '{1}' is not declared to be thread-safe.",
            memberKind, memberName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(MutableFieldInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class MutableFieldInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableFieldInThreadSafeType";

        public MutableFieldInThreadSafeType([NotNull] ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ThreadSafe] class should be readonly.",
            fieldName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(FieldOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class FieldOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.FieldOfNonThreadSafeTypeInThreadSafeType";

        private FieldOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' used by field '{0}' in a [ThreadSafe] type should be thread-safe.",
            fieldName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(MutableAutoPropertyInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class MutableAutoPropertyInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableAutoPropertyInThreadSafeType";

        public MutableAutoPropertyInThreadSafeType([NotNull] ITreeNode element, string propertyName) : base(
            element,
            "Setter of auto property '{0}' in a [ThreadSafe] class should be private.",
            propertyName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(AutoPropertyOfNonThreadSafeTypeInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class AutoPropertyOfNonThreadSafeTypeInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyOfNonThreadSafeTypeInThreadSafeType";

        private AutoPropertyOfNonThreadSafeTypeInThreadSafeType([NotNull] ITreeNode element, string propertyName, string typeName) : base(
            element,
            "Type '{1}' used by auto property '{0}' in a [ThreadSafe] type should be thread-safe.",
            propertyName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType.Id, CSharpLanguage.Name)]
    public partial class AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType";

        public AutoPropertyAssignmentOutsideOfConstructorInThreadSafeType([NotNull] ITreeNode element, string propertyName, string @static) : base(
            element,
            "{1}auto property '{0}' in a [ThreadSafe] class should only be assigned in a {1}constructor.",
            propertyName, @static
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ThreadSafe {
    [ConfigurableSeverityHighlighting(ParameterOfNonThreadSafeTypeInThreadSafeMethod.Id, CSharpLanguage.Name)]
    public partial class ParameterOfNonThreadSafeTypeInThreadSafeMethod : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.ParameterOfNonThreadSafeTypeInThreadSafeMethod";

        private ParameterOfNonThreadSafeTypeInThreadSafeMethod([NotNull] ITreeNode element, string parameterName, string typeName) : base(
            element,
            "Type '{1}' used by parameter '{0}' in a thread-safe method should be thread-safe unless method is [Pure].",
            parameterName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(NonReadOnlyBaseClassInReadOnlyClass.Id, CSharpLanguage.Name)]
    public partial class NonReadOnlyBaseClassInReadOnlyClass : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.NonReadOnlyBaseClassInReadOnlyClass";

        private NonReadOnlyBaseClassInReadOnlyClass([NotNull] ITreeNode element, string baseClassName, string typeName) : base(
            element,
            "Base class '{0}' is not readonly, but class '{1}' is annotated with [ReadOnly]",
            baseClassName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(MutableFieldInReadOnlyType.Id, CSharpLanguage.Name)]
    public partial class MutableFieldInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableFieldInReadOnlyType";

        public MutableFieldInReadOnlyType([NotNull] ITreeNode element, string fieldName) : base(
            element,
            "Field '{0}' in a [ReadOnly] type should be readonly.",
            fieldName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(FieldOfNonReadOnlyTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public partial class FieldOfNonReadOnlyTypeInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.FieldOfNonReadOnlyTypeInReadOnlyType";

        private FieldOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string fieldName, string typeName) : base(
            element,
            "Type '{1}' used by field '{0}' in a [ReadOnly] type should be readonly.",
            fieldName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(MutableAutoPropertyInReadOnlyType.Id, CSharpLanguage.Name)]
    public partial class MutableAutoPropertyInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.MutableAutoPropertyInReadOnlyType";

        public MutableAutoPropertyInReadOnlyType([NotNull] ITreeNode element, string propertyName) : base(
            element,
            "Setter of auto property '{0}' in a [ReadOnly] class should be private.",
            propertyName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(AutoPropertyOfNonReadOnlyTypeInReadOnlyType.Id, CSharpLanguage.Name)]
    public partial class AutoPropertyOfNonReadOnlyTypeInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyOfNonReadOnlyTypeInReadOnlyType";

        private AutoPropertyOfNonReadOnlyTypeInReadOnlyType([NotNull] ITreeNode element, string propertyName, string typeName) : base(
            element,
            "Type '{1}' used by auto property '{0}' in a [ReadOnly] type should be readonly.",
            propertyName, typeName
        ) {}
    }
}

namespace AgentHeisenbug.Highlightings.ReadOnly {
    [ConfigurableSeverityHighlighting(AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType.Id, CSharpLanguage.Name)]
    public partial class AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType : HeisenbugHighligtingBase {
        public const string Id = "AgentHeisenbug.AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType";

        public AutoPropertyAssignmentOutsideOfConstructorInReadOnlyType([NotNull] ITreeNode element, string propertyName, string @static) : base(
            element,
            "{1}auto property '{0}' in a [ReadOnly] class should only be assigned in a {1}constructor.",
            propertyName, @static
        ) {}
    }
}

