using NumberValidators.Utils;
using NumberValidators.Utils.GBT;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NumberValidators.Tests.Utils
{
    public class AreaHelperTests
    {
        [Fact]
        public void GetDeepestArea_Error()
        {
            Assert.Throws<ArgumentNullException>(() => AreaHelper.GetDeepestArea(10, null));
            Assert.Throws<ArgumentException>(() => AreaHelper.GetDeepestArea(1, GBT2260_2013.Singleton));
        }
    }
}
