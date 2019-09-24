using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MieruAlgo
{
//	class sorter_algorithms
//	{
//		public MainWindow main_Window;
//		private int comparison;
//		private int array_access;

//		public sorter_algorithms(MainWindow Window)
//		{
//			main_Window = Window;
//		}

//		int sort_initialize()
//		{
//			comparison = 0;
//			array_access = 0;
//			main_Window.comp_tex.Content = "0"; //比較回数のLabel.Contentを0にする
//			main_Window.access_tex.Content = "0";//アレイアクセスのLabel.Contentを0にする
//			foreach (Rectangle r in main_Window.sp.Children)//StackPanelの中の要素を一つずつ取り出す
//			{
//				r.Fill = main_Window.white;//すべてwhiteにする
//			}
//			return 0;
//		}
//		int add_comparison()
//		{
//			comparison++;
//			comp_tex.Content = comparison.ToString();
//			return 0;
//		}
//		int add_array_access()
//		{
//			array_access++;
//			access_tex.Content = array_access.ToString();
//			return 0;
//		}

//		async Task<int> swap(int[] arr, int from, int to)
//		{
//			Rectangle current, current2;

//			add_array_access();
//			int temp = arr[from];

//			current = FindNameRect_nolonger(from);
//			current2 = FindNameRect_nolonger(to);
//			current.Height = current2.Height;
//			current.Margin = new Thickness(0, 447 - current2.Height, 0, 0); //447は高さ合わせ
//			current2.Height = arr[from];
//			current2.Margin = new Thickness(0, 447 - arr[from], 0, 0);
//			mark_red(current);
//			mark_red(current2);
//			await Task.Delay(10); //10ms待機する

//			add_array_access();
//			add_array_access();
//			arr[from] = arr[to];

//			add_array_access();
//			arr[to] = temp;
//			return 0;
//		}

//		Rectangle FindNameRect_nolonger(int index)
//		{
//			Rectangle r = null;
//			int cindex = 0;
//			foreach (Rectangle rr in sp.Children)
//			{
//				if (cindex == index)
//				{
//					r = rr;
//					break;
//				}
//				cindex++;
//			}
//			return r;
//		}
//	}
}
