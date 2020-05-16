using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinearAlgebra;
namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMatrixTranspose()
        {
            Matrix left = new Matrix(3, 3,
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            Assert.IsTrue(!left == new Matrix(3, 3,
                1, 4, 7,
                2, 5, 8,
                3, 6, 9));
        }

        [TestMethod]
        public void TestMatrixAugmentation()
        {
            Matrix left = new Matrix(3, 1,
                1,
                4,
                7), right = new Matrix(3, 2,
                2, 3,
                5, 6,
                8, 9);
            Assert.IsTrue((left | right) == new Matrix(3, 3,
                1, 2, 3,
                4, 5, 6,
                7, 8, 9));
        }
    }
}
