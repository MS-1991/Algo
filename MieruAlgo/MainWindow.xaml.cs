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
        int[] array, karray;
        int array_size;
        public int ARRAY_SIZE = 100;
        public double MARGIN_HEIGHT = 0;
        public int DILAYTIME = 10;
		const int INSERTCHENGENUM = 5; //クイックソート　挿入ソート切り替え用

		public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void cmdReset_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
        }
        private void cmdBubble_Click(object sender, RoutedEventArgs e)
        {
            Sort_Initialize();  //ソート前処理
            Bubble_Sort();
        }
        private void cmdmerge_Click(object sender, RoutedEventArgs e)
        {
            Sort_Initialize();  //ソート前処理
            merge_sort(array, ARRAY_SIZE);
        }
        private void cmdBogo_Click(object sender, RoutedEventArgs e)
        {
            Sort_Initialize();  //ソート前処理
            Bogosort();
        }
		private void cmdInsert_Click(object sender, RoutedEventArgs e)
		{
			Sort_Initialize();  //ソート前処理
			InsertSort(array,0,array.Length);
		}
		private void cmdQuick_Click(object sender, RoutedEventArgs e)
		{
			Sort_Initialize();  //ソート前処理
			QuickSort(array, 0, array.Length - 1);
		}

		private void Delay_Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Delay_Time.Text == "" || Delay_Time.Text == "null")
            {
                Delay_Time.Text = "10";
            }
            DILAYTIME = int.Parse(Delay_Time.Text);
        }

        private void Delay_Time_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void Array_Size_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Array_Size.Text == "" || Array_Size.Text == "null" || int.Parse(Array_Size.Text) <= 0)
            {
                Array_Size.Text = "100";
            }
            array_size = int.Parse(Array_Size.Text);
        }
        private void Array_Size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 0-9のみ
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        void Initialize()
        {
            int temp;
            int j = 0;
            DILAYTIME = int.Parse(Delay_Time.Text);
            ARRAY_SIZE = array_size;
            array = new int[ARRAY_SIZE];
            karray = new int[ARRAY_SIZE];
            Random rd = new Random();

            sort_StackPanel.Children.Clear();

            MARGIN_HEIGHT = (sort_StackPanel.Height - 10);

            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                temp = rd.Next(1, ARRAY_SIZE + 1);//1~ARRAY_SIZEの間でランダム
                bool temp_flag = false;
                //重複排除
                for (int k = 0; k < ARRAY_SIZE; k++)
                {
                    if (temp_flag == true)
                    {
                        k = 0;
                        temp_flag = false;
                    }
                    if (karray[k] == temp)
                    {
                        temp = rd.Next(1, ARRAY_SIZE + 1);
                        temp_flag = true;
                    }
                }

                array[i] = temp;
                karray[i] = array[i];

                Rectangle rect = new Rectangle()
                {
                    Fill = white,
                    Width = (sort_StackPanel.Width / ARRAY_SIZE)
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
                        current.Height = array[j] * (sort_StackPanel.Height / ARRAY_SIZE);
                        current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                        current.Fill = white;
                    }
                    catch (Exception) { }
                }
                j++;
            }
        }
        int Sort_Initialize()
        {
            comparison = 0;
            compare_count.Content = "0"; //比較回数のLabel.Contentを0にする

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

        async Task<int> swap(int[] array, int from, int to)
        {
            Rectangle current, current2;
            int temp = array[from];

            current = FindNameRect_nolonger(from);
            current2 = FindNameRect_nolonger(to);

            current.Height = current2.Height;
            current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
            current2.Height = array[from] * (sort_StackPanel.Height / ARRAY_SIZE);
            current2.Margin = new Thickness(0, MARGIN_HEIGHT - current2.Height, 0, 0);

            #region #表示系更新処理
            current.Fill = yellow;
            current2.Fill = red;
            await TaskDelay();
            #endregion

            array[from] = array[to];
            array[to] = temp;

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

        /****************************************************************/
        /*バブルソート
        /****************************************************************/
        async void Bubble_Sort()
        {
            int arr_num;
            int sct_num;

            for (arr_num = 0; arr_num < (ARRAY_SIZE - 1); arr_num++)
            {
                for (sct_num = (ARRAY_SIZE - 1); sct_num > arr_num; sct_num--)
                {
                    #region #表示系更新処理
                    add_comparison();
                    #endregion

                    //左隣りの要素が大きければ入れ替え
                    if (array[sct_num - 1] > array[sct_num])
                    {
                        await swap(array, sct_num, sct_num - 1);
                    }
                }
            }
            sw.Stop();
        }

        /****************************************************************/
        /*マージソート (昇順)
        /****************************************************************/
        void merge_sort(int[] array, int size)
        {
            int[] buf;
           
            buf = new int[ARRAY_SIZE];  // 作業用領域を確保

			// 配列全体を対象にする
			merge_sort_rec(array, 0, size - 1, buf);
        }

		/****************************************************************/
		/*	マージソート(再帰部分)
		/****************************************************************/
		async Task<int> merge_sort_rec(int[] arr, int begin, int end, int[] work)
        {
            // 要素が１つしかなければ終了
            if (begin >= end)
            {
                return 0;
            }

            // ２つのデータ列に分割して、それぞれを再帰的に処理
            int mid = (begin + end) / 2;
            await merge_sort_rec(arr, begin, mid, work);
            await merge_sort_rec(arr, mid + 1, end, work);

            // マージ
            await merge(arr, begin, end, mid, work);
            return 0;
        }

		/****************************************************************/
		/*	マージ
		/****************************************************************/
		async Task<int> merge(int[] arr, int begin, int end, int mid, int[] work)
        {
            Rectangle current;

            // 前半の要素を作業用配列へ
            for (int i = begin; i <= mid; ++i)
            {

                work[i] = arr[i];
                current = FindNameRect_nolonger(i);
                current.Fill = green;
                current.Height = work[i] * (sort_StackPanel.Height / ARRAY_SIZE); ;
                current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                await TaskDelay();

            }

            // 後半の要素を逆順に作業用配列へ
            for (int i = mid + 1, j = end; i <= end; ++i, --j)
            {
                work[i] = arr[j];
                current = FindNameRect_nolonger(i);
                current.Fill = green;
                current.Height = work[i] * (sort_StackPanel.Height / ARRAY_SIZE); ;
                current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                await TaskDelay();
            }

            // 作業用配列の両端から取り出した要素をマージ
            {
                int i = begin;
                int j = end;
                for (int k = begin; k <= end; ++k)
                {
                    current = FindNameRect_nolonger(k);
                    current.Fill = red;
                    add_comparison();
                    // 昇順にソートするので、小さい方の要素を結果の配列へ移す。
                    if (work[i] <= work[j])
                    { // == の場合は先頭を優先すると安定なソートになる
                        arr[k] = work[i];
                        current.Height = array[k] * (sort_StackPanel.Height / ARRAY_SIZE); ;
                        current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                        await TaskDelay();
                        ++i;
                    }
                    else
                    {
                        arr[k] = work[j];
                        current.Height = array[k] * (sort_StackPanel.Height / ARRAY_SIZE); ;
                        current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                        await TaskDelay();
                        --j;
                    }
                    current.Fill = white;
                }
            }
            return 0;
        }

		/****************************************************************/
		/*	ボゴソート
		/****************************************************************/
		async Task<int> Bogosort()
        {
            Rectangle current;
            while (!isSorted(array))
            {
                array = array.OrderBy(i => Guid.NewGuid()).ToArray();

                for (int i = 0; i < array.Length; i++)
                {
                    current = FindNameRect_nolonger(i);
                    current.Height = array[i] * (sort_StackPanel.Height / ARRAY_SIZE); ;
                    current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
                }
                await TaskDelay();
            }
            return 0;
        }

		/****************************************************************/
		/*	ソートチェック
		/****************************************************************/
		private static bool isSorted(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i - 1] > arr[i])
                {
                    return false;
                }
            }
            return true;
        }

		/****************************************************************/
		/*	ソート
		/****************************************************************/
		async Task<int> QuickSort()
		{
			Rectangle current;
			while (!isSorted(array))
			{
				array = array.OrderBy(i => Guid.NewGuid()).ToArray();

				for (int i = 0; i < array.Length; i++)
				{
					current = FindNameRect_nolonger(i);
					current.Height = array[i] * (sort_StackPanel.Height / ARRAY_SIZE); ;
					current.Margin = new Thickness(0, MARGIN_HEIGHT - current.Height, 0, 0);
				}
				await TaskDelay();
			}
			return 0;
		}

		/****************************************************************/
		/*	挿入ソート
		/****************************************************************/
		async Task<int> InsertSort(int[] arr, int first, int last)
		{
			for (int i = first + 1; i <= last; i++)
			{
				for (int j = i; j > first && arr[j - 1].CompareTo(arr[j]) > 0; --j)
				{
					await swap(arr, j, j - 1);
				}
			}
			return 0;
		}


		async Task<int> QuickSort(int[] arr, int first, int last)
		{
			int pivot;

			// 要素数が少なくなってきたら挿入ソートに切り替え
			if (last - first < INSERTCHENGENUM)
			{
				await InsertSort(array, first, last);
				return 0;
			}

			// 枢軸決定（配列の先頭、ど真ん中、末尾の3つの値の中央値を使用。）
			pivot = Median3(arr[first], arr[(first + last) / 2], arr[last]);

			Rectangle current;
			current = FindNameRect_nolonger(pivot);
			current.Fill = green;
			await TaskDelay();

			// 左右分割
			int l = first;
			int r = last;

			while (l <= r)
			{
				while (l < last && arr[l].CompareTo(pivot) < 0) l++;
				while (r > first && arr[r].CompareTo(pivot) >= 0) r--;
				if (l > r) break;
				await swap(arr, l, r);
				l++; r--;
			}

			// 再帰呼び出し
			await QuickSort(arr, first, l - 1);
			await QuickSort(arr, l, last);
			current.Fill = white;
			return 0;
		}

		
		// 3つの値の中央値算出
		private static int Median3(int a, int b, int c)
		{
			int low = a, hight = b;
			if (a > b)
			{
				low = b;
				hight = a;
			}
			if (c <= low) return low;
			if (c >= hight) return hight;
			return c;
		}



		async Task<string> TaskDelay()
        {
            await Task.Delay(DILAYTIME);
            access_count.Content = sw.ElapsedMilliseconds;
            return "";
        }
    }
}