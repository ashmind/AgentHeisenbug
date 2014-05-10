using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Util;

namespace AgentHeisenbug.Processing {
    public class HeisenbugFeatures {
        private readonly bool _isReadOnly;
        private readonly bool _isPure;
        private readonly ThreadSafety _threadSafety;
        [CanBeNull] private readonly HeisenbugFeatures _containingTypeFeatures;

        public HeisenbugFeatures(bool isReadOnly, bool isPure, ThreadSafety threadSafety) : this(isReadOnly, isPure, threadSafety, null) {
        }

        public HeisenbugFeatures([NotNull] HeisenbugFeatures currentFeatures, [NotNull] HeisenbugFeatures containingTypeFeatures) 
            : this(currentFeatures.IsReadOnly, currentFeatures._isPure, currentFeatures.DeclaredThreadSafety, containingTypeFeatures) 
        {
        }

        private HeisenbugFeatures(bool isReadOnly, bool isPure, ThreadSafety threadSafety, [CanBeNull] HeisenbugFeatures containingTypeFeatures) {
            _isReadOnly = isReadOnly;
            _isPure = isPure;
            _threadSafety = threadSafety;
            _containingTypeFeatures = containingTypeFeatures;
        }

        public bool IsReadOnly {
            get { return _isReadOnly; }
        }

        public bool IsPure {
            get { return _isPure; }
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
                    || _isPure
                    || (_containingTypeFeatures != null && _containingTypeFeatures.Has(ThreadSafety.Static));
            }
        }

        [Pure]
        private bool Has(ThreadSafety threadSafety) {
            return _threadSafety.Has(threadSafety);
        }

        [Pure] [NotNull]
        public HeisenbugFeatures WithReadOnly(bool readOnly) {
            return new HeisenbugFeatures(readOnly, _isPure, _threadSafety, _containingTypeFeatures);
        }
    }
}
