using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;

namespace NewLandGameLauncher
{
    enum LauncherStatus
    {
        Ready,
        Failed,
        DownloadingGame,
        DownloadUpdate
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string rootPath;
        private string versionFile;
        private string gameZip;
        private string gameExe;

        private LauncherStatus status;

        internal LauncherStatus Status
        {
            get => status;

            set
            {
                status = value;

                switch (status)
                {
                    case LauncherStatus.Ready:
                        PlayButton.Content = "Play";
                        break;
                    case LauncherStatus.Failed:
                        PlayButton.Content = "Update Failed - Retry";
                        break;
                    case LauncherStatus.DownloadingGame:
                        PlayButton.Content = "Downloading Game";
                        break;
                    case LauncherStatus.DownloadUpdate:
                        PlayButton.Content = "Downloading Update";
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            rootPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Dots Game";

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            versionFile = Path.Combine(rootPath, "Version.txt");
            gameZip = Path.Combine(rootPath, "Build.zip");
            gameExe = Path.Combine(rootPath, "Dots Game.exe");
        }

        private void SetRuntimeText()
        {
            RuntimeText.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void CheckForUpdates()
        {
            SetRuntimeText();

            if (File.Exists(versionFile))
            {
                Version localVersion = new Version(File.ReadAllText(versionFile));

                VersionText.Text = localVersion.ToString();

                try
                {
                    WebClient webClient = new WebClient();

                    Version onlineVersion = new Version(webClient.DownloadString("https://docs.google.com/uc?export=download&id=1B5FN-rBCHTq64jbOvXaXrVJIjFZf919L"));

                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        InstallGameFiles(true, onlineVersion);
                    }
                    else
                    {
                        Status = LauncherStatus.Ready;
                    }
                }
                catch (Exception ex)
                {
                    Status = LauncherStatus.Failed;

                    MessageBox.Show($"Error checking for game updates : {ex}");
                }
            }
            else
            {
                InstallGameFiles(false, Version.zero);
            }
        }

        private void InstallGameFiles(bool isUpdate, Version onlineVersion)
        {
            try
            {
                WebClient webClient = new WebClient();

                if (isUpdate)
                {
                    Status = LauncherStatus.DownloadUpdate;
                }
                else
                {
                    Status = LauncherStatus.DownloadingGame;

                    onlineVersion = new Version(webClient.DownloadString("https://docs.google.com/uc?export=download&id=1B5FN-rBCHTq64jbOvXaXrVJIjFZf919L"));
                }

                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                webClient.DownloadFileAsync(new Uri("https://docs.google.com/uc?export=download&id=1GO0RTVzQnwkic2I1pF89M_q86POugQVu"), gameZip, onlineVersion);
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.Failed;

                MessageBox.Show($"Error installing game files : {ex}");
            }
        }

        private void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();

                ZipFile.ExtractToDirectory(gameZip, rootPath);

                File.Delete(gameZip);
                File.WriteAllText(versionFile, onlineVersion);

                VersionText.Text = onlineVersion;

                Status = LauncherStatus.Ready;
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.Failed;

                MessageBox.Show($"Error finishing download : {ex}");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(gameExe) && Status == LauncherStatus.Ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(gameExe);

                startInfo.WorkingDirectory = Path.Combine(rootPath);

                Process.Start(startInfo);

                Close();
            }
            else if (Status == LauncherStatus.Failed)
            {
                CheckForUpdates();
            }
        }
    }

    struct Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private short major;
        private short minor;
        private short subMinor;

        internal Version(short major, short minor, short subMinor)
        {
            this.major = major;
            this.minor = minor;
            this.subMinor = subMinor;
        }

        internal Version(string version)
        {
            string[] versionStrings = version.Split('.');

            if (versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;

                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            subMinor = short.Parse(versionStrings[2]);
        }

        internal bool IsDifferentThan(Version otherVersion)
        {
            if (major != otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }
}
