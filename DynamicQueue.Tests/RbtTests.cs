﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQueue.Tests
{
    [TestFixture]
    public class RbtTests
    {
        [Test]
        public void WhenSingleNodeAdded_ItIsMaximum()
        {
            const int key = 1;
            const int val = 2;
            var rbt = new Rbt<int, int>(Comparer<int>.Create((a,b) => a.CompareTo(b)));
            rbt.Insert(key, val);

            var max = rbt.Max();

            Assert.AreEqual(key, max.Key);
            Assert.AreEqual(val, max.Value);
        }

        [Test]
        public void WhenTwoNodesAdded_ItRetrurnsMaximum()
        {
            var rbt = new Rbt<int, int>(Comparer<int>.Create((a,b) => a.CompareTo(b)));
            rbt.Insert(1, 1);
            rbt.Insert(10, 10);

            var max = rbt.Max();

            Assert.AreEqual(10, max.Key);
            Assert.AreEqual(10, max.Value);
        }

        [Test]
        public void WhenNoNodesAdded_ItRetrurnsNull_AsMaximum()
        {
            var rbt = new Rbt<int, int>(Comparer<int>.Create((a,b) => a.CompareTo(b)));

            var max = rbt.Max();

            Assert.IsNull(max);
        }
        [Test]
        public void InOrderTraversal()
        {
            var rbt = new Rbt<int, int>(Comparer<int>.Create((a,b) => a.CompareTo(b)));

            for (int i = 10; i >= 0;  i--)
            {
                rbt.Insert(i, i);
            }

            CollectionAssert.AreEqual(Enumerable.Range(0,11), rbt);
        }

        [Test]
        public void DeleteTest()
        {
            var rbt = new Rbt<int, int>(Comparer<int>.Create((a, b) => a.CompareTo(b)));

            for (int i = 10; i >= 0; i--)
            {
                rbt.Insert(i, i);
            }

            rbt.Delete(rbt.Find(0));
            rbt.Delete(rbt.Find(5));
            rbt.Delete(rbt.Find(10));

            CollectionAssert.AreEqual(Enumerable.Range(1, 9).Except(new[] { 5 }), rbt);
        }

    }
}
