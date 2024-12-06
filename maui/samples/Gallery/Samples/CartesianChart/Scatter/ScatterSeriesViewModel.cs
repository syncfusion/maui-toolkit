using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class ScatterSeriesViewModel : BaseViewModel
	{
		public ObservableCollection<ChartDataModel> MaleData { get; set; }
		public ObservableCollection<ChartDataModel> FemaleData { get; set; }

		public ScatterSeriesViewModel()
		{
			MaleData =
			[
				new ChartDataModel( 161, 65 ), new ChartDataModel( 150,  65 ), new ChartDataModel(155,  65 ), new ChartDataModel(160, 65 ),
				new ChartDataModel( 148, 66 ), new ChartDataModel( 145,  66 ), new ChartDataModel(137,  66 ), new ChartDataModel(138, 66 ),
				new ChartDataModel( 162, 66 ), new ChartDataModel( 166,  66 ), new ChartDataModel(159,  66 ), new ChartDataModel(151, 66 ),
				new ChartDataModel( 180, 66 ), new ChartDataModel( 181,  66 ), new ChartDataModel(174,  66 ), new ChartDataModel(159, 66 ),
				new ChartDataModel( 151, 67 ), new ChartDataModel( 148,  67 ), new ChartDataModel(141,  67 ), new ChartDataModel(145, 67 ),
				new ChartDataModel( 165, 67 ), new ChartDataModel( 168,  67 ), new ChartDataModel(159,  67 ), new ChartDataModel(183, 67 ),
				new ChartDataModel( 188, 67 ), new ChartDataModel( 187,  67 ), new ChartDataModel(172,  67 ), new ChartDataModel(193, 67 ),
				new ChartDataModel( 153, 68 ), new ChartDataModel( 153,  68 ), new ChartDataModel(147,  68 ), new ChartDataModel(163, 68 ),
				new ChartDataModel( 174, 68 ), new ChartDataModel( 173,  68 ), new ChartDataModel(160,  68 ), new ChartDataModel(191, 68 ),
				new ChartDataModel( 131, 62 ), new ChartDataModel( 140,  62 ), new ChartDataModel(149,  62 ), new ChartDataModel(115, 62 ),
				new ChartDataModel( 164, 63 ), new ChartDataModel( 162,  63 ), new ChartDataModel(167,  63 ), new ChartDataModel(146, 63 ),
				new ChartDataModel( 150, 64 ), new ChartDataModel( 141,  64 ), new ChartDataModel(142,  64 ), new ChartDataModel(129, 64 ),
				new ChartDataModel( 159, 64 ), new ChartDataModel( 158,  64 ), new ChartDataModel(162,  64 ), new ChartDataModel(136, 64 ),
				new ChartDataModel( 176, 64 ), new ChartDataModel( 170,  64 ), new ChartDataModel(167,  64 ), new ChartDataModel(144, 64 ),
				new ChartDataModel( 143, 65 ), new ChartDataModel( 137,  65 ), new ChartDataModel(137,  65 ), new ChartDataModel(140, 65 ),
				new ChartDataModel( 182, 65 ), new ChartDataModel( 168,  65 ), new ChartDataModel(181,  65 ), new ChartDataModel(165, 65 ),
				new ChartDataModel( 214, 74 ), new ChartDataModel( 211,  74 ), new ChartDataModel(166,  74 ), new ChartDataModel(185, 74 ),
				new ChartDataModel( 189, 68 ), new ChartDataModel( 182,  68 ), new ChartDataModel(181,  68 ), new ChartDataModel(196, 68 ),
				new ChartDataModel( 152, 69 ), new ChartDataModel( 173,  69 ), new ChartDataModel(190,  69 ), new ChartDataModel(161, 69 ),
				new ChartDataModel( 173, 69 ), new ChartDataModel( 185,  69 ), new ChartDataModel(141,  69 ), new ChartDataModel(149, 69 ),
				new ChartDataModel( 134, 62 ), new ChartDataModel( 183,  62 ), new ChartDataModel(155,  62 ), new ChartDataModel(164, 62 ),
				new ChartDataModel( 169, 62 ), new ChartDataModel( 122,  62 ), new ChartDataModel(161,  62 ), new ChartDataModel(166, 62 ),
				new ChartDataModel( 137, 63 ), new ChartDataModel( 140,  63 ), new ChartDataModel(140,  63 ), new ChartDataModel(126, 63 ),
				new ChartDataModel( 150, 63 ), new ChartDataModel( 153,  63 ), new ChartDataModel(154,  63 ), new ChartDataModel(139, 63 ),
				new ChartDataModel( 186, 69 ), new ChartDataModel( 188,  69 ), new ChartDataModel(148,  69 ), new ChartDataModel(174, 69 ),
				new ChartDataModel( 164, 70 ), new ChartDataModel( 182,  70 ), new ChartDataModel(200,  70 ), new ChartDataModel(151, 70 ),
				new ChartDataModel( 204, 74 ), new ChartDataModel( 177,  74 ), new ChartDataModel(194,  74 ), new ChartDataModel(212, 74 ),
				new ChartDataModel( 162, 70 ), new ChartDataModel( 200,  70 ), new ChartDataModel(166,  70 ), new ChartDataModel(177, 70 ),
				new ChartDataModel( 188, 70 ), new ChartDataModel( 156,  70 ), new ChartDataModel(175,  70 ), new ChartDataModel(191, 70 ),
				new ChartDataModel( 174, 71 ), new ChartDataModel( 187,  71 ), new ChartDataModel(208,  71 ), new ChartDataModel(166, 71 ),
				new ChartDataModel( 150, 71 ), new ChartDataModel( 194,  71 ), new ChartDataModel(157,  71 ), new ChartDataModel(183, 71 ),
				new ChartDataModel( 204, 71 ), new ChartDataModel( 162,  71 ), new ChartDataModel(179,  71 ), new ChartDataModel(196, 71 ),
				new ChartDataModel( 170, 72 ), new ChartDataModel( 184,  72 ), new ChartDataModel(197,  72 ), new ChartDataModel(162, 72 ),
				new ChartDataModel( 177, 72 ), new ChartDataModel( 203,  72 ), new ChartDataModel(159,  72 ), new ChartDataModel(178, 72 ),
				new ChartDataModel( 198, 72 ), new ChartDataModel( 167,  72 ), new ChartDataModel(184,  72 ), new ChartDataModel(201, 72 ),
				new ChartDataModel( 167, 73 ), new ChartDataModel( 178,  73 ), new ChartDataModel(215,  73 ), new ChartDataModel(207, 73 ),
				new ChartDataModel( 172, 73 ), new ChartDataModel( 204,  73 ), new ChartDataModel(162,  73 ), new ChartDataModel(182, 73 ),
				new ChartDataModel( 201, 73 ), new ChartDataModel( 172,  73 ), new ChartDataModel(189,  73 ), new ChartDataModel(206, 73 ),
				new ChartDataModel( 150, 74 ), new ChartDataModel( 187,  74 ), new ChartDataModel(153,  74 ), new ChartDataModel(171, 74 ),
			];

			FemaleData =
			[
				new ChartDataModel(115, 57 ), new ChartDataModel(138, 57 ), new ChartDataModel(166, 57 ), new ChartDataModel(122,  57 ),
				new ChartDataModel(126, 57 ), new ChartDataModel(130, 57 ), new ChartDataModel(125, 57 ), new ChartDataModel(144,  57 ),
				new ChartDataModel(150, 57 ), new ChartDataModel(120, 57 ), new ChartDataModel(125, 57 ), new ChartDataModel(130,  57 ),
				new ChartDataModel(103, 58 ), new ChartDataModel(116, 58 ), new ChartDataModel(130, 58 ), new ChartDataModel(126,  58 ),
				new ChartDataModel(136, 58 ), new ChartDataModel(148, 58 ), new ChartDataModel(119, 58 ), new ChartDataModel(141,  58 ),
				new ChartDataModel(159, 58 ), new ChartDataModel(120, 58 ), new ChartDataModel(135, 58 ), new ChartDataModel(163,  58 ),
				new ChartDataModel(119, 59 ), new ChartDataModel(131, 59 ), new ChartDataModel(148, 59 ), new ChartDataModel(123,  59 ),
				new ChartDataModel(137, 59 ), new ChartDataModel(149, 59 ), new ChartDataModel(121, 59 ), new ChartDataModel(142,  59 ),
				new ChartDataModel(160, 59 ), new ChartDataModel(118, 59 ), new ChartDataModel(130, 59 ), new ChartDataModel(146,  59 ),
				new ChartDataModel(119, 60 ), new ChartDataModel(133, 60 ), new ChartDataModel(150, 60 ), new ChartDataModel(133,  60 ),
				new ChartDataModel(149, 60 ), new ChartDataModel(165, 60 ), new ChartDataModel(130, 60 ), new ChartDataModel(139,  60 ),
				new ChartDataModel(154, 60 ), new ChartDataModel(118, 60 ), new ChartDataModel(152, 60 ), new ChartDataModel(154,  60 ),
				new ChartDataModel(130, 61 ), new ChartDataModel(145, 61 ), new ChartDataModel(166, 61 ), new ChartDataModel(131,  61 ),
				new ChartDataModel(143, 61 ), new ChartDataModel(162, 61 ), new ChartDataModel(131, 61 ), new ChartDataModel(145,  61 ),
				new ChartDataModel(162, 61 ), new ChartDataModel(115, 61 ), new ChartDataModel(149, 61 ), new ChartDataModel(183,  61 ),
				new ChartDataModel(121, 62 ), new ChartDataModel(139, 62 ), new ChartDataModel(159, 62 ), new ChartDataModel(135,  62 ),
				new ChartDataModel(152, 62 ), new ChartDataModel(178, 62 ), new ChartDataModel(130, 62 ), new ChartDataModel(153,  62 ),
				new ChartDataModel(172, 62 ), new ChartDataModel(114, 62 ), new ChartDataModel(135, 62 ), new ChartDataModel(154,  62 ),
				new ChartDataModel(126, 63 ), new ChartDataModel(141, 63 ), new ChartDataModel(160, 63 ), new ChartDataModel(135,  63 ),
				new ChartDataModel(149, 63 ), new ChartDataModel(180, 63 ), new ChartDataModel(132, 63 ), new ChartDataModel(144,  63 ),
				new ChartDataModel(163, 63 ), new ChartDataModel(122, 63 ), new ChartDataModel(146, 63 ), new ChartDataModel(156,  63 ),
				new ChartDataModel(133, 64 ), new ChartDataModel(150, 64 ), new ChartDataModel(176, 64 ), new ChartDataModel(133,  64 ),
				new ChartDataModel(149, 64 ), new ChartDataModel(176, 64 ), new ChartDataModel(136, 64 ), new ChartDataModel(157,  64 ),
				new ChartDataModel(174, 64 ), new ChartDataModel(131, 64 ), new ChartDataModel(155, 64 ), new ChartDataModel(191,  64 ),
				new ChartDataModel(136, 65 ), new ChartDataModel(149, 65 ), new ChartDataModel(177, 65 ), new ChartDataModel(143,  65 ),
				new ChartDataModel(149, 65 ), new ChartDataModel(184, 65 ), new ChartDataModel(128, 65 ), new ChartDataModel(146,  65 ),
				new ChartDataModel(157, 65 ), new ChartDataModel(133, 65 ), new ChartDataModel(153, 65 ), new ChartDataModel(173,  65 ),
				new ChartDataModel(141, 66 ), new ChartDataModel(156, 66 ), new ChartDataModel(175, 66 ), new ChartDataModel(125,  66 ),
				new ChartDataModel(138, 66 ), new ChartDataModel(165, 66 ), new ChartDataModel(122, 66 ), new ChartDataModel(164,  66 ),
				new ChartDataModel(182, 66 ), new ChartDataModel(137, 66 ), new ChartDataModel(157, 66 ), new ChartDataModel(176,  66 ),
				new ChartDataModel(149, 67 ), new ChartDataModel(159, 67 ), new ChartDataModel(179, 67 ), new ChartDataModel(156,  67 ),
				new ChartDataModel(179, 67 ), new ChartDataModel(186, 67 ), new ChartDataModel(147, 67 ), new ChartDataModel(166,  67 ),
				new ChartDataModel(185, 67 ), new ChartDataModel(140, 67 ), new ChartDataModel(160, 67 ), new ChartDataModel(180,  67 ),
				new ChartDataModel(145, 68 ), new ChartDataModel(155, 68 ), new ChartDataModel(170, 68 ), new ChartDataModel(129,  68 ),
				new ChartDataModel(164, 68 ), new ChartDataModel(189, 68 ), new ChartDataModel(150, 68 ), new ChartDataModel(157,  68 ),
				new ChartDataModel(183, 68 ), new ChartDataModel(144, 68 ), new ChartDataModel(170, 68 ), new ChartDataModel(180,  68 )
			];
		}
	}
}
