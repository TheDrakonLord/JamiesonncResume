using System.Xml.Linq;

namespace JamiesonncResume
{
    /// <summary>
    /// This class acts as a data structure that holds the data from the xml file in a format usable by the site
    /// </summary>
    public class Resume
    {
        // establish varaibles
        private Name _name;
        private Address _address;
        private string _email;
        private string _phone;
        Dictionary<string, Uri> _sites = new Dictionary<string, Uri>();
        List<string> _generalSkills = new List<string>();
        List<string> _progLangSkills = new List<string>();
        List<string> _toolSkills = new List<string>();
        List<Education> _educationHistory = new List<Education>();
        List<Work> _workHistory = new List<Work>();
        List<Volunteer> _volunteerHistory = new List<Volunteer>();
        List<Award> _awards = new List<Award>();
        List<Clearance> _clearances = new List<Clearance>();

        /// <summary>
        /// on creation of the object, extract the data from the xml file
        /// </summary>
        /// <param name="xmlResumeData">the file path of the xml file that holds the resume data</param>
        public Resume(XElement xmlResumeData)
        {
            // locate the basic info element
            IEnumerable<XElement> basicInfo =
                from el in xmlResumeData.Elements("basicInfo")
                select el;

            // load the name information into the relevant enumeration
            _name = new Name(
                (string)
                (from el in basicInfo.Descendants("firstName")
                 select el).First(),
                (string)
                (from el in basicInfo.Descendants("middleName")
                 select el).First(),
                (string)
                (from el in basicInfo.Descendants("lastName")
                 select el).First());

            // locate the contact info element
            IEnumerable<XElement> contactInfo =
                from el in xmlResumeData.Elements("contactInfo")
                select el;

            // load the email string
            _email = (string)
                (from el in contactInfo.Descendants("email")
                 select el).First();

            // load the phone number string
            _phone = (string)
                (from el in contactInfo.Descendants("phone")
                 select el).First();

            // locate the address element
            IEnumerable<XElement> xmlAddress =
                from el in contactInfo.Elements("address")
                select el;

            // load the address data into its relevant enumeration
            _address = new Address(
                (string)
                (from el in xmlAddress.Descendants("street")
                 select el).First(),
                (string)
                (from el in xmlAddress.Descendants("apartment")
                 select el).First(),
                (string)
                (from el in xmlAddress.Descendants("city")
                 select el).First(),
                (string)
                (from el in xmlAddress.Descendants("state")
                 select el).First(),
                (int)
                (from el in xmlAddress.Descendants("zip")
                 select el).First());

            // locate the websites element
            IEnumerable<XElement> xmlWebsite =
                from el in xmlResumeData.Elements("websites")
                select el;

            // locate the sites element from within the websites
            IEnumerable<XElement> xmlSites =
                from el in xmlWebsite.Elements("site")
                select el;

            // load each site as a new item in the _sites list
            foreach (var s in xmlSites)
            {
                _sites.Add((string)s.Attribute("name"), new Uri(s.Value));
            }

            // locate the skills element
            IEnumerable<XElement> xmlSkills =
                from el in xmlResumeData.Elements("skills")
                select el;

            // locate the general element within skills
            IEnumerable<XElement> xmlGeneralSkills =
                from el in xmlSkills.Elements("general")
                select el;

            // locate the skill element within general
            IEnumerable<XElement> xmlGeneralSkillList =
                from el in xmlGeneralSkills.Elements("skill")
                select el;

            // load each general skill as a new item in the relevant list
            foreach (var g in xmlGeneralSkillList)
            {
                _generalSkills.Add(g.Value);
            }

            // locate the progLang element within skills
            IEnumerable<XElement> xmlProgLangSkills =
                from el in xmlSkills.Elements("progLang")
                select el;

            // locate the skill element within progLang
            IEnumerable<XElement> xmlProgLangSkillList =
                from el in xmlProgLangSkills.Elements("skill")
                select el;

            // load each programming language skill as a new item in the relevant list
            foreach (var p in xmlProgLangSkillList)
            {
                _progLangSkills.Add(p.Value);
            }

            // locate the tools element within skills
            IEnumerable<XElement> xmlToolSkills =
                from el in xmlSkills.Elements("tools")
                select el;

            // locate the skill element within tools
            IEnumerable<XElement> xmlToolSkillList =
                from el in xmlToolSkills.Elements("skill")
                select el;

            // load each tool skill as a new item in the relevant list
            foreach (var t in xmlToolSkillList)
            {
                _toolSkills.Add(t.Value);
            }

            // locate the education element
            IEnumerable<XElement> xmlEducation =
                from el in xmlResumeData.Elements("education")
                select el;

            // locate the history element within education
            IEnumerable<XElement> xmlEducationHistory =
                from el in xmlEducation.Elements("history")
                select el;

            // for each element, load it as a new item in the _educationHistory list
            foreach (var e in xmlEducationHistory)
            {
                IEnumerable<XElement> xmlDegree =
                    from el in e.Elements("degree")
                    select el;

                IEnumerable<XElement> xmlMajor =
                    from el in xmlDegree.Elements("major")
                    select el;

                IEnumerable<XElement> xmlMinor =
                    from el in xmlDegree.Elements("minor")
                    select el;

                _educationHistory.Add(new Education((string) e.Attribute("name"),
                    ((string)
                    (from el in xmlMajor.Descendants("type")
                     select el).First(),
                     (string)
                     (from el in xmlMajor.Descendants("field")
                      select el).First()),
                    (string)
                    (from el in xmlMinor.Descendants("field")
                     select el).First(),
                    DateTime.Parse((string)
                    (from el in e.Descendants("startDate")
                     select el).First()),
                    DateTime.Parse((string)
                    (from el in e.Descendants("endDate")
                     select el).First())));
            }

            // locate the workHistory element
            IEnumerable<XElement> xmlWork =
                from el in xmlResumeData.Elements("workHistory")
                select el;

            // locate the history element within workHistory
            IEnumerable<XElement> xmlWorkHistory =
                from el in xmlWork.Elements("history")
                select el;

            // load each work history element as a new item in the work history list
            foreach (var w in xmlWorkHistory)
            {
                IEnumerable<XElement> xmlAssignment =
                    from el in w.Elements("assignment")
                    select el;

                List<Assignment> assignments = new List<Assignment>();

                foreach (var a in xmlAssignment)
                {

                    IEnumerable<XElement> xmlResponsibilities =
                    from el in a.Elements("responsibilities")
                    select el;

                    IEnumerable<XElement> xmlResponsibility =
                    from el in xmlResponsibilities.Elements("responsibility")
                    select el;

                    List<(string name, string description)> responsibilities = new List<(string name, string description)>();

                    foreach (var r in xmlResponsibility)
                    {
                        responsibilities.Add((r.Attribute("name").Value,r.Value));
                    }

                    assignments.Add(new Assignment(a.Attribute("name").Value,
                        (string)
                        (from el in a.Descendants("location")
                         select el).First(),
                        DateTime.Parse((string)
                        (from el in a.Descendants("startDate")
                         select el).First()),
                        DateTime.Parse((string)
                        (from el in a.Descendants("endDate")
                         select el).First()),
                        responsibilities));
                }

                _workHistory.Add(new Work(w.Attribute("name").Value,
                    (string)
                    (from el in w.Descendants("jobTitle")
                     select el).First(),
                    assignments));
            }

            // locate the volunteerExperience element
            IEnumerable<XElement> xmlVolunteer =
                from el in xmlResumeData.Elements("volunteerExperience")
                select el;

            // locate the experience element within volunteerExperience
            IEnumerable<XElement> xmlVolunteerExperience =
                from el in xmlVolunteer.Elements("experience")
                select el;

            // load each element as a new item in the _volunteerHistory list
            foreach (var v in xmlVolunteerExperience)
            {
                _volunteerHistory.Add(new Volunteer(v.Attribute("name").Value,
                    (string)
                    (from el in v.Descendants("location")
                     select el).First(),
                    (string)
                    (from el in v.Descendants("event")
                     select el).First(),
                    DateTime.Parse((string)
                    (from el in v.Descendants("startDate")
                     select el).First()),
                    DateTime.Parse((string)
                    (from el in v.Descendants("endDate")
                     select el).First())));
            }

            // locate the awards element
            IEnumerable<XElement> xmlAward =
                from el in xmlResumeData.Elements("awards")
                select el;

            // locate the award element within awards
            IEnumerable<XElement> xmlAwardList =
                from el in xmlAward.Elements("award")
                select el;

            // load each element as a new item in the _awards list
            foreach (var d in xmlAwardList)
            {
                _awards.Add(new Award(d.Attribute("name").Value,
                    d.Attribute("type").Value,
                    DateTime.Parse((string)
                    (from el in d.Descendants("date")
                     select el).First()),
                    (string)
                    (from el in d.Descendants("description")
                     select el).First()));
            }

            // locate the currentClearance element
            IEnumerable<XElement> xmlClearances =
                from el in xmlResumeData.Elements("currentClearance")
                select el;

            // locate the clearance element within currentClearance
            IEnumerable<XElement> xmlClearanceList =
                from el in xmlClearances.Elements("clearance")
                select el;

            // load each element as a new item in the _clearances list
            foreach (var c in xmlClearanceList)
            {
                _clearances.Add(new Clearance(c.Attribute("name").Value,
                    DateTime.Parse((string)
                    (from el in c.Descendants("startDate")
                     select el).First()),
                    DateTime.Parse((string)
                    (from el in c.Descendants("endDate")
                     select el).First())));
            }
        }

