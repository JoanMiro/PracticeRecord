using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PracticeRecord.Tests
{
    using Services;

    [TestClass, Ignore]
    public class PeriodMaintenanceService_Tests
    {
        [TestMethod]
        public void WhenCheckPeriodEnd_ThenCorrectValue()
        {
            // Arrange
            // Act
            var dbPath = PeriodMaintenanceService.CheckPeriodEnd();

            // Assert
            Assert.IsNotNull(dbPath);
        }
    }
}
