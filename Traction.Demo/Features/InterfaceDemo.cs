namespace Traction.Demo {

    public interface InterfaceDemo {

        void PreconditionMethod([NonNull] string text);

        [return: NonNull]
        string Postconditionmethod();

        [NonNull]
        string Name { get; set; }
    }
}
