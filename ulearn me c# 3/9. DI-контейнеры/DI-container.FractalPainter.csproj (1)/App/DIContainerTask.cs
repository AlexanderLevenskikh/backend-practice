using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.Extensions.Conventions;

namespace FractalPainting.App
{
    public static class DIContainerTask
    {
        public static MainForm CreateMainForm()
        {
            // Example: ConfigureContainer()...
            return new MainForm();
        }

        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            // Example
            // container.Bind<TService>().To<TImplementation>();

            return container;
        }
    }

    public static class Services
    {
        private static readonly SettingsManager settingsManager;
        private static readonly PictureBoxImageHolder pictureBoxImageHolder;
        private static readonly Palette palette;
        private static readonly AppSettings appSettings;
        private static readonly IImageSettingsProvider imageSettingsProvider;
        private static readonly IImageDirectoryProvider imageDirectoryProvider;

        static Services()
        {
            palette = new Palette();
            pictureBoxImageHolder = new PictureBoxImageHolder();
            settingsManager = new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage());
            appSettings = settingsManager.Load();
            imageSettingsProvider = appSettings;
            imageDirectoryProvider = appSettings;
        }

        public static IObjectSerializer CreateObjectSerializer()
        {
            return new XmlObjectSerializer();
        }

        public static IBlobStorage CreateIBlobStorage()
        {
            return new FileBlobStorage();
        }

        public static SettingsManager GetSettingsManager()
        {
            return settingsManager;
        }

        public static PictureBoxImageHolder GetPictureBoxImageHolder()
        {
            return pictureBoxImageHolder;
        }

        public static IImageHolder GetImageHolder()
        {
            return pictureBoxImageHolder;
        }

        public static Palette GetPalette()
        {
            return palette;
        }

        public static ImageSettings GetImageSettings()
        {
            return appSettings.ImageSettings;
        }

        public static IImageSettingsProvider GetImageSettingsProvider()
        {
            return imageSettingsProvider;
        }

        public static AppSettings GetAppSettings()
        {
            return appSettings;
        }
    }

    public class DragonFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Дракон";
        public string Description => "Дракон Хартера-Хейтуэя";

        public void Perform()
        {
            var dragonSettings = CreateRandomSettings();
            // редактируем настройки:
            SettingsForm.For(dragonSettings).ShowDialog();
            // создаём painter с такими настройками
            var painter = new DragonPainter(Services.GetImageHolder(), dragonSettings);
            painter.Paint();
        }

        private static DragonSettings CreateRandomSettings()
        {
            return new DragonSettingsGenerator(new Random()).Generate();
        }
    }

    public class KochFractalAction : IUiAction
    {
        public MenuCategory Category => MenuCategory.Fractals;
        public string Name => "Кривая Коха";
        public string Description => "Кривая Коха";

        public void Perform()
        {
            var painter = new KochPainter(Services.GetImageHolder(), Services.GetPalette());
            painter.Paint();
        }
    }

    public class DragonPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly DragonSettings settings;
        private readonly float size;
        private Size imageSize;

        public DragonPainter(IImageHolder imageHolder, DragonSettings settings)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            imageSize = imageHolder.GetImageSize();
            size = Math.Min(imageSize.Width, imageSize.Height) / 2.1f;
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            {
                graphics.FillRectangle(Brushes.Black, 0, 0, imageSize.Width, imageSize.Height);
                var r = new Random();
                var cosa = (float)Math.Cos(settings.Angle1);
                var sina = (float)Math.Sin(settings.Angle1);
                var cosb = (float)Math.Cos(settings.Angle2);
                var sinb = (float)Math.Sin(settings.Angle2);
                var shiftX = settings.ShiftX * size * 0.8f;
                var shiftY = settings.ShiftY * size * 0.8f;
                var scale = settings.Scale;
                var p = new PointF(0, 0);
                foreach (var i in Enumerable.Range(0, settings.IterationsCount))
                {
                    graphics.FillRectangle(Brushes.Yellow, imageSize.Width / 3f + p.X, imageSize.Height / 2f + p.Y, 1, 1);
                    if (r.Next(0, 2) == 0)
                        p = new PointF(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
                    else
                        p = new PointF(scale * (p.X * cosb - p.Y * sinb) + shiftX, scale * (p.X * sinb + p.Y * cosb) + shiftY);
                    if (i % 100 == 0) imageHolder.UpdateUi();
                }
            }
            imageHolder.UpdateUi();
        }
    }
}
