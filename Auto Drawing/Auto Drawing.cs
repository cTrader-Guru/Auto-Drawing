/*  CTRADER GURU --> Template 1.0.6 --> https://ctrader.com/algos/indicators/show/2043

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/cTraderGURU/
    TOS         : https://ctrader.guru/termini-del-servizio/

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

        /// <summary>
        /// Definisce una struttura per i tipi di trend
        /// </summary>
        public enum _Trend
        {

            Bearish,
            Bullish,
            Neutral

        }

        /// <summary>
        /// Il punto di riferimento sul grafico
        /// </summary>
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

        /// <summary>
        /// I punti di riferimento sul grafico
        /// </summary>
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

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Auto Drawing";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.4";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/auto-drawing/")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// Il colore degli strumenti disegnati sul grafico in formato stringa
        /// </summary>
        [Parameter("Deviation Period", Group = "Params", DefaultValue = 5, MinValue = 2, Step = 1)]
        public int Deviation { get; set; }

        /// <summary>
        /// Il colore degli strumenti disegnati sul grafico in formato stringa
        /// </summary>
        [Parameter("Extend To Infinity ?", Group = "Styles", DefaultValue = false)]
        public bool EntendToInfinity { get; set; }

        /// <summary>
        /// Il colore degli strumenti disegnati sul grafico in formato stringa
        /// </summary>
        [Parameter("Panel BG Color", Group = "Styles", DefaultValue = MyColors.LightGray)]
        public MyColors PBackgroundColorString { get; set; }

        /// <summary>
        /// Il colore degli strumenti disegnati sul grafico in formato stringa
        /// </summary>
        [Parameter("Drawings Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors DrawingsColorString { get; set; }

        /// <summary>
        /// Il colore della selezione in formato stringa
        /// </summary>
        [Parameter("Selection Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors SelectionleColorString { get; set; }

        /// <summary>
        /// La trasparenza della selezione
        /// </summary>
        [Parameter("Opacity", Group = "Styles", DefaultValue = 50, MinValue = 0, MaxValue = 100, Step = 1)]
        public int Opacity { get; set; }

        #endregion

        #region Property

        /// <summary>
        /// Il colore di fondo della popup
        /// </summary>
        Color PBackgroundColor;

        /// <summary>
        /// Il colore dello strumento disegnato sul grafico
        /// </summary>
        Color DrawingsColor;

        /// <summary>
        /// Il colore del range selezionato sul grafico
        /// </summary>
        Color SelectionleColor;

        /// <summary>
        /// Il rettangolo di selezione, feedback visivo
        /// </summary>
        ChartRectangle SelectRectangle;

        /// <summary>
        /// La prima candela selezionata nel range
        /// </summary>
        int SelectedStartBarIndex;

        /// <summary>
        /// L'ultima candela selezionata nel range
        /// </summary>
        int SelectedEndBarIndex;

        /// <summary>
        /// I livelli standard di Fibonacci
        /// </summary>
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

        /// <summary>
        /// La finestra popup con i comandi
        /// </summary>
        ControlBase DrawingDialog;

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            PBackgroundColor = Color.FromName(PBackgroundColorString.ToString("G"));

            DrawingsColor = Color.FromName(DrawingsColorString.ToString("G"));

            SelectionleColor = Color.FromArgb(Opacity, Color.FromName(SelectionleColorString.ToString("G")));

            // --> Listner per gli eventi del grafico
            Chart.MouseDown += _chart_MouseDown;
            Chart.MouseUp += _chart_MouseUp;
            Chart.MouseMove += _chart_MouseMove;

            _createDrawingDialog();

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            // -->>> Qui calchiamo e assegnamo il valore del nostro indicatore nel buffer
            // --> Result[index] = ...

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Formalizza i tasti per il controllo popup
        /// </summary>
        private void _createDrawingDialog()
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

            var barAverageButton = new API.Button 
            {
                Text = "Bar Average",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var bodyAverageButton = new API.Button 
            {
                Text = "Body Average",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var equiChannelButton = new API.Button 
            {
                Text = "Equidistant Channel",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var linearRegressionButton = new API.Button 
            {
                Text = "Linear Regression",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var fibonacciRetracementButton = new API.Button 
            {
                Text = "Fibonacci Retracement",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var supportTrendLineButton = new API.Button 
            {
                Text = "Support Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var resistanceTrendLineButton = new API.Button 
            {
                Text = "Resistance Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var resistanceSupportTrendLineButton = new API.Button 
            {
                Text = "Rex / Sup Trend",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var supportLevelButton = new API.Button 
            {
                Text = "Support",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var resistanceLevelButton = new API.Button 
            {
                Text = "Resistance",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var removeAllObjButton = new API.Button 
            {
                Text = "Remove all (Shift + Click)",
                HorizontalContentAlignment = API.HorizontalAlignment.Left
            };

            var resistanceSupportLevelButton = new API.Button 
            {
                Text = "Rex / Sup Level",
                HorizontalContentAlignment = API.HorizontalAlignment.Left

            };

            var space = new API.Button 
            {
                Text = separator,
                HorizontalContentAlignment = API.HorizontalAlignment.Center

            };

            var space2 = new API.Button 
            {
                Text = separator,
                HorizontalContentAlignment = API.HorizontalAlignment.Center

            };

            barAverageButton.Click += _barAverageButton_Click;
            bodyAverageButton.Click += _bodyAverageButton_Click;
            equiChannelButton.Click += _equiChannelButton_Click;
            linearRegressionButton.Click += _linearRegressionTo_Click;
            fibonacciRetracementButton.Click += _fibonacciRetracementButton_Click;
            supportTrendLineButton.Click += _supportTrendLineButton_Click;
            resistanceTrendLineButton.Click += _resistanceTrendLineButton_Click;
            resistanceSupportTrendLineButton.Click += _resistanceSupportTrendLineButton_Click;
            supportLevelButton.Click += _supportLevelButton_Click;
            resistanceLevelButton.Click += _resistanceLevelButton_Click;
            resistanceSupportLevelButton.Click += _resistanceSupportLevelButton_Click;
            removeAllObjButton.Click += _removeAllObject_Click;

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

        private void _linearRegressionTo_Click(ButtonClickEventArgs obj)
        {

            DataSeries series = Bars.ClosePrices;

            int PeriodBars = SelectedEndBarIndex - SelectedStartBarIndex;

            double sum_x = 0, sum_x2 = 0, sum_y = 0, sum_xy = 0;

            //int start = SelectedStartBarIndex;
            //int end = SelectedEndBarIndex;
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


            // --> Calcola il massimo e la deviazione standard

            double maxDeviation = 0;
            double sumDevation = 0;

            for (int i = start; i <= end; i++)
            {

                double price = a * i + b;
                maxDeviation = Math.Max(Math.Abs(series[i] - price), maxDeviation);
                sumDevation += Math.Pow(series[i] - price, 2.0);

            }

            double stdDeviation = Math.Sqrt(sumDevation / PeriodBars);

            // --> Periodo nel futuro
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

            string namexx5 = string.Format("Linear Regression DB [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            ChartTrendLine xx5 = Chart.DrawTrendLine("Lineardev-bottom", start, pr1 - stdDeviation, end, pr2 - stdDeviation, DrawingsColor, 1, LineStyle.DotsVeryRare);
            xx5.IsInteractive = false;
            xx5.ExtendToInfinity = EntendToInfinity;

            _closeDrawingDialog();

        }

        private void _resistanceSupportLevelButton_Click(ButtonClickEventArgs obj)
        {

            _resistanceLevelButton_Click(obj);
            _supportLevelButton_Click(obj);
            _closeDrawingDialog();

        }

        private void _resistanceSupportTrendLineButton_Click(ButtonClickEventArgs obj)
        {

            _resistanceTrendLineButton_Click(obj);
            _supportTrendLineButton_Click(obj);
            _closeDrawingDialog();

        }

        private void _removeAllObject_Click(ButtonClickEventArgs obj)
        {

            Chart.RemoveAllObjects();

        }

        private void _fibonacciRetracementButton_Click(ButtonClickEventArgs obj)
        {
            var extremums = _getHighLowInSelection();
            var name = string.Format("Fibonacci Retracement [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var fibo = Chart.DrawFibonacciRetracement(name, point2.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            fibo.IsInteractive = true;
            fibo.DisplayPrices = false;
            _setDefaultFiboLevels(fibo.FibonacciLevels);
            _closeDrawingDialog();
        }

        private void _barAverageButton_Click(ButtonClickEventArgs obj)
        {

            var average = _getBodyAverageInSelection(true);
            MessageBox.Show(string.Format("Bar Average : {0}, for {1} bars", average[0], average[1]), "Bar Average");

        }

        private void _bodyAverageButton_Click(ButtonClickEventArgs obj)
        {

            var average = _getBodyAverageInSelection();
            MessageBox.Show(string.Format("Body Average : {0}, for {1} bars", average[0], average[1]), "Body Average");

        }

        private void _equiChannelButton_Click(ButtonClickEventArgs obj)
        {

            var extremums = _getTwoTopHighExtremumsInSelection();
            //_getFirstLastHighExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Equi Channel [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));

            double distance = (50) * Symbol.PipSize;

            var equi = Chart.DrawEquidistantChannel(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, distance, DrawingsColor);
            equi.IsInteractive = true;
            equi.ExtendToInfinity = EntendToInfinity;

            _closeDrawingDialog();

        }

        private void _setDefaultFiboLevels(IEnumerable<FibonacciLevel> levels)
        {
            foreach (var level in levels)
            {
                level.IsVisible = Array.IndexOf(DefaultFiboLevels, (decimal)level.PercentLevel) > -1;
            }
        }

        private void _resistanceTrendLineButton_Click(ButtonClickEventArgs obj)
        {
            var extremums = _getTwoTopHighExtremumsInSelection();
            //_getFirstLastHighExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Resistance Trend Line [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawTrendLine(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            line.IsInteractive = true;
            line.ExtendToInfinity = EntendToInfinity;
            _closeDrawingDialog();
        }

        private void _supportTrendLineButton_Click(ButtonClickEventArgs obj)
        {

            var extremums = _getTwoBottomLowExtremumsInSelection();
            //_getFirstLastLowExtremumsInSelection();
            var point1 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point1 : extremums.Point2;
            var point2 = extremums.Point1.BarIndex < extremums.Point2.BarIndex ? extremums.Point2 : extremums.Point1;
            var name = string.Format("Support Trend Line [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawTrendLine(name, point1.BarIndex, point1.Price, point2.BarIndex, point2.Price, DrawingsColor);
            line.IsInteractive = true;
            line.ExtendToInfinity = EntendToInfinity;

            _closeDrawingDialog();

        }

        private void _resistanceLevelButton_Click(ButtonClickEventArgs obj)
        {
            var maximum = _getMaximumInSelection();
            var name = string.Format("Resistance Level [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawHorizontalLine(name, maximum.Price, DrawingsColor);
            line.IsInteractive = true;
            _closeDrawingDialog();
        }

        private void _supportLevelButton_Click(ButtonClickEventArgs obj)
        {

            var minimum = _getMinimumInSelection();
            var name = string.Format("Support Level [Auto Drawing] {0}", DateTime.Now.ToString("dd.MM.yy HH:mm:ss.zzz"));
            var line = Chart.DrawHorizontalLine(name, minimum.Price, DrawingsColor);
            line.IsInteractive = true;
            _closeDrawingDialog();

        }

        private ChartPoint _getMaximumInSelection()
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

        private ChartPoint _getMinimumInSelection()
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

        private ChartPoints _getHighLowInSelection()
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

        private ChartPoints _getFirstLastHighExtremumsInSelection()
        {
            var highExtremums = new List<ChartPoint>();
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {
                bool isExtremum = false;
                var currHigh = Bars[i].High;

                if (i == 0 || i == Bars.ClosePrices.Count - 1)
                {
                    isExtremum = true;
                }
                else
                {
                    var nextHigh = Bars[i + 1].High;
                    var prevHigh = Bars[i - 1].High;
                    isExtremum = currHigh > nextHigh && currHigh > prevHigh;
                }

                if (isExtremum)
                    highExtremums.Add(new ChartPoint(i, currHigh));
            }
            if (highExtremums.Count < 2)
                return null;

            return new ChartPoints(highExtremums.First(), highExtremums.Last());
        }

        private ChartPoints _getTwoTopHighExtremumsInSelection()
        {

            int count = 0;
            ChartPoint firstHigh = new ChartPoint(0, 0);
            ChartPoint lastHigh = new ChartPoint(0, 0);

            // --> Partiamo dal primo, la sicurezza non è mai troppa
            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            // -->  Devo sapere la direzione per elaborare correttamente il trend      
            _Trend Direction = (Bars[SelectedStartBarIndex].Low > Bars[SelectedEndBarIndex].Low) ? _Trend.Bearish : _Trend.Bullish;

            if (Direction == _Trend.Bullish)
            {

                // --> Controllo ogni candela e registro i punti che mi interessano
                for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
                {

                    count++;

                    // --> Inizializzo, potrebbe esserci una selezione anomala
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

                // --> Controllo ogni candela e registro i punti che mi interessano
                for (int i = SelectedEndBarIndex; i >= SelectedStartBarIndex; i--)
                {

                    count++;

                    // --> Inizializzo, potrebbe esserci una selezione anomala
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


        private double[] _getBodyAverageInSelection(bool bar = false)
        {

            double total = 0;
            double count = 0;

            // --> Partiamo dal primo, la sicurezza non è mai troppa
            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            // --> Controllo ogni candela e registro i punti che mi interessano
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {

                count++;

                // --> Potrebbe essere una candela rialzista, restituisco sempre un numero positivo
                total += (bar) ? Math.Abs(Bars[i].High - Bars[i].Low) : Math.Abs(Bars[i].Open - Bars[i].Close);

            }

            // --> Restituisco il numero di pips
            return new double[] 
            {
                Math.Round((total / count) / Symbol.PipSize, 2),
                count
            };

        }

        private ChartPoints _getTwoBottomLowExtremumsInSelection()
        {

            int count = 0;
            ChartPoint firstLow = new ChartPoint(0, 0);
            ChartPoint lastLow = new ChartPoint(0, 0);

            // --> Partiamo dal primo, la sicurezza non è mai troppa
            if (SelectedStartBarIndex < 1)
                SelectedStartBarIndex = 1;

            // -->  Devo sapere la direzione per elaborare correttamente il trend      
            _Trend Direction = (Bars[SelectedStartBarIndex].Low > Bars[SelectedEndBarIndex].Low) ? _Trend.Bearish : _Trend.Bullish;

            if (Direction == _Trend.Bearish)
            {

                // --> Controllo ogni candela e registro i punti che mi interessano
                for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
                {

                    count++;

                    // --> Inizializzo, potrebbe esserci una selezione anomala
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

                // --> Controllo ogni candela e registro i punti che mi interessano
                for (int i = SelectedEndBarIndex; i >= SelectedStartBarIndex; i--)
                {

                    count++;

                    // --> Inizializzo, potrebbe esserci una selezione anomala
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

        private ChartPoints _getFirstLastLowExtremumsInSelection()
        {
            var lowExtremums = new List<ChartPoint>();
            for (int i = SelectedStartBarIndex; i <= SelectedEndBarIndex; i++)
            {
                bool isExtremum = false;
                var currLow = Bars[i].Low;

                if (i == 0 || i == Bars.ClosePrices.Count - 1)
                {
                    isExtremum = true;
                }
                else
                {
                    var nextLow = Bars[i + 1].Low;
                    var prevLow = Bars[i - 1].Low;
                    isExtremum = currLow < nextLow && currLow < prevLow;
                }

                if (isExtremum)
                    lowExtremums.Add(new ChartPoint(i, currLow));
            }
            if (lowExtremums.Count < 2)
                return null;

            return new ChartPoints(lowExtremums.First(), lowExtremums.Last());

        }

        private void _chart_MouseMove(ChartMouseEventArgs obj)
        {
            if (SelectRectangle == null)
                return;

            SelectRectangle.Time2 = obj.TimeValue;
        }

        private void _chart_MouseDown(ChartMouseEventArgs obj)
        {

            if (DrawingDialog.IsVisible)
                _closeDrawingDialog();

            if (obj.CtrlKey)
            {

                Chart.IsScrollingEnabled = false;
                SelectRectangle = _createDragRectangle(obj.TimeValue);

            }
            else if (obj.ShiftKey)
            {

                _removeAllObject_Click(null);

            }

        }

        private void _chart_MouseUp(ChartMouseEventArgs obj)
        {
            Chart.IsScrollingEnabled = true;

            if (SelectRectangle != null)
            {
                _setSelectedStartEndIndex(SelectRectangle);
                Chart.RemoveObject(SelectRectangle.Name);
                SelectRectangle = null;

                if (SelectedStartBarIndex >= 0 && SelectedEndBarIndex >= 0)
                {
                    _openDrawingDialog(obj.MouseX, obj.MouseY);
                }
            }
        }

        private void _setSelectedStartEndIndex(ChartRectangle rectangle)
        {
            var index1 = Bars.OpenTimes.GetIndexByTime(rectangle.Time1);
            var index2 = Bars.OpenTimes.GetIndexByTime(rectangle.Time2);
            SelectedStartBarIndex = Math.Min(index1, index2);
            SelectedEndBarIndex = Math.Max(index1, index2);
        }

        private void _openDrawingDialog(double mouseX, double mouseY)
        {
            DrawingDialog.IsVisible = true;
            var left = Chart.Width - mouseX > 160 ? mouseX : mouseX - 160;
            var right = Chart.Height - mouseY > 100 ? mouseY : mouseY - 100;
            DrawingDialog.Margin = new Thickness(left, right, 0, 0);

        }

        private void _closeDrawingDialog()
        {
            DrawingDialog.IsVisible = false;
        }

        private ChartRectangle _createDragRectangle(DateTime time)
        {
            var rect = Chart.DrawRectangle("DragRectangle", time, Chart.TopY, time, Chart.BottomY, SelectionleColor);
            rect.IsFilled = true;
            return rect;
        }

        /// <summary>
        /// In caso di necessità viene utilizzata per stampare dati sul grafico
        /// </summary>
        /// <param name="mex">Il messaggio da visualizzare</param>
        /// <param name="doPrint">Flag se si vuole stampare nei log</param>
        private void _debug(string mex = "...", bool doPrint = true)
        {

            Chart.DrawStaticText(NAME + "Debug", string.Format("{0} : {1}", NAME, mex), VerticalAlignment.Bottom, API.HorizontalAlignment.Right, Color.Red);
            if (doPrint)
                Print(mex);

        }

        #endregion

    }

}
