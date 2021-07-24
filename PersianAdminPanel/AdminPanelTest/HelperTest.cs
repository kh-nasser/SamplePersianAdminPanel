using System.Web;
using NUnit.Framework;
using PersianAdminPanel.Utils;
using Rhino.Mocks;

namespace AdminPanelTest
{

    [TestFixture]
    public class HelpersTests //: TestBase
    {
        HttpRequestBase _httpRequest;

        private const string XForwardedFor = "X_FORWARDED_FOR";
        private const string MalformedIpAddress = "MALFORMED";
        private const string DefaultIpAddress = "0.0.0.0";
        private const string GoogleIpAddress = "74.125.224.224";
        private const string MicrosoftIpAddress = "65.55.58.201";
        private const string Private24Bit = "10.0.0.0";
        private const string Private20Bit = "172.16.0.0";
        private const string Private16Bit = "192.168.0.0";
        private const string PrivateLinkLocal = "169.254.0.0";

        [SetUp]
        public void Setup()
        {
            _httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
        }

        [TearDown]
        public void Teardown()
        {
            _httpRequest = null;
        }

        [Test]
        public void PublicIpAndNullXForwardedFor_Returns_CorrectIp()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(null);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void PublicIpAndEmptyXForwardedFor_Returns_CorrectIp()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(string.Empty);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void MalformedUserHostAddress_Returns_DefaultIpAddress()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(MalformedIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(null);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void MalformedXForwardedFor_Returns_DefaultIpAddress()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(MalformedIpAddress);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void SingleValidPublicXForwardedFor_Returns_XForwardedFor()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(MicrosoftIpAddress);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void MultipleValidPublicXForwardedFor_Returns_LastXForwardedFor()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(GoogleIpAddress + "," + MicrosoftIpAddress);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void SinglePrivateXForwardedFor_Returns_UserHostAddress()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(Private24Bit);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void MultiplePrivateXForwardedFor_Returns_UserHostAddress()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            const string privateIpList = Private24Bit + "," + Private20Bit + "," + Private16Bit + "," + PrivateLinkLocal;
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(privateIpList);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }

        [Test]
        public void MultiplePublicXForwardedForWithPrivateLast_Returns_LastPublic()
        {
            // Arrange
            _httpRequest.Stub(x => x.UserHostAddress).Return(GoogleIpAddress);
            const string privateIpList = Private24Bit + "," + Private20Bit + "," + MicrosoftIpAddress + "," + PrivateLinkLocal;
            _httpRequest.Stub(x => x.ServerVariables[XForwardedFor]).Return(privateIpList);

            // Act
            var ip = IPAddressHelper.GetClientIpAddress(_httpRequest);

            // Assert
            Assert.AreEqual(ip, GoogleIpAddress);
        }
    }
}