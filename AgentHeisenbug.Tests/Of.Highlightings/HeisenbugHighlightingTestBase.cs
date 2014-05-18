using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace AgentHeisenbug.Tests.Of.Highlightings {
    public abstract class HeisenbugHighlightingTestBase : CSharpHighlightingTestNet45Base {
        protected override string RelativeTestDataPath {
            get { return Path.Combine(@"Highlightings", GetType().Name.SubstringBefore("Tests")); }
        }

        protected override bool HighlightingPredicate([NotNull] IHighlighting highlighting, IContextBoundSettingsStore settingsStore) {
            if (!base.HighlightingPredicate(highlighting, settingsStore))
                return false;

            var highlightingTypeName = highlighting.GetType().FullName;
            var attributes = GetAttributes<HighlightingFilterAttribute>().AsList();
            var include = attributes.Select(a => a.Include).FirstOrDefault(i => i != null);
            var exclude = attributes.Select(a => a.Exclude).FirstOrDefault(e => e != null);

            if (include != null && !include.IsMatch(highlightingTypeName))
                return false;

            if (exclude != null && exclude.IsMatch(highlightingTypeName))
                return false;

            return true;
        }
    }
}
