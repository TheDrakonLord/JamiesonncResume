using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace JamiesonncResume
{

    /// <summary>
    /// The model for the main (and only) page of the site
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Resume resumeData;

        /// <summary>
        /// creates the logger for the main index page
        /// </summary>
        /// <param name="logger">the logger to start</param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // when the page loads, get all the resume data from the XML file.
            resumeData = new Resume(XElement.Load(@"resumeData.xml"));
        }
    }
}
