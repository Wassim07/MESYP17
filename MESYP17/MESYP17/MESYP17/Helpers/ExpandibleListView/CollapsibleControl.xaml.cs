using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MESYP17.Helpers.ExpandibleListView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollapsibleControl : ContentView
    {
        private ViewCell _parentViewCell;

        public CollapsibleControl()
        {
            InitializeComponent();
            HeaderGrid.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    ToggleFrameVisibility();
                })
            });
        }

        private void ToggleFrameVisibility()
        {
            frame.IsVisible = !frame.IsVisible;
            if (_parentViewCell == null)
            {
                _parentViewCell = getParentViewCell();
            }

            if (_parentViewCell != null)
            {
                _parentViewCell.ForceUpdateSize();
            }
        }



        private ViewCell getParentViewCell()
        {
            try
            {
                Element currentView = this;
                for (int i = 0; i < 5; i++)
                {
                    currentView = currentView.Parent;
                    if (currentView is ViewCell)
                        return currentView as ViewCell;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ExpandedViewProperty.PropertyName)
            {
                frame.Content = ExpandedView;
            }


            if (propertyName == NameProperty.PropertyName)
            {
                NameLabel.Text = Name;
            }
            else if (propertyName == DateProperty.PropertyName)
            {
                if (string.IsNullOrEmpty(Date))
                {
                    DateLabel.IsVisible = false;
                }
                else
                {
                    //DateLabel.TextColor = DateTextColor;
                    DateLabel.Text = Date;
                    //DateLabel.IsVisible = true;

                }
            }
            //else if (propertyName == DateTextColorProperty.PropertyName)
            //{
            //    DateLabel.TextColor = DateTextColor;
            //}

            else if (propertyName == PlaceProperty.PropertyName)
            {
                PlaceLabel.Text = Place;
                //PlaceLabel.IsVisible = true;
            }
            else if (propertyName == SpeakerProperty.PropertyName)
            {
                SpeakerLabel.Text = Speaker;
            }
        }


        #region ExpandedView property       
        public static readonly BindableProperty ExpandedViewProperty =
                   BindableProperty.Create("ExpandedView", typeof(View), typeof(CollapsibleControl), default(View), propertyChanged: OnExpandedViewChanged);

        private static void OnExpandedViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // ExpandedView Property changed implementation goes here
        }

        public View ExpandedView
        {
            get { return (View)GetValue(ExpandedViewProperty); }
            set { SetValue(ExpandedViewProperty, value); }
        }
        #endregion


        #region Name property       
        public static readonly BindableProperty NameProperty =
                   BindableProperty.Create("Name", typeof(string), typeof(CollapsibleControl), default(string), propertyChanged: OnNameChanged);

        private static void OnNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Title Property changed implementation goes here
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        #endregion



        #region Date property       
        public static readonly BindableProperty DateProperty =
                   BindableProperty.Create("Date", typeof(string), typeof(CollapsibleControl), default(string), propertyChanged: OnDateChanged);

        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Subtitle Property changed implementation goes here
        }

        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        #endregion


        #region Place property       
        public static readonly BindableProperty PlaceProperty =
                   BindableProperty.Create("Place", typeof(string), typeof(CollapsibleControl), default(string), propertyChanged: OnPlaceChanged);

        private static void OnPlaceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Subtitle Property changed implementation goes here
        }

        public string Place
        {
            get { return (string)GetValue(PlaceProperty); }
            set { SetValue(PlaceProperty, value); }
        }
        #endregion

        #region Speaker property       
        public static readonly BindableProperty SpeakerProperty =
                   BindableProperty.Create("Speaker", typeof(string), typeof(CollapsibleControl), default(string), propertyChanged: OnSpeakerChanged);

        private static void OnSpeakerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Subtitle Property changed implementation goes here
        }

        public string Speaker
        {
            get { return (string)GetValue(SpeakerProperty); }
            set { SetValue(SpeakerProperty, value); }
        }
        #endregion

        #region DateTextColor property       
        //public static readonly BindableProperty DateTextColorProperty =
        //           BindableProperty.Create("DateTextColor", typeof(Color), typeof(CollapsibleControl), Color.White, propertyChanged: OnDateTextColorChanged);

        //private static void OnDateTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    // SubtitleTextColor Property changed implementation goes here
        //}

        //public Color DateTextColor
        //{
        //    get { return (Color)GetValue(DateTextColorProperty); }
        //    set { SetValue(DateTextColorProperty, value); }
        //}
        #endregion
    }
}