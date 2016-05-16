using NUnit.Framework;

namespace SimQLTask
{
    [TestFixture]
    public class SimQLProgram_Should
    {
        [Test]
        public void SimpleSum()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'a':{'x':3.14, 'b':[{'c':15}, {'c':9}]}, 'z':[2.65, 35]},
                                                        'queries': [ 'sum(a.b.c)']}");

            Assert.AreEqual(new[] {"sum(a.b.c) = 24"}, results);
        }

        [Test]
        public void ComplexSumQuery()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'z':[1,2,3,4,5,6,7,8,9,10,11,12]},
                                                        'queries': [ 'sum(z from 3 top 4)']}");

            Assert.AreEqual(new[] { "sum(z from 3 top 4) = 18" }, results);
        }

        [Test]
        public void ComplexSumQueryFromOnly()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'z':[1,2,3,4,5,6,7,8,9,10,11,12]},
                                                        'queries': [ 'sum(z from 11)']}");

            Assert.AreEqual(new[] { "sum(z from 11) = 23" }, results);
        }


        [Test]
        public void ComplexSumQueryTopOnly()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'z':[1,2,3,4,5,6,7,8,9,10,11,12]},
                                                        'queries': [ 'sum(z top 5)']}");

            Assert.AreEqual(new[] { "sum(z top 5) = 15" }, results);
        }

        [Test]
        public void ComplexQuery()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'a':[{'b': [3.14, 4]}, {'b':[{'c':15}, {'c':9}]}], 'z':[2.65, 35]},
                                                        'queries': [ 'sum(a.b.c)']}");

            Assert.AreEqual(new[] { "sum(a.b.c) = 24" }, results);
        }

        [Test]
        public void SimpleMin()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'a':{'x':3.14, 'b':[{'c':15}, {'c':9}]}, 'z':[2.65, 35]},
                                                        'queries': [ 'min(z)']}");

            Assert.AreEqual(new[] {"min(z) = 2.65"}, results);
        }

        [Test]
        public void SimpleMax()
        {
            var results = SimQLProgram.ExecuteQueries(@"{'data': {'a':{'x':3.14, 'b':[{'c':15}, {'c':9}]}, 'z':[2.65, 35]},
                                                        'queries': [ 'max(a.x)']}");

            Assert.AreEqual(new[] { "max(a.x) = 3.14" }, results);
        }

        [Test]
        public void SumEmptyDataToZero()
        {
            var results = SimQLProgram.ExecuteQueries(
                "{" +
                "'data': {}, " +
                "'queries': ['sum(item.cost)', 'sum(itemsCount)']}");
            Assert.AreEqual(new[] {"sum(item.cost) = 0", "sum(itemsCount) = 0"}, results);
        }

        [Test]
        public void SumSingleItem()
        {
            var results = SimQLProgram.ExecuteQueries(
                "{" +
                "'data': { 'itemsCount':42, 'foo':'bar' }, " +
                "'queries': ['sum(itemsCount)']}");
            Assert.AreEqual(new[] {"sum(itemsCount) = 42"}, results);
        }
    }
}