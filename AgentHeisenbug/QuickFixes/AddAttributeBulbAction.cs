using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AgentHeisenbug.QuickFixes {
    public class AddAttributeBulbAction : BulbActionBase {
        [NotNull] private readonly IAttributesOwnerDeclaration _target;
        [NotNull] public ITypeElement AttributeType { get; private set; }

        public AddAttributeBulbAction([NotNull] IAttributesOwnerDeclaration target, [NotNull] ITypeElement attributeType) {
            _target = target;
            AttributeType = attributeType;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            // ReSharper disable once PossibleNullReferenceException (R# annotation bug)
            var attributeInstance = CSharpElementFactory.GetInstance(_target).CreateAttribute(AttributeType, EmptyArray<AttributeValue>.Instance, EmptyArray<Pair<string, AttributeValue>>.Instance);
            using (_target.CreateWriteLock()) {
                _target.AddAttributeAfter(attributeInstance, _target.Attributes.LastOrDefault());
            }
            return null;
        }

        public override string Text {
            get { return string.Format("Add [{0}] attribute to '{1}'", AttributeType.ShortName.SubstringBefore("Attribute"), _target.DeclaredName); }
        }
    }
}
