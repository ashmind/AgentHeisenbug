using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug {
    public static class AstExtensions {
        [CanBeNull]
        public static IAccessorDeclaration GetSetter([NotNull] this IPropertyDeclaration property) {
            Argument.NotNull("property", property);

            // ReSharper disable once PossibleNullReferenceException
            return property.AccessorDeclarations.SingleOrDefault(a => a.Kind == AccessorKind.SETTER);
        }

        public static bool IsPrivate([NotNull] this IAccessRightsOwner element) {
            Argument.NotNull("element", element);
            return element.GetAccessRights().Has(AccessRights.PRIVATE);
        }

        [CanBeNull]
        public static ITypeElement GetCSharpDeclaredType([NotNull] this IDeclaredTypeUsage usage) {
            Argument.NotNull("usage", usage);

            var userDeclaredUsage = usage as IUserDeclaredTypeUsage;
            if (userDeclaredUsage != null) {
                var typeName = userDeclaredUsage.TypeName;
                if (typeName != null)
                    return typeName.Reference.Resolve().DeclaredElement as ITypeElement;
            }

            var declaredType = CSharpTypeFactory.CreateDeclaredType(usage);
            return declaredType.GetTypeElement();
        }

        [CanBeNull]
        public static string GetCSharpPresentableName([NotNull] this IType type) {
            Argument.NotNull("type", type);
            return type.GetPresentableName(CSharpLanguage.Instance);
        }

        [CanBeNull]
        public static IList<IAttributeInstance> ReliablyGetAttributeInstances([NotNull] this IAttributesOwner owner, bool inherit) {
            Argument.NotNull("owner", owner);
            if (owner.GetType().FullName == "JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2.TypeElement+TypeParameter")
                return GetAttributeInstancesFromTypeParameter((ITypeParameter)owner);

            return owner.GetAttributeInstances(inherit);
        }

        private static IList<IAttributeInstance> GetAttributeInstancesFromTypeParameter([NotNull] ITypeParameter owner) {
            var declarations = owner.GetDeclarations().Cast<IAttributesOwnerDeclaration>();
            // ReSharper disable once PossibleNullReferenceException
            var attributes = declarations.SelectMany(d => d.AttributesEnumerable);
            return attributes.Select(CSharpImplUtil.GetAttributeInstance).AsIList();
        }
    }
}
