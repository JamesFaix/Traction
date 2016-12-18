namespace Traction.Demo {

    public interface IDemo {

        void PreconditionMethod([NonNull] string text);

        [return: NonNull]
        string PostconditionMethod(string text);

        [NonNull]
        string Property { get; set; }
    }

    public class ImplicitInterfaceDemo : IDemo {
        //Virtual so another demo can override it for multi-layer inheritance tests
        public virtual void PreconditionMethod(string text) {

        }

        public string PostconditionMethod(string text) {
            return text;
        }

        public string _property;
        public string Property {
            get { return _property; }
            set { _property = value; }
        }
    }

    public class ExplicitInterfaceDemo : IDemo {
        void IDemo.PreconditionMethod(string text) {

        }

        string IDemo.PostconditionMethod(string text) {
            return text;
        }

        public string _property;
        string IDemo.Property {
            get { return _property; }
            set { _property = value; }
        }
    }
}
