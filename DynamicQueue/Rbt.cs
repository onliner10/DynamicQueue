using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicQueue.Tests")]
namespace DynamicQueue
{
    internal class Rbt<TKey, TValue> : IEnumerable<TValue> where TKey : IComparable<TKey>
    {
        private RbtNode<TKey, TValue> _root = null;
        private static RbtNode<TKey, TValue> Nil = RbtNode<TKey, TValue>.Nil;

        public RbtNode<TKey, TValue> Root => _root;

        public void Insert(TKey key, TValue value)
        {
            var node = new RbtNode<TKey, TValue>(key, value);
            var parent = Nil;
            var x = _root;

            while(x != null && x != Nil)
            {
                parent = x;

                var keyComparision = node.Key.CompareTo(x.Key);
                if (keyComparision < 0)
                    x = x.Left;
                else
                    x = x.Right;
            }

            node.Parent = parent;

            if (parent == Nil)
                _root = node;
            else if (node.Key.CompareTo(parent.Key) < 0)
                parent.Left = node;
            else
                parent.Right = node;
        }

        private void LeftRotate(RbtNode<TKey, TValue> x)
        {
            var y = x.Right;
            x.Right = y.Left;

            if(y.Left != Nil) 
                y.Left.Parent = x;

            y.Parent = x.Parent;
            if (x.Parent == Nil)
                _root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }
        private void RightRotate(RbtNode<TKey, TValue> x)
        {
            var y = x.Left;
            x.Left = y.Right;

            if(y.Right != Nil) 
                y.Right.Parent = x;

            y.Parent = x.Parent;
            if (x.Parent == Nil)
                _root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        public RbtNode<TKey, TValue> Max()
        {
            var result = _root;
            var x = _root;
            while(x != null && x != Nil)
            {
                result = x;
                x = x.Right;
            }
            return result;
        }

        private static IEnumerable<TValue> InOrder(RbtNode<TKey, TValue> n)
        {
            if (n == null || n == Nil) yield break;
            
            if(n.Left != Nil)
            {
                foreach (var x in InOrder(n.Left))
                    yield return x;
            }

            yield return n.Value;

            if(n.Right != Nil)
            {
                foreach (var x in InOrder(n.Right))
                    yield return x;
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return InOrder(_root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
