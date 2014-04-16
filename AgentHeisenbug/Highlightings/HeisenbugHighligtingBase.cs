using System;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Highlightings {
    public class HeisenbugHighligtingBase : IHighlightingWithRange {
        private readonly ITreeNode element;
        private readonly string message;

        [StringFormatMethod("messageFormat")]
        protected HeisenbugHighligtingBase(ITreeNode element, string messageFormat, params object[] args) {
            this.element = element;
            this.message = string.Format(messageFormat, args);
            if (!this.message[0].IsUpper()) {
                // simplifies format strings that start with optional substitutions
                this.message = this.message[0].ToUpperInvariant() + this.message.Substring(1);
            }
        }

        public int NavigationOffsetPatch {
            get { return 0; }
        }

        public string ToolTip {
            get { return this.message; }
        }

        public string ErrorStripeToolTip {
            get { return this.message; }
        }

        public bool IsValid() {
            return this.element != null && this.element.IsValid();
        }

        public DocumentRange CalculateRange() {
            return this.element.GetHighlightingRange();
        }
    }
}