﻿namespace Unosquare.RaspberryIO.Computer
{
    using System.Globalization;
    using System.IO;
    using Swan;

    /// <summary>
    /// The Official Raspberry Pi 7-inch touch display from the foundation
    /// Some docs available here:
    /// http://forums.pimoroni.com/t/official-7-raspberry-pi-touch-screen-faq/959.
    /// </summary>
    public class DsiDisplay : SingletonBase<DsiDisplay>
    {
        private const string BacklightFilename = "/sys/class/backlight/rpi_backlight/bl_power";
        private const string BrightnessFilename = "/sys/class/backlight/rpi_backlight/brightness";

        /// <summary>
        /// Prevents a default instance of the <see cref="DsiDisplay"/> class from being created.
        /// </summary>
        private DsiDisplay()
        {
            // placeholder
        }

        /// <summary>
        /// Gets a value indicating whether the Pi Foundation Display files are present.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is present; otherwise, <c>false</c>.
        /// </value>
        public bool IsPresent => File.Exists(BrightnessFilename);

        /// <summary>
        /// Gets or sets the brightness of the DSI display via filesystem.
        /// </summary>
        /// <value>
        /// The brightness.
        /// </value>
        public byte Brightness
        {
            get =>
                IsPresent
                    ? byte.TryParse(File.ReadAllText(BrightnessFilename).Trim(), out var brightness) ? brightness : (byte)0 :
                    (byte)0;
            set
            {
                if (IsPresent)
                    File.WriteAllText(BrightnessFilename, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the backlight of the DSI display on.
        /// This operation is performed via the file system.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is backlight on; otherwise, <c>false</c>.
        /// </value>
        public bool IsBacklightOn
        {
            get =>
                IsPresent && (int.TryParse(File.ReadAllText(BacklightFilename).Trim(), out var value) &&
                              value == 0);
            set
            {
                if (IsPresent)
                    File.WriteAllText(BacklightFilename, value ? "0" : "1");
            }
        }
    }
}
