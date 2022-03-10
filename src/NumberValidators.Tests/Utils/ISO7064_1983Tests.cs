using NumberValidators.Utils.ISO;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NumberValidators.Tests.Utils
{
    public class ISO7064_1983Tests
    {
        [Fact]
        public void MOD_11_2_Error()
        {
            Assert.Throws<ArgumentException>(() => ISO7064_1983.MOD_11_2(null, null, 10));
            Assert.Throws<ArgumentException>(() => ISO7064_1983.MOD_11_2(new int[] { 1, 2 }, null, 10));
            Assert.Throws<ArgumentException>(() => ISO7064_1983.MOD_11_2(new int[] { 1, 2 }, new int[] { 1, 2, 3 }, 10));
        }
    }
}
