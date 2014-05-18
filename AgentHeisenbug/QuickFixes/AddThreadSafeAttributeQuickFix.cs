using System;
using System.Collections.Generic;
using AgentHeisenbug.Highlightings.ThreadSafe;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AgentHeisenbug.QuickFixes {
    [QuickFix]
    public class AddThreadSafeAttributeFix : QuickFixBase {
        [CanBeNull] private readonly IAttributesOwnerDeclaration _target;

        public AddThreadSafeAttributeFix([NotNull] IFixableByThreadSafeAttribute highlighting) {
            _target = highlighting.GetTargetDeclaration();
        }

        public override bool IsAvailable(IUserDataHolder cache) {
            return _target != null
                && _target.IsValid()
                && GetAttributeType() != null;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            Assertion.Assert(_target != null, "Target declaration should not be null at this point.");

            var attributeType = GetAttributeType();
            if (attributeType == null)
                return null;
            
            // ReSharper disable once PossibleNullReferenceException (R# annotation bug)
            var attributeInstance = CSharpElementFactory.GetInstance(_target).CreateAttribute(attributeType, EmptyArray<AttributeValue>.Instance, EmptyArray<Pair<string, AttributeValue>>.Instance);
            using (_target.CreateWriteLock()) {
                _target.AddAttributeAfter(attributeInstance, _target.Attributes.LastOrDefault());
            }
            return null;
        }

        [CanBeNull]
        private ITypeElement GetAttributeType() {
            Assertion.Assert(_target != null, "Target declaration should not be null at this point.");

            var targetElement = (IClrDeclaredElement)_target.DeclaredElement;
            if (targetElement == null)
                return null;
            
            var module = _target.GetPsiModule();
            var scope = _target.GetPsiServices().Symbols.GetSymbolScope(module, targetElement.ResolveContext, true, true);
            var attributeType = scope.GetTypeElementByCLRName("JetBrains.Annotations.ThreadSafeAttribute");
            return attributeType;
        }

        public override string Text {
            get { return "Add [ThreadSafe] attribute to " + _target.NotNull().DeclaredName; }
        }
    }
}
