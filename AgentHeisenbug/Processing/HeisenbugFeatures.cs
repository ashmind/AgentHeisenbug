using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Util;

namespace AgentHeisenbug.Processing {
    public class HeisenbugFeatures {
        private readonly bool _isReadOnly;
        private readonly ThreadSafety _threadSafety;
        [CanBeNull] private readonly HeisenbugFeatures _containingTypeFeatures;

        public HeisenbugFeatures(bool isReadOnly, ThreadSafety threadSafety) : this(isReadOnly, threadSafety, null) {
        }

        public HeisenbugFeatures([NotNull] HeisenbugFeatures currentFeatures, [NotNull] HeisenbugFeatures containingTypeFeatures) 
            : this(currentFeatures.IsReadOnly, currentFeatures._threadSafety, containingTypeFeatures) 
        {
        }

        private HeisenbugFeatures(bool isReadOnly, ThreadSafety threadSafety, [CanBeNull] HeisenbugFeatures containingTypeFeatures) {
            _isReadOnly = isReadOnly;
            _threadSafety = threadSafety;
            _containingTypeFeatures = containingTypeFeatures;
        }

        public bool IsReadOnly {
            get { return _isReadOnly; }
        }

        public ThreadSafety DeclaredThreadSafety {
            get { return _threadSafety; }
        }
        
        public bool IsInstanceAccessThreadSafeOrReadOnly {
            get {
                return IsReadOnly
                    || Has(ThreadSafety.Instance) 
                    || (_containingTypeFeatures != null && _containingTypeFeatures.Has(ThreadSafety.Instance));
            }
        }

        public bool IsStaticAccessThreadSafe {
            get {
                return Has(ThreadSafety.Static)
                    || (_containingTypeFeatures != null && _containingTypeFeatures.Has(ThreadSafety.Static));
            }
        }

        [Pure]
        private bool Has(ThreadSafety threadSafety) {
            return _threadSafety.Has(threadSafety);
        }

        [Pure] [NotNull]
        public HeisenbugFeatures WithReadOnly(bool readOnly) {
            return new HeisenbugFeatures(readOnly, _threadSafety, _containingTypeFeatures);
        }
    }
}
