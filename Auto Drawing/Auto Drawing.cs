/*  CTRADER GURU --> Template 1.0.6 --> https://ctrader.com/algos/indicators/show/2043

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/cTraderGURU/

*/

using System;
using cAlgo.API;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class AutoDrawing : Indicator
    {

        #region Enums & Class


        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        public enum Trend
        {

            Bearish,
            Bullish,
            Neutral

        }

        class ChartPoint
        {

            public ChartPoint(int barIndex, double price)
            {

                BarIndex = barIndex;
                Price = price;

            }

            public int BarIndex { get; private set; }
            public double Price { get; private set; }

        }

        class ChartPoints
        {

            public ChartPoints(ChartPoint point1, ChartPoint point2)
            {

                Point1 = point1;
                Point2 = point2;

            }

            public ChartPoint Point1 { get; private set; }
            public ChartPoint Point2 { get; private set; }

        }

        #endregion

        #region Identity

        public const string NAME = "Auto Drawing";

        public const string VERSION = "1.0.6";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+auto+drawing")]
        public string ProductInfo { get; set; }

        [Parameter("Deviation Period", Group = "Params", DefaultValue = 5, MinValue = 2, Step = 1)]
        public int Deviation { get; set; }

        [Parameter("Extend To Infinity ?", Group = "Styles", DefaultValue = false)]
        public bool EntendToInfinity { get; set; }

        [Parameter("Panel BG Color", Group = "Styles", DefaultValue = MyColors.LightGray)]
        public MyColors PBackgroundColorString { get; set; }

        [Parameter("Drawings Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors DrawingsColorString { get; set; }

        [Parameter("Selection Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors SelectionleColorString { get; set; }

        [Parameter("Opacity", Group = "Styles", DefaultValue = 50, MinValue = 0, MaxValue = 100, Step = 1)]
        public int Opacity { get; set; }

        #endregion

        #region Property

        Color PBackgroundColor;

        Color DrawingsColor;

        Color SelectionleColor;

        ChartRectangle SelectRectangle;

        int SelectedStartBarIndex;

        int SelectedEndBarIndex;

        readonly decimal[] DefaultFiboLevels = new[] 
        {
            0.0m,
            23.6m,
            38.2m,
            50.0m,
            61.8m,
            76.4m,
            100.0m
        };

        ControlBase DrawingDialog;

        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

            PBackgroundColor = Color.FromName(PBackgroundColorString.ToString("G"));

            DrawingsColor = Color.FromName(DrawingsColorString.ToString("G"));

            SelectionleColor = Color.FromArgb(Opacity, Color.FromName(SelectionleColorString.ToString("G")));

            Chart.MouseDown += Chart_MouseDown;
            Chart.MouseUp += Chart_MouseUp;
            Chart.MouseMove += Chart_MouseMove;

            CreateDrawingDialog();

        }

        public override void Calculate(int index)
        {



        }

        #endregion

        #region Private Methods

        private void CreateDrawingDialog()
        {

            string separator = "---------------------";

            var stackPanel = new StackPanel 
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = API.HorizontalAlignment.Left,
                Orientation = API.Orientation.Vertical,
                IsVisible = false,
                Width = 160,
                BackgroundColor = PBackgroundColor
            };

            var trendLineHorizontalButton = new API.Button 
            {
                Text = "TrendLine Horizontal",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var barAverageButton = new API.Button 
            {
                Text = "Bar Average",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var bodyAverageButton = new API.Button 
            {
                Text = "Body Average",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var equiChannelButton = new API.Button 
            {
                Text = "Equidistant Channel",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var linearRegressionButton = new API.Button 
            {
                Text = "Linear Regression",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var fibonacciRetracementButton = new API.Button 
            {
                Text = "Fibonacci Retracement",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var supportTrendLineButton = new API.Button 
            {
                Text = "Support Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var resistanceTrendLineButton = new API.Button 
            {
                Text = "Resistance Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var resistanceSupportTrendLineButton = new API.Button 
            {
                Text = "Rex / Sup Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var supportLevelButton = new API.Button 
            {
                Text = "Support",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var resistanceLevelButton = new API.Button 
            {
                Text = "Resistance",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var removeAllObjButton = new API.Button 
            {
                Text = "Remove all (Shift + Click)",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0
            };

            var resistanceSupportLevelButton = new API.Button 
            {
                Text = "Rex / Sup Level",
                HorizontalContentAlignment = API.HorizontalAlignment.Left,
                CornerRadius = 0

            };

            var space = new API.Button 
            {
                Text = separator,
                HorizontalContentAlignment = API.HorizontalAlignment.Center,
                CornerRadius = 0

            };

            var space2 = new API.Button 
            {
                Text = separator,
                HorizontalContentAlignment = API.HorizontalAlignment.Center,
                CornerRadius = 0

            };

            trendLineHorizontalButton.Click += TrendLineHorizontalButton_Click;
            barAverageButton.Click += BarAverageButton_Click;
            bodyAverageButton.Click += BodyAverageButton_Click;
            equiChannelButton.Click += EquiChannelButton_Click;
            linearRegressionButton.Click += LinearRegressionTo_Click;
            fibonacciRetracementButton.Click += FibonacciRetracementButton_Click;
            supportTrendLineButton.Click += SupportTrendLineButton_Click;
            resistanceTrendLineButton.Click += ResistanceTrendLineButton_Click;
            resistanceSupportTrendLineButton.Click += ResistanceSupportTrendLineButton_Click;
            supportLevelButton.Click += SupportLevelButton_Click;
            resistanceLevelButton.Click += ResistanceLevelButton_Click;
            resistanceSupportLevelButton.Click += ResistanceSupportLevelButton_Click;
            removeAllObjButton.Click += RemoveAllObject_Click;

            stackPanel.AddChild(trendLineHorizontalButton);
            stackPanel.AddChild(barAverageButton);
            stackPanel.AddChild(bodyAverageButton);
            stackPanel.AddChild(fibonacciRetracementButton);
            stackPanel.AddChild(linearRegressionButton);
            stackPanel.AddChild(equiChannelButton);
            stackPanel.AddChild(space);
            stackPanel.AddChild(supportTrendLineButton);
            stackPanel.AddChild(resistanceTrendLineButton);
            stackPanel.AddChild(resistanceSupportTrendLineButton);
            stackPanel.AddChild(supportLevelButton);
            stackPanel.AddChild(resistanceLevelButton);
            stackPanel.AddChild(resistanceSupportLevelButton);
            stackPanel.AddChild(space2);
            stackPanel.AddChild(removeAllObjButton);

            DrawingDialog = stackPanel;
            Chart.AddControl(DrawingDialog);

        }

        private void LinearRegressionTo_Click(ButtonClickEventArgs obj)
        {

            DataSeries series = Bars.ClosePrices;

            int PeriodBars = SelectedEndBarIndex - SelectedStartBarIndex;

            double sum_x = 0, sum_x2 = 0, sum_y = 0, sum_xy = 0;

            int start = series.Count - PeriodBars;
            int end = series.Count - 1;

            for (int i = start; i <= end; i++)
            {
                sum_x += 1.0 * i;
                sum_x2 += 1.0 * i * i;
                sum_y += series[i];
                sum_xy += series[i] * i;
            }

            double a = (PeriodBars * sum_xy - sum_x * sum_y) / (PeriodBars * sum_x2 - sum_x * sum_x);
            double b = (sum_y - a * sum_x) / PeriodBars;

            double maxDeviation = 0;
            double sumDevation = 0;

            for (int i = start; i <= end; i++)
            {

                double price = a * i + b;
                maxDeviation = Math.Max(Math.Abs(series[i] - price), maxDeviation);
                sumDevation += Math.Pow(series[i] - price, 2.0);

            }

            double stdDeviation = Math.Sqrt(sumDevation / PeriodBars);

            end += 20;

            double pr1 = a * start + b;
            double pr2 = a * end + b;

            string namexx1 = string.Format("Linear Regression C [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            ChartTrendLine xx1 = Chart.DrawTrendLine(namexx1, start, pr1, end, pr2, DrawingsColor, 1, LineStyle.Lines);
            xx1.IsInteractive = false;
            xx1.ExtendToInfinity = EntendToInfinity;

            string namexx2 = string.Format("Linear Regression T [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            ChartTrendLine xx2 = Chart.DrawTrendLine(namexx2, start, pr1 + maxDeviation, end, pr2 + maxDeviation, DrawingsColor, 1, LineStyle.Solid);
            xx2.IsInteractive = false;
            xx2.ExtendToInfinity = EntendToInfinity;

            string namexx3 = string.Format("Linear Regression B [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            ChartTrendLine xx3 = Chart.DrawTrendLine(namexx3, start, pr1 - maxDeviation, end, pr2 - maxDeviation, DrawingsColor, 1, LineStyle.Solid);
            xx3.IsInteractive = false;
            xx3.ExtendToInfinity = EntendToInfinity;

            string namexx4 = string.Format("Linear Regression DT [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            ChartTrendLine xx4 = Chart.DrawTrendLine(namexx4, start, pr1 + stdDeviation, end, pr2 + stdDeviation, DrawingsColor, 1, LineStyle.DotsVeryRare);
            xx4.IsInteractive = false;
            xx4.ExtendToInfinity = EntendToInfinity;

            ChartTrendLine xx5 = Chart.DrawTrendLine("Lineardev-bottom", start, pr1 - stdDeviation, end, pr2 - stdDeviation, DrawingsColor, 1, LineStyle.DotsVeryRare);
            xx5.IsInteractive = false;
            xx5.ExtendToInfinity = EntendToInfinity;

            CloseDrawingDialog();

        }

        private void ResistanceSupportLevelButton_Click(ButtonClickEventArgs obj)
        {

            ResistanceLevelButton_Click(obj);
            SupportLevelButton_Click(obj);
            CloseDrawingDialog();

        }

        private void ResistanceSupportTrendLineButton_Click(ButtonClickEventArgs obj)
        {

            ResistanceTrendLineButton_Click(obj);
            SupportTrendLineButton_Click(obj);
            CloseDrawingDialog();

        }

        private void RemoveAllObject_Click(ButtonClickEventArgs obj)
        {

            Chart.RemoveAllObjects();

        }

        private void FibonacciRetracementButton_Click(ButtonClickEventArgs obj)
        {
            var extremums = GetHighLowInSelection();
            var name = string.Format("Fibonacci Retracement [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var fibo = Chart.DrawFibonacciRetracement(name, point2.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            fibo.IsInteractive = true;
            fibo.DisplayPrices = false;
            SetDefaultFiboLevels(fibo.FibonacciLevels);
            CloseDrawingDialog();
        }

        private void TrendLineHorizontalButton_Click(ButtonClickEventArgs obj)
        {

            var name = string.Format("Horizontal Trend Line [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawTrendLine(name, SelectedStartBarIndex, Bars.ClosePrices[SelectedStartBarIndex], SelectedEndBarIndex, Bars.ClosePrices[SelectedStartBarIndex], DrawingsColor);
            line.IsInteractive = true;
            line.ExtendToInfinity = EntendToInfinity;
            CloseDrawingDialog();

        }

        private void BarAverageButton_Click(ButtonClickEventArgs obj)
        {

            var average = GetBodyAverageInSelection(true);
            MessageBox.Show(string.Format("Bar Average : {0}, for {1} bars", average[0], average[1]), "Bar Average");

        }

        private void BodyAverageButton_Click(ButtonClickEventArgs obj)
        {

            var average = GetBodyAverageInSelection();
            MessageBox.Show(string.Format("Body Average : {0}, for {1} bars", average[0], average[1]), "Body Average");

        }

        private void EquiChannelButton_Click(ButtonClickEventArgs obj)
        {

            var extremums = GetTwoTopHighExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Equi Channel [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));

            double distance = (50) * Symbol.PipSize;

            var equi = Chart.DrawEquidistantChannel(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, distance, DrawingsColor);
            equi.IsInteractive = true;
            equi.ExtendToInfinity = EntendToInfinity;

            CloseDrawingDialog();

        }

        private void SetDefaultFiboLevels(IEnumerable<FibonacciLevel> levels)
        {
            foreach (var level in levels)
            {
                level.IsVisible = Array.IndexOf(DefaultFiboLevels, (decimal)level.PercentLevel) > -1;
            }
        }

        private void ResistanceTrendLineButton_Click(ButtonClickEventArgs obj)
        {
            var extremums = GetTwoTopHighExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Resistance Trend Line [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawTrendLine(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            line.IsInteractive = true;
            line.ExtendToInfinity = EntendToInfinity;
            CloseDrawingDialog();
        }

        private void SupportTrendLineButton_Click(ButtonClickEventArgs obj)
        {

            var extremums = GetTwoBottomLowExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Support Trend Line [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawTrendLine(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            line.IsInteractive = true;
            line.ExtendToInfinity = EntendToInfinity;

            CloseDrawingDialog();

        }

        private void ResistanceLevelButton_Click(ButtonClickEventArgs obj)
        {
            var maximum = GetMaximumInSelection();
            var name = string.Format("Resistance Level [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawHorizontalLine(name, maximum.Price, DrawingsColor);
            line.IsInteractive = true;
            CloseDrawingDialog();
        }

        private void SupportLevelButton_Click(ButtonClickEventArgs obj)
        {

            var minimum = GetMinimumInSelection();
            var name = string.Format("Support Level [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawHorizontalLine(name, minimum.Price, DrawingsColor);
            line.IsInteractive = true;
            CloseDrawingDialog();

        }

        private ChartPoint GetMaximumInSelection()
        {
            var priceMax = double.MinValue;
            int barIndexMax = -1;
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {
                var high = Bars[i].High;
                if (high > priceMax)
                {
                    priceMax = high;
                    barIndexMax = i;
                }
            }
            return new ChartPoint(barIndexMax, priceMax);
        }

        private ChartPoint GetMinimumInSelection()
        {
            var priceMin = double.MaxValue;
            int barIndexMin = -1;
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {
                var low = Bars[i].Low;
                if (low < priceMin)
                {
                    priceMin = low;
                    barIndexMin = i;
                }
            }
            return new ChartPoint(barIndexMin, priceMin);
        }

        private ChartPoints GetHighLowInSelection()
        {
            var priceMax = double.MinValue;
            var priceMin = double.MaxValue;
            int barIndexMin = -1;
            int barIndexMax = -1;
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {
                var high = Bars[i].High;
                var low = Bars[i].Low;
                if (high > priceMax)
                {
                    priceMax = high;
                    barIndexMax = i;
                }
                if (low < priceMin)
                {
                    priceMin = low;
                    barIndexMin = i;
                }
            }

            var maximum = new ChartPoint(barIndexMax, priceMax);
            var minimum = new ChartPoint(barIndexMin, priceMin);
            return new ChartPoints(minimum, maximum);
        }

        private ChartPoints GetTwoTopHighExtremumsInSelection()
        {

            int count = 0;
            ChartPoint firstHigh = new ChartPoint(0, 0);
            ChartPoint lastHigh = new ChartPoint(0, 0);

            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            Trend Direction = (Bars[SelectedStartBarIndex].Low > Bars[SelectedEndBarIndex].Low) ? Trend.Bearish : Trend.Bullish;

            if (Direction == Trend.Bullish)
            {

                for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
                {

                    count++;

                    if (firstHigh.Price == 0)
                    {

                        firstHigh = new ChartPoint(i, Bars[i].High);
                        count--;

                    }
                    else if (count < Deviation && Bars[i].High > firstHigh.Price)
                    {

                        firstHigh = new ChartPoint(i, Bars[i].High);
                        count--;

                    }

                    if (count > Deviation && Bars[i].High > lastHigh.Price)
                        lastHigh = new ChartPoint(i, Bars[i].High);

                }

            }
            else
            {

                for (int i = SelectedEndBarIndex; i >= SelectedStartBarIndex; i--)
                {

                    count++;

                    if (firstHigh.Price == 0)
                    {

                        firstHigh = new ChartPoint(i, Bars[i].High);
                        count--;

                    }
                    else if (count < Deviation && Bars[i].High > firstHigh.Price)
                    {

                        firstHigh = new ChartPoint(i, Bars[i].High);
                        count--;

                    }

                    if (count > Deviation && Bars[i].High > lastHigh.Price)
                        lastHigh = new ChartPoint(i, Bars[i].High);

                }

            }

            if (firstHigh.Price == 0 || lastHigh.Price == 0)
                return null;

            return new ChartPoints(firstHigh, lastHigh);

        }


        private double[] GetBodyAverageInSelection(bool bar = false)
        {

            double total = 0;
            double count = 0;

            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {

                count++;

                total += (bar) ? Math.Abs(Bars[i].High - Bars[i].Low) : Math.Abs(Bars[i].Open - Bars[i].Close);

            }

            return new double[] 
            {
                Math.Round((total / count) / Symbol.PipSize, 2),
                count
            };

        }

        private ChartPoints GetTwoBottomLowExtremumsInSelection()
        {

            int count = 0;
            ChartPoint firstLow = new ChartPoint(0, 0);
            ChartPoint lastLow = new ChartPoint(0, 0);

            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            Trend Direction = (Bars[SelectedStartBarIndex].Low > Bars[SelectedEndBarIndex].Low) ? Trend.Bearish : Trend.Bullish;

            if (Direction == Trend.Bearish)
            {

                for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
                {

                    count++;

                    if (firstLow.Price == 0)
                    {

                        firstLow = new ChartPoint(i, Bars[i].Low);
                        count--;

                    }
                    else if (count < Deviation && Bars[i].Low < firstLow.Price)
                    {

                        firstLow = new ChartPoint(i, Bars[i].Low);
                        count--;

                    }

                    if (lastLow.Price == 0 || (count > Deviation && Bars[i].Low < lastLow.Price))
                        lastLow = new ChartPoint(i, Bars[i].Low);

                }

            }
            else
            {

                for (int i = SelectedEndBarIndex; i >= SelectedStartBarIndex; i--)
                {

                    count++;

                    if (firstLow.Price == 0)
                    {

                        firstLow = new ChartPoint(i, Bars[i].Low);
                        count--;

                    }
                    else if (count < Deviation && Bars[i].Low < firstLow.Price)
                    {

                        firstLow = new ChartPoint(i, Bars[i].Low);
                        count--;

                    }

                    if (lastLow.Price == 0 || (count > Deviation && Bars[i].Low < lastLow.Price))
                        lastLow = new ChartPoint(i, Bars[i].Low);

                }

            }

            if (firstLow.Price == 0 || lastLow.Price == 0)
                return null;

            return new ChartPoints(firstLow, lastLow);

        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            if (SelectRectangle == null)
                return;

            SelectRectangle.Time2 = obj.TimeValue;
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {

            if (DrawingDialog.IsVisible)
                CloseDrawingDialog();

            if (obj.CtrlKey)
            {

                Chart.IsScrollingEnabled = false;
                SelectRectangle = CreateDragRectangle(obj.TimeValue);

            }
            else if (obj.ShiftKey)
            {

                RemoveAllObject_Click(null);

            }

        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            Chart.IsScrollingEnabled = true;

            if (SelectRectangle != null)
            {
                SetSelectedStartEndIndex(SelectRectangle);
                Chart.RemoveObject(SelectRectangle.Name);
                SelectRectangle = null;

                if (SelectedStartBarIndex >= 0 && SelectedEndBarIndex >= 0)
                {
                    OpenDrawingDialog(obj.MouseX, obj.MouseY);
                }
            }
        }

        private void SetSelectedStartEndIndex(ChartRectangle rectangle)
        {
            var index1 = Bars.OpenTimes.GetIndexByTime(rectangle.Time1);
            var index2 = Bars.OpenTimes.GetIndexByTime(rectangle.Time2);
            SelectedStartBarIndex = Math.Min(index1, index2);
            SelectedEndBarIndex = Math.Max(index1, index2);
        }

        private void OpenDrawingDialog(double mouseX, double mouseY)
        {
            DrawingDialog.IsVisible = true;
            var left = Chart.Width - mouseX > 160 ? mouseX : mouseX - 160;
            var right = Chart.Height - mouseY > 100 ? mouseY : mouseY - 100;
            DrawingDialog.Margin = new Thickness(left, right, 0, 0);

        }

        private void CloseDrawingDialog()
        {
            DrawingDialog.IsVisible = false;
        }

        private ChartRectangle CreateDragRectangle(DateTime time)
        {
            var rect = Chart.DrawRectangle("DragRectangle", time, Chart.TopY, time, Chart.BottomY, SelectionleColor);
            rect.IsFilled = true;
            return rect;
        }

        #endregion

    }

}
