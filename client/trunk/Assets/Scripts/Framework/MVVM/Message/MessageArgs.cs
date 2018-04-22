namespace Framework.MVVM
{
    public class MessageArgs<T>
    {
        public T Item
        {
            get;
            private set;
        }

        public MessageArgs(T item)
        {
            Item = item;
        }
    }
}
