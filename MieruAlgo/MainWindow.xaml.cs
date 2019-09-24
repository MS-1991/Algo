using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MieruAlgo
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{

		//色定義
		SolidColorBrush green = new SolidColorBrush(Color.FromRgb(0, 255, 0));
		SolidColorBrush red = new SolidColorBrush(Color.FromRgb(255, 0, 0));
		SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(0, 0, 255));
		SolidColorBrush white = new SolidColorBrush(Color.FromRgb(255, 255, 255));
		SolidColorBrush yellow = new SolidColorBrush(Color.FromRgb(255, 255, 0));
		Stopwatch sw = new Stopwatch();

		int comparison;
		int array_access_count;
		int swap_count;
		bool delay_flag;
		int[] array, karray;

		static int ARRAY_SIZE = 100;
		static double MARGIN_HEIGHT = 0;
		static int DILAYTIME = 10;

		public MainWindow()
		{
			InitializeComponent();
			// コンテンツに合わせて自動的にWindow幅と高さをリサイズする
			this.SizeToContent = SizeToContent.WidthAndHeight;
			initialize();
		}

		private void cmdPlay_Click(object sender, RoutedEventArgs e)
		{
			initialize();
		}
		private void cmdStop_Click(object sender, RoutedEventArgs e)
		{
			sort_initialize();  //ソート前処理
			Bubble_Sort();
		}
		private void cmdStop_Click2(object sender, RoutedEventArgs e)
		{
			sort_initialize();  //ソート前処理
			mergesort(ARRAY_SIZE);
		}
		private void HandleCheck(object sender, RoutedEventArgs e)
		{
			delay_flag = true;
		}
		private void HandleUnchecked(object sender, RoutedEventArgs e)
		{
			delay_flag = false;
		}

		private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// 0-9のみ
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
			DILAYTIME = int.Parse(Delay_Time.Text);
		}
		
		private void Array_Size_TextInpu(object sender, TextCompositionEventArgs e)
		{
			// 0-9のみ
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
			ARRAY_SIZE = int.Parse(Delay_Time.Text);

			if (ARRAY_SIZE > sort_StackPanel.Height) {
				ARRAY_SIZE = (int)sort_StackPanel.Height;
			}
		}

		void initialize()
		{
			int temp = 0;
			int j = 0;
			DILAYTIME = int.Parse(Delay_Time.Text);
			ARRAY_SIZE = int.Parse(Array_Size.Text);
			array = new int[ARRAY_SIZE];
			karray = new int[ARRAY_SIZE];
			Random rd = new Random();
			sort_StackPanel.Children.Clear();

			if (isDelay.IsChecked == true) { delay_flag = true; }
			else { delay_flag = false; }

			MARGIN_HEIGHT = (sort_StackPanel.Height - 10);

			for (int i = 0; i < ARRAY_SIZE; i++)
			{
				temp = rd.Next(1, ARRAY_SIZE);//1~ARRAY_SIZEの間でランダム

				//重複排除
				for (int k = 0; k < ARRAY_SIZE; k++)
				{
					if (karray[k] == temp)
					{
						temp = rd.Next(1, ARRAY_SIZE);
						k = 0;
					}
				}

				array[i] = temp;
				karray[i] = array[i];

				Rectangle rect = new Rectangle() {
					Fill = white,
					Width = (sort_StackPanel.Width / ARRAY_SIZE )
				};
				sort_StackPanel.Children.Add(rect);
			}
			
			foreach (Rectangle current in sort_StackPanel.Children) //StackPanelから要素を取り出す
			{
				if (current == null)
				{
					Application.Current.Shutdown();
					return;
				}
				else
				{
					try
					{
						current.Height = array[j];
						current.Margin = new Thickness(0, MARGIN_HEIGHT - array[j], 0, 0);
						current.Fill = white;
					}
					catch (Exception) { }
				}
				j++;
			}
		}
		int sort_initialize()
		{
			swap_count = 0;
			comparison = 0;
			array_access_count = 0;
			compare_count.Content = "0"; //比較回数のLabel.Contentを0にする
			access_count.Content = "0";//アレイアクセスのLabel.Contentを0にする

			foreach (Rectangle r in sort_StackPanel.Children)//StackPanelの中の要素を一つずつ取り出す
			{
				r.Fill = white;//すべてwhiteにする
			}
			sw.Reset();
			sw.Start();
			return 0;
		}
		int add_comparison()
		{
			comparison++;
			compare_count.Content = comparison.ToString();
			return 0;
		}
		int array_access_counter()
		{
			array_access_count++;
			access_count.Content = array_access_count.ToString();
			return 0;
		}

		async Task<int> swap(int[] arr, int from, int to)
		{
			Rectangle current, current2;

			int temp = arr[from];

			current = FindNameRect_nolonger(from);
			current2 = FindNameRect_nolonger(to);

			current.Height = current2.Height;
			current.Margin = new Thickness(0, MARGIN_HEIGHT - current2.Height, 0, 0);
			current2.Height = arr[from];
			current2.Margin = new Thickness(0, MARGIN_HEIGHT - arr[from], 0, 0);

			#region #表示系更新処理
			current.Fill = yellow;
			current2.Fill = red;
			await TaskDelay();
			array_access_counter();
			array_access_counter();
			array_access_counter();
			array_access_counter();
			#endregion

			arr[from] = arr[to];
			arr[to] = temp;

			current.Fill = white;
			current2.Fill = white;
			return 0;
		}

		Rectangle FindNameRect_nolonger(int index)
		{
			Rectangle r = null;
			int cindex = 0;
			foreach (Rectangle rr in sort_StackPanel.Children)
			{
				if (cindex == index)
				{
					r = rr;
					break;
				}
				cindex++;
			}
			return r;
		}

		async void Bubble_Sort()
		{
			Rectangle current;
			int arr_sct_num, bubble_sct_num;
			for (arr_sct_num = 0; arr_sct_num < (ARRAY_SIZE - 1); arr_sct_num++)
			{
				current = FindNameRect_nolonger(arr_sct_num);
				current.Fill = green;
				for (bubble_sct_num = (ARRAY_SIZE - 1); bubble_sct_num > arr_sct_num; bubble_sct_num--)
				{
					#region #表示系更新処理
					array_access_counter(); 
					array_access_counter();
					add_comparison();
					#endregion
					if (array[bubble_sct_num - 1] > array[bubble_sct_num])
					{
						await swap(array, bubble_sct_num, bubble_sct_num - 1);
					}
				}
				current.Fill = white;
			}
			#region #ソート終了処理
			sw.Stop();
			ShowMessage();
			#endregion
		}
		async void mergesort(int num)
		{
			int[] buf;
			sort_initialize(); //ソート前処理
			buf = new int[ARRAY_SIZE]; //バッファ
			Rectangle current; //System.Windows.Shapes
			int rght, rend;
			int i, j, m;

			for (int k = 1; k < num; k *= 2)
			{
				for (int left = 0; left + k < num; left += k * 2)
				{
					rght = left + k;
					rend = rght + k;
					if (rend > num)
					{
						rend = num;
					}

					m = left;
					i = left;
					j = rght;

					while (i < rght && j < rend)
					{

						if (array[i] <= array[j])
						{
							current = FindNameRect_nolonger(i); //i番目のRectangleへの参照を返すメソッド
							current.Fill = red;
							buf[m] = array[i]; i++;
							await TaskDelay();
						}
						else
						{
							current = FindNameRect_nolonger(j);
							current.Fill = red;
							buf[m] = array[j]; j++;
							await TaskDelay();
						}
						#region #表示系更新処理
						array_access_counter();
						array_access_counter(); //配列へのアクセス回数をカウントするメソッド
						array_access_counter();
						add_comparison(); //こっちは比較回数をカウントするメソッド
						current.Fill = white;
						#endregion
						m++;
					}
					while (i < rght)
					{
						current = FindNameRect_nolonger(i);
						#region #表示系更新処理
						current.Fill = red;
						array_access_counter();
						#endregion
						buf[m] = array[i];
						#region #表示系更新処理
						await TaskDelay();
						current.Fill = white;
						#endregion
						i++; m++;
					}
					while (j < rend)
					{
						current = FindNameRect_nolonger(j);
						#region #表示系更新処理
						current.Fill = red;
						array_access_counter();
						#endregion
						buf[m] = array[j];
						#region #表示系更新処理
						await TaskDelay();
						current.Fill = white;
						#endregion
						j++; m++;
					}
					for (m = left; m < rend; m++)
					{
						current = FindNameRect_nolonger(m);
						#region #表示系更新処理
						current.Fill = blue;
						array_access_counter();
						#endregion
						array[m] = buf[m];
						current.Height = buf[m];
						current.Margin = new Thickness(0, MARGIN_HEIGHT - buf[m], 0, 0);
						#region #表示系更新処理
						await TaskDelay();
						current.Fill = white;
						#endregion
					}
				}
			}
			#region #ソート終了処理
			sw.Stop();
			ShowMessage();
			#endregion
		}

		void ShowMessage()
		{
			if (delay_flag == true)
			{
				MessageBox.Show("計算時間：" + (sw.ElapsedMilliseconds - swap_count) + "ミリ秒");
			}
			else
			{
				MessageBox.Show("計算時間：" + sw.ElapsedMilliseconds + "ミリ秒");
			}
		}

		async Task<int> TaskDelay()
		{
			if (delay_flag == true)
			{
				await Task.Delay(DILAYTIME);
				swap_count += DILAYTIME;
			}
			return 0;
		}

		void mark_red(Rectangle current_rect)
		{
			current_rect.Fill = red;
		}
	}
}
