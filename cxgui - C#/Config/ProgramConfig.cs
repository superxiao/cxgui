namespace CXGUI.Config
{
    using Microsoft.Win32;
    using System;
    using System.Configuration;

    [Serializable]
    public class ProgramConfig : ConfigurationSection
    {
        protected static System.Configuration.Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private ProgramConfig()
        {
            this["playerPath"] = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\wmplayer.exe", string.Empty, null);
            if (this["playerPath"] == null)
            {
                this["playerPath"] = string.Empty;
            }
        }

        public static ProgramConfig Get()
        {
            ProgramConfig section = (ProgramConfig) _config.Sections["programConfig"];
            if (section == null)
            {
                section = new ProgramConfig();
                _config.Sections.Add("programConfig", section);
                _config.Save();
            }
            return section;
        }

        public static void Save()
        {
            _config.Save();
        }

        [ConfigurationProperty("autoChangeAudioSourceFilter", DefaultValue=true)]
        public bool AutoChangeAudioSourceFilter
        {
            get
            {
                return (bool)this["autoChangeAudioSourceFilter"];
            }
            set
            {
                this["autoChangeAudioSourceFilter"] = value;
            }
        }

        [ConfigurationProperty("destDir", DefaultValue="")]
        public string DestDir
        {
            get
            {
                return (string) this["destDir"];
            }
            set
            {
                this["destDir"] = value;
            }
        }

        [ConfigurationProperty("inputDir", DefaultValue=false)]
        public bool OmitInputDir
        {
            get
            {
                return (bool)this["inputDir"];
            }
            set
            {
                this["inputDir"] = value;
            }
        }

        [ConfigurationProperty("playerPath", DefaultValue="")]
        public string PlayerPath
        {
            get
            {
                return (string) this["playerPath"];
            }
            set
            {
                this["playerPath"] = value;
            }
        }

        [ConfigurationProperty("profileName", DefaultValue="")]
        public string ProfileName
        {
            get
            {
                return (string) this["profileName"];
            }
            set
            {
                this["profileName"] = value;
            }
        }

        [ConfigurationProperty("silentRestart", DefaultValue=false)]
        public bool SilentRestart
        {
            get
            {
                return (bool)this["silentRestart"];
            }
            set
            {
                this["silentRestart"] = value;
            }
        }
    }
}