        /// <summary>
        /// returns the name enumeration which contains the first, middle, and last names
        /// </summary>
        public Name Name { get { return _name; } }

        /// <summary>
        /// returns an enumeration representing the current address
        /// </summary>
        public Address Address { get { return _address; } }

        /// <summary>
        /// returns a string containing the current email address
        /// </summary>
        public string Email { get { return _email; } }

        /// <summary>
        /// returns a string containing the current phone number
        /// </summary>
        public string Phone { get { return _phone; } }

        /// <summary>
        /// returns a dictionary containing all the websites loaded in
        /// </summary>
        public Dictionary<string, Uri> Sites { get { return _sites; } }

        /// <summary>
        /// returns a list containing all general skills loaded in
        /// </summary>
        public List<string> GeneralSkills { get { return _generalSkills; } }

        /// <summary>
        /// returns a list containing all programming language skills loaded in
        /// </summary>
        public List<string> ProgLangSkills { get { return _progLangSkills; } }

        /// <summary>
        /// returns a list containing all tool skills loaded in
        /// </summary>
        public List<string> ToolSkills { get { return _toolSkills; } }

        /// <summary>
        /// returns a list containing all education history entries
        /// </summary>
        public List<Education> EducationHistory { get { return _educationHistory; } }

        /// <summary>
        /// returns a list containing all work history entries
        /// </summary>
        public List<Work> WorkHistory { get { return _workHistory; } }

