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
        public void ComplexSum()
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