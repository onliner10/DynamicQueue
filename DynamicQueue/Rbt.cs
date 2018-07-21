using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicQueue.Tests")]
namespace DynamicQueue
{
    internal class Rbt<TKey, TValue> : IEnumerable<TValue> where TKey : IComparable<TKey>
    {
        private RbtNode<TKey, TValue> _root = Nil;
        private static RbtNode<TKey, TValue> Nil = RbtNode<TKey, TValue>.Nil;

        public RbtNode<TKey, TValue> Root => _root;

        public void Insert(TKey key, TValue value)
        {
            var node = new RbtNode<TKey, TValue>(key, value);
            var parent = Nil;
            var x = _root;

            while(x != Nil)
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

            FixTree(node);
        }

        private void FixTree(RbtNode<TKey, TValue> node)
        {
            while(node.Parent.IsRed)
            {
                if(node.Parent == node.Parent.Parent.Left)
                {
                    var aunt = node.Parent.Parent.Right;
                    if(aunt.IsRed)
                    {
                        node.Parent.MarkBlack();
                        aunt.MarkBlack();
                        node.Parent.Parent.MarkRed();

                        node = node.Parent.Parent;
                    }
                    else 
                    {
                        if(node == node.Parent.Right)
                        {
                            node = node.Parent;
                            LeftRotate(node);
                        }

                        node.Parent.MarkBlack();
                        node.Parent.Parent.MarkRed();
                        RightRotate(node.Parent.Parent);
                    }
                } else
                {
                    var aunt = node.Parent.Parent.Left;
                    if(aunt.IsRed)
                    {
                        node.Parent.MarkBlack();
                        aunt.MarkBlack();
                        node.Parent.Parent.MarkRed();

                        node = node.Parent.Parent;
                    }
                    else 
                    {
                        if(node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RightRotate(node);
                        }

                        node.Parent.MarkBlack();
                        node.Parent.Parent.MarkRed();
                        LeftRotate(node.Parent.Parent);
                    }
                }
            }

            _root.MarkBlack();
        }

        public RbtNode<TKey, TValue> Find(TKey key)
        {
            var node = _root;

            while(node != Nil)
            {
                var comparision = key.CompareTo(node.Key);
                if (comparision == 0)
                    return node;
                else if (comparision < 0)
                    node = node.Left;
                else
                    node = node.Right;

            }

            return null;
        }


        public void LeftRotate(RbtNode<TKey, TValue> node)
        {
            if (node.Right == Nil) return; 

            var y = node.Right;
            node.Right = y.Left;

            if(y.Left != Nil) 
                y.Left.Parent = node;

            y.Parent = node.Parent;
            if (node.Parent == Nil)
                _root = y;
            else if (node == node.Parent.Left)
                node.Parent.Left = y;
            else
                node.Parent.Right = y;

            y.Left = node;
            node.Parent = y;
        }
        public void RightRotate(RbtNode<TKey, TValue> node)
        {
            if (node.Left == Nil) return;

            var y = node.Left;
            node.Left = y.Right;

            if(y.Right != Nil) 
                y.Right.Parent = node;

            y.Parent = node.Parent;
            if (node.Parent == Nil)
                _root = y;
            else if (node == node.Parent.Left)
                node.Parent.Left = y;
            else
                node.Parent.Right = y;

            y.Right = node;
            node.Parent = y;
        }

        public RbtNode<TKey, TValue> Max()
        {
            var result = _root;
            var x = _root;
            while(x != Nil)
            {
                result = x;
                x = x.Right;
            }
            return result;
        }

        private static IEnumerable<TValue> InOrder(RbtNode<TKey, TValue> n)
        {
            if (n == Nil) yield break;
            
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
