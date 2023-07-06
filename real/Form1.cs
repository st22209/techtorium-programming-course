using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Siddhesh_Zantye_Student_Booking
{
    public partial class Form1 : Form
    {
        public FormData data = new FormData();

        public class ErrorMessage
        // This class is something i created to make managing errors easier
        // It will return the field that produced the error, the error message and if success
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

        /*
        This class is something I created to handle data more easily
        It allows all the data to be easily accessable from one instance (in this case is the data variable)
        */
        public class FormData
        {
            public string fullName;
            public string dob;
            public List<long> phoneNumbers = new List<long>();
            public string email;
            public string chosenCourse;
            public List<string> communicationPreferences = new List<string>();
            
            // this function will validate if all the input is correct
            // If it is not correct it will return a ErrorMessage error
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
                    return new ErrorMessage("Email", "You must provide an email address!");
                }
                if (chosenCourse == "")
                {
                    return new ErrorMessage("Course", "You must select a course");
                }
                return new ErrorMessage("", "");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        // This function will run upon the submit button being clicked
        // It will handle taking the information from the input fields
        // It will then display all the data in a list box
        private void submit_Click(object sender, EventArgs e)
        {
            // This is to check if there is only one phone number and the input is empty
            // This means the student only wants to enter one number so never hit the add button
            if (data.phoneNumbers.Count == 0 && PhoneNumberInput.Text != "")
            {
                // This tries to parse the phonenumber to a long
                // Since the phone number should be numeric, if this doesnt fail we know the user entered an number
                if (!long.TryParse(PhoneNumberInput.Text, out long x))
                {
                    MessageBox.Show("The phone number must be an integer!");
                    PhoneNumberInput.Focus();
                    return;
                }
                
                // Add the phone number to the phone numbers array
                data.phoneNumbers.Add(x);
                MessageBox.Show("Phone Number: " + x + " has been successfully added!");

                // Clear the input field so user can enter another number (if they want)
                PhoneNumberInput.Clear();
            }
            // GATHER USER DATA FROM FIELDS AND STORE IN DATA (INSTANCE OF FORMDATA) OBJECT
            
            data.communicationPreferences.Clear();

            data.fullName = FullNameInput.Text;
            data.dob = DOBPicker.Value.ToShortDateString();
            data.email = EmailAddressInput.Text;

            string chosenCourse = ISRadioButton.Checked ? "Information Systems" : EngineeringRadioButton.Checked ? "Engineering" : SDRadioButton.Checked ? "Software Development" : "";
            data.chosenCourse = chosenCourse;

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

            // Try to validate the data, if this fails it will return a object of type ErrorMessage
            ErrorMessage error = data.validateInput();
            if (!error.success)
            {
                handleError(error);
                return;
            }
            
            // Reset everything for next student
            resetInputFields();
            addToListBox();
            data.phoneNumbers.Clear();
        }

        private void handleError(ErrorMessage error)
        {
            // Mapping of error type to actual input instance
            Dictionary<string, dynamic> inputFields = new Dictionary<string, dynamic>()
            {
                {"Phone", PhoneNumberInput },
                {"DOB", DOBPicker },
                {"Fullname", FullNameInput },
                {"Email", EmailAddressInput },
            };

            // show error and focus field
            MessageBox.Show(error.errorMessage);
            if (inputFields.ContainsKey(error.input))
            {
                inputFields[error.input].Focus();
            }
        }


        private void resetInputFields()
        {
            // Reset everything

            FullNameInput.Clear();
            DOBPicker.ResetText();
            PhoneNumberInput.Clear();
            EmailAddressInput.Clear();

            ISRadioButton.Checked = false;
            EngineeringRadioButton.Checked = false;
            SDRadioButton.Checked = false;

            EmailCheckbox.Checked = false;
            TextCheckbox.Checked = false;
            PhoneCheckbox.Checked = false;
        }

        private void addToListBox()
        {
            // Each item in this array represents a line of the output
            string[] items = {
                "----------Student Information----------",
                "Full Name: "+ data.fullName,
                "Date Of Birth: "+ data.dob,
                "Phone Number(s): " + string.Join(", ", data.phoneNumbers),
                "Email: " + data.email,
                "Chosen Course: " + data.chosenCourse,
                "Communication Preferences: " + string.Join(", ", data.communicationPreferences),
            };

            // Display all these lines in the listbox
            foreach (string item in items)
            {
                UserInformationBox.Items.Add(item);
            }
            UserInformationBox.Items.Add("-----Information Added Successfully!-----");

        }


        // This function handles when a user clicks the add button
        // this is when they want to add more than one number
        private void AddPhoneNumberButton_Click(object sender, EventArgs e)
        {
            if (data.phoneNumbers.Count >= 3)
            {
                MessageBox.Show("You can only have 3 phone numbers!");
                PhoneNumberInput.Focus();
                return;
            }
            // Check if input was numeric
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
