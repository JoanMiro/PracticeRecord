using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PracticeRecord.UnitTests.Core
{
    using Services;

    [TestClass]
    public class PeriodMaintenanceServiceTests
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