        /// <summary>
        /// returns a list containing all volunteer history entries
        /// </summary>
        public List<Volunteer> VolunteerHistory { get { return _volunteerHistory; } }

        /// <summary>
        /// returns a list containing all awards
        /// </summary>
        public List<Award> Awards { get { return _awards; } }

        /// <summary>
        /// returns a list containing all clearances
        /// </summary>
        public List<Clearance> Clearances { get { return _clearances; } }
    }

    /// <summary>
    /// structure for holding name data
    /// </summary>
    public struct Name
    {
        /// <summary>
        /// default constructor for the name struct
        /// </summary>
        /// <param name="first">the first name of the subject</param>
        /// <param name="middle">the middle name of the subject</param>
        /// <param name="last">the last name of the subject</param>
        public Name(string first, string middle, string last)
        {
            First = first;
            Middle = middle;
            Last = last;
        }

        /// <summary>
        /// contains the first name of the subject
        /// </summary>
        public string First { get; }

        /// <summary>
        /// contains the middle name of the subject
        /// </summary>
        public string Middle { get; }

        /// <summary>
        /// contains the last name of the subject
        /// </summary>
        public string Last { get; }

        /// <summary>
        /// outputs the name struct in First Middle Last format
        /// </summary>
        /// <returns>a string with the name in First Middle Last format</returns>
        public override string ToString() => $"{First} {Middle} {Last}";
    }

