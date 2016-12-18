using System;

namespace Traction.Demo {

    public abstract class AbstractClassDemo {

        public abstract void PreconditionMethod([NonNull] string text);

        [return: NonNull]
        public abstract string PostconditionMethod(string text);

        [NonNull]
        public abstract string Property { get; set; }
    }

    public class DerivedClassDemo : AbstractClassDemo {

        public override void PreconditionMethod(string text) {

        }
        
        public override string PostconditionMethod(string text) {
            return text;
        }

        public string _property;
        public override string Property {
            get { return _property;  }
            set { _property = value; }
        }
    }
}
