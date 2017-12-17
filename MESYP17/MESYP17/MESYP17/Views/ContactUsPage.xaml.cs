using System.Collections.Generic;
using Plugin.Messaging;
using Xamarin.Forms;

namespace MESYP17.Views
{
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
            MembersList.ItemsSource=new List<TeamMember>()
            {
                //Contacts List
            };
        }

        private void PhoneCall(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as TeamMember ;
            var phone = CrossMessaging.Current.PhoneDialer;
            if (phone.CanMakePhoneCall)
            {
                phone.MakePhoneCall(user.phoneNumber);
            }
            else DisplayAlert("Error", "Your Device Cannot Make a Call", "ok");

            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
    

    //private void SendEmail(object sender, EventArgs e)
    //{
    //    var emailSender = CrossMessaging.Current.EmailMessenger;
    //    if (emailSender.CanSendEmail)
    //    {
    //        var email = new EmailMessageBuilder().To(participant.Email).Build();
    //        emailSender.SendEmail(email);
    //    }
    //    else DisplayAlert("Error", "Your Device Cannot Send a Message", "ok");
    //}
}

class TeamMember
{
    public string fullName { get; set; }
    public string avatar { get; set; }
    public string  poste { get; set; }
    public string phoneNumber { get; set; }
}