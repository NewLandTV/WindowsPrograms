using System.Text;
using System.Windows;

namespace NewLandCalculator
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringBuilder calculateBuilder = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Input_7(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('7');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_8(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('8');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_9(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('9');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_4(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('4');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_5(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('5');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_6(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('6');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_1(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('1');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_2(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('2');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Input_3(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[0] == '0')
            {
                calculateBuilder.Clear();
            }

            calculateBuilder.Append('3');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[calculateBuilder.Length - 1] != '+' && calculateBuilder[calculateBuilder.Length - 1] >= '0' && calculateBuilder[calculateBuilder.Length - 1] <= '9')
            {
                calculateBuilder.Append('+');

                CalculateResultLabel.Content = calculateBuilder.ToString();
            }
        }

        private void Subtract(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[calculateBuilder.Length - 1] != '-' && calculateBuilder[calculateBuilder.Length - 1] >= '0' && calculateBuilder[calculateBuilder.Length - 1] <= '9')
            {
                calculateBuilder.Append('-');

                CalculateResultLabel.Content = calculateBuilder.ToString();
            }
        }

        private void Multiply(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[calculateBuilder.Length - 1] != '*' && calculateBuilder[calculateBuilder.Length - 1] >= '0' && calculateBuilder[calculateBuilder.Length - 1] <= '9')
            {
                calculateBuilder.Append('*');

                CalculateResultLabel.Content = calculateBuilder.ToString();
            }
        }

        private void Divide(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0 && calculateBuilder[calculateBuilder.Length - 1] != '/' && calculateBuilder[calculateBuilder.Length - 1] >= '0' && calculateBuilder[calculateBuilder.Length - 1] <= '9')
            {
                calculateBuilder.Append('/');

                CalculateResultLabel.Content = calculateBuilder.ToString();
            }
        }

        private void Input_0(object sender, RoutedEventArgs e)
        {
            calculateBuilder.Append('0');

            CalculateResultLabel.Content = calculateBuilder.ToString();
        }

        private void CalculateResult(object sender, RoutedEventArgs e)
        {
            if (calculateBuilder.Length > 0)
            {
                int result = 0;
                char calculateOperator = ' ';

                StringBuilder a = new StringBuilder();
                StringBuilder b = new StringBuilder();

                string s = calculateBuilder.ToString();

                for (int i = 0; !(s[i] == '+' || s[i] == '-' || s[i] == '*' || s[i] == '/'); i++)
                {
                    a.Append(s[i]);
                }

                calculateOperator = s[a.Length];

                for (int i = a.Length + 1; i < s.Length; i++)
                {
                    b.Append(s[i]);
                }

                calculateBuilder.Clear();

                if (int.TryParse(a.ToString(), out int outA))
                {
                    if (int.TryParse(b.ToString(), out int outB))
                    {
                        switch (calculateOperator)
                        {
                            case '+':
                                result = outA + outB;

                                break;
                            case '-':
                                result = outA - outB;

                                break;
                            case '*':
                                result = outA * outB;

                                break;
                            case '/':
                                result = outA / outB;

                                break;
                        }
                    }
                }

                CalculateResultLabel.Content = result;
            }
        }
    }
}
