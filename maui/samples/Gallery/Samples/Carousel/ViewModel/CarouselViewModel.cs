using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
    public partial class CarouselViewModel : INotifyPropertyChanged
    {
        #region private collection variables

        /// <summary>
        /// The carousel collection.
        /// </summary>
        private List<CarouselModel> _carouselCollection = [];

        /// <summary>
        /// The data collection.
        /// </summary>
        private ObservableCollection<CarouselModel> _dataCollection = [];

        /// <summary>
        /// The application collection.
        /// </summary>
        private ObservableCollection<CarouselModel> _applicationCollection = [];

        /// <summary>
        /// The office collection.
        /// </summary>
        private ObservableCollection<CarouselModel> _officeCollection = [];

        /// <summary>
        /// The transport collection.
        /// </summary>
        private ObservableCollection<CarouselModel> _transportCollection = [];

        /// <summary>
        /// The dicItems.
        /// </summary>
        private readonly Dictionary<string, string> _s = [];

        /// <summary>
        /// The color code.
        /// </summary>
        public List<Color> ColorCode = [];

        /// <summary>
        /// The color count.
        /// </summary>
        public int ColorCount = 15;

        /// <summary>
        /// The temp collection.
        /// </summary>
        internal ObservableCollection<CarouselModel> _tempCollection = [];


        #endregion

        #region notify event

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Ons the property change.
        /// </summary>
        /// <param name="property">Property.</param>
        public void OnPropertyChange(string property)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}

        #endregion


        #region public collection variables

        /// <summary>
        /// Gets or sets the carousel collection.
        /// </summary>
        /// <value>The carousel collection.</value>
        public List<CarouselModel> CarouselCollection
        {
            get
            {
                return _carouselCollection;
            }

            set
            {
                _carouselCollection = value;
            }
        }


        /// <summary>
        /// Gets or sets the data collection.
        /// </summary>
        /// <value>The data collection.</value>
        public ObservableCollection<CarouselModel> DataCollection
        {
            get
            {
                return _dataCollection;
            }
            set
            {
                _dataCollection = value;
            }

        }

        /// <summary>
        /// Gets or sets the application collection.
        /// </summary>
        /// <value>The application collection.</value>
        public ObservableCollection<CarouselModel> ApplicationCollection
        {
            get
            {
                return _applicationCollection;
            }
            set
            {
                _applicationCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the office collection.
        /// </summary>
        /// <value>The office collection.</value>
        public ObservableCollection<CarouselModel> OfficeCollection
        {
            get
            {
                return _officeCollection;
            }
            set
            {
                _officeCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the transport collection.
        /// </summary>
        /// <value>The transport collection.</value>
        public ObservableCollection<CarouselModel> TransportCollection
        {
            get
            {
                return _transportCollection;
            }
            set
            {
                _transportCollection = value;
            }
        }



        #endregion


        #region carousel view model

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CarouselViewModel()
        {
            GetColor();
            GetIcon();
            GetImageCollection();
            AddVirutalizationIcons();

        }

        #endregion

        #region pick color
        /// <summary>
        /// Picks the color.
        /// </summary>
        /// <returns>The color.</returns>
        public Color PickColor()
        {
            ColorCount++;
            if ((DeviceInfo.Platform == DevicePlatform.Android) || (DeviceInfo.Platform == DevicePlatform.iOS))
            {
                if (ColorCount >= 16)
				{
					ColorCount = 0;
				}
			}
            else
            {
                if (ColorCount >= 15)
				{
					ColorCount = 0;
				}
			}
            return ColorCode[ColorCount];
        }
        #endregion

        #region get color
        /// <summary>
        /// Gets the color.
        /// </summary>
        public void GetColor()
        {
            ColorCode.Add(Colors.Violet);
            ColorCode.Add(Colors.OrangeRed);
            ColorCode.Add(Colors.Blue);
            ColorCode.Add(Colors.MediumVioletRed);
            ColorCode.Add(Colors.DeepSkyBlue);
            ColorCode.Add(Colors.DarkOrange);
            ColorCode.Add(Colors.PaleVioletRed);
            ColorCode.Add(Colors.LightGreen);
            ColorCode.Add(Colors.Gold);
            ColorCode.Add(Colors.DeepSkyBlue);
            ColorCode.Add(Colors.RoyalBlue);
            ColorCode.Add(Colors.Orange);
            ColorCode.Add(Colors.PaleVioletRed);
            ColorCode.Add(Colors.CornflowerBlue);
            ColorCode.Add(Colors.DeepPink);
            ColorCode.Add(Colors.DarkOliveGreen);
        }
        #endregion

        #region get image collection

        private void GetImageCollection()
        {          
            _carouselCollection.Add(new CarouselModel("austria.jpg","Austria", "Landlocked European country known for its stunning Alpine scenery, classical music heritage, and historic architecture."));
            _carouselCollection.Add(new CarouselModel("india.jpg", "India", "Diverse South Asian nation with a rich tapestry of cultures, languages, and traditions, marked by a long history, vibrant spirituality, and rapid modernization."));
            _carouselCollection.Add(new CarouselModel("dubai.jpg", "Dubai", "Dynamic city in the United Arab Emirates known for its futuristic skyline, opulent shopping malls, luxury lifestyle, and as a global business and tourism hub."));
            _carouselCollection.Add(new CarouselModel("canada.jpg", "Canada", "Vast North American nation celebrated for its stunning natural landscapes, cultural diversity, and reputation for politeness and inclusiveness."));
            _carouselCollection.Add(new CarouselModel("kremlin.jpg", "Russia", "Vast Eurasian nation with a rich cultural heritage, diverse landscapes, and a complex geopolitical history."));
            _carouselCollection.Add(new CarouselModel("colombia.jpg", "Colombia", "South American country with diverse landscapes, rich biodiversity, a complex history marked by both challenges and resilience, and a vibrant cultural scene."));
            _carouselCollection.Add(new CarouselModel("france.jpg", "France", "European nation synonymous with art, fashion, culinary excellence, and a rich cultural heritage, including iconic architectural landmarks such as the Eiffel Tower."));
            _carouselCollection.Add(new CarouselModel("germany.jpg", "Germany", "Central European powerhouse renowned for its engineering prowess, efficiency, and cultural contributions, with a complex history and a key role in the European Union"));
            _carouselCollection.Add(new CarouselModel("italy.jpg", "Italy", "Mediterranean nation celebrated for its art, history, architecture, and cuisine, with a cultural legacy that includes ancient Rome and the European Renaissance."));
        }

        #endregion

        #region get icons collection

        private void AddVirutalizationIcons()
        {
            ObservableCollection<string> applicationicons = CarouselViewModel.GetApplicationIcon();
            ObservableCollection<string> officeicons = CarouselViewModel.GetOfficeIcon();
            ObservableCollection<string> transporticons = CarouselViewModel.GetTransportCollection();

			using Stream stream = (FileSystem.OpenAppPackageFileAsync("CustomSearch.txt")).Result;
			if (stream != null)
			{
				string? text = "";
				using var reader = new System.IO.StreamReader(stream);
				while ((text = reader.ReadLine()) != null)
				{
#if ANDROID
					if (_officeCollection.Count > 20 && _transportCollection.Count > 20)
					{
						break;
					}
#endif
					string[] splits = text.Split('*');

					if (splits.Length == 3)
					{
						try
						{
							if (applicationicons.Contains(splits[0]))
							{
#if ANDROID
								if (_dataCollection.Count > 650)
								{
									continue;
								}
#endif
								CarouselModel model = new CarouselModel
								{
									ItemColor = PickColor(),
									Name = splits[0],
									Unicode = _s[splits[1]],
									Tag = splits[2]
								};
								_dataCollection.Add(model);
								_tempCollection.Add(model);
							}
							else if (officeicons.Contains(splits[0]))
							{
								CarouselModel model = new CarouselModel
								{
									ItemColor = PickColor(),
									Name = splits[0],
									Unicode = _s[splits[1]],
									Tag = splits[2]
								};
								if (_officeCollection.Count < 20)
								{
									_officeCollection.Add(model);
								}
								_dataCollection.Add(model);
								_tempCollection.Add(model);
							}
							else if (transporticons.Contains(splits[0]))
							{
								CarouselModel model = new CarouselModel
								{
									ItemColor = PickColor(),
									Name = splits[0],
									Unicode = _s[splits[1]],
									Tag = splits[2]
								};
								if (_transportCollection.Count < 20)
								{
									_transportCollection.Add(model);
								}
								_dataCollection.Add(model);
								_tempCollection.Add(model);
							}
						}
						catch (Exception e)
						{
							var ee = e.Message;
						}

					}
				}
#if !ANDROID
                        for (int i = 0; i < 4; i++)
                        {
                            foreach (CarouselModel model in _tempCollection)
                            {
                                _dataCollection.Add(model);
                            }
                        }
#endif
			}

		}
#endregion

        /// <summary>
        /// Gets the icon.
        /// </summary>
        private void GetIcon()
        {
            _s.Add("e956", "\U0000E956");
            _s.Add("eb2e", "\U0000EB2E");
            _s.Add("eb27", "\U0000EB27");
            _s.Add("eb2d", "\U0000EB2D");
            _s.Add("ed25", "\U0000ED25");
            _s.Add("eb0b", "\U0000EB0B");
            _s.Add("eb4a", "\U0000EB4A");
            _s.Add("eafe", "\U0000EAFE");
            _s.Add("eb38", "\U0000EB38");
            _s.Add("ed23", "\U0000ED23");
            _s.Add("eb0c", "\U0000EB0C");
            _s.Add("ed27", "\U0000ED27");
            _s.Add("eb56", "\U0000EB56");
            _s.Add("eb48", "\U0000EB48");
            _s.Add("eb36", "\U0000EB36");
            _s.Add("eafd", "\U0000EAFD");
            _s.Add("eb54", "\U0000EB54");
            _s.Add("eaf7", "\U0000EAF7");
            _s.Add("ec67", "\U0000EC67");
            _s.Add("eabf", "\U0000EABF");
            _s.Add("eb4c", "\U0000EB4C");
            _s.Add("ec2b", "\U0000EC2B");
            _s.Add("eaba", "\U0000EABA");
            _s.Add("ec65", "\U0000EC65");
            _s.Add("eb55", "\U0000EB55");
            _s.Add("eb46", "\U0000EB46");
            _s.Add("eb26", "\U0000EB26");
            _s.Add("e9c9", "\U0000E9C9");
            _s.Add("eb44", "\U0000EB44");
            _s.Add("eaf5", "\U0000EAF5");
            _s.Add("eb3d", "\U0000EB3D");
            _s.Add("eb4e", "\U0000EB4E");
            _s.Add("ed24", "\U0000ED24");
            _s.Add("eb28", "\U0000EB28");
            _s.Add("eb3f", "\U0000EB3F");
            _s.Add("eb35", "\U0000EB35");
            _s.Add("eb33", "\U0000EB33");
            _s.Add("eaf6", "\U0000EAF6");
            _s.Add("eb5e", "\U0000EB5E");
            _s.Add("e9ca", "\U0000E9CA");
            _s.Add("eb42", "\U0000EB42");
            _s.Add("eb10", "\U0000EB10");
            _s.Add("ed26", "\U0000ED26");
            _s.Add("ec64", "\U0000EC64");
            _s.Add("eb04", "\U0000EB04");
            _s.Add("eb08", "\U0000EB08");
            _s.Add("eab9", "\U0000EAB9");
            _s.Add("eaff", "\U0000EAFF");
            _s.Add("eb52", "\U0000EB52");
            _s.Add("ec66", "\U0000EC66");
            _s.Add("eaf4", "\U0000EAF4");
            _s.Add("e9ee", "\U0000E9EE");
            _s.Add("eba1", "\U0000EBA1");
            _s.Add("eabb", "\U0000EABB");
            _s.Add("eb50", "\U0000EB50");
            _s.Add("eab3", "\U0000EAB3");
            _s.Add("eab2", "\U0000EAB2");
            _s.Add("eab1", "\U0000EAB1");
            _s.Add("e8ca", "\U0000E8CA");
            _s.Add("e93e", "\U0000E93E");
            _s.Add("eba0", "\U0000EBA0");
            _s.Add("eb39", "\U0000EB39");
            _s.Add("ec2a", "\U0000EC2A");
            _s.Add("e9ed", "\U0000E9ED");
            _s.Add("eb5d", "\U0000EB5D");
            _s.Add("ec38", "\U0000EC38");
            _s.Add("e954", "\U0000E954");
            _s.Add("ed15", "\U0000ED15");
            _s.Add("ed21", "\U0000ED21");
            _s.Add("ec45", "\U0000EC45");
            _s.Add("ed34", "\U0000ED34");
            _s.Add("ed37", "\U0000ED37");
            _s.Add("ebf5", "\U0000EBF5");
            _s.Add("ec4e", "\U0000EC4E");
            _s.Add("ecb9", "\U0000ECB9");
            _s.Add("e952", "\U0000E952");
            _s.Add("ebff", "\U0000EBFF");
            _s.Add("ed0f", "\U0000ED0F");
            _s.Add("ed1c", "\U0000ED1C");
            _s.Add("e955", "\U0000E955");
            _s.Add("ecc5", "\U0000ECC5");
            _s.Add("ecd6", "\U0000ECD6");
            _s.Add("ecec", "\U0000ECEC");
            _s.Add("ecf4", "\U0000ECF4");
            _s.Add("ecc2", "\U0000ECC2");
            _s.Add("e958", "\U0000E958");
            _s.Add("ec47", "\U0000EC47");
            _s.Add("e950", "\U0000E950");
            _s.Add("ed06", "\U0000ED06");
            _s.Add("ecc3", "\U0000ECC3");
            _s.Add("e948", "\U0000E948");
            _s.Add("ece7", "\U0000ECE7");
            _s.Add("ecd7", "\U0000ECD7");
            _s.Add("ed19", "\U0000ED19");
            _s.Add("ed18", "\U0000ED18");
            _s.Add("e949", "\U0000E949");
            _s.Add("ed28", "\U0000ED28");
            _s.Add("ed31", "\U0000ED31");
            _s.Add("ed3f", "\U0000ED3F");
            _s.Add("ed0a", "\U0000ED0A");
            _s.Add("e94b", "\U0000E94B");
            _s.Add("e98a", "\U0000E98A");
            _s.Add("e94a", "\U0000E94A");
            _s.Add("ecc4", "\U0000ECC4");
            _s.Add("ed13", "\U0000ED13");
            _s.Add("ec54", "\U0000EC54");
            _s.Add("ecc6", "\U0000ECC6");
            _s.Add("ed1e", "\U0000ED1E");
            _s.Add("ecd1", "\U0000ECD1");
            _s.Add("e986", "\U0000E986");
            _s.Add("ecd9", "\U0000ECD9");
            _s.Add("e953", "\U0000E953");
            _s.Add("ed08", "\U0000ED08");
            _s.Add("e9dd", "\U0000E9DD");
            _s.Add("ed0c", "\U0000ED0C");
            _s.Add("e94e", "\U0000E94E");
            _s.Add("e94f", "\U0000E94F");
            _s.Add("ecf2", "\U0000ECF2");
            _s.Add("ecdf", "\U0000ECDF");
            _s.Add("ecc7", "\U0000ECC7");
            _s.Add("e95a", "\U0000E95A");
            _s.Add("ecea", "\U0000ECEA");
            _s.Add("ecbf", "\U0000ECBF");
            _s.Add("ecb8", "\U0000ECB8");
            _s.Add("e95b", "\U0000E95B");
            _s.Add("eafa", "\U0000EAFA");
            _s.Add("ed40", "\U0000ED40");
            _s.Add("ecc1", "\U0000ECC1");
            _s.Add("ed11", "\U0000ED11");
            _s.Add("e959", "\U0000E959");
            _s.Add("ed07", "\U0000ED07");
            _s.Add("e957", "\U0000E957");
            _s.Add("ed10", "\U0000ED10");
            _s.Add("ed1d", "\U0000ED1D");
            _s.Add("ed0e", "\U0000ED0E");
            _s.Add("ecda", "\U0000ECDA");
            _s.Add("eceb", "\U0000ECEB");
            _s.Add("ece0", "\U0000ECE0");
            _s.Add("ecee", "\U0000ECEE");
            _s.Add("ecba", "\U0000ECBA");
            _s.Add("ecdd", "\U0000ECDD");
            _s.Add("ed12", "\U0000ED12");
            _s.Add("e94c", "\U0000E94C");
            _s.Add("ed32", "\U0000ED32");
            _s.Add("ec41", "\U0000EC41");
            _s.Add("ed43", "\U0000ED43");
            _s.Add("eced", "\U0000ECED");
            _s.Add("ecde", "\U0000ECDE");
            _s.Add("e951", "\U0000E951");
            _s.Add("ed46", "\U0000ED46");
            _s.Add("ed0d", "\U0000ED0D");
            _s.Add("ecdb", "\U0000ECDB");
            _s.Add("ed16", "\U0000ED16");
            _s.Add("ed14", "\U0000ED14");
            _s.Add("e9c3", "\U0000E9C3");
            _s.Add("ecdc", "\U0000ECDC");
            _s.Add("ec46", "\U0000EC46");
            _s.Add("e94d", "\U0000E94D");
            _s.Add("ed1f", "\U0000ED1F");
            _s.Add("ec53", "\U0000EC53");
            _s.Add("e947", "\U0000E947");
            _s.Add("ec21", "\U0000EC21");
            _s.Add("ec91", "\U0000EC91");
            _s.Add("ec8e", "\U0000EC8E");
            _s.Add("ec78", "\U0000EC78");
            _s.Add("eca5", "\U0000ECA5");
            _s.Add("ecb7", "\U0000ECB7");
            _s.Add("ecb0", "\U0000ECB0");
            _s.Add("ec24", "\U0000EC24");
            _s.Add("ec3f", "\U0000EC3F");
            _s.Add("eca1", "\U0000ECA1");
            _s.Add("ec84", "\U0000EC84");
            _s.Add("ebfe", "\U0000EBFE");
            _s.Add("ec87", "\U0000EC87");
            _s.Add("ec7c", "\U0000EC7C");
            _s.Add("ecad", "\U0000ECAD");
            _s.Add("ec9e", "\U0000EC9E");
            _s.Add("ec89", "\U0000EC89");
            _s.Add("ec9b", "\U0000EC9B");
            _s.Add("eca6", "\U0000ECA6");
            _s.Add("ec7e", "\U0000EC7E");
            _s.Add("ec8a", "\U0000EC8A");
            _s.Add("ecab", "\U0000ECAB");
            _s.Add("ec9f", "\U0000EC9F");
            _s.Add("ec80", "\U0000EC80");
            _s.Add("ec75", "\U0000EC75");
            _s.Add("ec7f", "\U0000EC7F");
            _s.Add("ec82", "\U0000EC82");
            _s.Add("ecb1", "\U0000ECB1");
            _s.Add("ecaa", "\U0000ECAA");
            _s.Add("ec26", "\U0000EC26");
            _s.Add("ec7b", "\U0000EC7B");
            _s.Add("eca4", "\U0000ECA4");
            _s.Add("ec86", "\U0000EC86");
            _s.Add("ec8c", "\U0000EC8C");
            _s.Add("ec1f", "\U0000EC1F");
            _s.Add("ec8f", "\U0000EC8F");
            _s.Add("ec20", "\U0000EC20");
            _s.Add("ec9c", "\U0000EC9C");
            _s.Add("eca2", "\U0000ECA2");
            _s.Add("ecae", "\U0000ECAE");
            _s.Add("ec1e", "\U0000EC1E");
            _s.Add("ec3e", "\U0000EC3E");
            _s.Add("ec76", "\U0000EC76");
            _s.Add("ec98", "\U0000EC98");
            _s.Add("ebb1", "\U0000EBB1");
            _s.Add("ebdc", "\U0000EBDC");
            _s.Add("ebac", "\U0000EBAC");
            _s.Add("ec6e", "\U0000EC6E");
            _s.Add("eb23", "\U0000EB23");
            _s.Add("eb67", "\U0000EB67");
            _s.Add("ebc3", "\U0000EBC3");
            _s.Add("ebf3", "\U0000EBF3");
            _s.Add("eb99", "\U0000EB99");
            _s.Add("ebd0", "\U0000EBD0");
            _s.Add("ebb0", "\U0000EBB0");
            _s.Add("ebb4", "\U0000EBB4");
            _s.Add("eb6e", "\U0000EB6E");
            _s.Add("ebb7", "\U0000EBB7");
            _s.Add("ebb3", "\U0000EBB3");
            _s.Add("eb66", "\U0000EB66");
            _s.Add("eb61", "\U0000EB61");
            _s.Add("eb7f", "\U0000EB7F");
            _s.Add("ebee", "\U0000EBEE");
            _s.Add("ebe8", "\U0000EBE8");
            _s.Add("eb74", "\U0000EB74");
            _s.Add("eb98", "\U0000EB98");
            _s.Add("ebae", "\U0000EBAE");
            _s.Add("ec07", "\U0000EC07");
            _s.Add("eb77", "\U0000EB77");
            _s.Add("eb59", "\U0000EB59");
            _s.Add("ebef", "\U0000EBEF");
            _s.Add("ebad", "\U0000EBAD");
            _s.Add("ebb5", "\U0000EBB5");
            _s.Add("ec05", "\U0000EC05");
            _s.Add("ec23", "\U0000EC23");
            _s.Add("ebe7", "\U0000EBE7");
            _s.Add("ec6d", "\U0000EC6D");
            _s.Add("eb9c", "\U0000EB9C");
            _s.Add("eb8c", "\U0000EB8C");
            _s.Add("eb68", "\U0000EB68");
            _s.Add("eb6a", "\U0000EB6A");
            _s.Add("eb8e", "\U0000EB8E");
            _s.Add("eb5f", "\U0000EB5F");
            _s.Add("eb24", "\U0000EB24");
            _s.Add("ebd5", "\U0000EBD5");
            _s.Add("eb65", "\U0000EB65");
            _s.Add("eb9b", "\U0000EB9B");
            _s.Add("eb64", "\U0000EB64");
            _s.Add("ebd4", "\U0000EBD4");
            _s.Add("eb6b", "\U0000EB6B");
            _s.Add("ebe5", "\U0000EBE5");
            _s.Add("ebaa", "\U0000EBAA");
            _s.Add("ebe3", "\U0000EBE3");
            _s.Add("eb25", "\U0000EB25");
            _s.Add("eba6", "\U0000EBA6");
            _s.Add("eb5c", "\U0000EB5C");
            _s.Add("ec0b", "\U0000EC0B");
            _s.Add("eba4", "\U0000EBA4");
            _s.Add("eb75", "\U0000EB75");
            _s.Add("eba8", "\U0000EBA8");
            _s.Add("eb58", "\U0000EB58");
            _s.Add("ebc1", "\U0000EBC1");
            _s.Add("eb69", "\U0000EB69");
            _s.Add("ebe1", "\U0000EBE1");
            _s.Add("eb9a", "\U0000EB9A");
            _s.Add("ec1d", "\U0000EC1D");
            _s.Add("eca8", "\U0000ECA8");
            _s.Add("ec39", "\U0000EC39");
            _s.Add("ec79", "\U0000EC79");
            _s.Add("ec92", "\U0000EC92");
            _s.Add("ec94", "\U0000EC94");
            _s.Add("ec95", "\U0000EC95");
            _s.Add("ec56", "\U0000EC56");
            _s.Add("ec8d", "\U0000EC8D");
            _s.Add("ec0d", "\U0000EC0D");
            _s.Add("ec3b", "\U0000EC3B");
            _s.Add("ec97", "\U0000EC97");
            _s.Add("ec99", "\U0000EC99");
            _s.Add("e7c3", "\U0000E7C3");
            _s.Add("e7d5", "\U0000E7D5");
            _s.Add("e703", "\U0000E703");
            _s.Add("e8ba", "\U0000E8BA");
            _s.Add("ea86", "\U0000EA86");
            _s.Add("e8b9", "\U0000E8B9");
            _s.Add("e74a", "\U0000E74A");
            _s.Add("ec68", "\U0000EC68");
            _s.Add("e7ab", "\U0000E7AB");
            _s.Add("e8e4", "\U0000E8E4");
            _s.Add("e93f", "\U0000E93F");
            _s.Add("ea70", "\U0000EA70");
            _s.Add("e73d", "\U0000E73D");
            _s.Add("e88e", "\U0000E88E");
            _s.Add("eae9", "\U0000EAE9");
            _s.Add("e8cc", "\U0000E8CC");
            _s.Add("e746", "\U0000E746");
            _s.Add("e81a", "\U0000E81A");
            _s.Add("e909", "\U0000E909");
            _s.Add("e994", "\U0000E994");
            _s.Add("e81c", "\U0000E81C");
            _s.Add("ea42", "\U0000EA42");
            _s.Add("e801", "\U0000E801");
            _s.Add("e883", "\U0000E883");
            _s.Add("eae8", "\U0000EAE8");
            _s.Add("ea7e", "\U0000EA7E");
            _s.Add("e803", "\U0000E803");
            _s.Add("ea53", "\U0000EA53");
            _s.Add("eab4", "\U0000EAB4");
            _s.Add("eaef", "\U0000EAEF");
            _s.Add("e735", "\U0000E735");
            _s.Add("e755", "\U0000E755");
            _s.Add("e884", "\U0000E884");
            _s.Add("ea00", "\U0000EA00");
            _s.Add("e7b0", "\U0000E7B0");
            _s.Add("e778", "\U0000E778");
            _s.Add("e740", "\U0000E740");
            _s.Add("e887", "\U0000E887");
            _s.Add("e9ff", "\U0000E9FF");
            _s.Add("e9ec", "\U0000E9EC");
            _s.Add("e8b6", "\U0000E8B6");
            _s.Add("e7eb", "\U0000E7EB");
            _s.Add("e98f", "\U0000E98F");
            _s.Add("e857", "\U0000E857");
            _s.Add("eaa5", "\U0000EAA5");
            _s.Add("e8df", "\U0000E8DF");
            _s.Add("ec69", "\U0000EC69");
            _s.Add("e76e", "\U0000E76E");
            _s.Add("e99d", "\U0000E99D");
            _s.Add("e734", "\U0000E734");
            _s.Add("e7ed", "\U0000E7ED");
            _s.Add("e7cc", "\U0000E7CC");
            _s.Add("ea91", "\U0000EA91");
            _s.Add("e8d5", "\U0000E8D5");
            _s.Add("ead0", "\U0000EAD0");
            _s.Add("eaa7", "\U0000EAA7");
            _s.Add("eae1", "\U0000EAE1");
            _s.Add("e862", "\U0000E862");
            _s.Add("e802", "\U0000E802");
            _s.Add("e88b", "\U0000E88B");
            _s.Add("e7a7", "\U0000E7A7");
            _s.Add("e736", "\U0000E736");
            _s.Add("e8e7", "\U0000E8E7");
            _s.Add("e763", "\U0000E763");
            _s.Add("ea6d", "\U0000EA6D");
            _s.Add("e864", "\U0000E864");
            _s.Add("e7cf", "\U0000E7CF");
            _s.Add("e8e3", "\U0000E8E3");
            _s.Add("e7d2", "\U0000E7D2");
            _s.Add("e9a0", "\U0000E9A0");
            _s.Add("ea6f", "\U0000EA6F");
            _s.Add("e88a", "\U0000E88A");
            _s.Add("e9c0", "\U0000E9C0");
            _s.Add("e888", "\U0000E888");
            _s.Add("e81d", "\U0000E81D");
            _s.Add("e89c", "\U0000E89C");
            _s.Add("e914", "\U0000E914");
            _s.Add("e80c", "\U0000E80C");
            _s.Add("ea34", "\U0000EA34");
            _s.Add("eae3", "\U0000EAE3");
            _s.Add("e9fe", "\U0000E9FE");
            _s.Add("e8e0", "\U0000E8E0");
            _s.Add("e9b7", "\U0000E9B7");
            _s.Add("ea90", "\U0000EA90");
            _s.Add("e744", "\U0000E744");
            _s.Add("e859", "\U0000E859");
            _s.Add("e872", "\U0000E872");
            _s.Add("e928", "\U0000E928");
            _s.Add("e839", "\U0000E839");
            _s.Add("e7be", "\U0000E7BE");
            _s.Add("e818", "\U0000E818");
            _s.Add("e7a5", "\U0000E7A5");
            _s.Add("e7fd", "\U0000E7FD");
            _s.Add("e9b8", "\U0000E9B8");
            _s.Add("e7d4", "\U0000E7D4");
            _s.Add("e8ad", "\U0000E8AD");
            _s.Add("e88c", "\U0000E88C");
            _s.Add("e7b7", "\U0000E7B7");
            _s.Add("ea27", "\U0000EA27");
            _s.Add("e7c9", "\U0000E7C9");
            _s.Add("e8c1", "\U0000E8C1");
            _s.Add("e776", "\U0000E776");
            _s.Add("ea1d", "\U0000EA1D");
            _s.Add("e9b4", "\U0000E9B4");
            _s.Add("e8c6", "\U0000E8C6");
            _s.Add("ead9", "\U0000EAD9");
            _s.Add("e99a", "\U0000E99A");
            _s.Add("e910", "\U0000E910");
            _s.Add("ea1a", "\U0000EA1A");
            _s.Add("ea5a", "\U0000EA5A");
            _s.Add("e87f", "\U0000E87F");
            _s.Add("e847", "\U0000E847");
            _s.Add("e9e5", "\U0000E9E5");
            _s.Add("e700", "\U0000E700");
            _s.Add("ea18", "\U0000EA18");
            _s.Add("e81b", "\U0000E81B");
            _s.Add("e774", "\U0000E774");
            _s.Add("e990", "\U0000E990");
            _s.Add("e73c", "\U0000E73C");
            _s.Add("eaca", "\U0000EACA");
            _s.Add("eac2", "\U0000EAC2");
            _s.Add("e71e", "\U0000E71E");
            _s.Add("e729", "\U0000E729");
            _s.Add("e757", "\U0000E757");
            _s.Add("eaa6", "\U0000EAA6");
            _s.Add("e7d6", "\U0000E7D6");
            _s.Add("ebbc", "\U0000EBBC");
            _s.Add("e8e8", "\U0000E8E8");
            _s.Add("e9ad", "\U0000E9AD");
            _s.Add("e768", "\U0000E768");
            _s.Add("e7d0", "\U0000E7D0");
            _s.Add("e7c8", "\U0000E7C8");
            _s.Add("e8ed", "\U0000E8ED");
            _s.Add("ea56", "\U0000EA56");
            _s.Add("e9a9", "\U0000E9A9");
            _s.Add("e8c7", "\U0000E8C7");
            _s.Add("ec29", "\U0000EC29");
            _s.Add("eae7", "\U0000EAE7");
            _s.Add("ea44", "\U0000EA44");
            _s.Add("e8ec", "\U0000E8EC");
            _s.Add("eb7e", "\U0000EB7E");
            _s.Add("e796", "\U0000E796");
            _s.Add("e9af", "\U0000E9AF");
            _s.Add("ea3a", "\U0000EA3A");
            _s.Add("e8ea", "\U0000E8EA");
            _s.Add("e9bf", "\U0000E9BF");
            _s.Add("e8e5", "\U0000E8E5");
            _s.Add("e8ac", "\U0000E8AC");
            _s.Add("e76a", "\U0000E76A");
            _s.Add("e7f0", "\U0000E7F0");
            _s.Add("e82f", "\U0000E82F");
            _s.Add("ea38", "\U0000EA38");
            _s.Add("ea3f", "\U0000EA3F");
            _s.Add("eaf8", "\U0000EAF8");
            _s.Add("e885", "\U0000E885");
            _s.Add("e91e", "\U0000E91E");
            _s.Add("eaee", "\U0000EAEE");
            _s.Add("e7f6", "\U0000E7F6");
            _s.Add("e817", "\U0000E817");
            _s.Add("e9c1", "\U0000E9C1");
            _s.Add("ea23", "\U0000EA23");
            _s.Add("eac8", "\U0000EAC8");
            _s.Add("e9b6", "\U0000E9B6");
            _s.Add("e83c", "\U0000E83C");
            _s.Add("eac6", "\U0000EAC6");
            _s.Add("ea94", "\U0000EA94");
            _s.Add("e924", "\U0000E924");
            _s.Add("e723", "\U0000E723");
            _s.Add("e804", "\U0000E804");
            _s.Add("e74e", "\U0000E74E");
            _s.Add("ea97", "\U0000EA97");
            _s.Add("e91c", "\U0000E91C");
            _s.Add("e84c", "\U0000E84C");
            _s.Add("ea71", "\U0000EA71");
            _s.Add("ea22", "\U0000EA22");
            _s.Add("e77a", "\U0000E77A");
            _s.Add("e9bd", "\U0000E9BD");
            _s.Add("ea24", "\U0000EA24");
            _s.Add("ea4b", "\U0000EA4B");
            _s.Add("e881", "\U0000E881");
            _s.Add("ea46", "\U0000EA46");
            _s.Add("e8eb", "\U0000E8EB");
            _s.Add("e766", "\U0000E766");
            _s.Add("eaa8", "\U0000EAA8");
            _s.Add("e92e", "\U0000E92E");
            _s.Add("e8a4", "\U0000E8A4");
            _s.Add("e8c9", "\U0000E8C9");
            _s.Add("eacd", "\U0000EACD");
            _s.Add("e7f5", "\U0000E7F5");
            _s.Add("eac4", "\U0000EAC4");
            _s.Add("eaf0", "\U0000EAF0");
            _s.Add("e83b", "\U0000E83B");
            _s.Add("e772", "\U0000E772");
            _s.Add("e7b4", "\U0000E7B4");
            _s.Add("ea60", "\U0000EA60");
            _s.Add("e7ae", "\U0000E7AE");
            _s.Add("e75a", "\U0000E75A");
            _s.Add("e732", "\U0000E732");
            _s.Add("e91a", "\U0000E91A");
            _s.Add("e8ab", "\U0000E8AB");
            _s.Add("e8e2", "\U0000E8E2");
            _s.Add("ea3c", "\U0000EA3C");
            _s.Add("e798", "\U0000E798");
            _s.Add("e7c0", "\U0000E7C0");
            _s.Add("e8e9", "\U0000E8E9");
            _s.Add("ea8f", "\U0000EA8F");
            _s.Add("e7d3", "\U0000E7D3");
            _s.Add("e81f", "\U0000E81F");
            _s.Add("eae5", "\U0000EAE5");
            _s.Add("e899", "\U0000E899");
            _s.Add("e8de", "\U0000E8DE");
            _s.Add("e9c2", "\U0000E9C2");
            _s.Add("e93d", "\U0000E93D");
            _s.Add("eaed", "\U0000EAED");
            _s.Add("e762", "\U0000E762");
            _s.Add("e8ce", "\U0000E8CE");
            _s.Add("e815", "\U0000E815");
            _s.Add("e75d", "\U0000E75D");
            _s.Add("e748", "\U0000E748");
            _s.Add("e81e", "\U0000E81E");
            _s.Add("e713", "\U0000E713");
            _s.Add("e84b", "\U0000E84B");
            _s.Add("e786", "\U0000E786");
            _s.Add("e8dc", "\U0000E8DC");
            _s.Add("e877", "\U0000E877");
            _s.Add("e790", "\U0000E790");
            _s.Add("eb1f", "\U0000EB1F");
            _s.Add("e7c7", "\U0000E7C7");
            _s.Add("e779", "\U0000E779");
            _s.Add("e75e", "\U0000E75E");
            _s.Add("ea0b", "\U0000EA0B");
            _s.Add("e7f4", "\U0000E7F4");
            _s.Add("e7a9", "\U0000E7A9");
            _s.Add("e7ac", "\U0000E7AC");
            _s.Add("e85b", "\U0000E85B");
            _s.Add("ea1b", "\U0000EA1B");
            _s.Add("ea05", "\U0000EA05");
            _s.Add("e747", "\U0000E747");
            _s.Add("ea03", "\U0000EA03");
            _s.Add("ea04", "\U0000EA04");
            _s.Add("ea01", "\U0000EA01");
            _s.Add("ea02", "\U0000EA02");
            _s.Add("e84f", "\U0000E84F");
            _s.Add("ea3e", "\U0000EA3E");
            _s.Add("eafc", "\U0000EAFC");
            _s.Add("ea5d", "\U0000EA5D");
            _s.Add("e9cd", "\U0000E9CD");
            _s.Add("e9ce", "\U0000E9CE");
            _s.Add("eb90", "\U0000EB90");
            _s.Add("e76c", "\U0000E76C");
            _s.Add("ea5c", "\U0000EA5C");
            _s.Add("ea4e", "\U0000EA4E");
            _s.Add("e9bc", "\U0000E9BC");
            _s.Add("e725", "\U0000E725");
            _s.Add("e9e6", "\U0000E9E6");
            _s.Add("e8e1", "\U0000E8E1");
            _s.Add("e7ce", "\U0000E7CE");
            _s.Add("e7bc", "\U0000E7BC");
            _s.Add("e897", "\U0000E897");
            _s.Add("e826", "\U0000E826");
            _s.Add("ea61", "\U0000EA61");
            _s.Add("e7c6", "\U0000E7C6");
            _s.Add("e75f", "\U0000E75F");
            _s.Add("ea8d", "\U0000EA8D");
            _s.Add("eaa4", "\U0000EAA4");
            _s.Add("e7b9", "\U0000E7B9");
            _s.Add("e78e", "\U0000E78E");
            _s.Add("e9ea", "\U0000E9EA");
            _s.Add("e8b1", "\U0000E8B1");
            _s.Add("e9df", "\U0000E9DF");
            _s.Add("ea1e", "\U0000EA1E");
            _s.Add("e9cf", "\U0000E9CF");
            _s.Add("ea06", "\U0000EA06");
            _s.Add("e890", "\U0000E890");
            _s.Add("e814", "\U0000E814");
            _s.Add("e7b2", "\U0000E7B2");
            _s.Add("e787", "\U0000E787");
            _s.Add("e9f0", "\U0000E9F0");
            _s.Add("e92c", "\U0000E92C");
            _s.Add("e880", "\U0000E880");
            _s.Add("e77b", "\U0000E77B");
            _s.Add("eacf", "\U0000EACF");
            _s.Add("ea76", "\U0000EA76");
            _s.Add("e752", "\U0000E752");
            _s.Add("e90f", "\U0000E90F");
            _s.Add("e835", "\U0000E835");
            _s.Add("e90b", "\U0000E90B");
            _s.Add("e770", "\U0000E770");
            _s.Add("e90d", "\U0000E90D");
            _s.Add("e7ba", "\U0000E7BA");
            _s.Add("e848", "\U0000E848");
            _s.Add("e92a", "\U0000E92A");
            _s.Add("e761", "\U0000E761");
            _s.Add("e8e6", "\U0000E8E6");
            _s.Add("e79e", "\U0000E79E");
            _s.Add("e72c", "\U0000E72C");
            _s.Add("ea0c", "\U0000EA0C");
            _s.Add("ea82", "\U0000EA82");
            _s.Add("ea19", "\U0000EA19");
            _s.Add("ec22", "\U0000EC22");
            _s.Add("ea5e", "\U0000EA5E");
            _s.Add("e8dd", "\U0000E8DD");
            _s.Add("e9a1", "\U0000E9A1");
            _s.Add("e7d1", "\U0000E7D1");
            _s.Add("ea58", "\U0000EA58");
            _s.Add("e753", "\U0000E753");
            _s.Add("e87c", "\U0000E87C");
            _s.Add("e819", "\U0000E819");
            _s.Add("e85e", "\U0000E85E");
            _s.Add("ea1f", "\U0000EA1F");
            _s.Add("eadf", "\U0000EADF");
            _s.Add("e882", "\U0000E882");
            _s.Add("ead5", "\U0000EAD5");
            _s.Add("e940", "\U0000E940");
            _s.Add("e820", "\U0000E820");
            _s.Add("ea5f", "\U0000EA5F");
            _s.Add("e8cf", "\U0000E8CF");
            _s.Add("ead7", "\U0000EAD7");
            _s.Add("e9ae", "\U0000E9AE");
            _s.Add("e8db", "\U0000E8DB");
            _s.Add("e706", "\U0000E706");
            _s.Add("eadd", "\U0000EADD");
            _s.Add("eaad", "\U0000EAAD");
            _s.Add("ead3", "\U0000EAD3");
            _s.Add("e920", "\U0000E920");
            _s.Add("e88f", "\U0000E88F");
            _s.Add("ea6e", "\U0000EA6E");
            _s.Add("e7ef", "\U0000E7EF");
            _s.Add("ea66", "\U0000EA66");
            _s.Add("e944", "\U0000E944");
            _s.Add("e8d7", "\U0000E8D7");
            _s.Add("e8d3", "\U0000E8D3");
            _s.Add("e7f9", "\U0000E7F9");
            _s.Add("e889", "\U0000E889");
            _s.Add("e886", "\U0000E886");
            _s.Add("e721", "\U0000E721");
            _s.Add("e922", "\U0000E922");
            _s.Add("ea7d", "\U0000EA7D");
            _s.Add("e849", "\U0000E849");
            _s.Add("e731", "\U0000E731");
            _s.Add("eaec", "\U0000EAEC");
            _s.Add("e891", "\U0000E891");
            _s.Add("e82a", "\U0000E82A");
            _s.Add("e75b", "\U0000E75B");
            _s.Add("e9aa", "\U0000E9AA");
            _s.Add("ea50", "\U0000EA50");
            _s.Add("ea1c", "\U0000EA1C");
            _s.Add("e743", "\U0000E743");
            _s.Add("e707", "\U0000E707");
            _s.Add("e927", "\U0000E927");
            _s.Add("e816", "\U0000E816");
            _s.Add("e916", "\U0000E916");
            _s.Add("e918", "\U0000E918");
            _s.Add("e88d", "\U0000E88D");
            _s.Add("eadb", "\U0000EADB");
            _s.Add("e9fd", "\U0000E9FD");
            _s.Add("e84a", "\U0000E84A");
            _s.Add("e906", "\U0000E906");
            _s.Add("ea28", "\U0000EA28");
            _s.Add("eaae", "\U0000EAAE");
            _s.Add("e892", "\U0000E892");
            _s.Add("ea2b", "\U0000EA2B");
            _s.Add("e8ee", "\U0000E8EE");
            _s.Add("e72e", "\U0000E72E");
            _s.Add("e7da", "\U0000E7DA");
            _s.Add("ea8a", "\U0000EA8A");
            _s.Add("e84e", "\U0000E84E");
            _s.Add("e791", "\U0000E791");
            _s.Add("ece1", "\U0000ECE1");
            _s.Add("ed39", "\U0000ED39");
            _s.Add("ed38", "\U0000ED38");
            _s.Add("ed3a", "\U0000ED3A");
            _s.Add("eb84", "\U0000EB84");
            _s.Add("eb83", "\U0000EB83");
            _s.Add("eb87", "\U0000EB87");
            _s.Add("eb86", "\U0000EB86");
            _s.Add("eb85", "\U0000EB85");
            _s.Add("e7c5", "\U0000E7C5");
            _s.Add("ec08", "\U0000EC08");
            _s.Add("e9a7", "\U0000E9A7");
            _s.Add("e9a6", "\U0000E9A6");
            _s.Add("e9a5", "\U0000E9A5");
            _s.Add("e855", "\U0000E855");
            _s.Add("e8c3", "\U0000E8C3");
            _s.Add("e936", "\U0000E936");
            _s.Add("e70e", "\U0000E70E");
            _s.Add("e980", "\U0000E980");
            _s.Add("e976", "\U0000E976");
            _s.Add("e96f", "\U0000E96F");
            _s.Add("e806", "\U0000E806");
            _s.Add("ebb2", "\U0000EBB2");
            _s.Add("eb1d", "\U0000EB1D");
            _s.Add("e73b", "\U0000E73B");
            _s.Add("eb63", "\U0000EB63");
            _s.Add("ec52", "\U0000EC52");
            _s.Add("e858", "\U0000E858");
            _s.Add("eba9", "\U0000EBA9");
            _s.Add("ec7a", "\U0000EC7A");
            _s.Add("ea74", "\U0000EA74");
            _s.Add("ea48", "\U0000EA48");
            _s.Add("ea72", "\U0000EA72");
            _s.Add("ea75", "\U0000EA75");
            _s.Add("ed2d", "\U0000ED2D");
            _s.Add("e8a5", "\U0000E8A5");
            _s.Add("e829", "\U0000E829");
            _s.Add("ecb6", "\U0000ECB6");
            _s.Add("ec61", "\U0000EC61");
            _s.Add("e937", "\U0000E937");
            _s.Add("e9ab", "\U0000E9AB");
            _s.Add("eb70", "\U0000EB70");
            _s.Add("eb9d", "\U0000EB9D");
            _s.Add("eb41", "\U0000EB41");
            _s.Add("eb22", "\U0000EB22");
            _s.Add("eb0a", "\U0000EB0A");
            _s.Add("e7f7", "\U0000E7F7");
            _s.Add("e896", "\U0000E896");
            _s.Add("e8f0", "\U0000E8F0");
            _s.Add("ed45", "\U0000ED45");
            _s.Add("eb92", "\U0000EB92");
            _s.Add("ea20", "\U0000EA20");
            _s.Add("ec93", "\U0000EC93");
            _s.Add("e8bd", "\U0000E8BD");
            _s.Add("ec28", "\U0000EC28");
            _s.Add("ea3d", "\U0000EA3D");
            _s.Add("e799", "\U0000E799");
            _s.Add("e9e8", "\U0000E9E8");
            _s.Add("e8f3", "\U0000E8F3");
            _s.Add("ed22", "\U0000ED22");
            _s.Add("ec43", "\U0000EC43");
            _s.Add("e7a6", "\U0000E7A6");
            _s.Add("ead4", "\U0000EAD4");
            _s.Add("e893", "\U0000E893");
            _s.Add("eca0", "\U0000ECA0");
            _s.Add("eb3e", "\U0000EB3E");
            _s.Add("ec73", "\U0000EC73");
            _s.Add("e993", "\U0000E993");
            _s.Add("e873", "\U0000E873");
            _s.Add("ebf2", "\U0000EBF2");
            _s.Add("eac3", "\U0000EAC3");
            _s.Add("e8b3", "\U0000E8B3");
            _s.Add("ec9d", "\U0000EC9D");
            _s.Add("e7a8", "\U0000E7A8");
            _s.Add("ec19", "\U0000EC19");
            _s.Add("ec1c", "\U0000EC1C");
            _s.Add("e989", "\U0000E989");
            _s.Add("e943", "\U0000E943");
            _s.Add("e91f", "\U0000E91F");
            _s.Add("e946", "\U0000E946");
            _s.Add("e733", "\U0000E733");
            _s.Add("ea52", "\U0000EA52");
            _s.Add("ea4c", "\U0000EA4C");
            _s.Add("e92d", "\U0000E92D");
            _s.Add("eccd", "\U0000ECCD");
            _s.Add("e8a3", "\U0000E8A3");
            _s.Add("e8f4", "\U0000E8F4");
            _s.Add("ea33", "\U0000EA33");
            _s.Add("e7bb", "\U0000E7BB");
            _s.Add("ea32", "\U0000EA32");
            _s.Add("e7aa", "\U0000E7AA");
            _s.Add("e8d8", "\U0000E8D8");
            _s.Add("e72d", "\U0000E72D");
            _s.Add("e742", "\U0000E742");
            _s.Add("eb8a", "\U0000EB8A");
            _s.Add("eb6d", "\U0000EB6D");
            _s.Add("eb2f", "\U0000EB2F");
            _s.Add("ece5", "\U0000ECE5");
            _s.Add("e823", "\U0000E823");
            _s.Add("e720", "\U0000E720");
            _s.Add("e9ba", "\U0000E9BA");
            _s.Add("e89b", "\U0000E89B");
            _s.Add("eb32", "\U0000EB32");
            _s.Add("e7b8", "\U0000E7B8");
            _s.Add("e765", "\U0000E765");
            _s.Add("ea98", "\U0000EA98");
            _s.Add("e984", "\U0000E984");
            _s.Add("e941", "\U0000E941");
            _s.Add("e764", "\U0000E764");
            _s.Add("ecce", "\U0000ECCE");
            _s.Add("e89f", "\U0000E89F");
            _s.Add("e85f", "\U0000E85F");
            _s.Add("ed05", "\U0000ED05");
            _s.Add("e8a8", "\U0000E8A8");
            _s.Add("e701", "\U0000E701");
            _s.Add("e8a9", "\U0000E8A9");
            _s.Add("ec4d", "\U0000EC4D");
            _s.Add("e751", "\U0000E751");
            _s.Add("ed09", "\U0000ED09");
            _s.Add("e788", "\U0000E788");
            _s.Add("e842", "\U0000E842");
            _s.Add("e841", "\U0000E841");
            _s.Add("ead2", "\U0000EAD2");
            _s.Add("eb07", "\U0000EB07");
            _s.Add("e769", "\U0000E769");
            _s.Add("eba7", "\U0000EBA7");
            _s.Add("e79d", "\U0000E79D");
            _s.Add("e79c", "\U0000E79C");
            _s.Add("eab6", "\U0000EAB6");
            _s.Add("ecf6", "\U0000ECF6");
            _s.Add("e8c0", "\U0000E8C0");
            _s.Add("eb49", "\U0000EB49");
            _s.Add("e925", "\U0000E925");
            _s.Add("e710", "\U0000E710");
            _s.Add("e73f", "\U0000E73F");
            _s.Add("eafb", "\U0000EAFB");
            _s.Add("eb20", "\U0000EB20");
            _s.Add("e96e", "\U0000E96E");
            _s.Add("eb40", "\U0000EB40");
            _s.Add("e97b", "\U0000E97B");
            _s.Add("ed35", "\U0000ED35");
            _s.Add("eb53", "\U0000EB53");
            _s.Add("e77d", "\U0000E77D");
            _s.Add("ebbd", "\U0000EBBD");
            _s.Add("ea9d", "\U0000EA9D");
            _s.Add("ecac", "\U0000ECAC");
            _s.Add("ed0b", "\U0000ED0B");
            _s.Add("e76d", "\U0000E76D");
            _s.Add("ed42", "\U0000ED42");
            _s.Add("e875", "\U0000E875");
            _s.Add("eb0f", "\U0000EB0F");
            _s.Add("e7df", "\U0000E7DF");
            _s.Add("eb8d", "\U0000EB8D");
            _s.Add("e7bd", "\U0000E7BD");
            _s.Add("ebd1", "\U0000EBD1");
            _s.Add("eae2", "\U0000EAE2");
            _s.Add("e7f3", "\U0000E7F3");
            _s.Add("e863", "\U0000E863");
            _s.Add("ea68", "\U0000EA68");
            _s.Add("e995", "\U0000E995");
            _s.Add("e8ef", "\U0000E8EF");
            _s.Add("e982", "\U0000E982");
            _s.Add("e9a4", "\U0000E9A4");
            _s.Add("ec62", "\U0000EC62");
            _s.Add("e97d", "\U0000E97D");
            _s.Add("e836", "\U0000E836");
            _s.Add("e780", "\U0000E780");
            _s.Add("e8b0", "\U0000E8B0");
            _s.Add("ed29", "\U0000ED29");
            _s.Add("ea64", "\U0000EA64");
            _s.Add("eab0", "\U0000EAB0");
            _s.Add("e9be", "\U0000E9BE");
            _s.Add("e942", "\U0000E942");
            _s.Add("e9bb", "\U0000E9BB");
            _s.Add("ec04", "\U0000EC04");
            _s.Add("ea63", "\U0000EA63");
            _s.Add("ec7d", "\U0000EC7D");
            _s.Add("eccc", "\U0000ECCC");
            _s.Add("e7a2", "\U0000E7A2");
            _s.Add("e8ff", "\U0000E8FF");
            _s.Add("ed1a", "\U0000ED1A");
            _s.Add("e902", "\U0000E902");
            _s.Add("ebfd", "\U0000EBFD");
            _s.Add("e8af", "\U0000E8AF");
            _s.Add("ea14", "\U0000EA14");
            _s.Add("e96d", "\U0000E96D");
            _s.Add("ecd4", "\U0000ECD4");
            _s.Add("e895", "\U0000E895");
            _s.Add("eb4b", "\U0000EB4B");
            _s.Add("eb6c", "\U0000EB6C");
            _s.Add("e7b6", "\U0000E7B6");
            _s.Add("e749", "\U0000E749");
            _s.Add("eac1", "\U0000EAC1");
            _s.Add("eb76", "\U0000EB76");
            _s.Add("e8f9", "\U0000E8F9");
            _s.Add("e92b", "\U0000E92B");
            _s.Add("ecc8", "\U0000ECC8");
            _s.Add("e8a6", "\U0000E8A6");
            _s.Add("ea36", "\U0000EA36");
            _s.Add("e7b1", "\U0000E7B1");
            _s.Add("e777", "\U0000E777");
            _s.Add("e935", "\U0000E935");
            _s.Add("ecc9", "\U0000ECC9");
            _s.Add("e745", "\U0000E745");
            _s.Add("e965", "\U0000E965");
            _s.Add("e966", "\U0000E966");
            _s.Add("e962", "\U0000E962");
            _s.Add("e963", "\U0000E963");
            _s.Add("e831", "\U0000E831");
            _s.Add("e971", "\U0000E971");
            _s.Add("e983", "\U0000E983");
            _s.Add("ec6c", "\U0000EC6C");
            _s.Add("e7c1", "\U0000E7C1");
            _s.Add("ec74", "\U0000EC74");
            _s.Add("ecb2", "\U0000ECB2");
            _s.Add("e824", "\U0000E824");
            _s.Add("ebcc", "\U0000EBCC");
            _s.Add("ecf7", "\U0000ECF7");
            _s.Add("e970", "\U0000E970");
            _s.Add("e93a", "\U0000E93A");
            _s.Add("ec55", "\U0000EC55");
            _s.Add("e8b8", "\U0000E8B8");
            _s.Add("e810", "\U0000E810");
            _s.Add("e878", "\U0000E878");
            _s.Add("e8d6", "\U0000E8D6");
            _s.Add("ea39", "\U0000EA39");
            _s.Add("eacb", "\U0000EACB");
            _s.Add("e77f", "\U0000E77F");
            _s.Add("e8cd", "\U0000E8CD");
            _s.Add("e843", "\U0000E843");
            _s.Add("ed00", "\U0000ED00");
            _s.Add("ec3d", "\U0000EC3D");
            _s.Add("e80d", "\U0000E80D");
            _s.Add("ebb8", "\U0000EBB8");
            _s.Add("eb3a", "\U0000EB3A");
            _s.Add("e76b", "\U0000E76B");
            _s.Add("ebd2", "\U0000EBD2");
            _s.Add("e7ca", "\U0000E7CA");
            _s.Add("e738", "\U0000E738");
            _s.Add("e99f", "\U0000E99F");
            _s.Add("ea92", "\U0000EA92");
            _s.Add("e833", "\U0000E833");
            _s.Add("e87b", "\U0000E87B");
            _s.Add("e8aa", "\U0000E8AA");
            _s.Add("eae0", "\U0000EAE0");
            _s.Add("ec12", "\U0000EC12");
            _s.Add("ec8b", "\U0000EC8B");
            _s.Add("ebd9", "\U0000EBD9");
            _s.Add("e70f", "\U0000E70F");
            _s.Add("e8d4", "\U0000E8D4");
            _s.Add("e8d2", "\U0000E8D2");
            _s.Add("ec1b", "\U0000EC1B");
            _s.Add("e969", "\U0000E969");
            _s.Add("eac7", "\U0000EAC7");
            _s.Add("ebc9", "\U0000EBC9");
            _s.Add("eaab", "\U0000EAAB");
            _s.Add("e9eb", "\U0000E9EB");
            _s.Add("eb47", "\U0000EB47");
            _s.Add("e967", "\U0000E967");
            _s.Add("e7ee", "\U0000E7EE");
            _s.Add("ec18", "\U0000EC18");
            _s.Add("eb72", "\U0000EB72");
            _s.Add("e7a4", "\U0000E7A4");
            _s.Add("ea5b", "\U0000EA5B");
            _s.Add("ece8", "\U0000ECE8");
            _s.Add("eb4d", "\U0000EB4D");
            _s.Add("eaa9", "\U0000EAA9");
            _s.Add("e811", "\U0000E811");
            _s.Add("e808", "\U0000E808");
            _s.Add("eb51", "\U0000EB51");
            _s.Add("ec27", "\U0000EC27");
            _s.Add("ebbb", "\U0000EBBB");
            _s.Add("ec25", "\U0000EC25");
            _s.Add("e97a", "\U0000E97A");
            _s.Add("e739", "\U0000E739");
            _s.Add("e83a", "\U0000E83A");
            _s.Add("e919", "\U0000E919");
            _s.Add("e830", "\U0000E830");
            _s.Add("e9b0", "\U0000E9B0");
            _s.Add("e784", "\U0000E784");
            _s.Add("ea95", "\U0000EA95");
            _s.Add("e7af", "\U0000E7AF");
            _s.Add("e7cd", "\U0000E7CD");
            _s.Add("e7cb", "\U0000E7CB");
            _s.Add("ea0f", "\U0000EA0F");
            _s.Add("e89a", "\U0000E89A");
            _s.Add("e730", "\U0000E730");
            _s.Add("e79f", "\U0000E79F");
            _s.Add("e903", "\U0000E903");
            _s.Add("ecfd", "\U0000ECFD");
            _s.Add("eb8f", "\U0000EB8F");
            _s.Add("e7db", "\U0000E7DB");
            _s.Add("e726", "\U0000E726");
            _s.Add("ea13", "\U0000EA13");
            _s.Add("e8b7", "\U0000E8B7");
            _s.Add("eabc", "\U0000EABC");
            _s.Add("ea80", "\U0000EA80");
            _s.Add("e9b2", "\U0000E9B2");
            _s.Add("e724", "\U0000E724");
            _s.Add("e973", "\U0000E973");
            _s.Add("ec13", "\U0000EC13");
            _s.Add("ea25", "\U0000EA25");
            _s.Add("e992", "\U0000E992");
            _s.Add("eab7", "\U0000EAB7");
            _s.Add("ec2f", "\U0000EC2F");
            _s.Add("e8b4", "\U0000E8B4");
            _s.Add("e975", "\U0000E975");
            _s.Add("ec2d", "\U0000EC2D");
            _s.Add("e80e", "\U0000E80E");
            _s.Add("ebed", "\U0000EBED");
            _s.Add("e8f6", "\U0000E8F6");
            _s.Add("eb5b", "\U0000EB5B");
            _s.Add("e850", "\U0000E850");
            _s.Add("e75c", "\U0000E75C");
            _s.Add("e822", "\U0000E822");
            _s.Add("ece2", "\U0000ECE2");
            _s.Add("eb78", "\U0000EB78");
            _s.Add("e767", "\U0000E767");
            _s.Add("ed2b", "\U0000ED2B");
            _s.Add("e8f8", "\U0000E8F8");
            _s.Add("eb2a", "\U0000EB2A");
            _s.Add("eb02", "\U0000EB02");
            _s.Add("e840", "\U0000E840");
            _s.Add("e915", "\U0000E915");
            _s.Add("ebe6", "\U0000EBE6");
            _s.Add("e90e", "\U0000E90E");
            _s.Add("e7e0", "\U0000E7E0");
            _s.Add("e861", "\U0000E861");
            _s.Add("e91d", "\U0000E91D");
            _s.Add("e8bf", "\U0000E8BF");
            _s.Add("ec0a", "\U0000EC0A");
            _s.Add("e9db", "\U0000E9DB");
            _s.Add("ebca", "\U0000EBCA");
            _s.Add("e8be", "\U0000E8BE");
            _s.Add("ecf1", "\U0000ECF1");
            _s.Add("e72b", "\U0000E72B");
            _s.Add("e8d0", "\U0000E8D0");
            _s.Add("e938", "\U0000E938");
            _s.Add("ecfa", "\U0000ECFA");
            _s.Add("e92f", "\U0000E92F");
            _s.Add("ea81", "\U0000EA81");
            _s.Add("eb06", "\U0000EB06");
            _s.Add("e9de", "\U0000E9DE");
            _s.Add("e9ac", "\U0000E9AC");
            _s.Add("ebf4", "\U0000EBF4");
            _s.Add("e9b5", "\U0000E9B5");
            _s.Add("ec0c", "\U0000EC0C");
            _s.Add("e7dd", "\U0000E7DD");
            _s.Add("eb21", "\U0000EB21");
            _s.Add("ea54", "\U0000EA54");
            _s.Add("e7e1", "\U0000E7E1");
            _s.Add("e93b", "\U0000E93B");
            _s.Add("e792", "\U0000E792");
            _s.Add("e78d", "\U0000E78D");
            _s.Add("e988", "\U0000E988");
            _s.Add("ec14", "\U0000EC14");
            _s.Add("eab5", "\U0000EAB5");
            _s.Add("e77e", "\U0000E77E");
            _s.Add("e7fc", "\U0000E7FC");
            _s.Add("ea2a", "\U0000EA2A");
            _s.Add("e80f", "\U0000E80F");
            _s.Add("e8fa", "\U0000E8FA");
            _s.Add("eb29", "\U0000EB29");
            _s.Add("e7a1", "\U0000E7A1");
            _s.Add("ece4", "\U0000ECE4");
            _s.Add("ec10", "\U0000EC10");
            _s.Add("ece3", "\U0000ECE3");
            _s.Add("e805", "\U0000E805");
            _s.Add("ecaf", "\U0000ECAF");
            _s.Add("e80a", "\U0000E80A");
            _s.Add("e91b", "\U0000E91B");
            _s.Add("e921", "\U0000E921");
            _s.Add("e8da", "\U0000E8DA");
            _s.Add("ebb6", "\U0000EBB6");
            _s.Add("ec63", "\U0000EC63");
            _s.Add("e987", "\U0000E987");
            _s.Add("e8b5", "\U0000E8B5");
            _s.Add("e800", "\U0000E800");
            _s.Add("e8ae", "\U0000E8AE");
            _s.Add("ea15", "\U0000EA15");
            _s.Add("e73a", "\U0000E73A");
            _s.Add("ebd3", "\U0000EBD3");
            _s.Add("eaac", "\U0000EAAC");
            _s.Add("e7c2", "\U0000E7C2");
            _s.Add("ea0e", "\U0000EA0E");
            _s.Add("e837", "\U0000E837");
            _s.Add("ed44", "\U0000ED44");
            _s.Add("ea57", "\U0000EA57");
            _s.Add("e9e7", "\U0000E9E7");
            _s.Add("ecd2", "\U0000ECD2");
            _s.Add("ea43", "\U0000EA43");
            _s.Add("e793", "\U0000E793");
            _s.Add("ebda", "\U0000EBDA");
            _s.Add("e90a", "\U0000E90A");
            _s.Add("eb30", "\U0000EB30");
            _s.Add("ecbe", "\U0000ECBE");
            _s.Add("ec85", "\U0000EC85");
            _s.Add("e930", "\U0000E930");
            _s.Add("e7e2", "\U0000E7E2");
            _s.Add("e74d", "\U0000E74D");
            _s.Add("ec2c", "\U0000EC2C");
            _s.Add("e9a2", "\U0000E9A2");
            _s.Add("eca9", "\U0000ECA9");
            _s.Add("e9cc", "\U0000E9CC");
            _s.Add("e78f", "\U0000E78F");
            _s.Add("e827", "\U0000E827");
            _s.Add("ebab", "\U0000EBAB");
            _s.Add("e82e", "\U0000E82E");
            _s.Add("ebe0", "\U0000EBE0");
            _s.Add("ec4f", "\U0000EC4F");
            _s.Add("e782", "\U0000E782");
            _s.Add("e961", "\U0000E961");
            _s.Add("ea59", "\U0000EA59");
            _s.Add("ea26", "\U0000EA26");
            _s.Add("ec90", "\U0000EC90");
            _s.Add("e8f7", "\U0000E8F7");
            _s.Add("e8d9", "\U0000E8D9");
            _s.Add("e834", "\U0000E834");
            _s.Add("ea9f", "\U0000EA9F");
            _s.Add("ecff", "\U0000ECFF");
            _s.Add("e964", "\U0000E964");
            _s.Add("e8b2", "\U0000E8B2");
            _s.Add("e71d", "\U0000E71D");
            _s.Add("ed02", "\U0000ED02");
            _s.Add("ecbd", "\U0000ECBD");
            _s.Add("eae4", "\U0000EAE4");
            _s.Add("ecbc", "\U0000ECBC");
            _s.Add("e825", "\U0000E825");
            _s.Add("ed01", "\U0000ED01");
            _s.Add("ec00", "\U0000EC00");
            _s.Add("e8c4", "\U0000E8C4");
            _s.Add("e89e", "\U0000E89E");
            _s.Add("ec11", "\U0000EC11");
            _s.Add("ea55", "\U0000EA55");
            _s.Add("ea9e", "\U0000EA9E");
            _s.Add("ec57", "\U0000EC57");
            _s.Add("e8bc", "\U0000E8BC");
            _s.Add("ea0a", "\U0000EA0A");
            _s.Add("e789", "\U0000E789");
            _s.Add("e79b", "\U0000E79B");
            _s.Add("e851", "\U0000E851");
            _s.Add("eadc", "\U0000EADC");
            _s.Add("e90c", "\U0000E90C");
            _s.Add("e7d9", "\U0000E7D9");
            _s.Add("ec3c", "\U0000EC3C");
            _s.Add("e8d1", "\U0000E8D1");
            _s.Add("e771", "\U0000E771");
            _s.Add("e865", "\U0000E865");
            _s.Add("ed3d", "\U0000ED3D");
            _s.Add("ea51", "\U0000EA51");
            _s.Add("ecd0", "\U0000ECD0");
            _s.Add("ec42", "\U0000EC42");
            _s.Add("e894", "\U0000E894");
            _s.Add("eaa2", "\U0000EAA2");
            _s.Add("e7de", "\U0000E7DE");
            _s.Add("e7ec", "\U0000E7EC");
            _s.Add("ec17", "\U0000EC17");
            _s.Add("e985", "\U0000E985");
            _s.Add("e852", "\U0000E852");
            _s.Add("ecf8", "\U0000ECF8");
            _s.Add("eacc", "\U0000EACC");
            _s.Add("e812", "\U0000E812");
            _s.Add("eb1c", "\U0000EB1C");
            _s.Add("ea9b", "\U0000EA9B");
            _s.Add("e8c8", "\U0000E8C8");
            _s.Add("e82d", "\U0000E82D");
            _s.Add("e974", "\U0000E974");
            _s.Add("ed03", "\U0000ED03");
            _s.Add("e785", "\U0000E785");
            _s.Add("ea6a", "\U0000EA6A");
            _s.Add("ea41", "\U0000EA41");
            _s.Add("e705", "\U0000E705");
            _s.Add("ead1", "\U0000EAD1");
            _s.Add("eb7d", "\U0000EB7D");
            _s.Add("e98c", "\U0000E98C");
            _s.Add("e7bf", "\U0000E7BF");
            _s.Add("e968", "\U0000E968");
            _s.Add("ea10", "\U0000EA10");
            _s.Add("e874", "\U0000E874");
            _s.Add("ea35", "\U0000EA35");
            _s.Add("e737", "\U0000E737");
            _s.Add("eb05", "\U0000EB05");
            _s.Add("e934", "\U0000E934");
            _s.Add("e7d8", "\U0000E7D8");
            _s.Add("e727", "\U0000E727");
            _s.Add("eccb", "\U0000ECCB");
            _s.Add("eb34", "\U0000EB34");
            _s.Add("ea99", "\U0000EA99");
            _s.Add("e8a1", "\U0000E8A1");
            _s.Add("eb31", "\U0000EB31");
            _s.Add("e87d", "\U0000E87D");
            _s.Add("ebc6", "\U0000EBC6");
            _s.Add("e908", "\U0000E908");
            _s.Add("ec6b", "\U0000EC6B");
            _s.Add("e929", "\U0000E929");
            _s.Add("e807", "\U0000E807");
            _s.Add("e9a8", "\U0000E9A8");
            _s.Add("eaaa", "\U0000EAAA");
            _s.Add("eb37", "\U0000EB37");
            _s.Add("e97c", "\U0000E97C");
            _s.Add("eb91", "\U0000EB91");
            _s.Add("ec51", "\U0000EC51");
            _s.Add("ebaf", "\U0000EBAF");
            _s.Add("ea8b", "\U0000EA8B");
            _s.Add("e913", "\U0000E913");
            _s.Add("eb6f", "\U0000EB6F");
            _s.Add("e933", "\U0000E933");
            _s.Add("e712", "\U0000E712");
            _s.Add("eb5a", "\U0000EB5A");
            _s.Add("e9a3", "\U0000E9A3");
            _s.Add("ebe4", "\U0000EBE4");
            _s.Add("ea49", "\U0000EA49");
            _s.Add("ea37", "\U0000EA37");
            _s.Add("eaf9", "\U0000EAF9");
            _s.Add("ec77", "\U0000EC77");
            _s.Add("eaa0", "\U0000EAA0");
            _s.Add("e87a", "\U0000E87A");
            _s.Add("e704", "\U0000E704");
            _s.Add("e870", "\U0000E870");
            _s.Add("e8a2", "\U0000E8A2");
            _s.Add("e8f1", "\U0000E8F1");
            _s.Add("e821", "\U0000E821");
            _s.Add("e82c", "\U0000E82C");
            _s.Add("ed2a", "\U0000ED2A");
            _s.Add("eb17", "\U0000EB17");
            _s.Add("eaa3", "\U0000EAA3");
            _s.Add("ebcf", "\U0000EBCF");
            _s.Add("e775", "\U0000E775");
            _s.Add("e85d", "\U0000E85D");
            _s.Add("e781", "\U0000E781");
            _s.Add("e73e", "\U0000E73E");
            _s.Add("e8f2", "\U0000E8F2");
            _s.Add("eb73", "\U0000EB73");
            _s.Add("e722", "\U0000E722");
            _s.Add("ea9a", "\U0000EA9A");
            _s.Add("e797", "\U0000E797");
            _s.Add("e9e9", "\U0000E9E9");
            _s.Add("ec50", "\U0000EC50");
            _s.Add("ecf5", "\U0000ECF5");
            _s.Add("eaa1", "\U0000EAA1");
            _s.Add("e9c5", "\U0000E9C5");
            _s.Add("ea29", "\U0000EA29");
            _s.Add("eba3", "\U0000EBA3");
            _s.Add("eada", "\U0000EADA");
            _s.Add("ec60", "\U0000EC60");
            _s.Add("eabe", "\U0000EABE");
            _s.Add("eabd", "\U0000EABD");
            _s.Add("ec9a", "\U0000EC9A");
            _s.Add("e7a0", "\U0000E7A0");
            _s.Add("e904", "\U0000E904");
            _s.Add("e9dc", "\U0000E9DC");
            _s.Add("ec06", "\U0000EC06");
            _s.Add("e70c", "\U0000E70C");
            _s.Add("ed20", "\U0000ED20");
            _s.Add("ec40", "\U0000EC40");
            _s.Add("e93c", "\U0000E93C");
            _s.Add("e9b1", "\U0000E9B1");
            _s.Add("eb60", "\U0000EB60");
            _s.Add("eca7", "\U0000ECA7");
            _s.Add("e74b", "\U0000E74B");
            _s.Add("ea9c", "\U0000EA9C");
            _s.Add("e828", "\U0000E828");
            _s.Add("e87e", "\U0000E87E");
            _s.Add("ea88", "\U0000EA88");
            _s.Add("eb03", "\U0000EB03");
            _s.Add("e7d7", "\U0000E7D7");
            _s.Add("e8f5", "\U0000E8F5");
            _s.Add("e96b", "\U0000E96B");
            _s.Add("e79a", "\U0000E79A");
            _s.Add("ea47", "\U0000EA47");
            _s.Add("ed2f", "\U0000ED2F");
            _s.Add("e8c5", "\U0000E8C5");
            _s.Add("ea69", "\U0000EA69");
            _s.Add("e98b", "\U0000E98B");
            _s.Add("eb62", "\U0000EB62");
            _s.Add("e98e", "\U0000E98E");
            _s.Add("eac0", "\U0000EAC0");
            _s.Add("e9ef", "\U0000E9EF");
            _s.Add("ea4a", "\U0000EA4A");
            _s.Add("e8fb", "\U0000E8FB");
            _s.Add("ecf3", "\U0000ECF3");
            _s.Add("ecca", "\U0000ECCA");
            _s.Add("ec36", "\U0000EC36");
            _s.Add("eb45", "\U0000EB45");
            _s.Add("e97f", "\U0000E97F");
            _s.Add("ed3e", "\U0000ED3E");
            _s.Add("e754", "\U0000E754");
            _s.Add("ec44", "\U0000EC44");
            _s.Add("e981", "\U0000E981");
            _s.Add("eb43", "\U0000EB43");
            _s.Add("e979", "\U0000E979");
            _s.Add("ecfc", "\U0000ECFC");
            _s.Add("eb9e", "\U0000EB9E");
            _s.Add("eac9", "\U0000EAC9");
            _s.Add("ed1b", "\U0000ED1B");
            _s.Add("e854", "\U0000E854");
            _s.Add("ea3b", "\U0000EA3B");
            _s.Add("ece6", "\U0000ECE6");
            _s.Add("e85c", "\U0000E85C");
            _s.Add("e917", "\U0000E917");
            _s.Add("ecef", "\U0000ECEF");
            _s.Add("e7e9", "\U0000E7E9");
            _s.Add("eb3b", "\U0000EB3B");
            _s.Add("e759", "\U0000E759");
            _s.Add("ed41", "\U0000ED41");
            _s.Add("ec83", "\U0000EC83");
            _s.Add("eaea", "\U0000EAEA");
            _s.Add("e7c4", "\U0000E7C4");
            _s.Add("ecb3", "\U0000ECB3");
            _s.Add("ec3a", "\U0000EC3A");
            _s.Add("e9b9", "\U0000E9B9");
            _s.Add("ecfe", "\U0000ECFE");
            _s.Add("e85a", "\U0000E85A");
            _s.Add("ebcb", "\U0000EBCB");
            _s.Add("ed33", "\U0000ED33");
            _s.Add("e80b", "\U0000E80B");
            _s.Add("e8bb", "\U0000E8BB");
            _s.Add("ed2c", "\U0000ED2C");
            _s.Add("e923", "\U0000E923");
            _s.Add("ec09", "\U0000EC09");
            _s.Add("ecd8", "\U0000ECD8");
            _s.Add("e9b3", "\U0000E9B3");
            _s.Add("ea96", "\U0000EA96");
            _s.Add("ec0f", "\U0000EC0F");
            _s.Add("eb9f", "\U0000EB9F");
            _s.Add("e932", "\U0000E932");
            _s.Add("e978", "\U0000E978");
            _s.Add("ea7f", "\U0000EA7F");
            _s.Add("ed04", "\U0000ED04");
            _s.Add("ebcd", "\U0000EBCD");
            _s.Add("e832", "\U0000E832");
            _s.Add("eb71", "\U0000EB71");
            _s.Add("e86b", "\U0000E86B");
            _s.Add("e99e", "\U0000E99E");
            _s.Add("e96c", "\U0000E96C");
            _s.Add("ea87", "\U0000EA87");
            _s.Add("e77c", "\U0000E77C");
            _s.Add("e7ea", "\U0000E7EA");
            _s.Add("ec1a", "\U0000EC1A");
            _s.Add("e8fc", "\U0000E8FC");
            _s.Add("e7dc", "\U0000E7DC");
            _s.Add("ecd3", "\U0000ECD3");
            _s.Add("ecf0", "\U0000ECF0");
            _s.Add("ec31", "\U0000EC31");
            _s.Add("ebc7", "\U0000EBC7");
            _s.Add("eaaf", "\U0000EAAF");
            _s.Add("e84d", "\U0000E84D");
            _s.Add("ea31", "\U0000EA31");
            _s.Add("ecbb", "\U0000ECBB");
            _s.Add("ea7b", "\U0000EA7B");
            _s.Add("e82b", "\U0000E82B");
            _s.Add("e901", "\U0000E901");
            _s.Add("e9c4", "\U0000E9C4");
            _s.Add("e898", "\U0000E898");
            _s.Add("e813", "\U0000E813");
            _s.Add("e945", "\U0000E945");
            _s.Add("ecd5", "\U0000ECD5");
            _s.Add("eba5", "\U0000EBA5");
            _s.Add("e926", "\U0000E926");
            _s.Add("ebba", "\U0000EBBA");
            _s.Add("e8fe", "\U0000E8FE");
            _s.Add("e7ad", "\U0000E7AD");
            _s.Add("e8fd", "\U0000E8FD");
            _s.Add("ec88", "\U0000EC88");
            _s.Add("e912", "\U0000E912");
            _s.Add("e70d", "\U0000E70D");
            _s.Add("ea11", "\U0000EA11");
            _s.Add("eb4f", "\U0000EB4F");
            _s.Add("ea17", "\U0000EA17");
            _s.Add("ed30", "\U0000ED30");
            _s.Add("e7f8", "\U0000E7F8");
            _s.Add("e728", "\U0000E728");
            _s.Add("e8a0", "\U0000E8A0");
            _s.Add("ed2e", "\U0000ED2E");
            _s.Add("e76f", "\U0000E76F");
            _s.Add("ecb4", "\U0000ECB4");
            _s.Add("ea4f", "\U0000EA4F");
            _s.Add("e97e", "\U0000E97E");
            _s.Add("ed3c", "\U0000ED3C");
            _s.Add("eae6", "\U0000EAE6");
            _s.Add("e853", "\U0000E853");
            _s.Add("ec37", "\U0000EC37");
            _s.Add("e844", "\U0000E844");
            _s.Add("e711", "\U0000E711");
            _s.Add("e900", "\U0000E900");
            _s.Add("ec15", "\U0000EC15");
            _s.Add("eb16", "\U0000EB16");
            _s.Add("e750", "\U0000E750");
            _s.Add("e96a", "\U0000E96A");
            _s.Add("e95c", "\U0000E95C");
            _s.Add("e95d", "\U0000E95D");
            _s.Add("e95e", "\U0000E95E");
            _s.Add("e95f", "\U0000E95F");
            _s.Add("e960", "\U0000E960");
            _s.Add("e860", "\U0000E860");
            _s.Add("ea21", "\U0000EA21");
            _s.Add("ebe2", "\U0000EBE2");
            _s.Add("e741", "\U0000E741");
            _s.Add("e931", "\U0000E931");
            _s.Add("eba2", "\U0000EBA2");
            _s.Add("e783", "\U0000E783");
            _s.Add("ecfb", "\U0000ECFB");
            _s.Add("ec30", "\U0000EC30");
            _s.Add("ea4d", "\U0000EA4D");
            _s.Add("e71f", "\U0000E71F");
            _s.Add("ebc8", "\U0000EBC8");
            _s.Add("ec2e", "\U0000EC2E");
            _s.Add("ecf9", "\U0000ECF9");
            _s.Add("eb8b", "\U0000EB8B");
            _s.Add("eb00", "\U0000EB00");
            _s.Add("eaf3", "\U0000EAF3");
            _s.Add("eaeb", "\U0000EAEB");
            _s.Add("e809", "\U0000E809");
            _s.Add("e773", "\U0000E773");
            _s.Add("eb3c", "\U0000EB3C");
            _s.Add("e905", "\U0000E905");
            _s.Add("ead8", "\U0000EAD8");
            _s.Add("ec16", "\U0000EC16");
            _s.Add("ea45", "\U0000EA45");
            _s.Add("e8a7", "\U0000E8A7");
            _s.Add("ecb5", "\U0000ECB5");
            _s.Add("ece9", "\U0000ECE9");
            _s.Add("ed36", "\U0000ED36");
            _s.Add("e911", "\U0000E911");
            _s.Add("e702", "\U0000E702");
            _s.Add("eb01", "\U0000EB01");
            _s.Add("e9f3", "\U0000E9F3");
            _s.Add("ed3b", "\U0000ED3B");
            _s.Add("e9f5", "\U0000E9F5");
            _s.Add("e9f4", "\U0000E9F4");
            _s.Add("e9f7", "\U0000E9F7");
            _s.Add("e9f6", "\U0000E9F6");
            _s.Add("e9f9", "\U0000E9F9");
            _s.Add("e9f8", "\U0000E9F8");
            _s.Add("ea12", "\U0000EA12");
            _s.Add("e9f2", "\U0000E9F2");
            _s.Add("e907", "\U0000E907");
            _s.Add("eca3", "\U0000ECA3");
            _s.Add("ec0e", "\U0000EC0E");
            _s.Add("e972", "\U0000E972");
            _s.Add("eace", "\U0000EACE");
            _s.Add("eade", "\U0000EADE");
            _s.Add("e977", "\U0000E977");
            _s.Add("ea40", "\U0000EA40");
            _s.Add("eb1e", "\U0000EB1E");
            _s.Add("e89d", "\U0000E89D");
            _s.Add("ead6", "\U0000EAD6");
            _s.Add("e7a3", "\U0000E7A3");
            _s.Add("ea73", "\U0000EA73");
            _s.Add("eccf", "\U0000ECCF");
            _s.Add("ea65", "\U0000EA65");
            _s.Add("e760", "\U0000E760");
            _s.Add("e9c7", "\U0000E9C7");
            _s.Add("eab8", "\U0000EAB8");
            _s.Add("e8cb", "\U0000E8CB");
            _s.Add("ec81", "\U0000EC81");
            _s.Add("e9da", "\U0000E9DA");
            _s.Add("ed17", "\U0000ED17");
            _s.Add("ea16", "\U0000EA16");
            _s.Add("ea67", "\U0000EA67");
            _s.Add("ebce", "\U0000EBCE");
            _s.Add("e991", "\U0000E991");
            _s.Add("e7b5", "\U0000E7B5");
            _s.Add("ebfc", "\U0000EBFC");
            _s.Add("ebfb", "\U0000EBFB");
            _s.Add("e939", "\U0000E939");
            _s.Add("ebd7", "\U0000EBD7");
            _s.Add("ebd6", "\U0000EBD6");
            _s.Add("e838", "\U0000E838");
            _s.Add("ebd8", "\U0000EBD8");
            _s.Add("eac5", "\U0000EAC5");
            _s.Add("ec96", "\U0000EC96");
            _s.Add("e856", "\U0000E856");
            _s.Add("e758", "\U0000E758");
            _s.Add("ebdb", "\U0000EBDB");
            _s.Add("e7b3", "\U0000E7B3");
            _s.Add("e999", "\U0000E999");
            _s.Add("ea8c", "\U0000EA8C");
            _s.Add("ea8e", "\U0000EA8E");
            _s.Add("e998", "\U0000E998");
            _s.Add("eb57", "\U0000EB57");
            _s.Add("ea6b", "\U0000EA6B");
            _s.Add("ea77", "\U0000EA77");
            _s.Add("e99b", "\U0000E99B");
            _s.Add("ea7a", "\U0000EA7A");
            _s.Add("ea6c", "\U0000EA6C");
            _s.Add("ebec", "\U0000EBEC");
            _s.Add("ea85", "\U0000EA85");
            _s.Add("ea83", "\U0000EA83");
            _s.Add("ea78", "\U0000EA78");
            _s.Add("ea79", "\U0000EA79");
            _s.Add("ea84", "\U0000EA84");
            _s.Add("ea7c", "\U0000EA7C");
            _s.Add("e99c", "\U0000E99C");
            _s.Add("ea93", "\U0000EA93");
            _s.Add("ea89", "\U0000EA89");
            _s.Add("eb79", "\U0000EB79");
            _s.Add("eb97", "\U0000EB97");
            _s.Add("e71b", "\U0000E71B");
            _s.Add("e9f1", "\U0000E9F1");
            _s.Add("eb19", "\U0000EB19");
            _s.Add("e7e3", "\U0000E7E3");
            _s.Add("ebe9", "\U0000EBE9");
            _s.Add("e9d2", "\U0000E9D2");
            _s.Add("eb1b", "\U0000EB1B");
            _s.Add("ec6f", "\U0000EC6F");
            _s.Add("e9e4", "\U0000E9E4");
            _s.Add("eb0e", "\U0000EB0E");
            _s.Add("ebc5", "\U0000EBC5");
            _s.Add("e70b", "\U0000E70B");
            _s.Add("eb1a", "\U0000EB1A");
            _s.Add("ea62", "\U0000EA62");
            _s.Add("eb96", "\U0000EB96");
            _s.Add("ea2f", "\U0000EA2F");
            _s.Add("e714", "\U0000E714");
            _s.Add("ea09", "\U0000EA09");
            _s.Add("e7e8", "\U0000E7E8");
            _s.Add("e70a", "\U0000E70A");
            _s.Add("ebde", "\U0000EBDE");
            _s.Add("e719", "\U0000E719");
            _s.Add("e9e2", "\U0000E9E2");
            _s.Add("e9d4", "\U0000E9D4");
            _s.Add("ebbf", "\U0000EBBF");
            _s.Add("e7f1", "\U0000E7F1");
            _s.Add("e756", "\U0000E756");
            _s.Add("ebb9", "\U0000EBB9");
            _s.Add("ea0d", "\U0000EA0D");
            _s.Add("ecc0", "\U0000ECC0");
            _s.Add("e9d0", "\U0000E9D0");
            _s.Add("eb89", "\U0000EB89");
            _s.Add("e86a", "\U0000E86A");
            _s.Add("eb80", "\U0000EB80");
            _s.Add("e9e1", "\U0000E9E1");
            _s.Add("ebdf", "\U0000EBDF");
            _s.Add("e9d7", "\U0000E9D7");
            _s.Add("e717", "\U0000E717");
            _s.Add("ebbe", "\U0000EBBE");
            _s.Add("ebea", "\U0000EBEA");
            _s.Add("ebeb", "\U0000EBEB");
            _s.Add("ec58", "\U0000EC58");
            _s.Add("eb13", "\U0000EB13");
            _s.Add("eb14", "\U0000EB14");
            _s.Add("eb12", "\U0000EB12");
            _s.Add("e9e0", "\U0000E9E0");
            _s.Add("e86d", "\U0000E86D");
            _s.Add("eb88", "\U0000EB88");
            _s.Add("e997", "\U0000E997");
            _s.Add("eb18", "\U0000EB18");
            _s.Add("e9cb", "\U0000E9CB");
            _s.Add("e9fc", "\U0000E9FC");
            _s.Add("ea07", "\U0000EA07");
            _s.Add("ec34", "\U0000EC34");
            _s.Add("e866", "\U0000E866");
            _s.Add("ebf0", "\U0000EBF0");
            _s.Add("ea2e", "\U0000EA2E");
            _s.Add("ec33", "\U0000EC33");
            _s.Add("e7e5", "\U0000E7E5");
            _s.Add("e9d9", "\U0000E9D9");
            _s.Add("ebc0", "\U0000EBC0");
            _s.Add("eb09", "\U0000EB09");
            _s.Add("ec48", "\U0000EC48");
            _s.Add("e86f", "\U0000E86F");
            _s.Add("e708", "\U0000E708");
            _s.Add("e9e3", "\U0000E9E3");
            _s.Add("e795", "\U0000E795");
            _s.Add("ec02", "\U0000EC02");
            _s.Add("e846", "\U0000E846");
            _s.Add("e7e4", "\U0000E7E4");
            _s.Add("e9c8", "\U0000E9C8");
            _s.Add("eb2c", "\U0000EB2C");
            _s.Add("ea08", "\U0000EA08");
            _s.Add("ec5b", "\U0000EC5B");
            _s.Add("e9fa", "\U0000E9FA");
            _s.Add("e9d8", "\U0000E9D8");
            _s.Add("e9d3", "\U0000E9D3");
            _s.Add("eb2b", "\U0000EB2B");
            _s.Add("ebf8", "\U0000EBF8");
            _s.Add("e74c", "\U0000E74C");
            _s.Add("e845", "\U0000E845");
            _s.Add("e72f", "\U0000E72F");
            _s.Add("ea2d", "\U0000EA2D");
            _s.Add("ec5a", "\U0000EC5A");
            _s.Add("ec32", "\U0000EC32");
            _s.Add("e7fe", "\U0000E7FE");
            _s.Add("e86e", "\U0000E86E");
            _s.Add("e74f", "\U0000E74F");
            _s.Add("ec72", "\U0000EC72");
            _s.Add("e9d6", "\U0000E9D6");
            _s.Add("e71a", "\U0000E71A");
            _s.Add("eb95", "\U0000EB95");
            _s.Add("e9c6", "\U0000E9C6");
            _s.Add("e71c", "\U0000E71C");
            _s.Add("eb15", "\U0000EB15");
            _s.Add("ebf7", "\U0000EBF7");
            _s.Add("ec5d", "\U0000EC5D");
            _s.Add("ebf1", "\U0000EBF1");
            _s.Add("eb82", "\U0000EB82");
            _s.Add("e9d5", "\U0000E9D5");
            _s.Add("ea30", "\U0000EA30");
            _s.Add("e718", "\U0000E718");
            _s.Add("e8c2", "\U0000E8C2");
            _s.Add("e98d", "\U0000E98D");
            _s.Add("eb81", "\U0000EB81");
            _s.Add("ec35", "\U0000EC35");
            _s.Add("eb0d", "\U0000EB0D");
            _s.Add("e7e7", "\U0000E7E7");
            _s.Add("e716", "\U0000E716");
            _s.Add("eb11", "\U0000EB11");
            _s.Add("e996", "\U0000E996");
            _s.Add("ec01", "\U0000EC01");
            _s.Add("ebc4", "\U0000EBC4");
            _s.Add("e715", "\U0000E715");
            _s.Add("e83e", "\U0000E83E");
            _s.Add("e876", "\U0000E876");
            _s.Add("e879", "\U0000E879");
            _s.Add("ec03", "\U0000EC03");
            _s.Add("e83f", "\U0000E83F");
            _s.Add("ec70", "\U0000EC70");
            _s.Add("ebc2", "\U0000EBC2");
            _s.Add("ebf6", "\U0000EBF6");
            _s.Add("ec6a", "\U0000EC6A");
            _s.Add("e9d1", "\U0000E9D1");
            _s.Add("ec71", "\U0000EC71");
            _s.Add("e794", "\U0000E794");
            _s.Add("ec5e", "\U0000EC5E");
            _s.Add("e86c", "\U0000E86C");
            _s.Add("ec5f", "\U0000EC5F");
            _s.Add("e7f2", "\U0000E7F2");
            _s.Add("e7e6", "\U0000E7E6");
            _s.Add("ebf9", "\U0000EBF9");
            _s.Add("ebfa", "\U0000EBFA");
            _s.Add("e83d", "\U0000E83D");
            _s.Add("ec5c", "\U0000EC5C");
            _s.Add("e709", "\U0000E709");
            _s.Add("ec59", "\U0000EC59");
            _s.Add("ebdd", "\U0000EBDD");
            _s.Add("eb7a", "\U0000EB7A");
            _s.Add("e9fb", "\U0000E9FB");
            _s.Add("e7ff", "\U0000E7FF");
            _s.Add("edec", "\U0000EDEC");
            _s.Add("ee3a", "\U0000EE3A");
            _s.Add("ed80", "\U0000ED80");
            _s.Add("edb4", "\U0000EDB4");
            _s.Add("eddd", "\U0000EDDD");
            _s.Add("ed97", "\U0000ED97");
            _s.Add("ed5e", "\U0000ED5E");
            _s.Add("edcd", "\U0000EDCD");
            _s.Add("ee28", "\U0000EE28");
            _s.Add("ed57", "\U0000ED57");
            _s.Add("ed55", "\U0000ED55");
            _s.Add("edeb", "\U0000EDEB");
            _s.Add("edba", "\U0000EDBA");
            _s.Add("ed8b", "\U0000ED8B");
            _s.Add("ed7f", "\U0000ED7F");
            _s.Add("edff", "\U0000EDFF");
            _s.Add("edb3", "\U0000EDB3");
            _s.Add("ee0d", "\U0000EE0D");
            _s.Add("edab", "\U0000EDAB");
            _s.Add("edea", "\U0000EDEA");
            _s.Add("ed9d", "\U0000ED9D");
            _s.Add("ed5c", "\U0000ED5C");
            _s.Add("ed9f", "\U0000ED9F");
            _s.Add("ed6f", "\U0000ED6F");
            _s.Add("edf7", "\U0000EDF7");
            _s.Add("edc9", "\U0000EDC9");
            _s.Add("ee1c", "\U0000EE1C");
            _s.Add("edac", "\U0000EDAC");
            _s.Add("ede3", "\U0000EDE3");
            _s.Add("ee30", "\U0000EE30");
            _s.Add("ed8c", "\U0000ED8C");
            _s.Add("ed8d", "\U0000ED8D");
            _s.Add("eda9", "\U0000EDA9");
            _s.Add("ed82", "\U0000ED82");
            _s.Add("edca", "\U0000EDCA");
            _s.Add("edc1", "\U0000EDC1");
            _s.Add("ee3c", "\U0000EE3C");
            _s.Add("ee36", "\U0000EE36");
            _s.Add("ee3b", "\U0000EE3B");
            _s.Add("ed88", "\U0000ED88");
            _s.Add("ed87", "\U0000ED87");
            _s.Add("ed7d", "\U0000ED7D");
            _s.Add("ed86", "\U0000ED86");
            _s.Add("edf6", "\U0000EDF6");
            _s.Add("ee14", "\U0000EE14");
            _s.Add("edf5", "\U0000EDF5");
            _s.Add("edd1", "\U0000EDD1");
            _s.Add("ee23", "\U0000EE23");
            _s.Add("ee34", "\U0000EE34");
            _s.Add("ee39", "\U0000EE39");
            _s.Add("ee46", "\U0000EE46");
            _s.Add("ed51", "\U0000ED51");
            _s.Add("ee37", "\U0000EE37");
            _s.Add("ed85", "\U0000ED85");
            _s.Add("ede5", "\U0000EDE5");
            _s.Add("ede6", "\U0000EDE6");
            _s.Add("ed9a", "\U0000ED9A");
            _s.Add("ee1e", "\U0000EE1E");
            _s.Add("ee20", "\U0000EE20");
            _s.Add("edd6", "\U0000EDD6");
            _s.Add("ed4f", "\U0000ED4F");
            _s.Add("ed98", "\U0000ED98");
            _s.Add("edf4", "\U0000EDF4");
            _s.Add("ed92", "\U0000ED92");
            _s.Add("ee17", "\U0000EE17");
            _s.Add("edc7", "\U0000EDC7");
            _s.Add("edce", "\U0000EDCE");
            _s.Add("ede8", "\U0000EDE8");
            _s.Add("edc2", "\U0000EDC2");
            _s.Add("ee40", "\U0000EE40");
            _s.Add("ed47", "\U0000ED47");
            _s.Add("ed49", "\U0000ED49");
            _s.Add("ede2", "\U0000EDE2");
            _s.Add("ede0", "\U0000EDE0");
            _s.Add("ed99", "\U0000ED99");
            _s.Add("eda7", "\U0000EDA7");
            _s.Add("edbb", "\U0000EDBB");
            _s.Add("edb6", "\U0000EDB6");
            _s.Add("edd3", "\U0000EDD3");
            _s.Add("ed59", "\U0000ED59");
            _s.Add("edd4", "\U0000EDD4");
            _s.Add("ee3d", "\U0000EE3D");
            _s.Add("ed90", "\U0000ED90");
            _s.Add("ee2f", "\U0000EE2F");
            _s.Add("ed6b", "\U0000ED6B");
            _s.Add("ed96", "\U0000ED96");
            _s.Add("ee02", "\U0000EE02");
            _s.Add("ee31", "\U0000EE31");
            _s.Add("edc4", "\U0000EDC4");
            _s.Add("ed53", "\U0000ED53");
            _s.Add("ed76", "\U0000ED76");
            _s.Add("edb1", "\U0000EDB1");
            _s.Add("ede4", "\U0000EDE4");
            _s.Add("ed64", "\U0000ED64");
            _s.Add("ed70", "\U0000ED70");
            _s.Add("ed6e", "\U0000ED6E");
            _s.Add("edd7", "\U0000EDD7");
            _s.Add("edcc", "\U0000EDCC");
            _s.Add("edd9", "\U0000EDD9");
            _s.Add("ed74", "\U0000ED74");
            _s.Add("edd8", "\U0000EDD8");
            _s.Add("edc5", "\U0000EDC5");
            _s.Add("eda0", "\U0000EDA0");
            _s.Add("eda5", "\U0000EDA5");
            _s.Add("eda8", "\U0000EDA8");
            _s.Add("ee2d", "\U0000EE2D");
            _s.Add("edfe", "\U0000EDFE");
            _s.Add("edcf", "\U0000EDCF");
            _s.Add("ed7a", "\U0000ED7A");
            _s.Add("ee3f", "\U0000EE3F");
            _s.Add("ed79", "\U0000ED79");
            _s.Add("ee19", "\U0000EE19");
            _s.Add("ee33", "\U0000EE33");
            _s.Add("ed84", "\U0000ED84");
            _s.Add("ed56", "\U0000ED56");
            _s.Add("ed95", "\U0000ED95");
            _s.Add("ee0f", "\U0000EE0F");
            _s.Add("ee1b", "\U0000EE1B");
            _s.Add("edfa", "\U0000EDFA");
            _s.Add("ee38", "\U0000EE38");
            _s.Add("ede9", "\U0000EDE9");
            _s.Add("ed4d", "\U0000ED4D");
            _s.Add("ee07", "\U0000EE07");
            _s.Add("edbe", "\U0000EDBE");
            _s.Add("edf9", "\U0000EDF9");
            _s.Add("edd2", "\U0000EDD2");
            _s.Add("edc8", "\U0000EDC8");
            _s.Add("ee22", "\U0000EE22");
            _s.Add("eda4", "\U0000EDA4");
            _s.Add("edcb", "\U0000EDCB");
            _s.Add("ee47", "\U0000EE47");
            _s.Add("edd0", "\U0000EDD0");
            _s.Add("ee15", "\U0000EE15");
            _s.Add("ed94", "\U0000ED94");
            _s.Add("ed62", "\U0000ED62");
            _s.Add("ee11", "\U0000EE11");
            _s.Add("eda1", "\U0000EDA1");
            _s.Add("ee45", "\U0000EE45");
            _s.Add("ee0c", "\U0000EE0C");
            _s.Add("ee05", "\U0000EE05");
            _s.Add("edaf", "\U0000EDAF");
            _s.Add("edae", "\U0000EDAE");
            _s.Add("ee26", "\U0000EE26");
            _s.Add("ee29", "\U0000EE29");
            _s.Add("ed4b", "\U0000ED4B");
            _s.Add("ee01", "\U0000EE01");
            _s.Add("ed7e", "\U0000ED7E");
            _s.Add("ed6c", "\U0000ED6C");
            _s.Add("ed60", "\U0000ED60");
            _s.Add("eddb", "\U0000EDDB");
            _s.Add("ed8a", "\U0000ED8A");
            _s.Add("edbf", "\U0000EDBF");
            _s.Add("edb2", "\U0000EDB2");
            _s.Add("ed6d", "\U0000ED6D");
            _s.Add("ee0e", "\U0000EE0E");
            _s.Add("eddc", "\U0000EDDC");
            _s.Add("ee21", "\U0000EE21");
            _s.Add("eda6", "\U0000EDA6");
            _s.Add("ee2c", "\U0000EE2C");
            _s.Add("ed78", "\U0000ED78");
            _s.Add("ee10", "\U0000EE10");
            _s.Add("ee49", "\U0000EE49");
            _s.Add("edde", "\U0000EDDE");
            _s.Add("ed65", "\U0000ED65");
            _s.Add("ed61", "\U0000ED61");
            _s.Add("ee09", "\U0000EE09");
            _s.Add("ed73", "\U0000ED73");
            _s.Add("ee2e", "\U0000EE2E");
            _s.Add("ed9c", "\U0000ED9C");
            _s.Add("ee3e", "\U0000EE3E");
            _s.Add("ede7", "\U0000EDE7");
            _s.Add("ed7b", "\U0000ED7B");
            _s.Add("edf0", "\U0000EDF0");
            _s.Add("eda2", "\U0000EDA2");
            _s.Add("edd5", "\U0000EDD5");
            _s.Add("ee2a", "\U0000EE2A");
            _s.Add("ed77", "\U0000ED77");
            _s.Add("ede1", "\U0000EDE1");
            _s.Add("ed63", "\U0000ED63");
            _s.Add("ee16", "\U0000EE16");
            _s.Add("ee0b", "\U0000EE0B");
            _s.Add("edaa", "\U0000EDAA");
            _s.Add("edbc", "\U0000EDBC");
            _s.Add("ed50", "\U0000ED50");
            _s.Add("edf3", "\U0000EDF3");
            _s.Add("ed5a", "\U0000ED5A");
            _s.Add("ee48", "\U0000EE48");
            _s.Add("ed9e", "\U0000ED9E");
            _s.Add("edf8", "\U0000EDF8");
            _s.Add("ee12", "\U0000EE12");
            _s.Add("edef", "\U0000EDEF");
            _s.Add("ee1d", "\U0000EE1D");
            _s.Add("ed75", "\U0000ED75");
            _s.Add("ed54", "\U0000ED54");
            _s.Add("ed71", "\U0000ED71");
            _s.Add("ee00", "\U0000EE00");
            _s.Add("edc3", "\U0000EDC3");
            _s.Add("ed9b", "\U0000ED9B");
            _s.Add("ee1f", "\U0000EE1F");
            _s.Add("ed91", "\U0000ED91");
            _s.Add("ed69", "\U0000ED69");
            _s.Add("ed58", "\U0000ED58");
            _s.Add("edda", "\U0000EDDA");
            _s.Add("edfb", "\U0000EDFB");
            _s.Add("edbd", "\U0000EDBD");
            _s.Add("edad", "\U0000EDAD");
            _s.Add("ee08", "\U0000EE08");
            _s.Add("ee18", "\U0000EE18");
            _s.Add("ee44", "\U0000EE44");
            _s.Add("ed83", "\U0000ED83");
            _s.Add("ed4a", "\U0000ED4A");
            _s.Add("ee0a", "\U0000EE0A");
            _s.Add("ed4c", "\U0000ED4C");
            _s.Add("ed5f", "\U0000ED5F");
            _s.Add("ed4e", "\U0000ED4E");
            _s.Add("edf2", "\U0000EDF2");
            _s.Add("ed93", "\U0000ED93");
            _s.Add("edee", "\U0000EDEE");
            _s.Add("ed89", "\U0000ED89");
            _s.Add("ed6a", "\U0000ED6A");
            _s.Add("ed7c", "\U0000ED7C");
            _s.Add("ed5b", "\U0000ED5B");
            _s.Add("ed48", "\U0000ED48");
            _s.Add("ee25", "\U0000EE25");
            _s.Add("edb0", "\U0000EDB0");
            _s.Add("ed8f", "\U0000ED8F");
            _s.Add("ed8e", "\U0000ED8E");
            _s.Add("edc6", "\U0000EDC6");
            _s.Add("ee35", "\U0000EE35");
            _s.Add("eda3", "\U0000EDA3");
            _s.Add("ed52", "\U0000ED52");
            _s.Add("ee04", "\U0000EE04");
            _s.Add("ee03", "\U0000EE03");
            _s.Add("ed68", "\U0000ED68");
            _s.Add("ee32", "\U0000EE32");
            _s.Add("ee24", "\U0000EE24");
            _s.Add("ee13", "\U0000EE13");
            _s.Add("ed66", "\U0000ED66");
            _s.Add("eddf", "\U0000EDDF");
            _s.Add("ee27", "\U0000EE27");
            _s.Add("ee2b", "\U0000EE2B");
            _s.Add("ed72", "\U0000ED72");
            _s.Add("edb5", "\U0000EDB5");
            _s.Add("ee1a", "\U0000EE1A");
            _s.Add("ed81", "\U0000ED81");
            _s.Add("edf1", "\U0000EDF1");
            _s.Add("edb7", "\U0000EDB7");
            _s.Add("ed67", "\U0000ED67");
            _s.Add("ed5d", "\U0000ED5D");
            _s.Add("edc0", "\U0000EDC0");
            _s.Add("ef25", "\U0000EF25");
            _s.Add("ef23", "\U0000EF23");
            _s.Add("ee5b", "\U0000EE5B");
            _s.Add("eeb5", "\U0000EEB5");
            _s.Add("ee58", "\U0000EE58");
            _s.Add("ef05", "\U0000EF05");
            _s.Add("efcf", "\U0000EFCF");
            _s.Add("eeda", "\U0000EEDA");
            _s.Add("ee6a", "\U0000EE6A");
            _s.Add("eed5", "\U0000EED5");
            _s.Add("ee4f", "\U0000EE4F");
            _s.Add("ef03", "\U0000EF03");
            _s.Add("efc2", "\U0000EFC2");
            _s.Add("ee91", "\U0000EE91");
            _s.Add("ee5a", "\U0000EE5A");
            _s.Add("ee50", "\U0000EE50");
            _s.Add("ee5f", "\U0000EE5F");
            _s.Add("ef24", "\U0000EF24");
            _s.Add("ef34", "\U0000EF34");
            _s.Add("ef37", "\U0000EF37");
            _s.Add("ef73", "\U0000EF73");
            _s.Add("ef5f", "\U0000EF5F");
            _s.Add("ef97", "\U0000EF97");
            _s.Add("ef50", "\U0000EF50");
            _s.Add("ef60", "\U0000EF60");
            _s.Add("ee69", "\U0000EE69");
            _s.Add("eeb9", "\U0000EEB9");
            _s.Add("eed9", "\U0000EED9");
            _s.Add("eefd", "\U0000EEFD");
            _s.Add("ef6b", "\U0000EF6B");
            _s.Add("eea5", "\U0000EEA5");
            _s.Add("ee89", "\U0000EE89");
            _s.Add("ef62", "\U0000EF62");
            _s.Add("ee98", "\U0000EE98");
            _s.Add("ee7a", "\U0000EE7A");
            _s.Add("eeff", "\U0000EEFF");
            _s.Add("ef4a", "\U0000EF4A");
            _s.Add("ef67", "\U0000EF67");
            _s.Add("eecf", "\U0000EECF");
            _s.Add("eebb", "\U0000EEBB");
            _s.Add("ef1e", "\U0000EF1E");
            _s.Add("eed0", "\U0000EED0");
            _s.Add("efb5", "\U0000EFB5");
            _s.Add("eef6", "\U0000EEF6");
            _s.Add("efa7", "\U0000EFA7");
            _s.Add("ef0d", "\U0000EF0D");
            _s.Add("eefe", "\U0000EEFE");
            _s.Add("ef96", "\U0000EF96");
            _s.Add("ef33", "\U0000EF33");
            _s.Add("ee68", "\U0000EE68");
            _s.Add("eea8", "\U0000EEA8");
            _s.Add("efb1", "\U0000EFB1");
            _s.Add("eec6", "\U0000EEC6");
            _s.Add("ef42", "\U0000EF42");
            _s.Add("eec3", "\U0000EEC3");
            _s.Add("ee8c", "\U0000EE8C");
            _s.Add("eed3", "\U0000EED3");
            _s.Add("ee59", "\U0000EE59");
            _s.Add("ee65", "\U0000EE65");
            _s.Add("ef9e", "\U0000EF9E");
            _s.Add("ef3b", "\U0000EF3B");
            _s.Add("ee70", "\U0000EE70");
            _s.Add("eedf", "\U0000EEDF");
            _s.Add("eed1", "\U0000EED1");
            _s.Add("ee5c", "\U0000EE5C");
            _s.Add("ef77", "\U0000EF77");
            _s.Add("ef2d", "\U0000EF2D");
            _s.Add("ef94", "\U0000EF94");
            _s.Add("eee4", "\U0000EEE4");
            _s.Add("ef6f", "\U0000EF6F");
            _s.Add("eea1", "\U0000EEA1");
            _s.Add("ef64", "\U0000EF64");
            _s.Add("ef78", "\U0000EF78");
            _s.Add("eea2", "\U0000EEA2");
            _s.Add("ee71", "\U0000EE71");
            _s.Add("ef82", "\U0000EF82");
            _s.Add("ee6b", "\U0000EE6B");
            _s.Add("efd6", "\U0000EFD6");
            _s.Add("eee8", "\U0000EEE8");
            _s.Add("ef1c", "\U0000EF1C");
            _s.Add("efca", "\U0000EFCA");
            _s.Add("ef6e", "\U0000EF6E");
            _s.Add("eedc", "\U0000EEDC");
            _s.Add("ef5d", "\U0000EF5D");
            _s.Add("ef0e", "\U0000EF0E");
            _s.Add("ef8f", "\U0000EF8F");
            _s.Add("ef17", "\U0000EF17");
            _s.Add("eec9", "\U0000EEC9");
            _s.Add("ee6c", "\U0000EE6C");
            _s.Add("ee8d", "\U0000EE8D");
            _s.Add("eeaa", "\U0000EEAA");
            _s.Add("eef2", "\U0000EEF2");
            _s.Add("ef5a", "\U0000EF5A");
            _s.Add("ef61", "\U0000EF61");
            _s.Add("ee62", "\U0000EE62");
            _s.Add("ef2b", "\U0000EF2B");
            _s.Add("ef2e", "\U0000EF2E");
            _s.Add("ee9f", "\U0000EE9F");
            _s.Add("efb8", "\U0000EFB8");
            _s.Add("ee93", "\U0000EE93");
            _s.Add("ef7c", "\U0000EF7C");
            _s.Add("eec5", "\U0000EEC5");
            _s.Add("eeed", "\U0000EEED");
            _s.Add("eef0", "\U0000EEF0");
            _s.Add("ef20", "\U0000EF20");
            _s.Add("ef69", "\U0000EF69");
            _s.Add("ef7d", "\U0000EF7D");
            _s.Add("efa9", "\U0000EFA9");
            _s.Add("ef1a", "\U0000EF1A");
            _s.Add("ef22", "\U0000EF22");
            _s.Add("ef4c", "\U0000EF4C");
            _s.Add("ee75", "\U0000EE75");
            _s.Add("efa8", "\U0000EFA8");
            _s.Add("eeef", "\U0000EEEF");
            _s.Add("ee64", "\U0000EE64");
            _s.Add("ef6d", "\U0000EF6D");
            _s.Add("efa0", "\U0000EFA0");
            _s.Add("ef53", "\U0000EF53");
            _s.Add("ef72", "\U0000EF72");
            _s.Add("ef56", "\U0000EF56");
            _s.Add("efcc", "\U0000EFCC");
            _s.Add("eea7", "\U0000EEA7");
            _s.Add("eef7", "\U0000EEF7");
            _s.Add("ef65", "\U0000EF65");
            _s.Add("ef41", "\U0000EF41");
            _s.Add("eec2", "\U0000EEC2");
            _s.Add("ef26", "\U0000EF26");
            _s.Add("ef92", "\U0000EF92");
            _s.Add("efd4", "\U0000EFD4");
            _s.Add("eeee", "\U0000EEEE");
            _s.Add("ef52", "\U0000EF52");
            _s.Add("eede", "\U0000EEDE");
            _s.Add("ef2a", "\U0000EF2A");
            _s.Add("ef36", "\U0000EF36");
            _s.Add("eefb", "\U0000EEFB");
            _s.Add("ef2c", "\U0000EF2C");
            _s.Add("ef55", "\U0000EF55");
            _s.Add("eee5", "\U0000EEE5");
            _s.Add("ef81", "\U0000EF81");
            _s.Add("ef70", "\U0000EF70");
            _s.Add("ee9c", "\U0000EE9C");
            _s.Add("efae", "\U0000EFAE");
            _s.Add("ef1f", "\U0000EF1F");
            _s.Add("ee9e", "\U0000EE9E");
            _s.Add("ef8a", "\U0000EF8A");
            _s.Add("efb3", "\U0000EFB3");
            _s.Add("efcd", "\U0000EFCD");
            _s.Add("efaf", "\U0000EFAF");
            _s.Add("ee86", "\U0000EE86");
            _s.Add("ee8e", "\U0000EE8E");
            _s.Add("ef7e", "\U0000EF7E");
            _s.Add("efd3", "\U0000EFD3");
            _s.Add("eea4", "\U0000EEA4");
            _s.Add("ef57", "\U0000EF57");
            _s.Add("ef01", "\U0000EF01");
            _s.Add("ee7b", "\U0000EE7B");
            _s.Add("ee88", "\U0000EE88");
            _s.Add("ef46", "\U0000EF46");
            _s.Add("ee6e", "\U0000EE6E");
            _s.Add("efa6", "\U0000EFA6");
            _s.Add("efa2", "\U0000EFA2");
            _s.Add("eee9", "\U0000EEE9");
            _s.Add("ef90", "\U0000EF90");
            _s.Add("ef35", "\U0000EF35");
            _s.Add("ef71", "\U0000EF71");
            _s.Add("eea3", "\U0000EEA3");
            _s.Add("ef4b", "\U0000EF4B");
            _s.Add("efa5", "\U0000EFA5");
            _s.Add("eeb2", "\U0000EEB2");
            _s.Add("ef68", "\U0000EF68");
            _s.Add("ef3f", "\U0000EF3F");
            _s.Add("ef15", "\U0000EF15");
            _s.Add("ee95", "\U0000EE95");
            _s.Add("eef4", "\U0000EEF4");
            _s.Add("ee61", "\U0000EE61");
            _s.Add("ef66", "\U0000EF66");
            _s.Add("ef5e", "\U0000EF5E");
            _s.Add("efa4", "\U0000EFA4");
            _s.Add("ef0a", "\U0000EF0A");
            _s.Add("ef2f", "\U0000EF2F");
            _s.Add("ef00", "\U0000EF00");
            _s.Add("ef5c", "\U0000EF5C");
            _s.Add("ef30", "\U0000EF30");
            _s.Add("eeeb", "\U0000EEEB");
            _s.Add("efbf", "\U0000EFBF");
            _s.Add("ef07", "\U0000EF07");
            _s.Add("ee60", "\U0000EE60");
            _s.Add("ee7e", "\U0000EE7E");
            _s.Add("eec8", "\U0000EEC8");
            _s.Add("ef5b", "\U0000EF5B");
            _s.Add("ef86", "\U0000EF86");
            _s.Add("efc6", "\U0000EFC6");
            _s.Add("eebf", "\U0000EEBF");
            _s.Add("eeb0", "\U0000EEB0");
            _s.Add("eee0", "\U0000EEE0");
            _s.Add("ef11", "\U0000EF11");
            _s.Add("ee92", "\U0000EE92");
            _s.Add("ef18", "\U0000EF18");
            _s.Add("efad", "\U0000EFAD");
            _s.Add("eebe", "\U0000EEBE");
            _s.Add("efab", "\U0000EFAB");
            _s.Add("efb6", "\U0000EFB6");
            _s.Add("eec4", "\U0000EEC4");
            _s.Add("ef06", "\U0000EF06");
            _s.Add("ef9a", "\U0000EF9A");
            _s.Add("ef74", "\U0000EF74");
            _s.Add("efc1", "\U0000EFC1");
            _s.Add("ee80", "\U0000EE80");
            _s.Add("eeb1", "\U0000EEB1");
            _s.Add("ee99", "\U0000EE99");
            _s.Add("ef16", "\U0000EF16");
            _s.Add("eed8", "\U0000EED8");
            _s.Add("eeab", "\U0000EEAB");
            _s.Add("ee78", "\U0000EE78");
            _s.Add("ef0f", "\U0000EF0F");
            _s.Add("ee82", "\U0000EE82");
            _s.Add("ee56", "\U0000EE56");
            _s.Add("ef29", "\U0000EF29");
            _s.Add("eeb8", "\U0000EEB8");
            _s.Add("ef38", "\U0000EF38");
            _s.Add("ef49", "\U0000EF49");
            _s.Add("ee5e", "\U0000EE5E");
            _s.Add("ef0c", "\U0000EF0C");
            _s.Add("eedd", "\U0000EEDD");
            _s.Add("ef04", "\U0000EF04");
            _s.Add("ef12", "\U0000EF12");
            _s.Add("eef3", "\U0000EEF3");
            _s.Add("ee87", "\U0000EE87");
            _s.Add("ef54", "\U0000EF54");
            _s.Add("ee7d", "\U0000EE7D");
            _s.Add("eeaf", "\U0000EEAF");
            _s.Add("ef48", "\U0000EF48");
            _s.Add("eee3", "\U0000EEE3");
            _s.Add("efb7", "\U0000EFB7");
            _s.Add("ee97", "\U0000EE97");
            _s.Add("ee85", "\U0000EE85");
            _s.Add("ef7b", "\U0000EF7B");
            _s.Add("efd0", "\U0000EFD0");
            _s.Add("ef21", "\U0000EF21");
            _s.Add("ef7f", "\U0000EF7F");
            _s.Add("eecd", "\U0000EECD");
            _s.Add("ef0b", "\U0000EF0B");
            _s.Add("ef4e", "\U0000EF4E");
            _s.Add("efa1", "\U0000EFA1");
            _s.Add("ef9d", "\U0000EF9D");
            _s.Add("ee4d", "\U0000EE4D");
            _s.Add("ee4a", "\U0000EE4A");
            _s.Add("efd1", "\U0000EFD1");
            _s.Add("ee4e", "\U0000EE4E");
            _s.Add("ef8b", "\U0000EF8B");
            _s.Add("ee79", "\U0000EE79");
            _s.Add("eeb6", "\U0000EEB6");
            _s.Add("eeec", "\U0000EEEC");
            _s.Add("ef08", "\U0000EF08");
            _s.Add("ee73", "\U0000EE73");
            _s.Add("ee7f", "\U0000EE7F");
            _s.Add("ef98", "\U0000EF98");
            _s.Add("efd2", "\U0000EFD2");
            _s.Add("eecb", "\U0000EECB");
            _s.Add("eed2", "\U0000EED2");
            _s.Add("ee7c", "\U0000EE7C");
            _s.Add("ee8b", "\U0000EE8B");
            _s.Add("ef02", "\U0000EF02");
            _s.Add("eeca", "\U0000EECA");
            _s.Add("ef9b", "\U0000EF9B");
            _s.Add("ee81", "\U0000EE81");
            _s.Add("efb4", "\U0000EFB4");
            _s.Add("ef76", "\U0000EF76");
            _s.Add("eea6", "\U0000EEA6");
            _s.Add("efaa", "\U0000EFAA");
            _s.Add("ef99", "\U0000EF99");
            _s.Add("ef09", "\U0000EF09");
            _s.Add("efc4", "\U0000EFC4");
            _s.Add("ef51", "\U0000EF51");
            _s.Add("eef1", "\U0000EEF1");
            _s.Add("ee8a", "\U0000EE8A");
            _s.Add("eee7", "\U0000EEE7");
            _s.Add("eeb3", "\U0000EEB3");
            _s.Add("ee4c", "\U0000EE4C");
            _s.Add("eeb7", "\U0000EEB7");
            _s.Add("efbe", "\U0000EFBE");
            _s.Add("efcb", "\U0000EFCB");
            _s.Add("ef40", "\U0000EF40");
            _s.Add("eecc", "\U0000EECC");
            _s.Add("ef84", "\U0000EF84");
            _s.Add("ee5d", "\U0000EE5D");
            _s.Add("efce", "\U0000EFCE");
            _s.Add("eeea", "\U0000EEEA");
            _s.Add("efc0", "\U0000EFC0");
            _s.Add("eece", "\U0000EECE");
            _s.Add("efc3", "\U0000EFC3");
            _s.Add("ef28", "\U0000EF28");
            _s.Add("efc5", "\U0000EFC5");
            _s.Add("eea0", "\U0000EEA0");
            _s.Add("ef91", "\U0000EF91");
            _s.Add("ef1d", "\U0000EF1D");
            _s.Add("f079", "\U0000F079");
            _s.Add("f075", "\U0000F075");
            _s.Add("f02e", "\U0000F02E");
            _s.Add("f02b", "\U0000F02B");
            _s.Add("efff", "\U0000EFFF");
            _s.Add("f002", "\U0000F002");
            _s.Add("efdf", "\U0000EFDF");
            _s.Add("f077", "\U0000F077");
            _s.Add("f04f", "\U0000F04F");
            _s.Add("f038", "\U0000F038");
            _s.Add("efe8", "\U0000EFE8");
            _s.Add("efd8", "\U0000EFD8");
            _s.Add("f00e", "\U0000F00E");
            _s.Add("f037", "\U0000F037");
            _s.Add("f07f", "\U0000F07F");
            _s.Add("f040", "\U0000F040");
            _s.Add("f04e", "\U0000F04E");
            _s.Add("f066", "\U0000F066");
            _s.Add("f036", "\U0000F036");
            _s.Add("efe9", "\U0000EFE9");
            _s.Add("f004", "\U0000F004");
            _s.Add("f032", "\U0000F032");
            _s.Add("f060", "\U0000F060");
            _s.Add("efe7", "\U0000EFE7");
            _s.Add("f02f", "\U0000F02F");
            _s.Add("efdc", "\U0000EFDC");
            _s.Add("f06d", "\U0000F06D");
            _s.Add("effa", "\U0000EFFA");
            _s.Add("f042", "\U0000F042");
            _s.Add("f03e", "\U0000F03E");
            _s.Add("f051", "\U0000F051");
            _s.Add("efe0", "\U0000EFE0");
            _s.Add("effc", "\U0000EFFC");
            _s.Add("f001", "\U0000F001");
            _s.Add("f061", "\U0000F061");
            _s.Add("f00b", "\U0000F00B");
            _s.Add("f00a", "\U0000F00A");
            _s.Add("f00c", "\U0000F00C");
            _s.Add("f007", "\U0000F007");
            _s.Add("f031", "\U0000F031");
            _s.Add("f009", "\U0000F009");
            _s.Add("f008", "\U0000F008");
            _s.Add("f065", "\U0000F065");
            _s.Add("f076", "\U0000F076");
            _s.Add("f030", "\U0000F030");
            _s.Add("f064", "\U0000F064");
            _s.Add("f03b", "\U0000F03B");
            _s.Add("efda", "\U0000EFDA");
            _s.Add("f071", "\U0000F071");
            _s.Add("f05f", "\U0000F05F");
            _s.Add("f003", "\U0000F003");
            _s.Add("efe5", "\U0000EFE5");
            _s.Add("f03f", "\U0000F03F");
            _s.Add("f063", "\U0000F063");
            _s.Add("f06c", "\U0000F06C");
            _s.Add("f050", "\U0000F050");
            _s.Add("efea", "\U0000EFEA");
            _s.Add("f070", "\U0000F070");
            _s.Add("f006", "\U0000F006");
            _s.Add("f07e", "\U0000F07E");
            _s.Add("f06e", "\U0000F06E");
            _s.Add("efde", "\U0000EFDE");
            _s.Add("f035", "\U0000F035");
            _s.Add("f06f", "\U0000F06F");
            _s.Add("f052", "\U0000F052");
            _s.Add("effb", "\U0000EFFB");
            _s.Add("f02c", "\U0000F02C");
            _s.Add("effe", "\U0000EFFE");
            _s.Add("efe6", "\U0000EFE6");
            _s.Add("f000", "\U0000F000");
            _s.Add("f034", "\U0000F034");
            _s.Add("f033", "\U0000F033");
            _s.Add("f039", "\U0000F039");
            _s.Add("f041", "\U0000F041");
            _s.Add("f027", "\U0000F027");
            _s.Add("f03c", "\U0000F03C");
            _s.Add("f03d", "\U0000F03D");
            _s.Add("f03a", "\U0000F03A");
            _s.Add("effd", "\U0000EFFD");
            _s.Add("f00d", "\U0000F00D");
            _s.Add("f02a", "\U0000F02A");
            _s.Add("f062", "\U0000F062");
            _s.Add("f02d", "\U0000F02D");
            _s.Add("eff7", "\U0000EFF7");
            _s.Add("eff8", "\U0000EFF8");
            _s.Add("eff5", "\U0000EFF5");
            _s.Add("eff2", "\U0000EFF2");
            _s.Add("f01f", "\U0000F01F");
            _s.Add("efee", "\U0000EFEE");
            _s.Add("f011", "\U0000F011");
            _s.Add("f045", "\U0000F045");
            _s.Add("f015", "\U0000F015");
            _s.Add("f01e", "\U0000F01E");
            _s.Add("f020", "\U0000F020");
            _s.Add("f055", "\U0000F055");
            _s.Add("eff6", "\U0000EFF6");
            _s.Add("efe4", "\U0000EFE4");
            _s.Add("f048", "\U0000F048");
            _s.Add("f013", "\U0000F013");
            _s.Add("f078", "\U0000F078");
            _s.Add("efef", "\U0000EFEF");
            _s.Add("f043", "\U0000F043");
            _s.Add("f074", "\U0000F074");
            _s.Add("f024", "\U0000F024");
            _s.Add("efe3", "\U0000EFE3");
            _s.Add("f04b", "\U0000F04B");
            _s.Add("f05b", "\U0000F05B");
            _s.Add("efd7", "\U0000EFD7");
            _s.Add("f025", "\U0000F025");
            _s.Add("f047", "\U0000F047");
            _s.Add("f05c", "\U0000F05C");
            _s.Add("f06b", "\U0000F06B");
            _s.Add("f05e", "\U0000F05E");
            _s.Add("f026", "\U0000F026");
            _s.Add("f01a", "\U0000F01A");
            _s.Add("f01d", "\U0000F01D");
            _s.Add("efdb", "\U0000EFDB");
            _s.Add("eff1", "\U0000EFF1");
            _s.Add("f04c", "\U0000F04C");
            _s.Add("f053", "\U0000F053");
            _s.Add("f017", "\U0000F017");
            _s.Add("f04a", "\U0000F04A");
            _s.Add("f056", "\U0000F056");
            _s.Add("f057", "\U0000F057");
            _s.Add("f058", "\U0000F058");
            _s.Add("f059", "\U0000F059");
            _s.Add("f05a", "\U0000F05A");
            _s.Add("efe1", "\U0000EFE1");
            _s.Add("f07b", "\U0000F07B");
            _s.Add("eff9", "\U0000EFF9");
            _s.Add("f010", "\U0000F010");
            _s.Add("eff4", "\U0000EFF4");
            _s.Add("f06a", "\U0000F06A");
            _s.Add("f044", "\U0000F044");
            _s.Add("f005", "\U0000F005");
            _s.Add("f029", "\U0000F029");
            _s.Add("f014", "\U0000F014");
            _s.Add("f01c", "\U0000F01C");
            _s.Add("eff3", "\U0000EFF3");
            _s.Add("f01b", "\U0000F01B");
            _s.Add("eff0", "\U0000EFF0");
            _s.Add("f018", "\U0000F018");
            _s.Add("f012", "\U0000F012");
            _s.Add("efeb", "\U0000EFEB");
            _s.Add("f016", "\U0000F016");
            _s.Add("efdd", "\U0000EFDD");
            _s.Add("f069", "\U0000F069");
            _s.Add("f04d", "\U0000F04D");
            _s.Add("efec", "\U0000EFEC");
            _s.Add("f07d", "\U0000F07D");
            _s.Add("efd9", "\U0000EFD9");
            _s.Add("f022", "\U0000F022");
            _s.Add("f07a", "\U0000F07A");
            _s.Add("f028", "\U0000F028");
            _s.Add("f068", "\U0000F068");
            _s.Add("f07c", "\U0000F07C");
            _s.Add("f054", "\U0000F054");
            _s.Add("efed", "\U0000EFED");
            _s.Add("f021", "\U0000F021");
            _s.Add("f046", "\U0000F046");
            _s.Add("f023", "\U0000F023");
            _s.Add("f049", "\U0000F049");
            _s.Add("f067", "\U0000F067");
            _s.Add("f072", "\U0000F072");
            _s.Add("f073", "\U0000F073");
            _s.Add("f05d", "\U0000F05D");
            _s.Add("f00f", "\U0000F00F");
            _s.Add("efe2", "\U0000EFE2");
            _s.Add("f019", "\U0000F019");
            _s.Add("f2cc", "\U0000F2CC");
            _s.Add("f2aa", "\U0000F2AA");
            _s.Add("f23f", "\U0000F23F");
            _s.Add("f0b5", "\U0000F0B5");
            _s.Add("f0b7", "\U0000F0B7");
            _s.Add("f0b6", "\U0000F0B6");
            _s.Add("f0da", "\U0000F0DA");
            _s.Add("f0a3", "\U0000F0A3");
            _s.Add("f0a2", "\U0000F0A2");
            _s.Add("f0b1", "\U0000F0B1");
            _s.Add("f0e3", "\U0000F0E3");
            _s.Add("f09a", "\U0000F09A");
            _s.Add("f084", "\U0000F084");
            _s.Add("f0af", "\U0000F0AF");
            _s.Add("f0dd", "\U0000F0DD");
            _s.Add("f0a5", "\U0000F0A5");
            _s.Add("f0d4", "\U0000F0D4");
            _s.Add("f0c2", "\U0000F0C2");
            _s.Add("f087", "\U0000F087");
            _s.Add("f0e1", "\U0000F0E1");
            _s.Add("f08f", "\U0000F08F");
            _s.Add("f0d1", "\U0000F0D1");
            _s.Add("f0d3", "\U0000F0D3");
            _s.Add("f098", "\U0000F098");
            _s.Add("f091", "\U0000F091");
            _s.Add("f0c1", "\U0000F0C1");
            _s.Add("f0e7", "\U0000F0E7");
            _s.Add("f09b", "\U0000F09B");
            _s.Add("f0a1", "\U0000F0A1");
            _s.Add("f0e5", "\U0000F0E5");
            _s.Add("f0cf", "\U0000F0CF");
            _s.Add("f0a0", "\U0000F0A0");
            _s.Add("f09e", "\U0000F09E");
            _s.Add("f095", "\U0000F095");
            _s.Add("f090", "\U0000F090");
            _s.Add("f0ad", "\U0000F0AD");
            _s.Add("f0b3", "\U0000F0B3");
            _s.Add("f0b8", "\U0000F0B8");
            _s.Add("f0a6", "\U0000F0A6");
            _s.Add("f08b", "\U0000F08B");
            _s.Add("f0e4", "\U0000F0E4");
            _s.Add("f0aa", "\U0000F0AA");
            _s.Add("f0d8", "\U0000F0D8");
            _s.Add("f0de", "\U0000F0DE");
            _s.Add("f0e2", "\U0000F0E2");
            _s.Add("f086", "\U0000F086");
            _s.Add("f088", "\U0000F088");
            _s.Add("f0a4", "\U0000F0A4");
            _s.Add("f0c0", "\U0000F0C0");
            _s.Add("f089", "\U0000F089");
            _s.Add("f0c3", "\U0000F0C3");
            _s.Add("f0ba", "\U0000F0BA");
            _s.Add("f0b0", "\U0000F0B0");
            _s.Add("f0b2", "\U0000F0B2");
            _s.Add("f093", "\U0000F093");
            _s.Add("f0b9", "\U0000F0B9");
            _s.Add("f0ae", "\U0000F0AE");
            _s.Add("f0df", "\U0000F0DF");
            _s.Add("f09c", "\U0000F09C");
            _s.Add("f0ca", "\U0000F0CA");
            _s.Add("f0a8", "\U0000F0A8");
            _s.Add("f08d", "\U0000F08D");
            _s.Add("f0bb", "\U0000F0BB");
            _s.Add("f0e8", "\U0000F0E8");
            _s.Add("f0bc", "\U0000F0BC");
            _s.Add("f0ce", "\U0000F0CE");
            _s.Add("f0d5", "\U0000F0D5");
            _s.Add("f0d0", "\U0000F0D0");
            _s.Add("f097", "\U0000F097");
            _s.Add("f09d", "\U0000F09D");
            _s.Add("f0cc", "\U0000F0CC");
            _s.Add("f0bf", "\U0000F0BF");
            _s.Add("f0c6", "\U0000F0C6");
            _s.Add("f099", "\U0000F099");
            _s.Add("f0c7", "\U0000F0C7");
            _s.Add("f0ac", "\U0000F0AC");
            _s.Add("f094", "\U0000F094");
            _s.Add("f0dc", "\U0000F0DC");
            _s.Add("f085", "\U0000F085");
            _s.Add("f0d2", "\U0000F0D2");
            _s.Add("f0ab", "\U0000F0AB");
            _s.Add("f0cd", "\U0000F0CD");
            _s.Add("f0a7", "\U0000F0A7");
            _s.Add("f09f", "\U0000F09F");
            _s.Add("f0c8", "\U0000F0C8");
            _s.Add("f096", "\U0000F096");
            _s.Add("f0d6", "\U0000F0D6");
            _s.Add("f0cb", "\U0000F0CB");
            _s.Add("f08a", "\U0000F08A");
            _s.Add("f0c9", "\U0000F0C9");
            _s.Add("f0b4", "\U0000F0B4");
            _s.Add("f0e0", "\U0000F0E0");
            _s.Add("f0d9", "\U0000F0D9");
            _s.Add("f0bd", "\U0000F0BD");
            _s.Add("f0c5", "\U0000F0C5");
            _s.Add("f08c", "\U0000F08C");
            _s.Add("f0be", "\U0000F0BE");
            _s.Add("f0d7", "\U0000F0D7");
            _s.Add("f0e6", "\U0000F0E6");
            _s.Add("f0db", "\U0000F0DB");
            _s.Add("f0c4", "\U0000F0C4");
            _s.Add("f0a9", "\U0000F0A9");
            _s.Add("f08e", "\U0000F08E");
            _s.Add("f092", "\U0000F092");
            _s.Add("f1cc", "\U0000F1CC");
            _s.Add("f1cb", "\U0000F1CB");
            _s.Add("f19f", "\U0000F19F");
            _s.Add("f11e", "\U0000F11E");
            _s.Add("f1c8", "\U0000F1C8");
            _s.Add("f1c7", "\U0000F1C7");
            _s.Add("f10b", "\U0000F10B");
            _s.Add("f1ae", "\U0000F1AE");
            _s.Add("f192", "\U0000F192");
            _s.Add("f110", "\U0000F110");
            _s.Add("f1b2", "\U0000F1B2");
            _s.Add("f184", "\U0000F184");
            _s.Add("f16b", "\U0000F16B");
            _s.Add("f165", "\U0000F165");
            _s.Add("f122", "\U0000F122");
            _s.Add("f16f", "\U0000F16F");
            _s.Add("f1a6", "\U0000F1A6");
            _s.Add("f136", "\U0000F136");
            _s.Add("f19b", "\U0000F19B");
            _s.Add("f111", "\U0000F111");
            _s.Add("f169", "\U0000F169");
            _s.Add("f1b0", "\U0000F1B0");
            _s.Add("f117", "\U0000F117");
            _s.Add("f1c1", "\U0000F1C1");
            _s.Add("f109", "\U0000F109");
            _s.Add("f10c", "\U0000F10C");
            _s.Add("f10d", "\U0000F10D");
            _s.Add("f13b", "\U0000F13B");
            _s.Add("f141", "\U0000F141");
            _s.Add("f0ec", "\U0000F0EC");
            _s.Add("f168", "\U0000F168");
            _s.Add("f10f", "\U0000F10F");
            _s.Add("f126", "\U0000F126");
            _s.Add("f1b4", "\U0000F1B4");
            _s.Add("f14a", "\U0000F14A");
            _s.Add("f140", "\U0000F140");
            _s.Add("f1ca", "\U0000F1CA");
            _s.Add("f197", "\U0000F197");
            _s.Add("f158", "\U0000F158");
            _s.Add("f179", "\U0000F179");
            _s.Add("f11c", "\U0000F11C");
            _s.Add("f1bb", "\U0000F1BB");
            _s.Add("f16d", "\U0000F16D");
            _s.Add("f13a", "\U0000F13A");
            _s.Add("f182", "\U0000F182");
            _s.Add("f1c9", "\U0000F1C9");
            _s.Add("f16c", "\U0000F16C");
            _s.Add("f1a4", "\U0000F1A4");
            _s.Add("f13e", "\U0000F13E");
            _s.Add("f105", "\U0000F105");
            _s.Add("f143", "\U0000F143");
            _s.Add("f144", "\U0000F144");
            _s.Add("f17a", "\U0000F17A");
            _s.Add("f131", "\U0000F131");
            _s.Add("f172", "\U0000F172");
            _s.Add("f130", "\U0000F130");
            _s.Add("f177", "\U0000F177");
            _s.Add("f12f", "\U0000F12F");
            _s.Add("f113", "\U0000F113");
            _s.Add("f1b5", "\U0000F1B5");
            _s.Add("f10a", "\U0000F10A");
            _s.Add("f104", "\U0000F104");
            _s.Add("f1bf", "\U0000F1BF");
            _s.Add("f108", "\U0000F108");
            _s.Add("f188", "\U0000F188");
            _s.Add("f166", "\U0000F166");
            _s.Add("f18e", "\U0000F18E");
            _s.Add("f173", "\U0000F173");
            _s.Add("f159", "\U0000F159");
            _s.Add("f127", "\U0000F127");
            _s.Add("f102", "\U0000F102");
            _s.Add("f134", "\U0000F134");
            _s.Add("f1aa", "\U0000F1AA");
            _s.Add("f147", "\U0000F147");
            _s.Add("f10e", "\U0000F10E");
            _s.Add("f19d", "\U0000F19D");
            _s.Add("f181", "\U0000F181");
            _s.Add("f186", "\U0000F186");
            _s.Add("f129", "\U0000F129");
            _s.Add("f12b", "\U0000F12B");
            _s.Add("f19e", "\U0000F19E");
            _s.Add("f18f", "\U0000F18F");
            _s.Add("f13f", "\U0000F13F");
            _s.Add("f14f", "\U0000F14F");
            _s.Add("f11f", "\U0000F11F");
            _s.Add("f13c", "\U0000F13C");
            _s.Add("f11a", "\U0000F11A");
            _s.Add("f16e", "\U0000F16E");
            _s.Add("f146", "\U0000F146");
            _s.Add("f128", "\U0000F128");
            _s.Add("f1b9", "\U0000F1B9");
            _s.Add("f18c", "\U0000F18C");
            _s.Add("f115", "\U0000F115");
            _s.Add("f1be", "\U0000F1BE");
            _s.Add("f0ea", "\U0000F0EA");
            _s.Add("f138", "\U0000F138");
            _s.Add("f17e", "\U0000F17E");
            _s.Add("f112", "\U0000F112");
            _s.Add("f107", "\U0000F107");
            _s.Add("f178", "\U0000F178");
            _s.Add("f12e", "\U0000F12E");
            _s.Add("f167", "\U0000F167");
            _s.Add("f14c", "\U0000F14C");
            _s.Add("f18a", "\U0000F18A");
            _s.Add("f198", "\U0000F198");
            _s.Add("f1ab", "\U0000F1AB");
            _s.Add("f106", "\U0000F106");
            _s.Add("f1a1", "\U0000F1A1");
            _s.Add("f103", "\U0000F103");
            _s.Add("f16a", "\U0000F16A");
            _s.Add("f1b7", "\U0000F1B7");
            _s.Add("f15c", "\U0000F15C");
            _s.Add("f0ed", "\U0000F0ED");
            _s.Add("f150", "\U0000F150");
            _s.Add("f187", "\U0000F187");
            _s.Add("f15d", "\U0000F15D");
            _s.Add("f17b", "\U0000F17B");
            _s.Add("f193", "\U0000F193");
            _s.Add("f15b", "\U0000F15B");
            _s.Add("f1a3", "\U0000F1A3");
            _s.Add("f0f8", "\U0000F0F8");
            _s.Add("f1ba", "\U0000F1BA");
            _s.Add("f125", "\U0000F125");
            _s.Add("f15a", "\U0000F15A");
            _s.Add("f1b8", "\U0000F1B8");
            _s.Add("f101", "\U0000F101");
            _s.Add("f18b", "\U0000F18B");
            _s.Add("f0ff", "\U0000F0FF");
            _s.Add("f19c", "\U0000F19C");
            _s.Add("f132", "\U0000F132");
            _s.Add("f195", "\U0000F195");
            _s.Add("f119", "\U0000F119");
            _s.Add("f1b3", "\U0000F1B3");
            _s.Add("f120", "\U0000F120");
            _s.Add("f0f9", "\U0000F0F9");
            _s.Add("f135", "\U0000F135");
            _s.Add("f0f3", "\U0000F0F3");
            _s.Add("f0fd", "\U0000F0FD");
            _s.Add("f14b", "\U0000F14B");
            _s.Add("f11b", "\U0000F11B");
            _s.Add("f1b6", "\U0000F1B6");
            _s.Add("f17f", "\U0000F17F");
            _s.Add("f161", "\U0000F161");
            _s.Add("f0f1", "\U0000F0F1");
            _s.Add("f180", "\U0000F180");
            _s.Add("f174", "\U0000F174");
            _s.Add("f151", "\U0000F151");
            _s.Add("f164", "\U0000F164");
            _s.Add("f196", "\U0000F196");
            _s.Add("f15f", "\U0000F15F");
            _s.Add("f0f2", "\U0000F0F2");
            _s.Add("f1ac", "\U0000F1AC");
            _s.Add("f124", "\U0000F124");
            _s.Add("f118", "\U0000F118");
            _s.Add("f17d", "\U0000F17D");
            _s.Add("f0fa", "\U0000F0FA");
            _s.Add("f12a", "\U0000F12A");
            _s.Add("f163", "\U0000F163");
            _s.Add("f133", "\U0000F133");
            _s.Add("f185", "\U0000F185");
            _s.Add("f1a9", "\U0000F1A9");
            _s.Add("f123", "\U0000F123");
            _s.Add("f0fb", "\U0000F0FB");
            _s.Add("f0f4", "\U0000F0F4");
            _s.Add("f0eb", "\U0000F0EB");
            _s.Add("f149", "\U0000F149");
            _s.Add("f160", "\U0000F160");
            _s.Add("f1a2", "\U0000F1A2");
            _s.Add("f0ee", "\U0000F0EE");
            _s.Add("f100", "\U0000F100");
            _s.Add("f18d", "\U0000F18D");
            _s.Add("f0f5", "\U0000F0F5");
            _s.Add("f19a", "\U0000F19A");
            _s.Add("f0fc", "\U0000F0FC");
            _s.Add("f12d", "\U0000F12D");
            _s.Add("f0fe", "\U0000F0FE");
            _s.Add("f1bd", "\U0000F1BD");
            _s.Add("f1b1", "\U0000F1B1");
            _s.Add("f0f0", "\U0000F0F0");
            _s.Add("f1af", "\U0000F1AF");
            _s.Add("f171", "\U0000F171");
            _s.Add("f199", "\U0000F199");
            _s.Add("f17c", "\U0000F17C");
            _s.Add("f139", "\U0000F139");
            _s.Add("f170", "\U0000F170");
            _s.Add("f148", "\U0000F148");
            _s.Add("f116", "\U0000F116");
            _s.Add("f13d", "\U0000F13D");
            _s.Add("f12c", "\U0000F12C");
            _s.Add("f175", "\U0000F175");
            _s.Add("f162", "\U0000F162");
            _s.Add("f142", "\U0000F142");
            _s.Add("f137", "\U0000F137");
            _s.Add("f14d", "\U0000F14D");
            _s.Add("f11d", "\U0000F11D");
            _s.Add("f1a0", "\U0000F1A0");
            _s.Add("f0f6", "\U0000F0F6");
            _s.Add("f114", "\U0000F114");
            _s.Add("f15e", "\U0000F15E");
            _s.Add("f191", "\U0000F191");
            _s.Add("f0ef", "\U0000F0EF");
            _s.Add("f14e", "\U0000F14E");
            _s.Add("f176", "\U0000F176");
            _s.Add("f183", "\U0000F183");
            _s.Add("f1c0", "\U0000F1C0");
            _s.Add("f1ad", "\U0000F1AD");
            _s.Add("f1c3", "\U0000F1C3");
            _s.Add("f1c4", "\U0000F1C4");
            _s.Add("f189", "\U0000F189");
            _s.Add("f1c2", "\U0000F1C2");
            _s.Add("f190", "\U0000F190");
            _s.Add("f145", "\U0000F145");
            _s.Add("f1c5", "\U0000F1C5");
            _s.Add("f0f7", "\U0000F0F7");
            _s.Add("f1c6", "\U0000F1C6");
            _s.Add("f1a5", "\U0000F1A5");
            _s.Add("f121", "\U0000F121");
            _s.Add("f1bc", "\U0000F1BC");
            _s.Add("f2ed", "\U0000F2ED");
            _s.Add("f2a8", "\U0000F2A8");
            _s.Add("f1e4", "\U0000F1E4");
            _s.Add("f218", "\U0000F218");
            _s.Add("f1e6", "\U0000F1E6");
            _s.Add("f1ce", "\U0000F1CE");
            _s.Add("f23a", "\U0000F23A");
            _s.Add("f274", "\U0000F274");
            _s.Add("f281", "\U0000F281");
            _s.Add("f296", "\U0000F296");
            _s.Add("f2dc", "\U0000F2DC");
            _s.Add("f246", "\U0000F246");
            _s.Add("f20f", "\U0000F20F");
            _s.Add("f240", "\U0000F240");
            _s.Add("f258", "\U0000F258");
            _s.Add("f29e", "\U0000F29E");
            _s.Add("f27f", "\U0000F27F");
            _s.Add("f2c0", "\U0000F2C0");
            _s.Add("f2d7", "\U0000F2D7");
            _s.Add("f263", "\U0000F263");
            _s.Add("f23e", "\U0000F23E");
            _s.Add("f21e", "\U0000F21E");
            _s.Add("f213", "\U0000F213");
            _s.Add("f222", "\U0000F222");
            _s.Add("f236", "\U0000F236");
            _s.Add("f1f2", "\U0000F1F2");
            _s.Add("f2fa", "\U0000F2FA");
            _s.Add("f27a", "\U0000F27A");
            _s.Add("f283", "\U0000F283");
            _s.Add("f297", "\U0000F297");
            _s.Add("f247", "\U0000F247");
            _s.Add("f257", "\U0000F257");
            _s.Add("f215", "\U0000F215");
            _s.Add("f278", "\U0000F278");
            _s.Add("f268", "\U0000F268");
            _s.Add("f1f3", "\U0000F1F3");
            _s.Add("f1dd", "\U0000F1DD");
            _s.Add("f282", "\U0000F282");
            _s.Add("f2f7", "\U0000F2F7");
            _s.Add("f214", "\U0000F214");
            _s.Add("f256", "\U0000F256");
            _s.Add("f2ad", "\U0000F2AD");
            _s.Add("f25c", "\U0000F25C");
            _s.Add("f2bf", "\U0000F2BF");
            _s.Add("f292", "\U0000F292");
            _s.Add("f2a3", "\U0000F2A3");
            _s.Add("f1d8", "\U0000F1D8");
            _s.Add("f1d5", "\U0000F1D5");
            _s.Add("f211", "\U0000F211");
            _s.Add("f212", "\U0000F212");
            _s.Add("f1d3", "\U0000F1D3");
            _s.Add("f1d9", "\U0000F1D9");
            _s.Add("f23b", "\U0000F23B");
            _s.Add("f290", "\U0000F290");
            _s.Add("f2cb", "\U0000F2CB");
            _s.Add("f2d9", "\U0000F2D9");
            _s.Add("f2d5", "\U0000F2D5");
            _s.Add("f231", "\U0000F231");
            _s.Add("f249", "\U0000F249");
            _s.Add("f1f4", "\U0000F1F4");
            _s.Add("f2b7", "\U0000F2B7");
            _s.Add("f2de", "\U0000F2DE");
            _s.Add("f1ee", "\U0000F1EE");
            _s.Add("f23c", "\U0000F23C");
            _s.Add("f229", "\U0000F229");
            _s.Add("f22b", "\U0000F22B");
            _s.Add("f2da", "\U0000F2DA");
            _s.Add("f235", "\U0000F235");
            _s.Add("f24f", "\U0000F24F");
            _s.Add("f1f8", "\U0000F1F8");
            _s.Add("f27d", "\U0000F27D");
            _s.Add("f1d1", "\U0000F1D1");
            _s.Add("f28e", "\U0000F28E");
            _s.Add("f1f9", "\U0000F1F9");
            _s.Add("f2bc", "\U0000F2BC");
            _s.Add("f269", "\U0000F269");
            _s.Add("f24b", "\U0000F24B");
            _s.Add("f24d", "\U0000F24D");
            _s.Add("f2f9", "\U0000F2F9");
            _s.Add("f221", "\U0000F221");
            _s.Add("f2c2", "\U0000F2C2");
            _s.Add("f2b1", "\U0000F2B1");
            _s.Add("f204", "\U0000F204");
            _s.Add("f29c", "\U0000F29C");
            _s.Add("f1fe", "\U0000F1FE");
            _s.Add("f224", "\U0000F224");
            _s.Add("f2d8", "\U0000F2D8");
            _s.Add("f223", "\U0000F223");
            _s.Add("f2b2", "\U0000F2B2");
            _s.Add("f2fd", "\U0000F2FD");
            _s.Add("f253", "\U0000F253");
            _s.Add("f28a", "\U0000F28A");
            _s.Add("f300", "\U0000F300");
            _s.Add("f206", "\U0000F206");
            _s.Add("f288", "\U0000F288");
            _s.Add("f2be", "\U0000F2BE");
            _s.Add("f1dc", "\U0000F1DC");
            _s.Add("f2b3", "\U0000F2B3");
            _s.Add("f25b", "\U0000F25B");
            _s.Add("f2e5", "\U0000F2E5");
            _s.Add("f1fa", "\U0000F1FA");
            _s.Add("f251", "\U0000F251");
            _s.Add("f289", "\U0000F289");
            _s.Add("f2e3", "\U0000F2E3");
            _s.Add("f29b", "\U0000F29B");
            _s.Add("f2f2", "\U0000F2F2");
            _s.Add("f2f3", "\U0000F2F3");
            _s.Add("f1e2", "\U0000F1E2");
            _s.Add("f1e0", "\U0000F1E0");
            _s.Add("f2a6", "\U0000F2A6");
            _s.Add("f255", "\U0000F255");
            _s.Add("f1ff", "\U0000F1FF");
            _s.Add("f210", "\U0000F210");
            _s.Add("f21f", "\U0000F21F");
            _s.Add("f2db", "\U0000F2DB");
            _s.Add("f1e8", "\U0000F1E8");
            _s.Add("f2ac", "\U0000F2AC");
            _s.Add("f254", "\U0000F254");
            _s.Add("f22d", "\U0000F22D");
            _s.Add("f220", "\U0000F220");
            _s.Add("f2f0", "\U0000F2F0");
            _s.Add("f22f", "\U0000F22F");
            _s.Add("f200", "\U0000F200");
            _s.Add("f27b", "\U0000F27B");
            _s.Add("f2b6", "\U0000F2B6");
            _s.Add("f2ef", "\U0000F2EF");
            _s.Add("f2ee", "\U0000F2EE");
            _s.Add("f234", "\U0000F234");
            _s.Add("f20e", "\U0000F20E");
            _s.Add("f2c3", "\U0000F2C3");
            _s.Add("f267", "\U0000F267");
            _s.Add("f241", "\U0000F241");
            _s.Add("f25d", "\U0000F25D");
            _s.Add("f2d4", "\U0000F2D4");
            _s.Add("f2bd", "\U0000F2BD");
            _s.Add("f25a", "\U0000F25A");
            _s.Add("f2a5", "\U0000F2A5");
            _s.Add("f276", "\U0000F276");
            _s.Add("f2ce", "\U0000F2CE");
            _s.Add("f266", "\U0000F266");
            _s.Add("f2b9", "\U0000F2B9");
            _s.Add("f2d6", "\U0000F2D6");
            _s.Add("f2a1", "\U0000F2A1");
            _s.Add("f1ed", "\U0000F1ED");
            _s.Add("f295", "\U0000F295");
            _s.Add("f1cd", "\U0000F1CD");
            _s.Add("f2ff", "\U0000F2FF");
            _s.Add("f1eb", "\U0000F1EB");
            _s.Add("f2eb", "\U0000F2EB");
            _s.Add("f2bb", "\U0000F2BB");
            _s.Add("f2b8", "\U0000F2B8");
            _s.Add("f2ba", "\U0000F2BA");
            _s.Add("f272", "\U0000F272");
            _s.Add("f26d", "\U0000F26D");
            _s.Add("f270", "\U0000F270");
            _s.Add("f244", "\U0000F244");
            _s.Add("f2ec", "\U0000F2EC");
            _s.Add("f238", "\U0000F238");
            _s.Add("f26c", "\U0000F26C");
            _s.Add("f250", "\U0000F250");
            _s.Add("f2b4", "\U0000F2B4");
            _s.Add("f2e1", "\U0000F2E1");
            _s.Add("f275", "\U0000F275");
            _s.Add("f2d3", "\U0000F2D3");
            _s.Add("f2ea", "\U0000F2EA");
            _s.Add("f273", "\U0000F273");
            _s.Add("f228", "\U0000F228");
            _s.Add("f293", "\U0000F293");
            _s.Add("f284", "\U0000F284");
            _s.Add("f23d", "\U0000F23D");
            _s.Add("f286", "\U0000F286");
            _s.Add("f285", "\U0000F285");
            _s.Add("f2ab", "\U0000F2AB");
            _s.Add("f1f1", "\U0000F1F1");
            _s.Add("f1d6", "\U0000F1D6");
            _s.Add("f2e2", "\U0000F2E2");
            _s.Add("f287", "\U0000F287");
            _s.Add("f262", "\U0000F262");
            _s.Add("f239", "\U0000F239");
            _s.Add("f26e", "\U0000F26E");
            _s.Add("f2d0", "\U0000F2D0");
            _s.Add("f24a", "\U0000F24A");
            _s.Add("f294", "\U0000F294");
            _s.Add("f22c", "\U0000F22C");
            _s.Add("f216", "\U0000F216");
            _s.Add("f2c9", "\U0000F2C9");
            _s.Add("f2cf", "\U0000F2CF");
            _s.Add("f26a", "\U0000F26A");
            _s.Add("f20d", "\U0000F20D");
            _s.Add("f21a", "\U0000F21A");
            _s.Add("f2e0", "\U0000F2E0");
            _s.Add("f299", "\U0000F299");
            _s.Add("f298", "\U0000F298");
            _s.Add("f2f8", "\U0000F2F8");
            _s.Add("f2e4", "\U0000F2E4");
            _s.Add("f277", "\U0000F277");
            _s.Add("f25f", "\U0000F25F");
            _s.Add("f226", "\U0000F226");
            _s.Add("f2f4", "\U0000F2F4");
            _s.Add("f1d0", "\U0000F1D0");
            _s.Add("f291", "\U0000F291");
            _s.Add("f2a4", "\U0000F2A4");
            _s.Add("f201", "\U0000F201");
            _s.Add("f243", "\U0000F243");
            _s.Add("f232", "\U0000F232");
            _s.Add("f24c", "\U0000F24C");
            _s.Add("f27e", "\U0000F27E");
            _s.Add("f1f7", "\U0000F1F7");
            _s.Add("f1f6", "\U0000F1F6");
            _s.Add("f219", "\U0000F219");
            _s.Add("f25e", "\U0000F25E");
            _s.Add("f2f6", "\U0000F2F6");
            _s.Add("f1ea", "\U0000F1EA");
            _s.Add("f1e5", "\U0000F1E5");
            _s.Add("f21d", "\U0000F21D");
            _s.Add("f1d2", "\U0000F1D2");
            _s.Add("f2d2", "\U0000F2D2");
            _s.Add("f1e3", "\U0000F1E3");
            _s.Add("f2a9", "\U0000F2A9");
            _s.Add("f264", "\U0000F264");
            _s.Add("f2a7", "\U0000F2A7");
            _s.Add("f203", "\U0000F203");
            _s.Add("f2a0", "\U0000F2A0");
            _s.Add("f21b", "\U0000F21B");
            _s.Add("f2df", "\U0000F2DF");
            _s.Add("f1e7", "\U0000F1E7");
            _s.Add("f1e9", "\U0000F1E9");
            _s.Add("f1db", "\U0000F1DB");
            _s.Add("f2ca", "\U0000F2CA");
            _s.Add("f22e", "\U0000F22E");
            _s.Add("f245", "\U0000F245");
            _s.Add("f2f5", "\U0000F2F5");
            _s.Add("f2e6", "\U0000F2E6");
            _s.Add("f252", "\U0000F252");
            _s.Add("f217", "\U0000F217");
            _s.Add("f265", "\U0000F265");
            _s.Add("f2fb", "\U0000F2FB");
            _s.Add("f2fc", "\U0000F2FC");
            _s.Add("f20b", "\U0000F20B");
            _s.Add("f20a", "\U0000F20A");
            _s.Add("f28d", "\U0000F28D");
            _s.Add("f20c", "\U0000F20C");
            _s.Add("f207", "\U0000F207");
            _s.Add("f209", "\U0000F209");
            _s.Add("f208", "\U0000F208");
            _s.Add("f2c5", "\U0000F2C5");
            _s.Add("f261", "\U0000F261");
            _s.Add("f230", "\U0000F230");
            _s.Add("f242", "\U0000F242");
            _s.Add("f2e8", "\U0000F2E8");
            _s.Add("f2c4", "\U0000F2C4");
            _s.Add("f1f5", "\U0000F1F5");
            _s.Add("f2c1", "\U0000F2C1");
            _s.Add("f28f", "\U0000F28F");
            _s.Add("f28c", "\U0000F28C");
            _s.Add("f2b5", "\U0000F2B5");
            _s.Add("f2e9", "\U0000F2E9");
            _s.Add("f2c7", "\U0000F2C7");
            _s.Add("f233", "\U0000F233");
            _s.Add("f237", "\U0000F237");
            _s.Add("f2ae", "\U0000F2AE");
            _s.Add("f248", "\U0000F248");
            _s.Add("f2f1", "\U0000F2F1");
            _s.Add("f2cd", "\U0000F2CD");
            _s.Add("f26b", "\U0000F26B");
            _s.Add("f2af", "\U0000F2AF");
            _s.Add("f21c", "\U0000F21C");
            _s.Add("f1ec", "\U0000F1EC");
            _s.Add("f2b0", "\U0000F2B0");
            _s.Add("f1d4", "\U0000F1D4");
            _s.Add("f29a", "\U0000F29A");
            _s.Add("f22a", "\U0000F22A");
            _s.Add("f2a2", "\U0000F2A2");
            _s.Add("f27c", "\U0000F27C");
            _s.Add("f2dd", "\U0000F2DD");
            _s.Add("f202", "\U0000F202");
            _s.Add("f2c6", "\U0000F2C6");
            _s.Add("f205", "\U0000F205");
            _s.Add("f225", "\U0000F225");
            _s.Add("f26f", "\U0000F26F");
            _s.Add("f24e", "\U0000F24E");
            _s.Add("f2c8", "\U0000F2C8");
            _s.Add("f279", "\U0000F279");
            _s.Add("f28b", "\U0000F28B");
            _s.Add("f1d7", "\U0000F1D7");
            _s.Add("f1ef", "\U0000F1EF");
            _s.Add("f1da", "\U0000F1DA");
            _s.Add("f1de", "\U0000F1DE");
            _s.Add("f1f0", "\U0000F1F0");
            _s.Add("f271", "\U0000F271");
            _s.Add("f227", "\U0000F227");
            _s.Add("f259", "\U0000F259");
            _s.Add("f29f", "\U0000F29F");
            _s.Add("f1e1", "\U0000F1E1");
            _s.Add("f1fd", "\U0000F1FD");
            _s.Add("f2fe", "\U0000F2FE");
            _s.Add("f1fc", "\U0000F1FC");
            _s.Add("f1fb", "\U0000F1FB");
            _s.Add("f1df", "\U0000F1DF");
            _s.Add("f260", "\U0000F260");
            _s.Add("f2e7", "\U0000F2E7");
            _s.Add("f29d", "\U0000F29D");
            _s.Add("f2d1", "\U0000F2D1");
            _s.Add("f280", "\U0000F280");
            _s.Add("f1cf", "\U0000F1CF");
            _s.Add("f301", "\U0000F301");
            _s.Add("f30f", "\U0000F30F");
            _s.Add("f32b", "\U0000F32B");
            _s.Add("f334", "\U0000F334");
            _s.Add("f305", "\U0000F305");
            _s.Add("f309", "\U0000F309");
            _s.Add("f311", "\U0000F311");
            _s.Add("f319", "\U0000F319");
            _s.Add("f31a", "\U0000F31A");
            _s.Add("f31c", "\U0000F31C");
            _s.Add("f32e", "\U0000F32E");
            _s.Add("f303", "\U0000F303");
            _s.Add("f317", "\U0000F317");
            _s.Add("f307", "\U0000F307");
            _s.Add("f332", "\U0000F332");
            _s.Add("f331", "\U0000F331");
            _s.Add("f313", "\U0000F313");
            _s.Add("f32c", "\U0000F32C");
            _s.Add("f30c", "\U0000F30C");
            _s.Add("f314", "\U0000F314");
            _s.Add("f30a", "\U0000F30A");
            _s.Add("f32f", "\U0000F32F");
            _s.Add("f310", "\U0000F310");
            _s.Add("f304", "\U0000F304");
            _s.Add("f308", "\U0000F308");
            _s.Add("f30e", "\U0000F30E");
            _s.Add("f31b", "\U0000F31B");
            _s.Add("f336", "\U0000F336");
            _s.Add("f306", "\U0000F306");
            _s.Add("f30b", "\U0000F30B");
            _s.Add("f333", "\U0000F333");
            _s.Add("f318", "\U0000F318");
            _s.Add("f335", "\U0000F335");
            _s.Add("f32d", "\U0000F32D");
            _s.Add("f312", "\U0000F312");
            _s.Add("f30d", "\U0000F30D");
            _s.Add("f330", "\U0000F330");
        }

        /// <summary>
        /// Gets the application icon.
        /// </summary>
        /// <returns>The application icon.</returns>
        private static ObservableCollection<string> GetApplicationIcon()
        {
            ObservableCollection<string> Icons = new ObservableCollection<string>();

            #region Add Application icons name

            Icons.Add("Bookmark-Up");
            Icons.Add("Product Box-02-WF");
            Icons.Add("User OK-02-WF");
            Icons.Add("Stack-02");
            Icons.Add("Data-Collapse");
            Icons.Add("User Modify1-WF");
            Icons.Add("Pressure-01-WF");
            Icons.Add("Cut");
            Icons.Add("Text Decoration - 01");
            Icons.Add("Share-03");
            Icons.Add("Maximize - 02");
            Icons.Add("Mobile-Phone-Message");
            Icons.Add("Mail Settings");
            Icons.Add("Calendar1-WF");
            Icons.Add("Requirement-02-WF");
            Icons.Add("Phonebook Info");
            Icons.Add("File-Format-ZIP");
            Icons.Add("User Delete-01-WF");
            Icons.Add("Bullet-WF");
            Icons.Add("Webpage Next-WF");
            Icons.Add("Music Icon-WF");
            Icons.Add("Pointer-WF");
            Icons.Add("Forest Fire");
            Icons.Add("WebPage Search");
            Icons.Add("Task-01");
            Icons.Add("Media Next-WF");
            Icons.Add("Media Forward-WF");
            Icons.Add("Arrowhead-Left-01");
            Icons.Add("Computer that is Member of security goup-WF");
            Icons.Add("Rope Lasso-WF");
            Icons.Add("File Next-WF");
            Icons.Add("Wind-03-WF");
            Icons.Add("Search-WF");
            Icons.Add("User Settings-02-WF");
            Icons.Add("Text Decoration2-WF");
            Icons.Add("Check-03");
            Icons.Add("Phonebook Search-WF");
            Icons.Add("Navigation-Down-Right");
            Icons.Add("Cut-WF");
            Icons.Add("Connection to Multiple Computer-WF");
            Icons.Add("Textdecorations-01-WF");
            Icons.Add("Margin - 01");
            Icons.Add("Movie Next-WF");
            Icons.Add("Networks-WF");
            Icons.Add("Message Voice Mail2-WF");
            Icons.Add("Link - 01");
            Icons.Add("Link - 03");
            Icons.Add("Link - 02");
            Icons.Add("Link - 05");
            Icons.Add("Link - 04");
            Icons.Add("Link - 07");
            Icons.Add("Link - 06");
            Icons.Add("Temporary Folder");
            Icons.Add("Contacts Save");
            Icons.Add("Favorite Delete-WF");
            Icons.Add("Send to back-WF");
            Icons.Add("Calculator-02-WF");
            Icons.Add("Vertical align Center-WF");
            Icons.Add("Volume Down 1-WF");
            Icons.Add("Bookmarks-WF");
            Icons.Add("User Modify-WF");
            Icons.Add("Message Mail-WF");
            Icons.Add("User Favourite-01-WF");
            Icons.Add("Move-WF");
            Icons.Add("Text Decoration - 05");
            Icons.Add("Folders-WF");
            Icons.Add("Document-Settings-01");
            Icons.Add("Text Decoration - 11");
            Icons.Add("File-Format-EPS");
            Icons.Add("Noise");
            Icons.Add("Pressure-03-WF");
            Icons.Add("DocumentCheck-WF");
            Icons.Add("Mug-02-WF");
            Icons.Add("Drag - 01");
            Icons.Add("Disc-Error");
            Icons.Add("Contacts Find");
            Icons.Add("Favourites Delete");
            Icons.Add("Document-Share-01");
            Icons.Add("Calendar edit-WF");
            Icons.Add("Bookmark Add-WF");
            Icons.Add("Loading4-WF");
            Icons.Add("Bookmark Remove-WF");
            Icons.Add("Web Page Info");
            Icons.Add("Mail-Box");
            Icons.Add("ArrowHeadRight-WF");
            Icons.Add("Contact Edit-WF");
            Icons.Add("Folder Remove-WF");
            Icons.Add("Lambda-WF");
            Icons.Add("User Unlocked-02-WF");
            Icons.Add("Add-New");
            Icons.Add("Web Page Upload");
            Icons.Add("Mail Unlocked-WF");
            Icons.Add("Movie OK-WF");
            Icons.Add("Phonebook remove-WF");
            Icons.Add("Orientation Portrait-02-WF");
            Icons.Add("MailRefresh-WF");
            Icons.Add("Rating 1-WF");
            Icons.Add("User Save -01-WF");
            Icons.Add("Basket-WF");
            Icons.Add("Find-Previous");
            Icons.Add("Document-01");
            Icons.Add("Stack add");
            Icons.Add("Favorite Search-WF");
            Icons.Add("File");
            Icons.Add("Black List-WF");
            Icons.Add("Mail Delete-WF");
            Icons.Add("Clipboard Next-WF");
            Icons.Add("Volume Up 1 -WF");
            Icons.Add("Favourites Previous");
            Icons.Add("Calendar-Next-WF");
            Icons.Add("Movie Find");
            Icons.Add("Active Directory");
            Icons.Add("File-WF");
            Icons.Add("CD Software-WF");
            Icons.Add("Visual-Studio");
            Icons.Add("Text Braille-WF");
            Icons.Add("Folder-Open-01");
            Icons.Add("Password-Text-01");
            Icons.Add("Text Decoration - 09");
            Icons.Add("Media Rewind -01");
            Icons.Add("Movie Remove");
            Icons.Add("File Upload");
            Icons.Add("MS System settings Configuration Manger-01");
            Icons.Add("Display Brightness-WF");
            Icons.Add("Movie Info-WF");
            Icons.Add("Horizontal-Align-Left");
            Icons.Add("CD New-WF");
            Icons.Add("Navigation UpLeft-WF");
            Icons.Add("Movie Download-WF");
            Icons.Add("Hook1-WF");
            Icons.Add("Timer");
            Icons.Add("Para Mark - 02");
            Icons.Add("Folder Remove 2-WF");
            Icons.Add("Document-02");
            Icons.Add("Message Voice Mail1-WF");
            Icons.Add("Mail -03");
            Icons.Add("Contacts edit");
            Icons.Add("File Setting-WF");
            Icons.Add("File Sync-WF");
            Icons.Add("Document ZoomIn-WF");
            Icons.Add("Text-Mark");
            Icons.Add("Phonebook UnLocked");
            Icons.Add("DocumentSave-WF");
            Icons.Add("Tsunami");
            Icons.Add("Agreement-02");
            Icons.Add("Top-WF");
            Icons.Add("Mail Search");
            Icons.Add("Webpage Sync-WF");
            Icons.Add("Show");
            Icons.Add("Phonebook , upload");
            Icons.Add("Movie Refresh-WF");
            Icons.Add("Blog-WF");
            Icons.Add("User Help");
            Icons.Add("Folder-Edit");
            Icons.Add("Text-Braille");
            Icons.Add("Phonebook- WF");
            Icons.Add("Product-Box");
            Icons.Add("File Next");
            Icons.Add("Folder-Download-02");
            Icons.Add("Data Erase-WF");
            Icons.Add("Media Play1-WF");
            Icons.Add("Network-01");
            Icons.Add("Document ZoomOut-WF");
            Icons.Add("CD-Pause-WF");
            Icons.Add("Folder-Remove-03");
            Icons.Add("MS System Setting3-WF");
            Icons.Add("Align-Center");
            Icons.Add("Pressure");
            Icons.Add("Globe-01-WF");
            Icons.Add("Key-Hash-WF");
            Icons.Add("Favourites Search");
            Icons.Add("Navigation-Up-Left");
            Icons.Add("User Search-01-WF");
            Icons.Add("Sync-WF");
            Icons.Add("Media Previous");
            Icons.Add("Rating - 03");
            Icons.Add("Arrowhead-Down");
            Icons.Add("Movie unlocked");
            Icons.Add("Media-Play");
            Icons.Add("Webpage Remove-WF");
            Icons.Add("Black List");
            Icons.Add("Web Page ");
            Icons.Add("Folder-Movie-03");
            Icons.Add("Calender Refresh-WF");
            Icons.Add("Document Warning-01-WF");
            Icons.Add("MS system setting2-WF");
            Icons.Add("File Refresh-WF");
            Icons.Add("Zoom - corner - 04");
            Icons.Add("Comodo Dragon");
            Icons.Add("Network-Drives");
            Icons.Add("Web Page Ok");
            Icons.Add("Arrowhead-Top");
            Icons.Add("Movie Lock1-WF");
            Icons.Add("User Favourites-02-WF");
            Icons.Add("Media Previous-WF");
            Icons.Add("Phonebook Ok");
            Icons.Add("Webpage Split-WF");
            Icons.Add("UpArrow-01-WF");
            Icons.Add("File Lock-WF");
            Icons.Add("Favourites");
            Icons.Add("Requirement-05-WF");
            Icons.Add("Loading - 10");
            Icons.Add("Calendar Delete");
            Icons.Add("DataMerge-WF");
            Icons.Add("Folder-Find-01");
            Icons.Add("File Delete-WF");
            Icons.Add("UpArrow-WF");
            Icons.Add("Documents-01");
            Icons.Add("Arrowhead-Right-01");
            Icons.Add("Trash can - 03");
            Icons.Add("Phonebook Refresh");
            Icons.Add("Phonebook settings-WF");
            Icons.Add("Web Search-WF");
            Icons.Add("Mail Ok");
            Icons.Add("DocumentRefresh-WF");
            Icons.Add("Document-Delete-01");
            Icons.Add("Media-Back");
            Icons.Add("Folder-Remove-04");
            Icons.Add("Single Quotation Mark-WF");
            Icons.Add("Phonebook Favourites");
            Icons.Add("Maximize4-WF");
            Icons.Add("Folder Add 1-WF");
            Icons.Add("Calendar Help");
            Icons.Add("Webpage Favourites-WF");
            Icons.Add("Calendar Save");
            Icons.Add("User Remove");
            Icons.Add("Media Eject");
            Icons.Add("Phonebook refresh-WF");
            Icons.Add("User Search");
            Icons.Add("Phonebook Next");
            Icons.Add("View Details 2-WF");
            Icons.Add("Maximize1-WF");
            Icons.Add("User Next-02-WF");
            Icons.Add("Mail Favorite-WF");
            Icons.Add("RSS-Feeds");
            Icons.Add("CCleaner");
            Icons.Add("Movie Save");
            Icons.Add("Contact Next-WF");
            Icons.Add("Blog");
            Icons.Add("Message-Mail");
            Icons.Add("Favorite Next-WF");
            Icons.Add("Orientation Portrait-01-WF");
            Icons.Add("Speaker - 04");
            Icons.Add("Phonebook Edit-WF");
            Icons.Add("Stop Media-01-WF");
            Icons.Add("Wifi-WF");
            Icons.Add("Sort Ascending-WF");
            Icons.Add("Connection to multiple computer");
            Icons.Add("Favourites locked");
            Icons.Add("Bookmark Settings");
            Icons.Add("Move - 01");
            Icons.Add("Stack-04-WF");
            Icons.Add("Text Decoration-04");
            Icons.Add("Maximize -03");
            Icons.Add("Mail Previous-WF");
            Icons.Add("Mug-03-WF");
            Icons.Add("List all security groups on computer");
            Icons.Add("Visual-Studio-2011");
            Icons.Add("Animation-01");
            Icons.Add("User Sync-01-WF");
            Icons.Add("Loading6-WF");
            Icons.Add("Para Mark - 03");
            Icons.Add("Folder-Zoom-Out-01");
            Icons.Add("Login-01");
            Icons.Add("Text Decoration - 10");
            Icons.Add("Rating - 02");
            Icons.Add("Folder-New");
            Icons.Add("Link12-WF");
            Icons.Add("User Refresh");
            Icons.Add("Contacts Ok-WF");
            Icons.Add("Phonebook favourite -WF");
            Icons.Add("Wind-02-WF");
            Icons.Add("Help - 01");
            Icons.Add("Help - 02");
            Icons.Add("Folder-Zoom-In");
            Icons.Add("File Ok");
            Icons.Add("Document-New");
            Icons.Add("Contact Refresh-WF");
            Icons.Add("Loading1-WF");
            Icons.Add("Folder-Cube-01");
            Icons.Add("Up Arrow - 02");
            Icons.Add("Up Arrow - 03");
            Icons.Add("Up Arrow - 01");
            Icons.Add("Format-Bullets-02");
            Icons.Add("Format-Bullets-01");
            Icons.Add("Add Computer");
            Icons.Add("Phonebook help-WF");
            Icons.Add("Play once-02-WF");
            Icons.Add("Bookmark Info");
            Icons.Add("CD Catalog-01-WF");
            Icons.Add("Sort-Descending");
            Icons.Add("Mail,Find");
            Icons.Add("Folder Zoom In -WF");
            Icons.Add("Favorite-Remove-WF");
            Icons.Add("Login-Arrow");
            Icons.Add("User Globe-WF");
            Icons.Add("Dialog box About-WF");
            Icons.Add("Upload - 02");
            Icons.Add("Window Horizontal Split-WF");
            Icons.Add("Media-Start");
            Icons.Add("User Edit-02-WF");
            Icons.Add("Folder-Upload-02");
            Icons.Add("Arrow Down-WF");
            Icons.Add("Maximize3-WF");
            Icons.Add("Favorite Add-WF");
            Icons.Add("Favorite Ok-WF");
            Icons.Add("Data-Exchange");
            Icons.Add("User Settings");
            Icons.Add("Window-Delete");
            Icons.Add("Movie edit");
            Icons.Add("Volume-Up");
            Icons.Add("User Upload-02-WF");
            Icons.Add("Calendar info-WF");
            Icons.Add("Align-Horizontal-Center");
            Icons.Add("Restore-02");
            Icons.Add("Movie Next");
            Icons.Add("Favourites Add");
            Icons.Add("Vibration-WF");
            Icons.Add("Movie Edit-WF");
            Icons.Add("Movie Help-WF");
            Icons.Add("Data-Export");
            Icons.Add("Hide");
            Icons.Add("Phonebook Find");
            Icons.Add("Media Fast-forward - 01");
            Icons.Add("Login2-WF");
            Icons.Add("Water Recycling-WF");
            Icons.Add("Mouse-Drag");
            Icons.Add("Edit-WF");
            Icons.Add("Row Selection-WF");
            Icons.Add("Loading - 05");
            Icons.Add("Maximize2-WF");
            Icons.Add("Align-Left");
            Icons.Add("Tasks");
            Icons.Add("Arrowhead-Up");
            Icons.Add("Folder OK-WF");
            Icons.Add("Cloud");
            Icons.Add("Shape cube-WF");
            Icons.Add("Bookmarks Upload");
            Icons.Add("Infrared");
            Icons.Add("Google Drive");
            Icons.Add("View-Incident");
            Icons.Add("Row-Selection");
            Icons.Add("Data-Information");
            Icons.Add("Help");
            Icons.Add("Stack-01");
            Icons.Add("Contacts upload");
            Icons.Add("Upload - 01");
            Icons.Add("Folder-Picture");
            Icons.Add("Mail - Sent");
            Icons.Add("Web Page Refresh");
            Icons.Add("Favourites Download");
            Icons.Add("Bookmark-Settings");
            Icons.Add("CD-Valid");
            Icons.Add("File Search");
            Icons.Add("Data Merge-WF");
            Icons.Add("Bookmarks favourites");
            Icons.Add("Product Box With Disc-01-WF");
            Icons.Add("Command Undo-WF");
            Icons.Add("Virtual Apps-WF");
            Icons.Add("Tile shape - 01");
            Icons.Add("Upload-01-WF");
            Icons.Add("Upload-02-WF");
            Icons.Add("Expansion-02-WF");
            Icons.Add("Calendar-02");
            Icons.Add("Basket");
            Icons.Add("Media First");
            Icons.Add("Conference-Call-03");
            Icons.Add("Document-Error");
            Icons.Add("Bookmark Delete01-WF");
            Icons.Add("Expansion - 01");
            Icons.Add("Phonebook Sync-WF");
            Icons.Add("Folder-Share-01");
            Icons.Add("Movie Delete");
            Icons.Add("Contacts Upload-WF");
            Icons.Add("Document-Check");
            Icons.Add("Link");
            Icons.Add("Graphics-Card");
            Icons.Add("Volume Up-WF");
            Icons.Add("CD-View");
            Icons.Add("Requirement-04-WF");
            Icons.Add("Paragraph-Indent-Increase");
            Icons.Add("Adobe-Illustrator");
            Icons.Add("Numbering ");
            Icons.Add("Earth Quake");
            Icons.Add("Drag-02");
            Icons.Add("Navigation-Right");
            Icons.Add("Window-WF");
            Icons.Add("Favourites Next -2");
            Icons.Add("Speaker Audible-WF");
            Icons.Add("Bookmark Save");
            Icons.Add("Loading - 01");
            Icons.Add("Bookmark Upload-WF");
            Icons.Add("Loading - 08");
            Icons.Add("Speaker  low - 02");
            Icons.Add("FavoriteSetting-WF");
            Icons.Add("All software updates that are installed in computer");
            Icons.Add("Command-Refresh-01");
            Icons.Add("Tsunami-01-WF");
            Icons.Add("Clipboard Next Down");
            Icons.Add("Speaker Increase - 02");
            Icons.Add("Speaker Increase - 01");
            Icons.Add("Webpage Save-WF");
            Icons.Add("Windows Tablet-WF");
            Icons.Add("Mail Sync");
            Icons.Add("Text Decoration - 04");
            Icons.Add("Device Tablet-WF");
            Icons.Add("RSS Feed-WF");
            Icons.Add("Favourite-WF");
            Icons.Add("Folder New -WF");
            Icons.Add("Reload-WF");
            Icons.Add("Contacts info");
            Icons.Add("Folder Movie-WF");
            Icons.Add("Check-04-WF");
            Icons.Add("Bookmark Find");
            Icons.Add("Add Computer-WF");
            Icons.Add("Movie Search");
            Icons.Add("Mug-04-WF");
            Icons.Add("Windows-Environment");
            Icons.Add("Folder-Delete-01");
            Icons.Add("File-Format-PPT");
            Icons.Add("Movie Settings");
            Icons.Add("Listen-WF");
            Icons.Add("File Share-WF");
            Icons.Add("Folder-Music-02");
            Icons.Add("View-Medium-Icons-01");
            Icons.Add("Mail Previous");
            Icons.Add("Display-Contrast-02");
            Icons.Add("Burn-Disk-01-WF");
            Icons.Add("Horizontal-Align-Right");
            Icons.Add("To Do List 2-WF");
            Icons.Add("Contact Remove-WF");
            Icons.Add("Key-Access-01");
            Icons.Add("Document-Warning-01");
            Icons.Add("Garbage-Closed");
            Icons.Add("Folder-Upload");
            Icons.Add("Power-Off");
            Icons.Add("Spell Check-WF");
            Icons.Add("Format-Bullets");
            Icons.Add("Brackets-square-WF");
            Icons.Add("File-Format-Mp3");
            Icons.Add("Data-Erase");
            Icons.Add("Tile-04-WF");
            Icons.Add("Globe");
            Icons.Add("Bookmark Edit-WF");
            Icons.Add("Folder Search-WF");
            Icons.Add("Speaker-02");
            Icons.Add("Gsm Tower");
            Icons.Add("Bookmark Info-WF");
            Icons.Add("Folder-Download");
            Icons.Add("Database Connection");
            Icons.Add("Media Rewind");
            Icons.Add("Sort-Ascending");
            Icons.Add("Document Favourite-WF");
            Icons.Add("Text Decoration - 08");
            Icons.Add("Mail Add");
            Icons.Add("Hide-WF");
            Icons.Add("Movie Save-WF");
            Icons.Add("Computer-Desktop");
            Icons.Add("Mail unlocked");
            Icons.Add("Noise-02-WF");
            Icons.Add("Phonebook , download");
            Icons.Add("User Download-02-WF");
            Icons.Add("Animation-1-WF");
            Icons.Add("Wind");
            Icons.Add("File Help-WF");
            Icons.Add("Link11-WF");
            Icons.Add("Bookmark-Down");
            Icons.Add("List of all application installed on computer-WF");
            Icons.Add("Braces-WF");
            Icons.Add("Window-Information");
            Icons.Add("Document-Warning");
            Icons.Add("Attachment-01-WF");
            Icons.Add("Message Warning-WF");
            Icons.Add("CD View-WF");
            Icons.Add("File-Format-TIFF");
            Icons.Add("View Details-WF");
            Icons.Add("Scale to Fit-02");
            Icons.Add("Check-02");
            Icons.Add("Align-Justify");
            Icons.Add("Web Page Delete-WF");
            Icons.Add("Calendar Ok-WF");
            Icons.Add("Pen Line -WF");
            Icons.Add("Phonebook Remove");
            Icons.Add("Folder-Empty");
            Icons.Add("Mouse Drag-WF");
            Icons.Add("Document Edit-WF");
            Icons.Add("Folder-Add");
            Icons.Add("Document-Exchange");
            Icons.Add("Horizontal Align Left-WF");
            Icons.Add("Trash Can-WF");
            Icons.Add("Movie Sync-WF");
            Icons.Add("Mail info");
            Icons.Add("To Do List-WF");
            Icons.Add("Save");
            Icons.Add("C Sharp-02");
            Icons.Add("User Profile 1-WF");
            Icons.Add("Expansion - 02");
            Icons.Add("Mug");
            Icons.Add("Contact info-WF");
            Icons.Add("DocumentExchange-WF");
            Icons.Add("User Upload-01-WF");
            Icons.Add("Calender-03-WF");
            Icons.Add("Calendar -01 ");
            Icons.Add("Command-Paste");
            Icons.Add("Vertical-Align-Bottom");
            Icons.Add("Contact Save-WF");
            Icons.Add("Phonebook Search");
            Icons.Add("File Delete");
            Icons.Add("Search-Find");
            Icons.Add("Tile shape - 02");
            Icons.Add("Favorite Help-WF");
            Icons.Add("Full-Screen-Expand");
            Icons.Add("Volume Speaker 1-WF");
            Icons.Add("Column-Selection-WF");
            Icons.Add("Data-Files");
            Icons.Add("Up arrow");
            Icons.Add("Document-Edit");
            Icons.Add("Zoom vertical - 02");
            Icons.Add("Contacts Ok");
            Icons.Add("Text Decoration-05");
            Icons.Add("Login3-WF");
            Icons.Add("Arrow");
            Icons.Add("Arrow-Expand");
            Icons.Add("User-Login");
            Icons.Add("Tiles-03-WF");
            Icons.Add("Media Pause");
            Icons.Add("Data-Edit");
            Icons.Add("Login-WF");
            Icons.Add("Text-Edit");
            Icons.Add("Mail info-WF");
            Icons.Add("Full Screen Collapsed-WF");
            Icons.Add("Window-New-Open");
            Icons.Add("Contacts Locked");
            Icons.Add("File favourite");
            Icons.Add("Calculator-02");
            Icons.Add("Calculator-01");
            Icons.Add("User Favourites");
            Icons.Add("Agreement-01");
            Icons.Add("Money Coin-02-WF");
            Icons.Add("Horizontal Align Right-WF");
            Icons.Add("Warning Shield-WF");
            Icons.Add("Measurement-WF");
            Icons.Add("Calendar Add");
            Icons.Add("Folder Share-WF");
            Icons.Add("Calender Previous-WF");
            Icons.Add("Text-Read");
            Icons.Add("Document-Download-01");
            Icons.Add("Loading - 11");
            Icons.Add("User Previous-WF");
            Icons.Add("File info");
            Icons.Add("Phonebook Help");
            Icons.Add("CD-Music");
            Icons.Add("Web Page Favourites");
            Icons.Add("Zoom Corner1-WF");
            Icons.Add("Phonebook Add-WF");
            Icons.Add("Bookmark Previous");
            Icons.Add("Find-Replace-01");
            Icons.Add("Webpage New open1-WF");
            Icons.Add("Folder Download-WF");
            Icons.Add("Windows Tablet");
            Icons.Add("Expand-03");
            Icons.Add("Contacts favourites");
            Icons.Add("Expand-01");
            Icons.Add("User Search--02WF");
            Icons.Add("Data-Share");
            Icons.Add("Security Group Member");
            Icons.Add("Tiles");
            Icons.Add("Mail -Open");
            Icons.Add("Contacts Download");
            Icons.Add("Bookmark Favourities-WF");
            Icons.Add("Globe-02-WF");
            Icons.Add("Favourites Refresh");
            Icons.Add("Phonebook Next-WF");
            Icons.Add("User Save");
            Icons.Add("Mail  Next-WF");
            Icons.Add("Virtual-Apps");
            Icons.Add("AlignRight-WF");
            Icons.Add("Quotation Mark-WF");
            Icons.Add("Scale-To-Fit");
            Icons.Add("Share-02-WF");
            Icons.Add("Data-Synchronize");
            Icons.Add("File Unlock-WF");
            Icons.Add("Clipboard1-WF");
            Icons.Add("Rectangle-WF");
            Icons.Add("Folder-New-01");
            Icons.Add("Calendar Locked-WF");
            Icons.Add("AlignCenter-WF");
            Icons.Add("Contacts Refresh");
            Icons.Add("Reload-01-WF");
            Icons.Add("Favourites Upload");
            Icons.Add("CD Music-WF");
            Icons.Add("Tab History-WF");
            Icons.Add("File-Format-Icon");
            Icons.Add("Mail Upload-WF");
            Icons.Add("Folder Upload-WF");
            Icons.Add("Connection to a Computer");
            Icons.Add("User");
            Icons.Add("Connection to a Computer-WF");
            Icons.Add("Adobe-Dreamweaver");
            Icons.Add("User Add");
            Icons.Add("User-Add");
            Icons.Add("Ellipse-Selection");
            Icons.Add("Bookmark Ok-WF");
            Icons.Add("Installation-03");
            Icons.Add("Measurements - 03");
            Icons.Add("Measurements - 02");
            Icons.Add("Measurements - 01");
            Icons.Add("Text Decoration - 13");
            Icons.Add("Calendar Ok");
            Icons.Add("Measurements - 04");
            Icons.Add("Orientation landscape-01-WF");
            Icons.Add("Key-Hash");
            Icons.Add("View-Tiles");
            Icons.Add("Document-Find-02");
            Icons.Add("Command-Undo");
            Icons.Add("Message-Voice-Mail");
            Icons.Add("File Remove-WF");
            Icons.Add("Movie Upload-WF");
            Icons.Add("User Save-02-WF");
            Icons.Add("Arrowhead-01");
            Icons.Add("Webpage Upload-WF");
            Icons.Add("Phonebook Sync");
            Icons.Add("User Download");
            Icons.Add("MS System Setting-WF");
            Icons.Add("Bookmark-Delete");
            Icons.Add("Password-Text");
            Icons.Add("Mug-01-WF");
            Icons.Add("View List-WF");
            Icons.Add("To do list - 03");
            Icons.Add("Loading2-WF");
            Icons.Add("Disc-Information");
            Icons.Add("Movie Help");
            Icons.Add("Hook");
            Icons.Add("Zoom Horizontal - 02");
            Icons.Add("Check-01");
            Icons.Add("Document Sharing-WF");
            Icons.Add("Favorite Sync-WF");
            Icons.Add("Conference-Call-02");
            Icons.Add("Link10-WF");
            Icons.Add("Save-02-WF");
            Icons.Add("Bookmark Refresh-WF");
            Icons.Add("Device-Headphone");
            Icons.Add("Contacts");
            Icons.Add("User Sync");
            Icons.Add("Calendar Next");
            Icons.Add("File-Torrent");
            Icons.Add("Bring to Front-WF");
            Icons.Add("Windows");
            Icons.Add("Movie Download");
            Icons.Add("Favorite Download-WF");
            Icons.Add("Connectivity-Error-WF");
            Icons.Add("Folder-Open-03");
            Icons.Add("Database-WF");
            Icons.Add("Display-Contrast");
            Icons.Add("BookMarks");
            Icons.Add("Execute multiple queries");
            Icons.Add("Gear--03WF");
            Icons.Add("Arrowhead Top-WF");
            Icons.Add("Tile-01-WF");
            Icons.Add("File Ok-WF");
            Icons.Add("Way-Board");
            Icons.Add("Speaker  low -01");
            Icons.Add("File lock");
            Icons.Add("Web Page Settings");
            Icons.Add("Vertical-Align-Center");
            Icons.Add("Document Next-WF");
            Icons.Add("Clipboard-Next");
            Icons.Add("Mail edit");
            Icons.Add("Group Delete-WF");
            Icons.Add("Web Page refresh");
            Icons.Add("Calender Upload-WF");
            Icons.Add("Column-Selection");
            Icons.Add("Calendar Help-WF");
            Icons.Add("Favourites Next");
            Icons.Add("Webpage Info-WF");
            Icons.Add("Mobile-Phone-Music");
            Icons.Add("Folder Edit 1-WF");
            Icons.Add("Document-Music-02");
            Icons.Add("Attachment-02-WF");
            Icons.Add("Money-Credit-Card");
            Icons.Add("Document Music-02-WF");
            Icons.Add("Installation-WF");
            Icons.Add("Speaker Decrease - 01");
            Icons.Add("Margin - 02");
            Icons.Add("Lambda-01");
            Icons.Add("Book-Close");
            Icons.Add("Loading - 06");
            Icons.Add("Audit");
            Icons.Add("CD-Warning");
            Icons.Add("User Ok-01-WF");
            Icons.Add("Movie Fav");
            Icons.Add("Active Dictionary-WF");
            Icons.Add("Free Hand Selection-WF");
            Icons.Add("Calendar -02");
            Icons.Add("Movie Sync");
            Icons.Add("File Settings");
            Icons.Add("Volume-Mute");
            Icons.Add("Mail next");
            Icons.Add("Favourites Ok");
            Icons.Add("Group-Add");
            Icons.Add("Speaker - 03");
            Icons.Add("Text Decoration - 03");
            Icons.Add("User Remove-02-WF");
            Icons.Add("Mail Favourites");
            Icons.Add("Favorite upload-WF");
            Icons.Add("Window-Earth");
            Icons.Add("Folder-Upload-01");
            Icons.Add("Loading5-WF");
            Icons.Add("Mail download");
            Icons.Add("MailBox-WF");
            Icons.Add("Phonebook Locked");
            Icons.Add("User Next");
            Icons.Add("Text Decoration - 07");
            Icons.Add("Tile-02");
            Icons.Add("Media Last");
            Icons.Add("Group-Modify");
            Icons.Add("Calender Remove-WF");
            Icons.Add("CD-Play");
            Icons.Add("File-Format-TGA");
            Icons.Add("Media-Play-02");
            Icons.Add("User Info-01-WF");
            Icons.Add("Media-Play-01");
            Icons.Add("Window-Horizontal-Split");
            Icons.Add("MS System Setting4-WF");
            Icons.Add("Volume Icon");
            Icons.Add("Vibration");
            Icons.Add("Crop");
            Icons.Add("Check-02-WF");
            Icons.Add("User Refresh-01-WF");
            Icons.Add("Calendar Find-WF");
            Icons.Add("Movie Ok");
            Icons.Add("Contacts Delete");
            Icons.Add("Assign-WF");
            Icons.Add("Garbage Full1-WF");
            Icons.Add("Media-Fast-Forward");
            Icons.Add("Favourites Info");
            Icons.Add("Zoom Horizontal - 01");
            Icons.Add("Favourites favourites");
            Icons.Add("Phonebook Previous-WF");
            Icons.Add("Contacts Add");
            Icons.Add("User Previous1-WF");
            Icons.Add("Webpage Find");
            Icons.Add("Loading - 09");
            Icons.Add("Trash can - 01");
            Icons.Add("Favourites Download-2");
            Icons.Add("Movie Add-WF");
            Icons.Add("Contacts help");
            Icons.Add("Gear-02-WF");
            Icons.Add("Recycle-Bin ");
            Icons.Add("Web Page add");
            Icons.Add("Pointer");
            Icons.Add("Share-01");
            Icons.Add("Layers-WF");
            Icons.Add("Media-Player-Winamp");
            Icons.Add("Format Bullet1-WF");
            Icons.Add("Installation-02");
            Icons.Add("Movie Previous");
            Icons.Add("Money-Gold");
            Icons.Add("Volume Mute 1-WF");
            Icons.Add("Orientation-Landscape");
            Icons.Add("Group Modify-WF");
            Icons.Add("Zoom Vertical-WF");
            Icons.Add("CD-Stop");
            Icons.Add("Mail");
            Icons.Add("Network-Signal");
            Icons.Add("Mail Edit-WF");
            Icons.Add("User Find");
            Icons.Add("Bookmark Delete");
            Icons.Add("List All Application installed on computer-WF");
            Icons.Add("Document Unlocked-WF");
            Icons.Add("Document-Zoom-Out-02");
            Icons.Add("Margin-WF");
            Icons.Add("Movie");
            Icons.Add("Contacts-Delete-WF");
            Icons.Add("Phonebook Download-WF");
            Icons.Add("DataHistogram-WF");
            Icons.Add("Device-Radio");
            Icons.Add("Group-WF");
            Icons.Add("Installation-01");
            Icons.Add("Paragraph-Indent-Decrease");
            Icons.Add("Bookmark Add1-WF");
            Icons.Add("Zip file-03-WF");
            Icons.Add("Arrowhead Right1-WF");
            Icons.Add("UpArrow Line-03-WF");
            Icons.Add("Save-01-WF");
            Icons.Add("SCCM");
            Icons.Add("Data Export-WF");
            Icons.Add("Log Out-WF");
            Icons.Add("Bookmark Search");
            Icons.Add("Down Arrow-WF");
            Icons.Add("Crop - 01");
            Icons.Add("Add csv-02-WF");
            Icons.Add("Web Page Delete");
            Icons.Add("Calendar Upload");
            Icons.Add("Text Decoration - 02");
            Icons.Add("Contacts Remove");
            Icons.Add("Stack-02-WF");
            Icons.Add("Orientation-Portrait");
            Icons.Add("Movie Upload");
            Icons.Add("User Sync-02-WF");
            Icons.Add("Speaker Low-WF");
            Icons.Add("CD Catalog-02");
            Icons.Add("CD Catalog-01");
            Icons.Add("Submit-02");
            Icons.Add("User Refresh-02-WF");
            Icons.Add("Submit-01");
            Icons.Add("Reqiurement-03-WF");
            Icons.Add("Folder Edit-WF");
            Icons.Add("Black List Folder-WF");
            Icons.Add("Scale to Fi-03t-WF");
            Icons.Add("Slash");
            Icons.Add("Application-01");
            Icons.Add("Document Download-WF");
            Icons.Add("User Locked-02-WF");
            Icons.Add("Phonebook Save-WF");
            Icons.Add("View-Small-Icons-01");
            Icons.Add("Stack Add-WF");
            Icons.Add("CD Play-WF");
            Icons.Add("User Unlocked-01-WF");
            Icons.Add("Zoom - corner - 01");
            Icons.Add("Brackets-Square");
            Icons.Add("Web Page Next");
            Icons.Add("Hide1-WF");
            Icons.Add("DocumentPrevious-WF");
            Icons.Add("Folder-Movie");
            Icons.Add("Document Setting-WF");
            Icons.Add("File unlock");
            Icons.Add("Document-WF");
            Icons.Add("Phonebook Settings");
            Icons.Add("Money Coin-01-WF");
            Icons.Add("Web Page Add-WF");
            Icons.Add("View Medium icons-WF");
            Icons.Add("Media Next");
            Icons.Add("Bookmarks locked");
            Icons.Add("Expand-02-WF");
            Icons.Add("Expand-01-WF");
            Icons.Add("Data-Split");
            Icons.Add("Calendar Favourite");
            Icons.Add("Folder-Add-04");
            Icons.Add("Gear-01-WF");
            Icons.Add("Favourite Previous-WF");
            Icons.Add("Speaker Low1-WF");
            Icons.Add("Mouse Wireless-01-WF");
            Icons.Add("Link6-WF");
            Icons.Add("DocumentDelete-01-WF");
            Icons.Add("Animation-02");
            Icons.Add("Single-Curly-Quotation-Marks");
            Icons.Add("Calendar locked");
            Icons.Add("Bookmark Help");
            Icons.Add("Money-Coin");
            Icons.Add("Trash can - 04");
            Icons.Add("Arrowhead-Left");
            Icons.Add("Indent Decrease-WF");
            Icons.Add("Rating-WF");
            Icons.Add("UpArrow Line-01-WF");
            Icons.Add("Phonebook Add");
            Icons.Add("Avalanche");
            Icons.Add("Document Help-WF");
            Icons.Add("Add csv-01");
            Icons.Add("File Find");
            Icons.Add("Mail delete");
            Icons.Add("Folder-Remove-01");
            Icons.Add("Burn-Disk-01");
            Icons.Add("Webpage locked-WF");
            Icons.Add("Infrared1-WF");
            Icons.Add("Infrared2-WF");
            Icons.Add("Mail locked");
            Icons.Add("Vertical-Align-Top");
            Icons.Add("CD-Software");
            Icons.Add("Maximize  -04");
            Icons.Add("Parenthesis-01-WF");
            Icons.Add("Increase intend");
            Icons.Add("Movie Unlock-WF");
            Icons.Add("File edit");
            Icons.Add("Timer-WF");
            Icons.Add("File remove");
            Icons.Add("Sync - 03");
            Icons.Add("Sync - 02");
            Icons.Add("Sync - 01");
            Icons.Add("Drag -02");
            Icons.Add("Check-WF");
            Icons.Add("Volume Mute-WF");
            Icons.Add("Bookmark-Add");
            Icons.Add("UpArrow Line-04-WF");
            Icons.Add("CD Pause-WF");
            Icons.Add("Drag-01-WF");
            Icons.Add("User Remove-01-WF");
            Icons.Add("Movie Search-WF");
            Icons.Add("ArrowheadLeft1-WF");
            Icons.Add("Loading7-WF");
            Icons.Add("Movie1-WF");
            Icons.Add("Ellipse Selection-WF");
            Icons.Add("Sort Descending-WF");
            Icons.Add("Volume Down-WF");
            Icons.Add("Folder-01");
            Icons.Add("Folder-02");
            Icons.Add("Folder-03");
            Icons.Add("Folder-04");
            Icons.Add("Folder-05");
            Icons.Add("Folder-Add-05");
            Icons.Add("Folder-Add-01");
            Icons.Add("Folder-Add-02");
            Icons.Add("Folder-Add-03");
            Icons.Add("Requirement-01-WF");
            Icons.Add("File Previous-WF");
            Icons.Add("Data-Merge");
            Icons.Add("Document-Settings");
            Icons.Add("Crop - 02");
            Icons.Add("CD-Pause");
            Icons.Add("Network Drives-WF");
            Icons.Add("Folder-Movie-01");
            Icons.Add("File-Format-SWF");
            Icons.Add("Zoom Corner-WF");
            Icons.Add("View Tiles-WF");
            Icons.Add("Mail - 01");
            Icons.Add("Internet Facilities-WF");
            Icons.Add("Web Page Previous");
            Icons.Add("Mail Help");
            Icons.Add("Adobe-Photoshop");
            Icons.Add("Media-Player-VLC");
            Icons.Add("Command-Reset");
            Icons.Add("File Download-WF");
            Icons.Add("Mail Add-WF");
            Icons.Add("CD Warning-WF");
            Icons.Add("Command Redo-WF");
            Icons.Add("Bookmark remove");
            Icons.Add("Windows login-WF");
            Icons.Add("User Edit-01-WF");
            Icons.Add("Para Mark - 01");
            Icons.Add("Document Error-WF");
            Icons.Add("Movie-WF");
            Icons.Add("Contact Add-WF");
            Icons.Add("Full-Screen-Collapse");
            Icons.Add("Speaker Mute - 03");
            Icons.Add("Webpage 1-WF");
            Icons.Add("Contact previous-WF");
            Icons.Add("Speaker Mute - 04");
            Icons.Add("Folder Add-WF");
            Icons.Add("Garbage-Full");
            Icons.Add("Zip file-02-WF");
            Icons.Add("DataSplit-WF");
            Icons.Add("Way Board 3-WF");
            Icons.Add("Loading8-WF");
            Icons.Add("Favourites Unlocked");
            Icons.Add("Data-Histogram");
            Icons.Add("Mail Download-WF");
            Icons.Add("Contacts Settings");
            Icons.Add("Calendar unlocked");
            Icons.Add("Underline");
            Icons.Add("Certificate-02");
            Icons.Add("Favorite Locked-WF");
            Icons.Add("Bookmark Favourite-WF");
            Icons.Add("Loading - 02");
            Icons.Add("Media-Stop");
            Icons.Add("Maximize-WF");
            Icons.Add("Favourites Sync");
            Icons.Add("Multiple WebPage-WF");
            Icons.Add("Vertical Align Bottom-WF");
            Icons.Add("Text Decoration - 12");
            Icons.Add("Document-Zoom-Out");
            Icons.Add("Media Fast-forward");
            Icons.Add("Login-Door");
            Icons.Add("Calendar Info");
            Icons.Add("Bookmark Sync-WF");
            Icons.Add("Restore-01");
            Icons.Add("Rectangle-Selection");
            Icons.Add("Bookmark add");
            Icons.Add("Phonebook Delete");
            Icons.Add("Book Close-WF");
            Icons.Add("To do list - 01");
            Icons.Add("Software is Available to install");
            Icons.Add("Calendar Add-WF");
            Icons.Add("Money-Coin-01");
            Icons.Add("Calendar Settings");
            Icons.Add("Skew-WF");
            Icons.Add("Login Door1-WF");
            Icons.Add("View-Details-01");
            Icons.Add("User Setting-01-WF");
            Icons.Add("To do list - 02");
            Icons.Add("Tiles-01");
            Icons.Add("File Previous");
            Icons.Add("Document-Zoom-In-01");
            Icons.Add("Favorite Unlocked-WF");
            Icons.Add("Webpage Refresh-WF");
            Icons.Add("Fit to Size-WF");
            Icons.Add("Bookmark Previous-WF");
            Icons.Add("Link4-WF");
            Icons.Add("Mouse Wireless-02-WF");
            Icons.Add("View-Small-Icons");
            Icons.Add("Contact Sync-WF");
            Icons.Add("Document-Download");
            Icons.Add("Folder-Movie-02");
            Icons.Add("Indent Increase-WF");
            Icons.Add("Spell Check");
            Icons.Add("WebPage-WF");
            Icons.Add("Webpage ok-WF");
            Icons.Add("ArrowUp-WF");
            Icons.Add("Skew");
            Icons.Add("Minimize-WF");
            Icons.Add("Volume Speaker-WF");
            Icons.Add("Contact Download-WF");
            Icons.Add("Media End-WF");
            Icons.Add("Folder-Ok");
            Icons.Add("View-Details");
            Icons.Add("Text-Highlight");
            Icons.Add("Mail Save-WF");
            Icons.Add("Book-Open");
            Icons.Add("Text Decoration - 16");
            Icons.Add("Vertical align Top-WF");
            Icons.Add("Web Page Download");
            Icons.Add("Phonebook edit");
            Icons.Add("Zoom vertical - 01");
            Icons.Add("Phonebook ");
            Icons.Add("Document-Exchange-01");
            Icons.Add("Listen2-WF");
            Icons.Add("Mail-WF");
            Icons.Add("Webpage Download-WF");
            Icons.Add("View-Medium-Icons");
            Icons.Add("Movie Favorite-WF");
            Icons.Add("Bookmark Delete-WF");
            Icons.Add("CD-New");
            Icons.Add("Document Find-WF");
            Icons.Add("Phonebook Delete-WF");
            Icons.Add("Password-02-WF");
            Icons.Add("Favourites edit");
            Icons.Add("Volume-Down");
            Icons.Add("Bullet");
            Icons.Add("Flash-Player");
            Icons.Add("Web Page Find");
            Icons.Add("Way Board-WF");
            Icons.Add("Calendar -05");
            Icons.Add("Group Add-WF");
            Icons.Add("Document-Zoom-In-02");
            Icons.Add("Money Coin-03-WF");
            Icons.Add("Speaker - 02");
            Icons.Add("User Add-WF");
            Icons.Add("Phonebook Previous");
            Icons.Add("Document-Save");
            Icons.Add("User Upload");
            Icons.Add("Minimize - 01");
            Icons.Add("Webpage New open-WF");
            Icons.Add("Check");
            Icons.Add("Text Decoration-03");
            Icons.Add("Layers");
            Icons.Add("Calender-02-WF");
            Icons.Add("User Add-01-WF");
            Icons.Add("CD Valid-WF");
            Icons.Add("Bookmark Save-WF");
            Icons.Add("User Info");
            Icons.Add("Group-Cluster");
            Icons.Add("Document Music-WF");
            Icons.Add("Media Rewind/Back-WF");
            Icons.Add("Login-02");
            Icons.Add("Document-Add");
            Icons.Add("Audit-WF");
            Icons.Add("Login Door2-WF");
            Icons.Add("Align-Right");
            Icons.Add("Despeckle-01");
            Icons.Add("MS system setting1-WF");
            Icons.Add("DocumentAdd-WF");
            Icons.Add("Document-Find");
            Icons.Add("Arrow-WF");
            Icons.Add("FIle Save");
            Icons.Add("UpArrow Line-02-WF");
            Icons.Add("Maximize - 01");
            Icons.Add("Folder-Music-01");
            Icons.Add("Speaker  Audible - 01");
            Icons.Add("Para Mark-WF");
            Icons.Add("Favorite Save-WF");
            Icons.Add("ArrowheadLeft-WF");
            Icons.Add("Movie Delete-WF");
            Icons.Add("Folder-Connect");
            Icons.Add("Expand-03-WF");
            Icons.Add("Clipboard");
            Icons.Add("Movie Previous-WF");
            Icons.Add("BookMarks Sync");
            Icons.Add("Shape-Cube");
            Icons.Add("User Info-WF");
            Icons.Add("User Next -01-WF");
            Icons.Add("Bookmark Help-WF");
            Icons.Add("Media-Pause");
            Icons.Add("Device-Tablet");
            Icons.Add("Loading - 03");
            Icons.Add("Command-Redo");
            Icons.Add("CD Catalog-02-WF");
            Icons.Add("Data Collapse-WF");
            Icons.Add("Mail Setting-WF");
            Icons.Add("File-Format-PNG");
            Icons.Add("Zoom - corner - 02");
            Icons.Add("View-List");
            Icons.Add("Contacts Search-WF");
            Icons.Add("Way Board 2-WF");
            Icons.Add("Password-03-WF");
            Icons.Add("Noise-01-WF");
            Icons.Add("Send-To-Back");
            Icons.Add("User Help-02-WF");
            Icons.Add("Zoom Horizontal-WF");
            Icons.Add("Document-Music");
            Icons.Add("Text Decoration - 06");
            Icons.Add("Contacts Sync");
            Icons.Add("Share-02");
            Icons.Add("Play once-01-WF");
            Icons.Add("File-Format-JPEG");
            Icons.Add("Text Highlight-WF");
            Icons.Add("Document Locked-WF");
            Icons.Add("Expander down - 01");
            Icons.Add("Webpage edit");
            Icons.Add("Favourites remove");
            Icons.Add("File Save-WF");
            Icons.Add("Full Screen Expand-WF");
            Icons.Add("File Edit-WF");
            Icons.Add("Submit-02-WF");
            Icons.Add("Link9-WF");
            Icons.Add("Fit-To-Size");
            Icons.Add("Submit-01-WF");
            Icons.Add("Top");
            Icons.Add("Link5-WF");
            Icons.Add("Link2-WF");
            Icons.Add("Link3-WF");
            Icons.Add("Link1-WF");
            Icons.Add("Movie Remove-WF");
            Icons.Add("Way Board 1-WF");
            Icons.Add("File add");
            Icons.Add("Web Page Search");
            Icons.Add("Add csv-02");
            Icons.Add("Drop box");
            Icons.Add("MailSync-WF");
            Icons.Add("Document-Zoom-Out-01");
            Icons.Add("Document-Error-01");
            Icons.Add("Contact Settings-WF");
            Icons.Add("Document-Share");
            Icons.Add("Burn-Disk-02");
            Icons.Add("Expand-02");
            Icons.Add("Share-04-WF");
            Icons.Add("Windows Environment-WF");
            Icons.Add("Dialog-Box-About");
            Icons.Add("Infrared-WF");
            Icons.Add("Calender save-WF");
            Icons.Add("Garbage-Open");
            Icons.Add("Bookmark edit");
            Icons.Add("Speaker Mute - 01");
            Icons.Add("C Sharp-01");
            Icons.Add("Quotation-Marks");
            Icons.Add("Product-Box-With-Disc");
            Icons.Add("Hook-WF");
            Icons.Add("Command-Refresh");
            Icons.Add("File Add-WF");
            Icons.Add("Clipboard-Next-Down");
            Icons.Add("Window-New");
            Icons.Add("Document-Zoom-In");
            Icons.Add("Reload - 01");
            Icons.Add("Play-Once");
            Icons.Add("Text Decoration-WF");
            Icons.Add("Text-Italic");
            Icons.Add("User Help -01-WF");
            Icons.Add("Document Find-02-WF");
            Icons.Add("Calendar Favorite-WF");
            Icons.Add("Textdecorations");
            Icons.Add("Tab-History");
            Icons.Add("Tiles-02-WF");
            Icons.Add("File-Format-BitMap");
            Icons.Add("Bookmark Settings-WF");
            Icons.Add("Agreement-WF");
            Icons.Add("Text Decoration-06");
            Icons.Add("Team-Viewer");
            Icons.Add("Calendar download");
            Icons.Add("Calendar Previous");
            Icons.Add("AlignLeft-WF");
            Icons.Add("Volume-Speaker-02");
            Icons.Add("Volume-Speaker-01");
            Icons.Add("Data Split-WF");
            Icons.Add("Text");
            Icons.Add("MailOk-WF");
            Icons.Add("User previous");
            Icons.Add("Connectivity-Error");
            Icons.Add("Media Play2-WF");
            Icons.Add("DocumentRemove-WF");
            Icons.Add("Volume Speaker 2-WF");
            Icons.Add("Stop Media--02WF");
            Icons.Add("Task-02");
            Icons.Add("Key Hash-WF");
            Icons.Add("Tile Shape-WF");
            Icons.Add("File Sync");
            Icons.Add("Bookmark Down-WF");
            Icons.Add("Favorite Info-WF");
            Icons.Add("Movie Settings-WF");
            Icons.Add("Movie refresh");
            Icons.Add("Folder-Information");
            Icons.Add("Contacts Help-WF");
            Icons.Add("DocumentSync-WF");
            Icons.Add("Drop Box-WF");
            Icons.Add("Arrowhead down-WF");
            Icons.Add("Text-Normal");
            Icons.Add("Animation-03");
            Icons.Add("Bookmark Locked-WF");
            Icons.Add("WebPage Sync -2");
            Icons.Add("Decrease Intend");
            Icons.Add("Mail Help-WF");
            Icons.Add("Bring-To-Front");
            Icons.Add("Braces-01");
            Icons.Add("Find Previous-WF");
            Icons.Add("Task-01-WF");
            Icons.Add("Document-Music-01");
            Icons.Add("Share-03-WF");
            Icons.Add("Free-Hand-Selection");
            Icons.Add("Web Page Help");
            Icons.Add("Rope-Lasso");
            Icons.Add("User-WF");
            Icons.Add("Contacts Search");
            Icons.Add("Certificate-01");
            Icons.Add("Phonebook Info-WF");
            Icons.Add("Folder-Delete");
            Icons.Add("Stack-03");
            Icons.Add("Webpage Help-WF");
            Icons.Add("Edit");
            Icons.Add("Temporary folder1-WF");
            Icons.Add("DataFiles-WF");
            Icons.Add("E Doc Solution-WF");
            Icons.Add("Listen");
            Icons.Add("Numbering-01-WF");
            Icons.Add("View Small Icons-WF");
            Icons.Add("Task-02-WF");
            Icons.Add("Conference-Call");
            Icons.Add("Calendar edit");
            Icons.Add("Hash-WF");
            Icons.Add("WebPage Sync");
            Icons.Add("File-Format-Wave");
            Icons.Add("User-Modify");
            Icons.Add("Music Icon");
            Icons.Add("User that is member of security group");
            Icons.Add("Stop Media");
            Icons.Add("Calendar -03");
            Icons.Add("Arrowhead-Right");
            Icons.Add("Orientation landscape-02-WF");
            Icons.Add("Parenthesis-03-WF");
            Icons.Add("Log file icon");
            Icons.Add("Contacts Next");
            Icons.Add("Media Play-WF");
            Icons.Add("Contacts UnLocked");
            Icons.Add("FavoriteRefresh-WF");
            Icons.Add("Movie Locked");
            Icons.Add("To Do List 1-WF");
            Icons.Add("Cloud-WF");
            Icons.Add("File refresh");
            Icons.Add("Folder Upload 1-WF");
            Icons.Add("Product Box-03-WF");
            Icons.Add("Loading3-WF");
            Icons.Add("Folder-Remove-02");
            Icons.Add("Trash Can1-WF");
            Icons.Add("Speaker-01");
            Icons.Add("Bookmark Refresh");
            Icons.Add("Favourites Find");
            Icons.Add("Mail Refresh");
            Icons.Add("Folder Music-WF");
            Icons.Add("List All Application installed on cmputer");
            Icons.Add("User Ok");
            Icons.Add("Media Pause-WF");
            Icons.Add("Web Page unlocked");
            Icons.Add("Movie Add");
            Icons.Add("File Download");
            Icons.Add("User Delete-02-WF");
            Icons.Add("Garbage Full-WF");
            Icons.Add("Folder Save-WF");
            Icons.Add("Product Box -01-WF");
            Icons.Add("Conference-Call-01");
            Icons.Add("Document New-01-WF");
            Icons.Add("Mail - 02");
            Icons.Add("Message-Warning");
            Icons.Add("Power plant-01-WF");
            Icons.Add("Zip file-01-WF");
            Icons.Add("Parenthesis");
            Icons.Add("Data-Import");
            Icons.Add("Web Page Remove");
            Icons.Add("User Download-01-WF");
            Icons.Add("File Favorite-WF");
            Icons.Add("Database Connection-WF");
            Icons.Add("Web Page Locked");
            Icons.Add("Internet Facilities");
            Icons.Add("File Upload-WF");
            Icons.Add("Password-04-WF");
            Icons.Add("File Help");
            Icons.Add("Wind-01-WF");
            Icons.Add("Power plant");
            Icons.Add("MS System settings Configuration Manger-02");
            Icons.Add("Text Protected-02");
            Icons.Add("Text Protected-01");
            Icons.Add("Calendar Delete-WF");
            Icons.Add("Bookmark Up-WF");
            Icons.Add("Mail Save");
            Icons.Add("Warning-Shield");
            Icons.Add("Mail Locked-WF");
            Icons.Add("Calendar settings-WF");
            Icons.Add("Contacts Favorite-WF");
            Icons.Add("Window-New-01");
            Icons.Add("Documents-02");
            Icons.Add("Multiple threads");
            Icons.Add("Windows-8-Login");
            Icons.Add("Wifi");
            Icons.Add("Calendar remove");
            Icons.Add("Arrow -WF");
            Icons.Add("Calendar Download-WF");
            Icons.Add("Mail Remove-WF");
            Icons.Add("Folder-Edit-01");
            Icons.Add("Media First-WF");
            Icons.Add("Bookmark Ok");
            Icons.Add("Wind-04-WF");
            Icons.Add("Navigation Right-WF");
            Icons.Add("Filter-WF");
            Icons.Add("Power Off-01-WF");
            Icons.Add("User unlocked");
            Icons.Add("Favourites Settings");
            Icons.Add("User Delete");
            Icons.Add("TextDecoration1-WF");
            Icons.Add("Folder 1- WF");
            Icons.Add("Media Fast Forward/Last-WF");
            Icons.Add("Folder-Download-01");
            Icons.Add("Data-Text");
            Icons.Add("Bookmark locked");
            Icons.Add("Phonebook Upload-WF");
            Icons.Add("BookmarkSettings-02-WF");
            Icons.Add("Download-Error");
            Icons.Add("Favorite Edit-WF");
            Icons.Add("Log file icon 2");
            Icons.Add("User locked");
            Icons.Add("Phonebook OK-WF");
            Icons.Add("Folder-Cube");
            Icons.Add("Water Recycling");
            Icons.Add("Navigation-Down");
            Icons.Add("Mobile Phone Message-WF");
            Icons.Add("Add csv-WF");
            Icons.Add("Volume-Speaker");
            Icons.Add("Command Refresh-WF");
            Icons.Add("Earthquake-WF");
            Icons.Add("Volume Icon-WF");
            Icons.Add("Shrink-02-WF");
            Icons.Add("Assign");
            Icons.Add("Shrink-01-WF");
            Icons.Add("Shrink - 01");
            Icons.Add("Software-SAP");
            Icons.Add("Web Page Save");
            Icons.Add("All softwares Updates that are installed in computer-WF");
            Icons.Add("Phonebook Unlocked-WF");
            Icons.Add("Pressure-02-WF");
            Icons.Add("Check-01-WF");
            Icons.Add("Zoom - corner - 03");
            Icons.Add("Webpage Previous-WF");
            Icons.Add("Product Box With Disc-02-WF");
            Icons.Add("MailSearch-WF");
            Icons.Add("Mail upload");
            Icons.Add("User edit-01");
            Icons.Add("Mail remove");
            Icons.Add("Folder-WF");
            Icons.Add("Folder-Music");
            Icons.Add("Document Upload-WF");
            Icons.Add("DataSynchronize-WF");
            Icons.Add("Document-Add-01");
            Icons.Add("Black List Folder");
            Icons.Add("Mail1-WF");
            Icons.Add("Format Bullet-WF");
            Icons.Add("DocumentWarning-WF");
            Icons.Add("Webpage -WF");
            Icons.Add("User Locked-01-WF");
            Icons.Add("Stack-03-WF");
            Icons.Add("Find Replace-WF");
            Icons.Add("Clipboard-WF");
            Icons.Add("Curve");
            Icons.Add("User-Delete");
            Icons.Add("Trash can - 02");
            Icons.Add("Media-End");
            Icons.Add("Bookmark-New");
            Icons.Add("GSM Tower-WF");
            Icons.Add("Show-01-WF");
            Icons.Add("Calendar -04");
            Icons.Add("Speaker  Audible - 02");
            Icons.Add("Calendar-01");
            Icons.Add("Bookmark Search-WF");
            Icons.Add("Speaker - 01");
            Icons.Add("Favourites Save");
            Icons.Add("Document-Delete");
            Icons.Add("Adobe-Acrobat");
            Icons.Add("Data-Defragmentation");
            Icons.Add("File Info-WF");
            Icons.Add("User-Profile");
            Icons.Add("Webpage Settings-WF");
            Icons.Add("DataImport-WF");
            Icons.Add("Arrowhead-Down-01");
            Icons.Add("Share-06-WF");
            Icons.Add("Group-Delete");
            Icons.Add("Folder  Zoom Out- WF");
            Icons.Add("Scale to Fit-01 -WF");
            Icons.Add("Display-Brightness");
            Icons.Add("Wind-05-WF");
            Icons.Add("AlignJustify-WF");
            Icons.Add("Visual-Studio-2012");
            Icons.Add("Phonebook Save");
            Icons.Add("Text Decoration - 15");
            Icons.Add("Show-02-WF");
            Icons.Add("Folder Information-WF");
            Icons.Add("Data-Split-01");
            Icons.Add("Window Earth-WF");
            Icons.Add("Phone Book locked-WF");
            Icons.Add("Folder Remove 1-WF");
            Icons.Add("Key-Access");
            Icons.Add("Key Access-WF");
            Icons.Add("Computer that is Member of security goup");
            Icons.Add("Parenthesis-02-WF");
            Icons.Add("Contacts UnLocked-WF");
            Icons.Add("Text Decoration-07");
            Icons.Add("Find-Replace");
            Icons.Add("Mouse-Wireless");
            Icons.Add("Text-Bold");
            Icons.Add("Rating - 01");
            Icons.Add("Favourites Help");
            Icons.Add("Contact Lock-WF");
            Icons.Add("Navigation Down-WF");
            Icons.Add("Favorite Favorite-WF");
            Icons.Add("Contacts Previous");
            Icons.Add("Loading - 04");
            Icons.Add("Stack-01-WF");
            Icons.Add("View Details1-WF");
            Icons.Add("File-Format-GIF");
            Icons.Add("Money Gold-WF");
            Icons.Add("Movie Info");

            #endregion
            return Icons;

        }

        /// <summary>
        /// Gets the office icon.
        /// </summary>
        /// <returns>The office icon.</returns>
        private static ObservableCollection<string> GetOfficeIcon()
        {
            ObservableCollection<string> Icons =
			[
				"Won",
				"Home-Loan-WF",
				"Car-Loan",
				"Finance-03",
				"Transaction-Fee-WF",
				"Lithuanian-Litas-02",
				"Eritrean-Nakfa",
				"Croatian-Kuna-02",
				"ATM-03",
				"Ugandan-Shilling",
				"Somali-Shilling",
				"Money-Transfer-WF",
				"Coin-01-WF",
				"Netherlands Antillean-Guilder",
				"Cent",
				"Australian-Dollar",
				"Zambian-Kwacha",
				"Franc",
				"Chinese-Renminbi Yuan",
				"Cape Verdean-Escudo",
				"Paraguayan-Guarani",
				"Finance-02",
				"Pound",
				"Savings1-WF",
				"Euro",
				"Money",
				"Czech-Koruna",
				"ATM-02",
				"Bike-Loan-WF",
				"Cash-WF",
				"Djiboutian-Franc",
				"Euro-Coin",
				"Namibian-Dollar",
				"Icelandic-Krone",
				"Bhutanese-Chetrum",
				"Finance-01",
				"Malawian-Kwacha",
				"Money-Bag-WF",
				"Taiwanese-Dollar",
				"Coin - 01",
				"Vietnamese-Dong",
				"Mexican-Peso",
				"Cash",
				"Indian-Rupee",
				"Guatemalan-Quetzal",
				"Complaint-Box-WF",
				"Trinidad and Tobago-Dollar",
				"Ukrainian-Hryvnia",
				"Cheque-02",
				"Cheque-01",
				"Cambodian-Riel",
				"ATM-02-WF",
				"Central African-CFA Franc",
				"Myanmar-Kyat",
				"Mozambican-Metical",
				"Finance-04-WF",
				"Jewel-Loan",
				"Exchange-02-WF",
				"Payments-01-WF",
				"Calculator-WF",
				"Banker-01-WF",
				"Banker-02-WF",
				"Cheque-02-WF",
				"Tunisian-Dinar",
				"Safety-Box-01-WF",
				"Stock-Exchange-03",
				"Maldivian-Rufiyaa",
				"Master-Card",
				"Currency-Sign",
				"Bike-Loan",
				"Singapore-Dollar",
				"Yen",
				"Lao-Kip",
				"Accounts-Receivable",
				"Cuban-Convertible Peso",
				"Accounts-Book-WF",
				"Complaint-Box",
				"Agricultural-Loan",
				"Hong Kong-Dollar",
				"Israeli-New Shekel",
				"Guinean",
				"Locker-WF",
				"Payment-WF",
				"Wallet-WF",
				"Accounting-01",
				"Accounting-02",
				"Malagasy-Ariary",
				"Macanese-Pataca",
				"Cuban-Peso",
				"Egyptian-Pound",
				"Savings",
				"Finance-04",
				"Kenyan-Shilling",
				"Sierra Leonean-Leone",
				"ATM",
				"Kuwaiti-Dinar",
				"Samoan-Tala",
				"Algerian-Dinar",
				"Swiss-Franc",
				"Barbadian-Dollar",
				"Nigerian-Naira",
				"Croatian-Kuna-01",
				"Payments",
				"Telephone",
				"Account-Payable-WF",
				"Chilean-Peso",
				"Debit-Card",
				"Haitian-Gourde",
				"Albanian-Lek",
				"British-Pennies",
				"Fijian-Dollar",
				"Malaysian-Ringgit",
				"Sri Lankan-Rupee",
				"Bhutanese-Ngultrum",
				"Bermudian-Dollar",
				"Latvian-Lats",
				"Iranian-Rial",
				"Stock-Exchange-07",
				"Philippine-Peso",
				"Lesotho-Loti",
				"Botswana-Pula",
				"Latvian-Santims",
				"Home-Loan",
				"Dobra",
				"East Caribbean-Dollar",
				"Employee",
				"Swazi-Lilangeni",
				"Panamanian-Balboa",
				"Jamaican-Dollar",
				"Stock-Exchange-06",
				"Car-Loan-WF",
				"Burundian-Franc",
				"Accounting-02-WF",
				"Venezuelan-Bolivar",
				"Bulgarian-Lev",
				"Account-Payable",
				"Bank-WF",
				"Scanner",
				"Thai-Baht",
				"Cayman Islands-Dollar",
				"Angolan-Kwanza",
				"Counting-Machine",
				"Qatari-Riyal",
				"Serbian-Dinar",
				"Uruguayan-Peso",
				"Turkish-Lira",
				"Mill",
				"Accounts-Book",
				"Peruvian Nuevo-Sol",
				"Ghana-Cedi",
				"Stock-Exchange-04",
				"Nicaraguan-Cordoba",
				"Jordanian-Dinar",
				"Hungarian-Forint",
				"Gold-WF",
				"Special-Drawing-Rights",
				"Dominican-Peso",
				"Moroccan-Dirham",
				"Japanese-Yen",
				"Seychellois-Rupee",
				"Finance-03-WF",
				"Telephone-WF",
				"Costa Rican-Cólon",
				"Bahamian-Dollar",
				"Kyrgyzstani-Som",
				"Rwandan-Franc",
				"Dollar",
				"Coin - 02",
				"Polish-Zloty",
				"Pending-Payment",
				"Exchange - 02",
				"Exchange - 01",
				"Stock-Exchange-02",
				"Transaction-Fee",
				"Indonesian-Rupiah",
				"Payment-02",
				"Canadian-Dollar",
				"Belarusian-Ruble",
				"Colombian-Peso",
				"Libyan-Dinar",
				"United Arab Emirates-Dirham",
				"Coin-02-WF",
				"Stock-Exchange-01",
				"Gold",
				"Belize-Dollar",
				"Pound-Coin",
				"Phone-WF",
				"Bangladeshi-Taka",
				"United States-Dollar",
				"South African-Rand",
				"Bank-01-WF",
				"Egyptian-Piastre",
				"Stock-Exchange-05",
				"ATM-01-WF",
				"Brunei-Dollar",
				"Rupee",
				"Zimbabwean-Dollar",
				"Locker",
				"Stcok-Exchange-03-WF",
				"Bank",
				"Safety-Box-02",
				"Phone",
				"Bosnian and Herzegovina-Convertible Mark",
				"Swedish-Krona",
				"Czech-Haler",
				"Vanuatu-Vatu",
				"Mauritanian-Ouguiya",
				"Calculator",
				"Money-Transfer",
				"Dollar-Coin",
				"Money-Bag",
				"British-Pounds",
				"Accounting-01-WF",
				"ATM-03-WF",
				"Macedonian-Denar",
				"Bahraini-Dinar",
				"Saudi-Riyal",
				"Polish-Grosz",
				"Money-WF",
				"Ethiopian-Birr",
				"Gambian-Dalasi",
				"Afghan-Afghani",
				"Mongolian-Togrog",
				"ATM-01",
				"Surinamese-Dollar",
				"New Zealand-Dollar",
				"Safety-Box-01",
				"Money-Deposit",
				"Lithuanian-Litas-01",
				"Brazilian-Real",
				"Agricultural-Loan-WF",
				"Iraqi-Dinar",
				"Albanian-Qindarka",
				"Dollar-Coin-WF",
				"West African-CFA Franc",
				"Payment-01",
				"Guyanese-Dollar",
				"Customer",
				"Solomon Islands-Dollar",
				"Fund",
				"Comorian-Franc",
				"Banking-Transaction",
				"Aruban-Florin",
				"Scanner-WF",
				"Liberian-Dollar",
				"Omani-Rial",
				"Georgian-Lari",
				"European-Euro",
				"ATM-04",
			];
            return Icons;
        }

        /// <summary>
        /// Gets the transport collection.
        /// </summary>
        /// <returns>The transport collection.</returns>
        private static ObservableCollection<string> GetTransportCollection()
        {
            ObservableCollection<string> Icons =
			[
				"Wine-Glass-06",
				"Wine-Glass-05",
				"Wine-Glass-04",
				"Wine-Glass-03",
				"Wine-Glass-02",
				"Wine-Glass-01",
				"Beverage-Juice-01",
				"Vegetable-Carrot",
				"Beverage-Milk-Shake",
				"Milk-Bottle",
				"Fruit-Lemon",
				"Fruit-Apple",
				"Candy-Lollipop",
				"Tea-Bag",
				"Chocolate-03",
				"Cheese-01",
				"Snack-Burger",
				"Beverage-Tea",
				"Fruit-Cherry",
				"Vegetable-Onion",
				"Bread-02",
				"Bar",
				"Beverage-Coffee-03",
				"Restaurant",
				"Beverage-Juice-03",
				"Chocolate-01",
				"Coffee - 01",
				"Wine Glass - 01",
				"Fruit-Banana-02",
				"Milk",
				"Cereal-Corn-01",
				"Vegetable-Tomato-01",
				"Vegetable-Capsicum-Pepper",
				"Fruit-Orange-02",
				"Cocktail-03",
				"Seafood-Fish",
				"Fork and Knife",
				"Cooking-Gloves",
				"Ice-Cream-03",
				"Cake-Cookie",
				"Vegetable-Tomato-02",
				"Meat-Chicken-Kebab",
				"Chocolate-05",
				"Cereal-Wheat",
				"Vegetable-Radish",
				"Vegetable-Chilli",
				"Cake-02",
				"Strawberry1",
				"Beverage-Cocktail-01",
				"Coffee Cup - 01",
				"Chicken-Egg",
				"Fruit-Watermelon",
				"Chicken-02",
				"Ice-Cream-01",
				"Chicken-01",
				"Bowl",
				"Vegetable-Pumpkin",
				"Beverage-Coffee-04",
				"Beverage-Beer-02",
				"Beverage-Coffee-02",
				"Noodles-02",
				"Fruit-Apple-02",
				"Bean-01",
				"Beverage-Juice-02",
				"Fork and Spoon",
				"Cereal-Corn-02",
				"Beverage-Alcohol",
				"Chocolate-02",
				"Vegetable-Brinjal-Eggplant",
				"Soup-01",
				"Beverage-Milk-01",
				"Snack-Doughnut",
				"Beverage-Coffee-01",
				"Fruit-Strawberry",
				"Cheese-02",
				"Bread-01",
				"Snack-French-Fries",
				"Prawn-01",
				"Cocktail-02",
				"Cutlery-Fork-Knife",
				"Cake-Slice-01",
				"Chocolate-06",
				"Cake-01",
				"Fruit-Orange-03",
				"Coffee-Bean",
				"Jar",
				"Hand-Fork",
				"Pizza-02",
				"Cocktail-01",
				"Water-Drop",
				"Cooker",
				"Beverage-Milk-02",
				"Wine Bottle - 01",
				"Knife",
				"Beverage-Wine",
				"Chocolate-04",
				"Noodles-01",
				"Ice-Cream-02",
				"Chef-Cap",
				"Fruit-Orange-01",
				"Cup-Cake",
				"Pizza-01",
				"Seafood-Shrimp",
				"Beverage-Beer-01",
				"Prawn",
				"Vegetable-Cabbage",
				"Beverage-Cocktail-02",
				"Spices-Salt-Pepper",
				"Coffee Cup - 02",
				"Fruit-Grapes",
				"Fruit-Banana-01",
			];
            return Icons;
        }
    }

}
