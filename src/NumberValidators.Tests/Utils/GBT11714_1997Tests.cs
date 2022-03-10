using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NumberValidators.Tests.Utils
{
    public class GBT11714_1997Tests
    {
        [Fact]
        public void GetCheckBit_Error()
        {
            Assert.Throws<ArgumentException>(() => GBT11714_1997.GetCheckBit(null));
            Assert.Throws<ArgumentException>(() => GBT11714_1997.GetCheckBit("12345"));
        }
    }
}
