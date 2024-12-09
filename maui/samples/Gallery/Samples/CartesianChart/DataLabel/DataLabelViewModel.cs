#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DataLabelViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> RevenueCollection { get; set; }

		public const string DataSource = "1977\t443497461\t2.91\t9\t49277495\tStar Wars: Episode IV -A New Hope\r\n1978\t826413013\t0.86\t13\t63570231\tGrease\r\n1979\t1229428453\t0.49\t40\t30735711\tSuperman\r\n1980\t1642400771\t0.34\t68\t24152952\tStar Wars: Episode V -The Empire Strikes Back\r\n1981\t898686960\t - 0.45\t56\t16047981\tSuperman II\r\n1982\t3001761432\t2.34\t132\t22740616\tE.T.the Extra-Terrestrial\r\n1983\t2738195414\t - 0.09\t149\t18377150\tStar Wars: Episode VI -Return of the Jedi\r\n1984\t3066288000\t0.12\t169\t18143715\tGhostbusters\r\n1985\t3017379123\t - 0.02\t191\t15797796\tBack to the Future\r\n1986\t3065297715\t0.02\t201\t15250237\tTop Gun\r\n1987\t3343230996\t0.09\t226\t14793057\tBeverly Hills Cop II\r\n1988\t3542179944\t0.06\t239\t14820836\tWho Framed Roger Rabbit\r\n1989\t4084060815\t0.15\t235\t17378982\tBatman\r\n1990\t4326824577\t0.06\t236\t18334002\tGhost\r\n1991\t4336073530\t0\t253\t17138630\tTerminator 2: Judgment Day\r\n1992\t4519644026\t0.04\t247\t18298153\tBatman Returns\r\n1993\t4823116106\t0.07\t267\t18064105\tJurassic Park\r\n1994\t5061909583\t0.05\t259\t19544052\tThe Lion King\r\n1995\t5106129264\t0.01\t291\t17546835\tBatman Forever\r\n1996\t5600750973\t0.1\t306\t18303107\tIndependence Day\r\n1997\t6077250085\t0.09\t310\t19604032\tMen in Black\r\n1998\t6696383983\t0.1\t334\t20049053\tTitanic\r\n1999\t7340743186\t0.1\t448\t16385587\tStar Wars: Episode I -The Phantom Menace\r\n2000\t7476224772\t0.02\t439\t17030124\tHow the Grinch Stole Christmas\r\n2001\t7959296828\t0.07\t412\t19318681\tHarry Potter and the Sorcerer's Stone\r\n2002\t9164913438\t0.15\t570\t16078795\tSpider-Man\r\n2003\t9232953228\t0.01\t667\t13842508\tFinding Nemo\r\n2004\t9354636012\t0.01\t700\t13363765\tShrek 2\r\n2005\t8833618311\t-0.06\t676\t13067482\tStar Wars: Episode III - Revenge of the Sith\r\n2006\t9203041941\t0.04\t746\t12336517\tPirates of the Caribbean: Dead Man's Chest\r\n2007\t9679019852\t0.05\t775\t12489057\tSpider-Man 3\r\n2008\t9652648585\t0\t725\t13313998\tThe Dark Knight\r\n2009\t10615886283\t0.1\t646\t16433260\tTransformers: Revenge of the Fallen\r\n2010\t10585388159\t0\t651\t16260196\tAvatar\r\n2011\t10155695359\t-0.04\t731\t13892880\tHarry Potter and the Deathly Hallows: Part 2\r\n2012\t10843641372\t0.07\t807\t13436978\tThe Avengers\r\n2013\t10955524800\t0.01\t826\t13263347\tIron Man 3\r\n2014\t10368861849\t-0.05\t849\t12213029\tGuardians of the Galaxy\r\n2015\t11148780747\t0.08\t845\t13193823\tJurassic World\r\n2016\t11375225455\t0.02\t855\t13304357\tFinding Dory\r\n2017\t11075387520\t-0.03\t854\t12968837\tStar Wars: Episode VIII - The Last Jedi\r\n2018\t11892160011\t0.07\t993\t11975991\tBlack Panther\r\n2019\t11363360889\t-0.04\t910\t12487209\tAvengers: Endgame\r\n2020\t2113846800\t-0.81\t456\t4635628\tBad Boys for Life\r\n2021\t4482808453\t1.12\t440\t10188201\tSpider-Man: No Way Home\r\n2022\t7368971336\t0.64\t496\t14856797\tTop Gun: Maverick\r\n2023\t8906920114\t0.02\t589\t15122105\tBarbie\r\n";
		private static readonly string[] Separator = ["\r\n", "\n"];

		public DataLabelViewModel()
		{
			string[] rows = DataSource.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
			ObservableCollection<ChartDataModel> collection = [];
			RevenueCollection = [];
			foreach (string row in rows)
			{
				string[] columns = row.Split('\t');
				// Create a new DataItem object and populate its properties using the columns
				ChartDataModel item = new ChartDataModel(columns[0], DataLabelViewModel.GetDouble(columns[1]), DataLabelViewModel.GetDouble(columns[2]), DataLabelViewModel.GetDouble(columns[3]), DataLabelViewModel.GetDouble(columns[4]), columns[5]);

#if !WINDOWS && !MACCATALYST

				if (double.TryParse(item.Year, out double value))
				{
					if (value < 2000)
					{
						continue;
					}
				}
#endif
				// Add the item to the collection
				collection.Add(item);
			}

			RevenueCollection = collection;
		}

		static double GetDouble(string text)
		{

			if (double.TryParse(text, out var i))
			{
				return i;
			}

			return i;
		}

	}
}
