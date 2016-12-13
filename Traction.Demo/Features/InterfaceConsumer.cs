namespace Traction.Demo {

    public interface ISomething {
        
        int GetNumber([NonEmpty] string name);
    }

    class Something : ISomething {
        
        public int GetNumber(string name) {
            return 1;
        }
    }
}
