using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Siddhesh_Zantye_Visitor_Booking
{
    public partial class Form1 : Form
    {
        public FormData data = new FormData();

        public class ErrorMessage
        {
            public string input;
            public string errorMessage;
            public bool success = false;

            public ErrorMessage(string InputName, string ErrorMessage)
            {
                if (InputName == "" && ErrorMessage == "")
                {
                    success = true;
                }
                input = InputName;
                errorMessage = ErrorMessage;
            }
        }

        public class FormData
        {
            public string fullName;
            public string dob;
            public List<long> phoneNumbers = new List<long>();
            public string email;
            public string chosenEvent;
            public List<string> communicationPreferences = new List<string>();

            public ErrorMessage validateInput()
            {
                if (communicationPreferences.Count == 0)
                {
                    return new ErrorMessage("Communication", "You must select at least one prefered communication method");
                };
                if (fullName == "")
                {
                    return new ErrorMessage("Fullname", "You must provide your full name");
                }
                if (dob == "")
                {
                    return new ErrorMessage("DOB", "You must provide a date of birth");
                }
                if (phoneNumbers.Count == 0 || phoneNumbers.Count > 3)
                {
                    return new ErrorMessage("Phone", "You must provide at least one phone number and no more than 3! Click the Add button when adding a number");
                }
                if (email == "")
                {
                    return new ErrorMessage(
                        "Email", "You must provide an email address!");
                }
                if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    return new ErrorMessage("Email", "Please input a VALID email address!");
                }
                if (chosenEvent == "")
                {
                    return new ErrorMessage("Event", "You must select an event");
                }
                return new ErrorMessage("", "");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            data.communicationPreferences.Clear();

            data.fullName = FullNameInput.Text;
            data.dob = DOBPicker.Value.ToShortDateString();
            data.email = EmailAddressInput.Text;

            string chosenEvent = CultureRadioButton.Checked ? "Culture Show" : ScienceRadioButton.Checked ? "Science Event" : MagicRadioButton.Checked ? "Magic Show" : "";
            data.chosenEvent = chosenEvent;

            if (EmailCheckbox.Checked)
            {
                data.communicationPreferences.Add("Email");
            }
            if (TextCheckbox.Checked)
            {
                data.communicationPreferences.Add("Text");
            }
            if (PhoneCheckbox.Checked)
            {
                data.communicationPreferences.Add("Phone");
            }

            ErrorMessage error = data.validateInput();
            if (!error.success)
            {
                handleError(error);
                return;
            }

            resetInputFields();
            addToListBox();
        }

        private void handleError(ErrorMessage error)
        {
            Dictionary<string, dynamic> inputFields = new Dictionary<string, dynamic>()
            {
                {"Phone", PhoneNumberInput },
                {"DOB", DOBPicker },
                {"Fullname", FullNameInput },
                {"Email", EmailAddressInput },
            };

            MessageBox.Show(error.errorMessage);
            if (inputFields.ContainsKey(error.input))
            {
                inputFields[error.input].Focus();
            }
        }


        private void resetInputFields()
        {
            FullNameInput.Clear();
            DOBPicker.ResetText();
            PhoneNumberInput.Clear();
            EmailAddressInput.Clear();

            CultureRadioButton.Checked = false;
            ScienceRadioButton.Checked = false;
            MagicRadioButton.Checked = false;

            EmailCheckbox.Checked = false;
            TextCheckbox.Checked = false;
            PhoneCheckbox.Checked = false;
        }

        private void addToListBox()
        {
            string[] items = {
                "----------User Information----------",
                "Full Name: "+ data.fullName,
                "Date Of Birth: "+ data.dob,
                "Phone Number(s): " + string.Join(", ", data.phoneNumbers),
                "Email: " + data.email,
                "Chosen Event: " + data.chosenEvent,
                "Communication Preferences: " + string.Join(", ", data.communicationPreferences),
            };

            foreach (string item in items)
            {
                UserInformationBox.Items.Add(item);
            }
            UserInformationBox.Items.Add("-----Information Added Successfully!-----");

        }

        private void AddPhoneNumberButton_Click(object sender, EventArgs e)
        {
            if (data.phoneNumbers.Count >= 3)
            {
                MessageBox.Show("You can only have 3 phone numbers!");
                PhoneNumberInput.Focus();
                return;
            }
            if (!long.TryParse(PhoneNumberInput.Text, out long x))
            {
                MessageBox.Show("The phone number must be an integer!");
                PhoneNumberInput.Focus();
                return;
            }
            data.phoneNumbers.Add(x);
            if (MessageBox.Show("Are you sure you want to add this?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            MessageBox.Show("Phone Number: " + x + " has been successfully added!");
            PhoneNumberInput.Clear();
        }

    }
}
