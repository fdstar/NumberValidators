using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NumberValidators.Tests
{
    public class AreaTests
    {
        [Fact]
        public void Octr_Error()
        {
            Assert.Throws<ArgumentException>(() => new Area(1234, null));
            Assert.Throws<ArgumentException>(() => new Area(1, string.Empty));
        }
    }
}
