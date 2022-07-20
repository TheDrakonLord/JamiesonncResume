using System.Xml.Linq;

namespace JamiesonncResume
{
    /// <summary>
    /// 
    /// </summary>
    public class Resume
    {
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
        /// 
        /// </summary>
        /// <param name="xmlResumeData"></param>
        public Resume(XElement xmlResumeData)
        {
            IEnumerable<XElement> basicInfo =
                from el in xmlResumeData.Elements("basicInfo")
                select el;

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

            IEnumerable<XElement> contactInfo =
                from el in xmlResumeData.Elements("contactInfo")
                select el;

            _email = (string)
                (from el in contactInfo.Descendants("email")
                 select el).First();

            _phone = (string)
                (from el in contactInfo.Descendants("phone")
                 select el).First();

            IEnumerable<XElement> xmlAddress =
                from el in contactInfo.Elements("address")
                select el;

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

            IEnumerable<XElement> xmlWebsite =
                from el in xmlResumeData.Elements("websites")
                select el;

            IEnumerable<XElement> xmlSites =
                from el in xmlWebsite.Elements("site")
                select el;

            foreach (var s in xmlSites)
            {
                _sites.Add((string)s.Attribute("name"), new Uri(s.Value));
            }

            IEnumerable<XElement> xmlSkills =
                from el in xmlResumeData.Elements("skills")
                select el;

            IEnumerable<XElement> xmlGeneralSkills =
                from el in xmlSkills.Elements("general")
                select el;

            IEnumerable<XElement> xmlGeneralSkillList =
                from el in xmlGeneralSkills.Elements("skill")
                select el;

            foreach (var g in xmlGeneralSkillList)
            {
                _generalSkills.Add(g.Value);
            }

            IEnumerable<XElement> xmlProgLangSkills =
                from el in xmlSkills.Elements("progLang")
                select el;

            IEnumerable<XElement> xmlProgLangSkillList =
                from el in xmlProgLangSkills.Elements("skill")
                select el;

            foreach (var p in xmlProgLangSkillList)
            {
                _progLangSkills.Add(p.Value);
            }

            IEnumerable<XElement> xmlToolSkills =
                from el in xmlSkills.Elements("tools")
                select el;

            IEnumerable<XElement> xmlToolSkillList =
                from el in xmlToolSkills.Elements("skill")
                select el;

            foreach (var t in xmlToolSkillList)
            {
                _toolSkills.Add(t.Value);
            }

            IEnumerable<XElement> xmlEducation =
                from el in xmlResumeData.Elements("education")
                select el;

            IEnumerable<XElement> xmlEducationHistory =
                from el in xmlEducation.Elements("history")
                select el;

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

            IEnumerable<XElement> xmlWork =
                from el in xmlResumeData.Elements("workHistory")
                select el;

            IEnumerable<XElement> xmlWorkHistory =
                from el in xmlWork.Elements("history")
                select el;

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

            IEnumerable<XElement> xmlVolunteer =
                from el in xmlResumeData.Elements("volunteerExperience")
                select el;

            IEnumerable<XElement> xmlVolunteerExperience =
                from el in xmlVolunteer.Elements("experience")
                select el;

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

            IEnumerable<XElement> xmlAward =
                from el in xmlResumeData.Elements("awards")
                select el;

            IEnumerable<XElement> xmlAwardList =
                from el in xmlAward.Elements("award")
                select el;

            foreach (var d in xmlAwardList)
            {
                _awards.Add(new Award(d.Attribute("name").Value,
                    d.Attribute("type").Value,
                    DateTime.Parse((string)
                    (from el in d.Descendants("date")
                     select el).First()),
                    (string)
                    (from el in d.Descendants("date")
                     select el).First()));
            }

            IEnumerable<XElement> xmlClearances =
                from el in xmlResumeData.Elements("currentClearance")
                select el;

            IEnumerable<XElement> xmlClearanceList =
                from el in xmlClearances.Elements("clearance")
                select el;

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
        /// 
        /// </summary>
        public Name Name { get { return _name; } }

        /// <summary>
        /// 
        /// </summary>
        public Address Address { get { return _address; } }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get { return _email; } }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get { return _phone; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Uri> Sites { get { return _sites; } }

        /// <summary>
        /// 
        /// </summary>
        public List<string> GeneralSkills { get { return _generalSkills; } }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ProgLangSkills { get { return _progLangSkills; } }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ToolSkills { get { return _toolSkills; } }

        /// <summary>
        /// 
        /// </summary>
        public List<Education> EducationHistory { get { return _educationHistory; } }

        /// <summary>
        /// 
        /// </summary>
        public List<Work> WorkHistory { get { return _workHistory; } }

        /// <summary>
        /// 
        /// </summary>
        public List<Volunteer> VolunteerHistory { get { return _volunteerHistory; } }

        /// <summary>
        /// 
        /// </summary>
        public List<Award> Awards { get { return _awards; } }

        /// <summary>
        /// 
        /// </summary>
        public List<Clearance> Clearances { get { return _clearances; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Name
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="middle"></param>
        /// <param name="last"></param>
        public Name(string first, string middle, string last)
        {
            First = first;
            Middle = middle;
            Last = last;
        }

        /// <summary>
        /// 
        /// </summary>
        public string First { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Middle { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Last { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{First} {Middle} {Last}";
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Address
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="street"></param>
        /// <param name="apartment"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        public Address(string street, string apartment, string city, string state, int zip)
        {
            Street = street;
            Apartment = apartment;
            City = city;
            State = state;
            Zip = zip;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Street { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Apartment { get; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; }

        /// <summary>
        /// 
        /// </summary>
        public string State { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Zip { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Street}, {Apartment}, {City}, {State}, {Zip}";
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Education
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Education(string name, (string type, string field) major, string minor, DateTime start, DateTime end)
        {
            Name = name;
            Major = major;
            Minor = minor;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public (string type, string field) Major { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Minor { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Major.type} in {Major.field}, with a minor in {Minor}. Beginning on {StartDate.ToString()} and graduating {EndDate.ToString()}. From {Name}";
    }

    /// <summary>
    /// 
    /// </summary>
    public class Work
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Assignment> Assignments;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="assignments"></param>
        public Work(string name, string title, List<Assignment> assignments)
        {
            Name = name;
            Title = title;
            Assignments = assignments;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class Assignment
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public string Location;

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate;

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate;

        /// <summary>
        /// 
        /// </summary>
        public List<(string name, string description)> Responsibilities;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
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
    /// 
    /// </summary>
    public struct Volunteer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="strEvent"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Volunteer(string name, string location, string strEvent, DateTime start, DateTime end)
        {
            Name = name;
            Location = location;
            StrEvent = strEvent;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// 
        /// </summary>
        public string StrEvent { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name} at {Location}, {StrEvent} from {StartDate.ToString()} to {EndDate.ToString()}";
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Award
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="description"></param>
        public Award(string name, string type, DateTime date, string description)
        {
            Name = name;
            Type = type;
            Date = date;
            Description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Awarded {Name} on {Date.ToString()}: {Description}";
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Clearance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Clearance(string name, DateTime start, DateTime end)
        {
            Name = name;
            StartDate = start;
            EndDate = end;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name} from {StartDate.ToString()} to {EndDate.ToString()}";
    }
}
