using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicQueue.Tests")]
namespace DynamicQueue
{
    internal class Rbt<TKey, TValue> : IEnumerable<TValue> 
    {
        private RbtNode<TKey, TValue> _root = Nil;
        private static RbtNode<TKey, TValue> Nil = RbtNode<TKey, TValue>.Nil;
        private readonly IComparer<TKey> _keyComparer;

        public RbtNode<TKey, TValue> Root => _root;

        public Rbt(IComparer<TKey> keyComparer)
        {
            this._keyComparer = keyComparer;
        }

        public RbtNode<TKey, TValue> Insert(TKey key, TValue value)
        {
            var node = new RbtNode<TKey, TValue>(key, value);
            var parent = Nil;
            var x = _root;

            while(x != Nil)
            {
                parent = x;

                var keyComparision = _keyComparer.Compare(node.Key, x.Key);
                if (keyComparision < 0)
                    x = x.Left;
                else
                    x = x.Right;
            }

            node.Parent = parent;

            if (parent == Nil)
                _root = node;
            else if (_keyComparer.Compare(node.Key, parent.Key) < 0)
                parent.Left = node;
            else
                parent.Right = node;

            InsertFixup(node);
            return node;
        }

        public void Delete(RbtNode<TKey, TValue> node)
        {
            var y = node;
            var yOrigColor = node.IsRed;
            RbtNode<TKey, TValue> x;

            if(node.Left == Nil)
            {
                x = node.Right;
                Transplant(node, node.Right);
            }
            else if(node.Right == Nil)
            {
                x = node.Left;
                Transplant(node, node.Left);
            }
            else
            {
                y = MinOf(node.Right);
                yOrigColor = y.IsRed;
                x = y.Right;

                if (y.Parent == node)
                    x.Parent = y;
                else
                {
                    Transplant(y, y.Right);
                    y.Right = node.Right;
                    y.Right.Parent = y;
                }

                Transplant(node, y);
                y.Left = node.Left;
                y.Left.Parent = y;
                y.IsRed = node.IsRed;
            }

            if (!yOrigColor) // if is black
                DeleteFixup(x);
        }

        private void InsertFixup(RbtNode<TKey, TValue> node)
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

        private void DeleteFixup(RbtNode<TKey, TValue> node)
        {
            while(node != _root && node.IsBlack)
            {
                if (node == node.Parent.Left)
                {
                    var brother = node.Parent.Right;
                    if (brother.IsRed)
                    {
                        brother.MarkBlack();
                        node.Parent.MarkRed();
                        LeftRotate(node.Parent);
                        brother = node.Parent.Right;
                    }
                    if (brother.Left.IsBlack && brother.Right.IsBlack)
                    {
                        brother.MarkRed();
                        node = node.Parent;
                    }
                    else
                    {
                        if (brother.Right.IsBlack)
                        {
                            brother.Left.MarkBlack();
                            brother.MarkRed();
                            RightRotate(brother);
                            brother = node.Parent.Right;
                        }
                        brother.IsRed = node.Parent.IsRed;
                        node.Parent.MarkBlack();
                        brother.Right.MarkBlack();
                        LeftRotate(node.Parent);
                        node = _root;
                    }
                }
                else
                {
                    var brother = node.Parent.Left;
                    if (brother.IsRed)
                    {
                        brother.MarkBlack();
                        node.Parent.MarkRed();
                        RightRotate(node.Parent);
                        brother = node.Parent.Left;
                    }
                    if (brother.Right.IsBlack && brother.Left.IsBlack)
                    {
                        brother.MarkRed();
                        node = node.Parent;
                    }
                    else
                    {
                        if (brother.Left.IsBlack)
                        {
                            brother.Right.MarkBlack();
                            brother.MarkRed();
                            LeftRotate(brother);
                            brother = node.Parent.Left;
                        }
                        brother.IsRed = node.Parent.IsRed;
                        node.Parent.MarkBlack();
                        brother.Left.MarkBlack();
                        RightRotate(node.Parent);
                        node = _root;
                    }
                }
            }
        }

        private void Transplant(RbtNode<TKey, TValue> source, RbtNode<TKey, TValue> dest)
        {
            if (source.Parent == Nil)
                _root = dest;
            else if (source == source.Parent.Left)
                source.Parent.Left = dest;
            else
                source.Parent.Right = dest;

            dest.Parent = source.Parent;
        }

        public RbtNode<TKey, TValue> Find(TKey key)
        {
            var node = _root;

            while(node != Nil)
            {
                var comparision = _keyComparer.Compare(key, node.Key);
                if (comparision == 0)
                    return node;
                else if (comparision < 0)
                    node = node.Left;
                else
                    node = node.Right;

            }

            return null;
        }

        private void LeftRotate(RbtNode<TKey, TValue> node)
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
        private void RightRotate(RbtNode<TKey, TValue> node)
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
            return result == Nil ? null : result;
        }

        public RbtNode<TKey, TValue> Min()
        {
            return MinOf(_root);
        }

        private RbtNode<TKey, TValue> MinOf(RbtNode<TKey, TValue> node)
        {
            var result = node;
            var x = node;
            while(x != Nil)
            {
                result = x;
                x = x.Left;
            }
            return result == Nil ? null : result;
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
