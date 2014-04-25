using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace AgentHeisenbug.Analyzers.Helpers {
    public static class AstExtensions {
        [CanBeNull]
        public static IAccessorDeclaration GetSetter([NotNull] this IPropertyDeclaration property) {
            Argument.NotNull("property", property); Argument.NotNull("property", property);

            // ReSharper disable once PossibleNullReferenceException
            return property.AccessorDeclarations.SingleOrDefault(a => a.Kind == AccessorKind.SETTER);
        }

        public static bool IsPrivate([NotNull] this IAccessRightsOwner element) {
            Argument.NotNull("element", element);
            return element.GetAccessRights().Has(AccessRights.PRIVATE);
        }
    }
}
