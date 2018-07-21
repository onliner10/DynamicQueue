using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicQueue.Tests")]
namespace DynamicQueue
{
    internal class RbtNode<TKey, TValue> 
    {
        public static RbtNode<TKey,TValue> Nil = new RbtNode<TKey, TValue>() { IsRed = false };

        public RbtNode() { } 
        public RbtNode(TKey key, TValue value)
            :this()
        {
            Key = key;
            Value = value;
            Left = Nil;
            Right = Nil;
            Parent = Nil;
            IsRed = true;
        }

        
        public RbtNode(TKey key, TValue value, RbtNode<TKey, TValue> parent)
            :this(key, value)
        {
            Parent = parent;
        }

        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public RbtNode<TKey,TValue> Left { get; set; }
        public RbtNode<TKey,TValue> Right { get; set; }
        public RbtNode<TKey,TValue> Parent { get; set; }

        public void MarkBlack() => IsRed = false;
        public void MarkRed() => IsRed = true;

        public bool IsBlack => !IsRed;
        public bool IsRed { get; set; }

    }
}