    /// <summary>
    /// Struct for holding address data
    /// </summary>
    public struct Address
    {
        /// <summary>
        /// default contstructor for the address struct
        /// </summary>
        /// <param name="street">the street address of the subject</param>
        /// <param name="apartment">the appartment number of the subject</param>
        /// <param name="city">the city of the subject</param>
        /// <param name="state">the state of the subject</param>
        /// <param name="zip">the zip code of the subject</param>
        public Address(string street, string apartment, string city, string state, int zip)
        {
            Street = street;
            Apartment = apartment;
            City = city;
            State = state;
            Zip = zip;
        }

        /// <summary>
        /// contains the street address of the subject
        /// </summary>
        public string Street { get; }

        /// <summary>
        /// contains the apartment number of the subject
        /// </summary>
        public string Apartment { get; }

        /// <summary>
        /// contains the city of the subject
        /// </summary>
        public string City { get; }

        /// <summary>
        /// contains the state of the subject
        /// </summary>
        public string State { get; }

        /// <summary>
        /// contains the zip code of the subject
        /// </summary>
        public int Zip { get; }

        /// <summary>
        /// converts the struct into "street, apartment, city, state, zip code" format
        /// </summary>
        /// <returns>returns a string with the address in "street, apartment, city, state, zip code" format</returns>
        public override string ToString() => $"{Street}, {Apartment}, {City}, {State}, {Zip}";
    }

    /// <summary>
    /// struct that contains individual education history entries
    /// </summary>
    public struct Education
    {
        /// <summary>
        /// default constructor for the eductation struct
        /// </summary>
        /// <param name="name">the name of the school</param>
        /// <param name="major">a tuple containing data on the major</param>
        /// <param name="minor">the minor (if any)</param>
        /// <param name="start">the start date of the entry</param>
        /// <param name="end">the end date (graduation date) of the entry</param>
        public Education(string name, (string type, string field) major, string minor, DateTime start, DateTime end)
        {
            Name = name;
            Major = major;
            Minor = minor;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// contains the name of the school
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// a tuple with the type of degree and the field of study of the major
        /// </summary>
        public (string type, string field) Major { get; }

        /// <summary>
        /// contains the minor
        /// </summary>
        public string Minor { get; }

        /// <summary>
        /// the start date of the entry
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// the end date of the entry
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// converts the struct into a string in "Degree Type in Major Field, with a minor in Minor. Beginning on Start and Graduating End. From Name" format
        /// </summary>
        /// <returns>Returns a string in "Degree Type in Major Field, with a minor in Minor. Beginning on Start and Graduating End. From Name" format</returns>
        public override string ToString() => $"{Major.type} in {Major.field}, with a minor in {Minor}. Beginning on {StartDate.ToString()} and graduating {EndDate.ToString()}. From {Name}";
    }

    /// <summary>
    /// class that contains individual work history entries
    /// </summary>
    public class Work
    {
        /// <summary>
        /// the name of the employer
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// the job title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// the start date of the entry
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// the end date of the entry
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// a list containing all assignments of the entry
        /// </summary>
        public List<Assignment> Assignments;
        
        /// <summary>
        /// default constructor for the work class
        /// </summary>
        /// <param name="name">the name of the employer</param>
        /// <param name="title">the job title</param>
        /// <param name="assignments">a list containing all the assignments of the entry</param>
        public Work(string name, string title, List<Assignment> assignments)
        {
            Name = name;
            Title = title;
            Assignments = assignments;
        }


    }

