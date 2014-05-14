using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AshMind.Extensions;
using JetBrains.Annotations;
using JetBrains.Util;

namespace AgentHeisenbug.Indexer.ReadOnly {
    public class ExpressionAnnotationProvider : IAnnotationProvider {
        public const string ReadOnlyAttributeCtorXmlId = "M:JetBrains.Annotations.ReadOnlyAttribute.#ctor";

        public IEnumerable<AnnotationsByAssembly> GetAnnotationsByAssembly(Func<string, bool> assemblyNameFilter, Action<double> reportProgress) {
            var assembly = typeof(Expression).Assembly;
            var types = assembly.GetTypes().Where(t => t.IsSameAsOrSubclassOf<Expression>()).ToArray();
            var annotations = types.Select(GetAnnotation).OnAfterEach((_, index) => reportProgress((double)index / types.Length));
            var result = new AnnotationsByAssembly(assembly.GetName().Name.NotNull(), annotations.ToArray());

            return new[] { result };
        }

        private AnnotationsByMember GetAnnotation(Type t) {
            return new AnnotationsByMember(GenerateXmlId(t), new Annotation(ReadOnlyAttributeCtorXmlId));
        }

        [NotNull]
        private string GenerateXmlId([NotNull] Type type) {
            return "T:" + type.FullName;
        }
    }
}
