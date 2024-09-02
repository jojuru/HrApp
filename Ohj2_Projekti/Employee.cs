using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohj2_Projekti
{
    public class Employee
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string ssn { get; set; }
        public string street { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }

        public ObservableCollection<Employment> employment { get; set; }

        public Employee() { }
        public Employee(string firstname, string lastname, string nickname, string ssn, string street, string zipcode, string city)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.nickname = nickname;
            this.ssn = ssn;
            this.street = street;
            this.zipcode = zipcode;
            this.city = city;
            this.employment = new ObservableCollection<Employment> { };
        }
    }
}