    /// <summary>
    /// a class for containing individual assignments related to work history entries
    /// </summary>
    public class Assignment
    {
        /// <summary>
        /// the name of the assignment
        /// </summary>
        public string Name;

        /// <summary>
        /// the location of the assignment
        /// </summary>
        public string Location;

        /// <summary>
        /// the start date of the assignment
        /// </summary>
        public DateTime StartDate;

        /// <summary>
        /// the end date of the assignment
        /// </summary>
        public DateTime EndDate;

        /// <summary>
        /// a list of responsibilities associated with the assignment
        /// </summary>
        public List<(string name, string description)> Responsibilities;

        /// <summary>
        /// default constructor for the Assignment class
        /// </summary>
        /// <param name="name">the name of the assignment</param>
        /// <param name="location">the location of the assignment</param>
        /// <param name="start">the start date of the assignment</param>
        /// <param name="end">the end date of the assignment</param>
        public Assignment(string name, string location, DateTime start, DateTime end, List<(string name, string description)> responsibilities)
        {
            Name = name;
            Location = location;
            StartDate = start;
            EndDate = end;
            Responsibilities = responsibilities;    
        }
    }

    /// <summary>
    /// a struct for containing individual volunteer history entries
    /// </summary>
    public struct Volunteer
    {
        /// <summary>
        /// default constructor for the Volunteer struct
        /// </summary>
        /// <param name="name">the name of the volunteer organization</param>
        /// <param name="location">the location of the entry</param>
        /// <param name="strEvent">the name of the event associated with the entry</param>
        /// <param name="start">the start date of the entry</param>
        /// <param name="end">the end date of the entry</param>
        public Volunteer(string name, string location, string strEvent, DateTime start, DateTime end)
        {
            Name = name;
            Location = location;
            StrEvent = strEvent;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// the name of the volunteer organization
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// the location of the entry
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// the name of the event associated with the entry
        /// </summary>
        public string StrEvent { get; }

        /// <summary>
        /// the start date of the entry
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// the end date of the entry
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// converts the volunteer history entry to a string in the "name at Location, Event from Start to End" format
        /// </summary>
        /// <returns>Returns a string in the "name at Location, Event from Start to End" format</returns>
        public override string ToString() => $"{Name} at {Location}, {StrEvent} from {StartDate.ToString()} to {EndDate.ToString()}";
    }

    /// <summary>
    /// struct that contains the data on individual Award entries
    /// </summary>
    public struct Award
    {
        /// <summary>
        /// default constructor for the Award struct
        /// </summary>
        /// <param name="name">the name of the award</param>
        /// <param name="type">the type of the award</param>
        /// <param name="date">the date of the award</param>
        /// <param name="description">the description of the award</param>
        public Award(string name, string type, DateTime date, string description)
        {
            Name = name;
            Type = type;
            Date = date;
            Description = description;
        }

        /// <summary>
        /// the name of the award
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// the type of the award
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// the date of the award
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// a description of the award
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// converts the struct to a string in "Awarded Name on Date: Description" format
        /// </summary>
        /// <returns>Returns a string in "Awarded Name on Date: Description" format</returns>
        public override string ToString() => $"Awarded {Name} on {Date.ToString()}: {Description}";
    }

    /// <summary>
    /// struct for containing individual securty clearance entries
    /// </summary>
    public struct Clearance
    {
        /// <summary>
        /// The default constructor for the Clearance struct
        /// </summary>
        /// <param name="name">the name of the clearance</param>
        /// <param name="start">the start date of the clearance</param>
        /// <param name="end">the end date of the clearance</param>
        public Clearance(string name, DateTime start, DateTime end)
        {
            Name = name;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// the name of the clearance
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// the start date of the clearance
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// the end date of the clearance
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// converts the struct to a string in the "Name from Start to End" format
        /// </summary>
        /// <returns>Returns a string in the "Name from Start to End" format</returns>
        public override string ToString() => $"{Name} from {StartDate.ToString()} to {EndDate.ToString()}";
    }
}
