using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Data.Xml.Dom;
using Windows.System;

namespace Shagu.Utils.Utils
{
    public class AppInfoHelper
    {
        public static string Version
        {
            get
            {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public static async Task<string> GetAppName()
        {
            var file = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            var doc = await XmlDocument.LoadFromFileAsync(file);
            var property = doc.GetElementsByTagName("Properties").First();
            var display = property.ChildNodes.First(n => n.NodeName.Equals("DisplayName"));
            return display.InnerText;
        }

        public static async Task Rate(string productId)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=" + productId));
        }
    }
}
