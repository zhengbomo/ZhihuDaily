using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.Storage.Streams;
using Windows.System.Profile;

namespace Shagu.Utils
{
    public enum DeviceType
    {
        Mobile, Desktop
    }

    public class DeviceInfoHelper
    {
        public static DeviceType DeviceType
        {
            get
            {
                switch (AnalyticsInfo.VersionInfo.DeviceFamily)
                {
                    case "Windows.Mobile":
                        return DeviceType.Mobile;
                    case "Windows.Desktop":
                        return DeviceType.Desktop;
                    default:
                        return DeviceType.Desktop;
                }
            }
        }

        private static string uniqueId;


        /// <summary>
        /// CPU 核心数量
        /// </summary>
        public int ProcessorCount => Environment.ProcessorCount;

        /// <summary>
        /// 系统自上次启动以来所经过的毫秒数
        /// </summary>
        public int StartUpTickCount => Environment.TickCount;

        /// <summary>
        /// 当前系统版本号
        /// 这里获取的是Window NT的版本号
        ///     8.1为6.3
        ///     8.0为6.2
        /// </summary>
        public static async Task<string> GetWindowsVersion()
        {
            return await GetWindowsVersionAsync();
        }

        /// <summary>
        /// 是否是设计时
        /// </summary>
        public static bool IsDesignMode => Windows.ApplicationModel.DesignMode.DesignModeEnabled;


        /// <summary>
        /// 获取设备唯一标识
        /// </summary>
        public static string UniqueId
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueId))
                {
                    if (ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                    {
                        if (ApiInformation.IsMethodPresent("Windows.System.Profile.HardwareIdentification", "GetPackageSpecificToken", 1))
                        {
                            var token = HardwareIdentification.GetPackageSpecificToken(null);
                            var buffer = token.Id;
                            using (var dataReader = DataReader.FromBuffer(buffer))
                            {
                                var bytes = new byte[buffer.Length];
                                dataReader.ReadBytes(bytes);
                                uniqueId = BitConverter.ToString(bytes).Replace("-", string.Empty);
                                return uniqueId;
                            }
                        }
                    }
                    throw new NotImplementedException("不支持HardwareIdentification.GetPackageSpecificToken()方法");
                }
                return uniqueId;
            }
        }

        /// <summary>
        /// Property that returns the connection profile [ ie, availability of Internet ]
        /// Interface type can be [ 1,6,9,23,24,37,71,131,144 ]
        /// 1 - > Some other type of network interface.
        /// 6 - > An Ethernet network interface.
        /// 9 - > A token ring network interface.
        /// 23 -> A PPP network interface.
        /// 24 -> A software loopback network interface.
        /// 37 -> An ATM network interface.
        /// 71 -> An IEEE 802.11 wireless network interface.
        /// 131 -> A tunnel type encapsulation network interface.
        /// 144 -> An IEEE 1394 (Firewire) high performance serial bus network interface.
        /// </summary>
        public static bool IsConnectedToNetwork
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile != null)
                {
                    var interfaceType = profile.NetworkAdapter.IanaInterfaceType;
                    return interfaceType == 71 || interfaceType == 6;
                }
                return false;
            }
        }

        /// <summary>
        /// 网络是否可用，监听网络变化事件：NetworkChange.NetworkAddressChanged;
        /// </summary>
        public static bool IsNetworkAvailable => NetworkInterface.GetIsNetworkAvailable();

        public static bool IsOnWifi()
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return internetConnectionProfile != null && internetConnectionProfile.IsWlanConnectionProfile;
        }

        public static bool IsConnectedToDataRoaming()
        {
            bool isRoaming = false;
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile != null && internetConnectionProfile.IsWwanConnectionProfile)
            {
                ConnectionCost cc = internetConnectionProfile.GetConnectionCost();
                isRoaming = cc.Roaming;
            }
            return isRoaming;
        }


        #region 获取当前系统版本号，摘自：http://attackpattern.com/2013/03/device-information-in-windows-8-store-apps/

        private static async Task<string> GetWindowsVersionAsync()
        {
            var hal = await GetHalDevice(DEVICE_DRIVER_VERSION_KEY);
            if (hal == null || !hal.Properties.ContainsKey(DEVICE_DRIVER_VERSION_KEY))
                return null;

            var versionParts = hal.Properties[DEVICE_DRIVER_VERSION_KEY].ToString().Split('.');
            return string.Join(".", versionParts.Take(2).ToArray());
        }

        private static async Task<PnpObject> GetHalDevice(params string[] properties)
        {
            var actualProperties = properties.Concat(new[] {DEVICE_CLASS_KEY});
            var rootDevices = await PnpObject.FindAllAsync(PnpObjectType.Device,
                actualProperties, ROOT_QUERY);

            foreach (var rootDevice in rootDevices.Where(d => d.Properties != null && d.Properties.Any()))
            {
                var lastProperty = rootDevice.Properties.Last();
                if (lastProperty.Value != null)
                    if (lastProperty.Value.ToString().Equals(HAL_DEVICE_CLASS))
                        return rootDevice;
            }
            return null;
        }

        private const string DEVICE_CLASS_KEY = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
        private const string DEVICE_DRIVER_VERSION_KEY = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
        private const string ROOT_CONTAINER = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        private const string ROOT_QUERY = "System.Devices.ContainerId:=\"" + ROOT_CONTAINER + "\"";
        private const string HAL_DEVICE_CLASS = "4d36e966-e325-11ce-bfc1-08002be10318";

        #endregion






        private static async Task<ConnectionProfile> GetConectionProfile()
        {
            try
            {
                ConnectionProfileFilter filter = new ConnectionProfileFilter
                {
                    IsWwanConnectionProfile = true,
                    IsConnected = true
                };

                var result = await NetworkInformation.FindConnectionProfilesAsync(filter);

                if (result != null && result.Count > 0)
                    return result[result.Count - 1]; //通常连接的是最后一个，当然手机已连接上就只有一个(吧?)=。=
                else
                    return NetworkInformation.GetInternetConnectionProfile();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取运营商名称
        /// </summary>
        public static async Task<string> GetOperators()
        {
            var result = await GetConectionProfile();
            if (result != null && result.WwanConnectionProfileDetails != null)
            {
                switch (result.WwanConnectionProfileDetails.AccessPointName)
                {
                    case "cmnet":
                    case "cmwap":
                        return "中国移动";

                    case "3gwap":
                    case "3gnet":
                    case "uninet":
                    case "uniwap":
                        return "中国联通";

                    case "ctnet":
                    case "ctwap":
                        return "中国电信";

                    default:
                        return "无运营商";
                }
            }
            return "无运营商";
        }

    }
}