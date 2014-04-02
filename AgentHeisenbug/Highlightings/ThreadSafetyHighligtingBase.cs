using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentHeisenbug.Highlightings {
    public class ThreadSafetyHighligtingBase : IHighlightingWithRange {
        private readonly ITreeNode element;
        private readonly string message;

        [StringFormatMethod("messageFormat")]
        public ThreadSafetyHighligtingBase(ITreeNode element, string messageFormat, params object[] args) {
            this.element = element;
            this.message = string.Format(messageFormat, args);
        }

        public int NavigationOffsetPatch {
            get { return 0; }
        }

        public string ToolTip {
            get { return this.message; }
        }

        public string ErrorStripeToolTip {
            get { throw new NotSupportedException(); }
        }

        public bool IsValid() {
            return this.element != null && this.element.IsValid();
        }

        public DocumentRange CalculateRange() {
            return this.element.GetHighlightingRange();
        }
    }
}