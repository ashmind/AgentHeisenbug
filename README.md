## Purpose
Agent Heisenbug is a ReSharper plugin that should make thread-safe programming sightly easier.  
At the moment, it provides verification for two new annotations, `[ThreadSafe]` and `[ReadOnly]`.

One of my primary goals was to express MSDN "Thread Safety" sections as verifiable annotations.

## Usage

The following attribute definitions are required to use Agent Heisenbug:

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
public class ThreadSafeAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
public class ReadOnlyAttribute : Attribute {
}
```

`[ThreadSafe]` means that the specified class is safe for concurrent access. At the moment this is defined as having no mutable state and all statically known property, field and parameter types being thread-safe. In the future the implementation may be smarter.

`[ReadOnly]` means that the specified class is readonly. In implementation it is very similar to `ThreadSafe`, but gives a weaker guarantee.

## Plans

This is very alpha at the moment -- for example there are no QuickFixes and most ways of fixing warnings are just bey suppressing them. Here are some plans in no particular order:

1. Add QuickFixes
2. Allow `[ThreadSafe]` annotations on method level.
3. Allow `[ThreadSafe(false)]`.
4. Allow `[ThreadSafe]` annotations on delegate parameters and verify closure captures for safety.
5. Add `[ThreadSafe]` and `[ReadOnly]` annotations to ReSharper's 'Copy annotations to clipboard'.
6. Far future: More detailed side-effect management than just `[Pure]`.
7. Far future: detect locking, detect some obvious race conditions.