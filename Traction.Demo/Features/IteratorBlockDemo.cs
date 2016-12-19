using System.Collections.Generic;

namespace Traction.Demo {

    public class IteratorBlockDemo {

        public IEnumerable<int> NormalNonIteratorMethod(int n) {
            return new[] { 1 };
        }

        public IEnumerable<int> NormalIteratorMethod(int n) {
            yield return 1;
        }

        public IEnumerable<int> PreconditionNonIteratorMethod([Positive] int n) {
            return new[] { 1 };
        }

        public IEnumerable<int> PreconditionIteratorMethod([Positive] int n) {
            yield return 1;
        }
    }
}
