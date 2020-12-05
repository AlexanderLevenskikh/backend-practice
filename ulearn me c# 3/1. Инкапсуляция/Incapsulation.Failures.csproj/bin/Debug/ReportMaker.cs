using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
    }

    public class Failure
    {
        public FailureType Type { get; set; }
        public DateTime Date { get; set; }
        public int DeviceId { get; set; }

        public bool IsSerious()
        {
            return Type == FailureType.UnexpectedShutdown || Type == FailureType.HardwareFailure;
        }

        public bool IsEarlierThan(DateTime dateTime)
        {
            return Date < dateTime;
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailure,
        ConnectionProblems
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceId,
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var devicesMap = devices.Select(x => new Device
            {
                DeviceId = (int) x["DeviceId"],
                Name = (string) x["Name"],
            }).ToDictionary(x => x.DeviceId);

            var date = new DateTime(year, month, day);
            var failures = new List<Failure>();
            for (var i = 0; i < failureTypes.Length; i++)
                failures.Add(new Failure
                {
                    DeviceId = deviceId[i],
                    Date = new DateTime((int) times[i][2], (int) times[i][1], (int) times[i][0]),
                    Type = (FailureType) failureTypes[i],
                });

            var problematicDeviceIds = new HashSet<int>();
            foreach (var failure in failures)
            {
                if (failure.IsSerious() && failure.IsEarlierThan(date))
                {
                    problematicDeviceIds.Add(failure.DeviceId);
                }
            }

            return problematicDeviceIds
                .Select(d => devicesMap[d]?.Name)
                .ToList();
        }
    }
}