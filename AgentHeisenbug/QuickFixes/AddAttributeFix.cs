using System;
using System.Collections.Generic;
using AgentHeisenbug.Highlightings.Common;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.Util;

namespace AgentHeisenbug.QuickFixes {
    [QuickFix]
    public class AddAttributeFix : IQuickFix {
        [NotNull] private readonly IFixableByAnnotation _highlighting;

        public AddAttributeFix([NotNull] IFixableByAnnotation highlighting) {
            _highlighting = highlighting;
        }

        public bool IsAvailable(IUserDataHolder cache) {
            return _highlighting.IsValid();
        }

        public IEnumerable<IntentionAction> CreateBulbItems() {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var candidate in _highlighting.GetCandidates()) {
                var attributeType = GetAttributeType(candidate);
                if (attributeType == null)
                    continue;

                yield return new AddAttributeBulbAction(candidate.TargetDeclaration, attributeType).ToAnnotateAction();
            }
        }

        [CanBeNull]
        private ITypeElement GetAttributeType(AnnotationCandidate candidate) {
            var target = candidate.TargetDeclaration;
            var targetElement = (IClrDeclaredElement)target.DeclaredElement;
            if (targetElement == null)
                return null;
            
            var scope = target.GetPsiServices().Symbols.GetSymbolScope(target.GetPsiModule(), targetElement.ResolveContext, true, true);
            // TODO: other namespaces from settings
            return scope.GetTypeElementByCLRName("JetBrains.Annotations." + candidate.AnnotationTypeName);
        }
    }
}
