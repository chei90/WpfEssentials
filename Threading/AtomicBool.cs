namespace WpfEssentials.Threading
{
    public class AtomicBool
    {
        private const int TRUE = 1;
        private const int FALSE = 0;

        private int shallClose;

        public AtomicBool(bool initialState)
            => shallClose = initialState ? TRUE : FALSE;

        public bool Value => shallClose == TRUE ? true : false;

        public void Change(bool newValue) => System.Threading.Interlocked.Exchange(ref shallClose, newValue ? TRUE : FALSE);
    }
}
