using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Traction.Roslyn.Rewriting {

    public partial class CompositeRewriter : CSharpSyntaxRewriter, IList<CSharpSyntaxRewriter> {

        public CompositeRewriter() {
            items = new List<CSharpSyntaxRewriter>();
        }

        private readonly List<CSharpSyntaxRewriter> items;
        
        public CSharpSyntaxRewriter this[int index] {
            get { return items[index]; }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                items[index] = value;
            }
        }

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public void Add(CSharpSyntaxRewriter item) {
            if (item == null) throw new ArgumentNullException(nameof(item));
            items.Add(item);
        }

        public void Clear() => items.Clear();
        public bool Contains(CSharpSyntaxRewriter item) => items.Contains(item);
        public void CopyTo(CSharpSyntaxRewriter[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
        public IEnumerator<CSharpSyntaxRewriter> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
        public int IndexOf(CSharpSyntaxRewriter item) => items.IndexOf(item);
        public void Insert(int index, CSharpSyntaxRewriter item) => items.Insert(index, item);
        public bool Remove(CSharpSyntaxRewriter item) => items.Remove(item);
        public void RemoveAt(int index) => items.RemoveAt(index);
    }
}
