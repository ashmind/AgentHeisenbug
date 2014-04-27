using System;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Highlightings {
    public class HeisenbugHighligtingBase : IHighlightingWithRange {
        [NotNull] private readonly ITreeNode _element;
        [NotNull] private readonly string _message;

        [StringFormatMethod("messageFormat")]
        protected HeisenbugHighligtingBase([NotNull] ITreeNode element, [NotNull] string messageFormat, [NotNull] params object[] args) {
            this._element = element;
            this._message = string.Format(messageFormat, args);
            if (!this._message[0].IsUpper()) {
                // simplifies format strings that start with optional substitutions
                this._message = this._message[0].ToUpperInvariant() + this._message.Substring(1);
            }
        }

        public int NavigationOffsetPatch {
            get { return 0; }
        }

        public string ToolTip {
            get { return _message; }
        }

        public string ErrorStripeToolTip {
            get { return _message; }
        }

        public bool IsValid() {
            return _element.IsValid();
        }

        public DocumentRange CalculateRange() {
            return _element.GetHighlightingRange();
        }
    }
}