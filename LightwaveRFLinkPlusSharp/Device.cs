﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightwaveRFLinkPlusSharp
{
    public class Device
    {
        public string Id { get; }
        public string Name { get; }

        /// <summary>
        /// A list of the device's features. If you are wanting a specific feature's ID, use one of the helper
        /// properties (e.g. SwitchFeatureId) or the generic GetFeatureId instead
        /// </summary>
        public List<Feature> Features { get; }

        internal Device(JToken deviceJson)
        {
            Id = deviceJson["deviceId"].ToString();
            Name = deviceJson["name"].ToString();

            Features = new List<Feature>();

            JToken featureSets = deviceJson["featureSets"];
            if (featureSets != null)
            {
                foreach (var featureSet in featureSets)
                {
                    JToken features = featureSet["features"];
                    if (features != null)
                    {
                        foreach (var feature in features)
                        {
                            Features.Add(new Feature(feature["type"].ToString(), feature["featureId"].ToString()));
                        }
                    }
                }
            }
        }

        #region Properties for accessing the IDs of known feature types

        /// <summary>
        /// Note, it is simpler to use the typed GetSwitchStateAsync or SetSwitchStateAsync methods directly
        /// </summary>
        public string SwitchFeatureId => GetFeatureId("switch");

        public string ButtonPressFeatureId => GetFeatureId("buttonPress");
        public string CurrentTimeFeatureId => GetFeatureId("currentTime");
        public string DateFeatureId => GetFeatureId("date");
        public string DawnTimeFeatureId => GetFeatureId("dawnTime");
        public string DayFeatureId => GetFeatureId("day");
        public string DimLevelFeatureId => GetFeatureId("dimLevel");
        public string DuskTimeFeatureId => GetFeatureId("duskTime");
        public string IdentifyFeatureId => GetFeatureId("identify");
        public string LocationLatitudeFeatureId => GetFeatureId("locationLatitude");
        public string LocationLongitudeFeatureId => GetFeatureId("locationLongitude");
        public string MonthFeatureId => GetFeatureId("month");
        public string MonthArrayFeatureId => GetFeatureId("monthArray");
        public string ProtectionFeatureId => GetFeatureId("protection");
        public string RGBColorFeatureId => GetFeatureId("rgbColor");
        public string TimeFeatureId => GetFeatureId("time");
        public string TimeZoneFeatureId => GetFeatureId("timeZone");
        public string WeekdayFeatureId => GetFeatureId("weekday");
        public string WeekdayArrayFeatureId => GetFeatureId("weekdayArray");
        public string YearFeatureId => GetFeatureId("year");

        #endregion

        /// <summary>
        /// Gets the ID for a device's feature. Note there are also properties available for many common features - e.g. for
        /// the "switch" feature (whether the device is on or off), use SwitchFeatureId instead.
        /// </summary>
        /// <param name="type">The "type" of the device's desired feature, e.g. the "switch" feature controls whether the device
        /// is turned on or not</param>
        /// <returns>The ID of the device's feature, which can then be used with GetFeatureValue or SetFeatureValue</returns>
        public string GetFeatureId(string type)
        {
            Feature match = Features.FirstOrDefault(x => x.Type == type);
            return match?.Id;
        }

        #region Typed helper methods for getting and setting the state of various features

        /// <summary>
        /// Returns true if the device is switched on, or false if not
        /// </summary>
        public async Task<bool> GetSwitchStateAsync(LightwaveAPI api)
        {
            int featureValue = await api.GetFeatureValueAsync(SwitchFeatureId);
            return featureValue == 1 ? true : false;
        }

        /// <summary>
        /// Turn the device on or off
        /// </summary>
        public async Task SetSwitchStateAsync(bool on, LightwaveAPI api)
        {
            await api.SetFeatureValueAsync(SwitchFeatureId, on ? 1 : 0);
        }

        #endregion
    }
}